using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SampleApp.Application.Common.Builders;
using SampleApp.Application.Common.Enums;
using SampleApp.Data;
using SampleApp.Persistence.Infrastructure;

namespace SampleApp.Application.Post.Commands.Delete
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ResponseModel<bool>>
    {
        #region Members

        private readonly IGenericRepository<Data.Post> _postRepository;
        private readonly ILogger<DeletePostCommandHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;

        #endregion

        #region Ctor

        public DeletePostCommandHandler(IGenericRepository<Data.Post> postRepository,
            ILogger<DeletePostCommandHandler> logger, ICustomExceptionBuilder customExceptionBuilder)
        {
            _postRepository = postRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
        }

        #endregion
        
        public async Task<ResponseModel<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
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
                var post = await _postRepository.GetById(request.Id);
                if (post is null)
                {
                    _logger.LogWarning($"{request.Id} is deleted");
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                }

                await _postRepository.DeleteAsync(post);
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