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

namespace SampleApp.Application.Post.Queries.GetAll
{
    public class GetPaginatedPostQueryHandler : IRequestHandler<GetPaginatedPostQuery, ResponseModel<PaginatedResult<Data.Post>>>
    {
        #region Members

        private readonly IGenericRepository<Data.Post> _postRepository;
        private readonly ILogger<GetPaginatedPostQueryHandler> _logger;

        #endregion

        #region Ctor

        public GetPaginatedPostQueryHandler(IGenericRepository<Data.Post> postRepository,
            ILogger<GetPaginatedPostQueryHandler> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        #endregion
        public async Task<ResponseModel<PaginatedResult<Data.Post>>> Handle(GetPaginatedPostQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseModel<PaginatedResult<Data.Post>>
            {
                Status = HttpStatusCode.InternalServerError,
                IsSuccessful = false,
                Result = new PaginatedResult<Data.Post>(request.PageIndex, request.PageIndex),
                Errors = default
            };

            try
            {
                var paginatedPosts = await _postRepository.Filter(q => !q.IsDeleted);
                
                response.Status = HttpStatusCode.OK;
                response.IsSuccessful = true;
                response.Result = await paginatedPosts.AsQueryable().Paginate(request.PageSize, request.PageIndex);

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