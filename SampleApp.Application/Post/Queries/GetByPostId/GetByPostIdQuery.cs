using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;

namespace SampleApp.Application.Post.Queries.GetByPostId
{
    public class GetByPostIdQuery : BaseIdRequest, IRequest<ResponseModel<Data.Post>>
    {
    }
}