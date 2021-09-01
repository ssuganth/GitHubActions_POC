using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;

namespace SampleApp.Application.Category.Commands.Update
{
    public class UpdateCategoryCommand : BaseIdRequest, IRequest<ResponseModel<bool>>
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}