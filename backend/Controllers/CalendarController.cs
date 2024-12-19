using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using backendCRM.Services;
using backendCRM.Models;
using backendCRM.Models.DTOs;
using backendCRM.Data;
using System.Security.Claims;

namespace backendCRM.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly ApplicationDbContext _context;

        public CalendarController(ICalendarService calendarService, ApplicationDbContext context)
        {
            _calendarService = calendarService;
            _context = context;
        }

        [HttpGet("calendar")]
        public async Task<ActionResult<CalendarResponse>> GetCalendar(
            [FromQuery(Name = "monthOffset")] int monthOffset = 0,
            [FromQuery(Name = "user_id")] long? userId = null)
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
                    return NotFound("Current user not found");
                }

                User selectedUser;
                if (userId.HasValue)
                {
                    selectedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId.Value)
                        ?? throw new InvalidOperationException("Nie znaleziono użytkownika.");
                }
                else
                {
                    selectedUser = currentUser;
                }

                var result = await _calendarService.GetCalendar(monthOffset, selectedUser);
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

        [HttpPost("absences/store")]
        public async Task<ActionResult<AbsenceResponse>> StoreAbsence([FromBody] AbsenceCreateDto absenceData)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out long id))
                {
                    return Unauthorized();
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                    ?? throw new InvalidOperationException("User not found");

                var result = await _calendarService.CreateAbsence(user, absenceData);
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
