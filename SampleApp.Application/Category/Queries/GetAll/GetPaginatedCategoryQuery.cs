using MediatR;
using SampleApp.Application.Internal;
using SampleApp.Data;

namespace SampleApp.Application.Category.Queries.GetAll
{
    public class GetPaginatedCategoryQuery : IRequest<ResponseModel<PaginatedResult<Data.Category>>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}