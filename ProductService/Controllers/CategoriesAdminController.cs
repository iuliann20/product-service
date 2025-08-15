using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Abstractions;
using ProductService.Application.Commands.Categories.CreateCategory;
using ProductService.Application.Commands.Categories.DeleteCategory;
using ProductService.Application.Commands.Categories.UpdateCategory;
using ProductService.Domain.Contracts.Requests;

namespace ProductService.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesAdminController : ApiController
    {
        public CategoriesAdminController(ISender sender) : base(sender)
        {
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest req, CancellationToken ct)
        {
            var response = await Sender.Send(new CreateCategoryCommand(req.Name, req.Description), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryRequest req, CancellationToken ct)
        {
            var response = await Sender.Send(new UpdateCategoryCommand(id, req.Name, req.Description, req.IsActive), ct);

            return response.IsSuccess ? NoContent() : HandleFailure(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await Sender.Send(new DeleteCategoryCommand(id), ct);

            return response.IsSuccess ? NoContent() : HandleFailure(response);
        }
    }
}
