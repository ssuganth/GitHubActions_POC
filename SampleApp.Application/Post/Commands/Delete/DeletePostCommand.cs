using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;

namespace SampleApp.Application.Post.Commands.Delete
{
    public class DeletePostCommand : BaseIdRequest, IRequest<ResponseModel<bool>>
    {
    }
}