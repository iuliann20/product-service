using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Abstractions;
using ProductService.Application.Queries.Categories.GetCategories;
using ProductService.Application.Queries.Products.GetProductById;
using ProductService.Application.Queries.Products.SearchProducts;
using ProductService.Domain.Contracts.Requests;

namespace ProductService.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CatalogController : ApiController
    {
        public CatalogController(ISender sender) : base(sender)
        {
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> Categories(CancellationToken ct)
        {
            var response = await Sender.Send(new GetCategoriesQuery(), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [HttpPost("products/search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromBody] SearchProductsRequest req, CancellationToken ct)
        {
            var response = await Sender.Send(new SearchProductsQuery(req), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [HttpGet("products/{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await Sender.Send(new GetProductByIdQuery(id), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }
    }
}
