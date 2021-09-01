using System.Collections.Generic;
using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;

namespace SampleApp.Application.Category.Queries.GetParent
{
    public class GetParentQuery : BaseIdRequest, IRequest<ResponseModel<ICollection<CategoryResponse>>>
    {
    }

    public class CategoryResponse
    {
        public string Id { get; set; }
        public string ParentId { get; set; }

        public string Name { get; set; }
    }
}