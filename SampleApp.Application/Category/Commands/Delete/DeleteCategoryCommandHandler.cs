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

namespace SampleApp.Application.Category.Commands.Delete
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseModel<bool>>
    {
        #region Members

        private readonly IGenericRepository<Data.Category> _categoryRepository;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;

        #endregion

        #region Ctor

        public DeleteCategoryCommandHandler(IGenericRepository<Data.Category> categoryRepository,
            ILogger<DeleteCategoryCommandHandler> logger, ICustomExceptionBuilder customExceptionBuilder)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
        }

        #endregion
        
        public async Task<ResponseModel<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
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
                var category = await _categoryRepository.GetById(request.Id);
                if (category is null)
                {
                    _logger.LogWarning($"{request.Id} is deleted parent");
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                }

                await _categoryRepository.DeleteAsync(category);
                response.Status = HttpStatusCode.OK;
                response.IsSuccessful = true;
                response.Result = true;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace,"Method : DeletePostCommandHandler - Handle");
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