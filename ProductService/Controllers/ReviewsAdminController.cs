using Asp.Versioning;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Abstractions;
using ProductService.Application.Commands.Reviews.ApproveReview;
using ProductService.Application.Commands.Reviews.RejectReview;

namespace ProductService.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReviewsAdminController : ApiController
    {
        public ReviewsAdminController(ISender sender) : base(sender)
        {
        }

        [HttpPost("approve/{id:guid}")]
        public async Task<IActionResult> Approve([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await Sender.Send(new ApproveReviewCommand(id), ct);

            return response.IsSuccess ? NoContent() : HandleFailure(response);
        }

        [HttpPost("reject/{id:guid}")]
        public async Task<IActionResult> Reject([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await Sender.Send(new RejectReviewCommand(id), ct);

            return response.IsSuccess ? NoContent() : HandleFailure(response);
        }
    }
}
