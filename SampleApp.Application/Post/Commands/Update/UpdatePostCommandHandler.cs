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
using SampleApp.Utility.Interfaces;

namespace SampleApp.Application.Post.Commands.Update
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ResponseModel<bool>>
    {
        #region Members

        private readonly IGenericRepository<Data.Post> _postRepository;
        private readonly IGenericRepository<Data.Category> _categoryRepository;

        private readonly ILogger<UpdatePostCommandHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;
        private readonly IHtmlSanitizerHelper _htmlSanitizerHelper;

        #endregion

        #region Ctor

        public UpdatePostCommandHandler(IGenericRepository<Data.Post> postRepository,
            ILogger<UpdatePostCommandHandler> logger, ICustomExceptionBuilder customExceptionBuilder,
            IHtmlSanitizerHelper htmlSanitizerHelper, IGenericRepository<Data.Category> categoryRepository)
        {
            _postRepository = postRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
            _htmlSanitizerHelper = htmlSanitizerHelper;
            _categoryRepository = categoryRepository;
        }

        #endregion

        public async Task<ResponseModel<bool>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
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

                var category = await _categoryRepository.GetById(request.CategoryId);
                if (category is null)
                {
                    _logger.LogWarning($"{request.CategoryId} is not found in UpdatePostCommandHandler");
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                }

                var post = await _postRepository.GetById(request.Id);
                if (post is null)
                {
                    _logger.LogWarning($"Post - {request.Id} is not found in UpdatePostCommandHandler");
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.Id, ErrorTypes.EntityNotFound);
                }

                post.Category = category;
                post.Content = _htmlSanitizerHelper.SanitizeInput(request.Content);
                post.Title = _htmlSanitizerHelper.SanitizeInput(request.Title);
                post.PostStatus = request.PostStatus;

                await _postRepository.Update(post);
                response.Status = HttpStatusCode.OK;
                response.IsSuccessful = true;
                response.Result = true;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace, "Method : UpdatePostCommandHandler - Handle");
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