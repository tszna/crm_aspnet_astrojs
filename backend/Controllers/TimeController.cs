using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services;
using backend.Models;
using backend.Models.DTOs;
using backend.Data;
using System.Security.Claims;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        private readonly ITimeService _timeService;
        private readonly ApplicationDbContext _context;

        public TimeController(ITimeService timeService, ApplicationDbContext context)
        {
            _timeService = timeService;
            _context = context;
        }

        [HttpGet("getCurrentSession")]
        public async Task<ActionResult<CurrentSessionOut>> GetCurrentSession()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out long id))
                {
                    return Unauthorized();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _timeService.GetCurrentSession(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("startSession")]
        public async Task<IActionResult> StartSession()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out long id))
                {
                    return Unauthorized();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _timeService.StartNewSession(user);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("stopSession")]
        public async Task<IActionResult> StopSession()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out long id))
                {
                    return Unauthorized();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _timeService.StopSession(user);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("weekly-summary")]
        public async Task<ActionResult<WeeklySummaryResponse>> GetWeeklySummary([FromQuery] int weekOffset = 0, [FromQuery] long? userId = null)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserId) || !long.TryParse(currentUserId, out long id))
                {
                    return Unauthorized();
                }

                var currentUser = await _context.Users.FindAsync(id);
                if (currentUser == null)
                {
                    return NotFound("User not found");
                }

                var result = await _timeService.GetWeeklySummary(currentUser, weekOffset, userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
