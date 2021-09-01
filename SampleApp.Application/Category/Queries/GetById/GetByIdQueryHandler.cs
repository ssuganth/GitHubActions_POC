using MediatR;
using Microsoft.Extensions.Logging;
using SampleApp.Application.Common.Builders;
using SampleApp.Data;
using SampleApp.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SampleApp.Application.Common.Enums;

namespace SampleApp.Application.Category.Queries.GetById
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ResponseModel<Data.Category>>
    {
        #region Members

        private readonly IGenericRepository<Data.Category> _categoryRepository;
        private readonly ILogger<GetByIdQueryHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;

        #endregion

        #region Ctor

        public GetByIdQueryHandler(IGenericRepository<Data.Category> categoryRepository,
            ILogger<GetByIdQueryHandler> logger, ICustomExceptionBuilder customExceptionBuilder)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
        }

        #endregion
        public async Task<ResponseModel<Data.Category>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseModel<Data.Category>()
            {
                Status = HttpStatusCode.InternalServerError,
                IsSuccessful = false,
                Result = default,
                Errors = default
            };

            try
            {
                var category = await _categoryRepository.GetById(request.Id);
                if (category is null)
                {
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                }

                response.IsSuccessful = true;
                response.Result = category;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace, "Method : GetByIdQueryHandler - Handle / Category");
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