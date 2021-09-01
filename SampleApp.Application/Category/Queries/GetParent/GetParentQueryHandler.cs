using MediatR;
using Microsoft.Extensions.Logging;
using SampleApp.Application.Internal;
using SampleApp.Data;
using SampleApp.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SampleApp.Application.Category.Queries.GetById;

namespace SampleApp.Application.Category.Queries.GetParent
{
    public class GetParentQueryHandler :IRequestHandler<GetParentQuery, ResponseModel<ICollection<CategoryResponse>>>
    {
        #region Members

        private readonly IGenericRepository<Data.Category> _categoryRepository;
        private readonly ILogger<GetByIdQueryHandler> _logger;
        private readonly ResponseModel<ICollection<CategoryResponse>> _response;

        #endregion

        #region Ctor

        public GetParentQueryHandler(IGenericRepository<Data.Category> categoryRepository,
            ILogger<GetByIdQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _response = new ResponseModel<ICollection<CategoryResponse>>()
            {
                Status = HttpStatusCode.InternalServerError,
                Result = new List<CategoryResponse>(),
                IsSuccessful = false,
                Errors = default
            };
        }

        #endregion

        public async Task<ResponseModel<ICollection<CategoryResponse>>> Handle(GetParentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var parent = await _categoryRepository.GetById(request.Id);

                if (parent != null)
                {
                    _response.Result.Add(new CategoryResponse(){Id = request.Id, ParentId = parent.ParentId, Name = parent.Name});

                    await Handle(new GetParentQuery() { Id = parent.ParentId }, CancellationToken.None);
                }

                _response.Status = HttpStatusCode.OK;
                _response.IsSuccessful = true;
                _response.Result = _response.Result.NotNullOrEmpty() 
                    ? _response.Result : 
                    default;

                return _response;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.StackTrace, "Method : GetParentQueryHandler - Handle / Category");
                _response.Result = default;
                _response.Errors = new List<ErrorResponse>()
                {
                    new ErrorResponse()
                    {
                        Reason = (int) HttpStatusCode.InternalServerError,
                        Message = e.Message
                    }
                };
            }

            return _response;
        }
    }
}