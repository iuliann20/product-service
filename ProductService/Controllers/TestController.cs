using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Abstractions;
using ProductService.Application.Commands.Test;
using ProductService.Application.Queries.Test;
using ProductService.Contracts.Test;

namespace ProductService.Controllers
{
    [ApiVersion(1.0)]
    [ApiVersion(1.1)]
    [ApiVersion(2.0, Deprecated = true)]
    [ApiVersion(2.1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class TestController : ApiController
    {
        public TestController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        [MapToApiVersion(1.0)]
        [Route("getTest")]
        public async Task<IActionResult> GetTest([FromBody] TestRequest testRequest, CancellationToken cancellationToken)
        {
            var query = new GetTestQuery
            {
                Id = testRequest.Id
            };

            var response = await Sender.Send(query, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
        }

        [HttpPost]
        [Route("commandTest1")]
        [MapToApiVersion(1.1)]
        public async Task<IActionResult> CommandTest([FromBody] TestRequest testRequest, CancellationToken cancellationToken)
        {
            var command = new TestCommand
            {
                Id = testRequest.Id
            };

            var response = await Sender.Send(command, cancellationToken);

            return response.IsSuccess ? Ok() : BadRequest(response.Error);

        }


        [HttpPost]
        [Route("commandTest2")]
        [MapToApiVersion(2.0)]
        public async Task<IActionResult> CommandTest2([FromBody] TestRequest testRequest, CancellationToken cancellationToken)
        {
            var command = new TestCommand
            {
                Id = testRequest.Id
            };

            var response = await Sender.Send(command, cancellationToken);

            return response.IsSuccess ? Ok() : BadRequest(response.Error);

        }

        [HttpPost]
        [Route("commandTest2.1")]
        [Obsolete]
        [MapToApiVersion(2.1)]
        public async Task<IActionResult> CommandTest3([FromBody] TestRequest testRequest)
        {
            return RedirectPermanent("/commandTest2");

        }
    }
}
