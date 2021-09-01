using MediatR;
using SampleApp.Application.Internal;
using SampleApp.Data;

namespace SampleApp.Application.Post.Queries.GetAll
{
    public class GetPaginatedPostQuery : IRequest<ResponseModel<PaginatedResult<Data.Post>>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}