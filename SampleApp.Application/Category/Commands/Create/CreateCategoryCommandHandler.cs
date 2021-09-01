using MediatR;
using Microsoft.Extensions.Logging;
using SampleApp.Data;
using SampleApp.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SampleApp.Application.Common.Builders;
using SampleApp.Application.Common.Enums;

namespace SampleApp.Application.Category.Commands.Create
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseModel<bool>>
    {
        #region Members

        private readonly IGenericRepository<Data.Category> _categoryRepository;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;

        #endregion

        #region Ctor

        public CreateCategoryCommandHandler(IGenericRepository<Data.Category> categoryRepository,
            ILogger<CreateCategoryCommandHandler> logger, ICustomExceptionBuilder customExceptionBuilder)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
        }

        #endregion

        public async Task<ResponseModel<bool>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
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
                        _logger.LogWarning($"{request.ParentId} is deleted");
                        return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                    }
                }

                var category = new Data.Category()
                {
                    ParentId = request.ParentId,
                    Description = request.Description,
                    Name = request.Name,
                    Parent = parentCategory,
                    Id = request.Id
                };

                await _categoryRepository.CreateAsync(category);
                response.Status = HttpStatusCode.Created;
                response.IsSuccessful = true;
                response.Result = true;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace, "Method : CreateCategoryCommandHandler - Handle");

                var identicalPrimaryKeyExceptionResponse =
                    _customExceptionBuilder.BuildIdenticalPrimaryKeyException(e, response, request.Id);

                if (identicalPrimaryKeyExceptionResponse != null) return identicalPrimaryKeyExceptionResponse;
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