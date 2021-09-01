using MediatR;
using Microsoft.Extensions.Logging;
using SampleApp.Application.Internal;
using SampleApp.Data;
using SampleApp.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.Application.Category.Queries.GetAll
{
    public class GetPaginatedCategoryQueryHandler : IRequestHandler<GetPaginatedCategoryQuery, ResponseModel<PaginatedResult<Data.Category>>>
    {
        #region Members

        private readonly IGenericRepository<Data.Category> _categoryRepository;
        private readonly ILogger<GetPaginatedCategoryQueryHandler> _logger;

        #endregion

        #region Ctor

        public GetPaginatedCategoryQueryHandler(IGenericRepository<Data.Category> categoryRepository,
            ILogger<GetPaginatedCategoryQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        #endregion
        public async Task<ResponseModel<PaginatedResult<Data.Category>>> Handle(GetPaginatedCategoryQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseModel<PaginatedResult<Data.Category>>
            {
                Status = HttpStatusCode.InternalServerError,
                IsSuccessful = false,
                Result = new PaginatedResult<Data.Category>(request.PageIndex, request.PageIndex),
                Errors = default
            };

            try
            {
                var paginatedCategories = await _categoryRepository.Filter(q => !q.IsDeleted);
                
                response.Status = HttpStatusCode.OK;
                response.IsSuccessful = true;
                response.Result = await paginatedCategories.AsQueryable().Paginate(request.PageSize, request.PageIndex);

                return response;

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace, "Method : GetPaginatedCategoryQueryHandler - Handle");
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