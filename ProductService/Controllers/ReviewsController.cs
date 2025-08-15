using Asp.Versioning;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Abstractions;
using ProductService.Application.Commands.Reviews.CreateReview;
using ProductService.Application.Queries.Reviews.GetApprovedReviews;
using ProductService.Domain.Contracts.Requests;
using ProductService.Domain.Contracts.Responses;
using ProductService.Infrastructure.Auth;

namespace ProductService.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReviewsController : ApiController
    {
        public ReviewsController(ISender sender) : base(sender)
        {
        }

        [HttpGet("product/{productId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IReadOnlyList<ProductReviewDto>), 200)]
        public async Task<IActionResult> GetForProduct([FromRoute] Guid productId, CancellationToken ct)
        {
            var response = await Sender.Send(new GetApprovedReviewsQuery(productId), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest req, CancellationToken ct)
        {
            var userId = User.GetUserId();

            var response = await Sender.Send(new CreateReviewCommand(userId, req.ProductId, req.Rating, req.Comment), ct);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }
    }
}
