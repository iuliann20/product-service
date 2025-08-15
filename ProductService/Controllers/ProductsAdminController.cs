using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Abstractions;
using ProductService.Application.Commands.Products.AdjustStock;
using ProductService.Application.Commands.Products.CreateProduct;
using ProductService.Application.Commands.Products.Images.AddImage;
using ProductService.Application.Commands.Products.Images.RemoveImage;
using ProductService.Application.Commands.Products.UpdateProduct;
using ProductService.Domain.Contracts.Requests;

namespace ProductService.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsAdminController : ApiController
    {
        public ProductsAdminController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest req, CancellationToken ct)
        {
            var response = await Sender.Send(new CreateProductCommand(req.Name, req.Description, req.Price, req.CategoryId, req.StockQuantity, req.IsActive), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [HttpPut("edit/{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductRequest req, CancellationToken ct)
        {
            var response = await Sender.Send(new UpdateProductCommand(id, req.Name, req.Description, req.Price, req.CategoryId, req.IsActive), ct);

            return response.IsSuccess ? NoContent() : HandleFailure(response);
        }

        [HttpPost("stock/{id:guid}")]
        public async Task<IActionResult> AdjustStock([FromRoute] Guid id, [FromBody] AdjustStockRequest req, CancellationToken ct)
        {
            var response = await Sender.Send(new AdjustStockCommand(id, req.Delta), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [HttpPost("images/{id:guid}")]
        public async Task<IActionResult> AddImage([FromRoute] Guid id, [FromBody] AddImageRequest req, CancellationToken ct)
        {
            var response = await Sender.Send(new AddProductImageCommand(id, req.ImageUrl, req.IsMain), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [HttpDelete("images/{id:guid}/{imageId:guid}")]
        public async Task<IActionResult> RemoveImage([FromRoute] Guid id, [FromRoute] Guid imageId, CancellationToken ct)
        {
            var response = await Sender.Send(new RemoveProductImageCommand(id, imageId), ct);

            return response.IsSuccess ? NoContent() : HandleFailure(response);
        }
    }
}
