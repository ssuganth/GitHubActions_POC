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
using SampleApp.Utility.Interfaces;

namespace SampleApp.Application.Post.Commands.Create
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ResponseModel<bool>>
    {
        #region Members

        private readonly IGenericRepository<Data.Post> _postRepository;
        private readonly IGenericRepository<Data.Category> _categoryRepository;

        private readonly ILogger<CreatePostCommandHandler> _logger;
        private readonly ICustomExceptionBuilder _customExceptionBuilder;
        private readonly IHtmlSanitizerHelper _htmlSanitizerHelper;

        #endregion

        #region Ctor

        public CreatePostCommandHandler(IGenericRepository<Data.Post> postRepository,
            ILogger<CreatePostCommandHandler> logger, ICustomExceptionBuilder customExceptionBuilder,
            IHtmlSanitizerHelper htmlSanitizerHelper, IGenericRepository<Data.Category> categoryRepository)
        {
            _postRepository = postRepository;
            _logger = logger;
            _customExceptionBuilder = customExceptionBuilder;
            _htmlSanitizerHelper = htmlSanitizerHelper;
            _categoryRepository = categoryRepository;
        }

        #endregion

        public async Task<ResponseModel<bool>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
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
                    _logger.LogWarning($"{request.CategoryId} is not found");
                    return _customExceptionBuilder.BuildEntityNotFoundException(response, request.CategoryId, ErrorTypes.EntityNotFound);
                }
                var post = new Data.Post()
                {
                   Category = category,
                   PostStatus = request.PostStatus,
                   Content = _htmlSanitizerHelper.SanitizeInput(request.Content),
                   Title = _htmlSanitizerHelper.SanitizeInput(request.Title),
                    Id = request.Id
                };

                await _postRepository.CreateAsync(post);
                response.Status = HttpStatusCode.Created;
                response.IsSuccessful = true;
                response.Result = true;

                return response;
            }
            catch (Exception e)
            {
                if (e is ArgumentException)
                {
                    _logger.LogCritical(e, e.StackTrace, "Method : CreatePostCommandHandler - Handle - ArgumentException");
                    return _customExceptionBuilder.BuildMaliciousInputFoundException(response, e.Message,
                        ErrorTypes.MaliciousInput);

                }
                _logger.LogCritical(e, e.StackTrace, "Method : CreatePostCommandHandler - Handle");

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