using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Models.DTOs;
using backend.Services;
using backend.Data;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return BadRequest("Email jest już zarejestrowany");

            if (await _context.Users.AnyAsync(u => u.Name == registerDto.Name))
                return BadRequest("Nazwa użytkownika jest już zajęta");

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = _authService.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        [HttpPost("token")]
        public async Task<ActionResult<object>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !_authService.VerifyPassword(loginDto.Password, user.Password))
                return Unauthorized("Nieprawidłowy email lub hasło");

            var token = _authService.CreateToken(user);

            return new { access_token = token, token_type = "bearer" };
        }

        [Authorize]
        [HttpGet("users/me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out long id))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
