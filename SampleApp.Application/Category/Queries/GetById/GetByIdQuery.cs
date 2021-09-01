using MediatR;
using SampleApp.Application.Common.Models.Request;
using SampleApp.Data;

namespace SampleApp.Application.Category.Queries.GetById
{
    public class GetByIdQuery : BaseIdRequest, IRequest<ResponseModel<Data.Category>>
    {
    }
}