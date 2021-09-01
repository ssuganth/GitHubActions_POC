using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Application.Category.Commands.Create;
using SampleApp.Application.Category.Commands.Delete;
using SampleApp.Application.Category.Commands.Update;
using SampleApp.Application.Category.Queries.GetAll;
using SampleApp.Application.Category.Queries.GetById;
using SampleApp.Application.Category.Queries.GetParent;
using SampleApp.Application.Internal;
using SampleApp.Data;
using System.Threading.Tasks;

namespace DotnetCoreSampleApp.Controllers
{
    public class CategoryController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel<Category>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var response = await Mediator.Send(new GetByIdQuery(){Id = id});
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await Mediator.Send(new DeleteCategoryCommand(){Id = id});
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] UpdateCategoryCommand request, string id)
        {
            request.Id = id;
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("pagination")]
        [ProducesResponseType(typeof(ResponseModel<PaginatedResult<Category>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int? pageIndex, int? pageSize)
        {
            var response = await Mediator.Send(new GetPaginatedCategoryQuery()
            {
                PageIndex = pageIndex ?? default,
                PageSize = pageSize ?? default
            });
            return Ok(response);
        }

        [HttpGet("parent/{id}")]
        [ProducesResponseType(typeof(ResponseModel<Category>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParentById(string id)
        {
            var response = await Mediator.Send(new GetParentQuery() { Id = id });
            return Ok(response);
        }
    }
}