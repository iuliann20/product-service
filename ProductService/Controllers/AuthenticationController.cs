using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Abstractions;
using ProductService.Contracts.Authentication;
using ProductService.Domain.Services;
using ProductService.Domain.Services.Models.Authentication;
using ProductService.Helpers;

namespace ProductService.Controllers
{
    [ApiVersion(1.0)]
    [ApiVersion(1.1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _authenticateService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(ISender sender, IAuthenticationService authenticateService, IHttpContextAccessor httpContextAccessor) : base(sender)
        {
            _authenticateService = authenticateService;
            _httpContextAccessor = httpContextAccessor;
        }

        //[AllowAnonymous]
        //[HttpPost, Route("token")]
        //public IActionResult Authenticate([FromBody] TokenRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tokenModel = Mapping.Mapper.Map<TokenModel>(request);
        //    var result = _authenticateService.IsAuthenticated(tokenModel);

        //    return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        //}

        //[Authorize]
        //[HttpGet, Route("login")]
        //public async Task<IActionResult> Login()
        //{
        //    var username = _httpContextAccessor.HttpContext.User.Identity?.Name!;

        //    if (string.IsNullOrEmpty(username))
        //    {
        //        return BadRequest("Invalid Request");
        //    }

        //    var result = _authenticateService.Login(username!, _httpContextAccessor);

        //    return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        //}
    }
}
