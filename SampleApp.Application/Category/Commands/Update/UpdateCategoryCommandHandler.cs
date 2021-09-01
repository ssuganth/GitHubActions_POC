using MediatR;
using Microsoft.Extensions.Logging;
using SampleApp.Application.Common.Builders;
using SampleApp.Application.Common.Enums;
using SampleApp.Data;
using SampleApp.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.Application.Category.Commands.Update
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResponseModel<bool>>
    {
        #region Members

        private readonly IGenericRepository<Data.Category> _categoryRepository;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;

        #endregion

        #region Ctor

        public UpdateCategoryCommandHandler(IGenericRepository<Data.Category> categoryRepository,
            ILogger<UpdateCategoryCommandHandler> logger, ICustomExceptionBuilder customExceptionBuilder)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
        }

        #endregion

        public async Task<ResponseModel<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseModel<bool>()
            {
                Status = HttpStatusCode.InternalServerError,
                IsSuccessful = false,
                Result = false,
                Errors = default
            };

            try
            {
                var parentCategory = default(Data.Category);

                if (!string.IsNullOrEmpty(request.ParentId))
                {
                    parentCategory = await _categoryRepository.GetById(request.ParentId);
                    if (parentCategory is null)
                    {
                        _logger.LogWarning($"{request.ParentId} is deleted parent in UpdateCategoryCommandHandler");
                        return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                    }

                    var isParentAndCategorySame = parentCategory.Id.Equals(request.Id);
                    if (isParentAndCategorySame)
                    {
                        _logger.LogWarning("Child and Parent cannot be same");
                        return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.CategoryAndParentSame);
                    }
                }

                var category = await _categoryRepository.GetById(request.Id);
                if (category is null)
                {
                    _logger.LogWarning($"{request.Id} is deleted in UpdateCategoryCommandHandler");
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                }

                category.Description = request.Description;
                category.Name = request.Name;
                category.Parent = parentCategory;

                await _categoryRepository.Update(category);
                response.Status = HttpStatusCode.OK;
                response.IsSuccessful = true;
                response.Result = true;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace, "Method : UpdateCategoryCommandHandler - Handle");
            }

            response.Errors = new List<ErrorResponse>()
            {
                new ErrorResponse()
                {
                    Reason = 500,
                    Message = "An unexpected error occured"
                }
            };

            return response;
        }
    }
}