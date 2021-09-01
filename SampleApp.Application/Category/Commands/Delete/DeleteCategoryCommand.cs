using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;

namespace SampleApp.Application.Category.Commands.Delete
{
    public class DeleteCategoryCommand : BaseIdRequest, IRequest<ResponseModel<bool>>
    {
    }
}