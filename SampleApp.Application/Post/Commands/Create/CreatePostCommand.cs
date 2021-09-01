using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;
using System;
using SampleApp.Data.Enums;

namespace SampleApp.Application.Post.Commands.Create
{
    public class CreatePostCommand : BaseIdRequest, IRequest<ResponseModel<bool>>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public PostStatus PostStatus => PostStatus.Pending;
        public string CategoryId { get; set; }

        public override string Id => Guid.NewGuid().ToString("D");
    }
}