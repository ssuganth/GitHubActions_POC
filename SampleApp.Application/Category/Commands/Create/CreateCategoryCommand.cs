using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;
using System;

namespace SampleApp.Application.Category.Commands.Create
{
    public class CreateCategoryCommand : BaseIdRequest, IRequest<ResponseModel<bool>>
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string Id => Guid.NewGuid().ToString("D");
    }
}