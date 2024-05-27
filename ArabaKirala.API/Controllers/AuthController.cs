using System.Security.Claims;
using ArabaKirala.API.Abstractions.Services;
using ArabaKirala.API.Dtos.Login;
using ArabaKirala.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArabaKirala.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var token = await _authService.LoginAsync(loginRequest.UsernameOrEmail, loginRequest.Password, 365 * 24 * 60 * 60);
            return Ok(new { AccessToken = token.AccessToken});
        }
        [Authorize]
        [HttpGet("protected-resource")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");
            return Ok(new { user.Id, user.UserName, user.Email });
        }

    }
}