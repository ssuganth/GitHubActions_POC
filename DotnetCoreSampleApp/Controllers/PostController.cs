using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Application.Internal;
using SampleApp.Application.Post.Commands.Create;
using SampleApp.Application.Post.Commands.Delete;
using SampleApp.Application.Post.Commands.Update;
using SampleApp.Application.Post.Queries.GetAll;
using SampleApp.Application.Post.Queries.GetByPostId;
using SampleApp.Data;
using System.Threading.Tasks;

namespace DotnetCoreSampleApp.Controllers
{
    public class PostController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreatePostCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel<Post>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var response = await Mediator.Send(new GetByPostIdQuery() { Id = id });
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await Mediator.Send(new DeletePostCommand() { Id = id });
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] UpdatePostCommand request, string id)
        {
            request.Id = id;
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("pagination")]
        [ProducesResponseType(typeof(ResponseModel<PaginatedResult<Post>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int? pageIndex, int? pageSize)
        {
            var response = await Mediator.Send(new GetPaginatedPostQuery()
            {
                PageIndex = pageIndex ?? default,
                PageSize = pageSize ?? default
            });
            return Ok(response);
        }
    }
}