using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;
using SampleApp.Data.Enums;

namespace SampleApp.Application.Post.Commands.Update
{
    public class UpdatePostCommand : BaseIdRequest, IRequest<ResponseModel<bool>>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public PostStatus PostStatus { get; set; }
        public string CategoryId { get; set; }
    }
}