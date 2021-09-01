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
using SampleApp.Application.Post.Queries.GetByPostId;

namespace SampleApp.Application.Post.Queries.GetByPostId
{
    public class GetByPostIdQueryHandler : IRequestHandler<GetByPostIdQuery, ResponseModel<Data.Post>>
    {
        #region Members

        private readonly IGenericRepository<Data.Post> _postRepository;
        private readonly ILogger<GetByPostIdQueryHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;

        #endregion

        #region Ctor

        public GetByPostIdQueryHandler(IGenericRepository<Data.Post> postRepository,
            ILogger<GetByPostIdQueryHandler> logger, ICustomExceptionBuilder customExceptionBuilder)
        {
            _postRepository = postRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
        }

        #endregion
        public async Task<ResponseModel<Data.Post>> Handle(GetByPostIdQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseModel<Data.Post>()
            {
                Status = HttpStatusCode.InternalServerError,
                IsSuccessful = false,
                Result = default,
                Errors = default
            };

            try
            {
                var post = await _postRepository.GetById(request.Id);
                if (post is null)
                {
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                }

                response.IsSuccessful = true;
                response.Result = post;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace, "Method : GetByPostIdQueryHandler - Handle / Post");
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