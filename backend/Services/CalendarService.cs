using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Models.DTOs;
using backend.Data;

namespace backend.Services
{
    public interface ICalendarService
    {
        Task<CalendarResponse> GetCalendar(int monthOffset, User selectedUser);
        Task<AbsenceResponse> CreateAbsence(User user, AbsenceCreateDto absenceData);
    }

    public class CalendarService : ICalendarService
    {
        private readonly ApplicationDbContext _context;

        public CalendarService(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetAbsenceLetter(string reason)
        {
            return reason switch
            {
                "Urlop_zwykły" => "U - Urlop zwykły",
                "Urlop_bezpłatny" => "Ub - Urlop bezpłatny",
                "Nadwyżka" => "Nad - Wolne z nadwyżki",
                "Praca_zdalna" => "Z - Praca zdalna",
                "Delegacja" => "D - Delegacja",
                "Choroba" => "C",
                _ => "Inny"
            };
        }

        public async Task<CalendarResponse> GetCalendar(int monthOffset, User selectedUser)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var firstDayCurrentMonth = new DateOnly(today.Year, today.Month, 1);
            var currentMonth = firstDayCurrentMonth.AddMonths(monthOffset);
            var daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

            var polishCulture = new CultureInfo("pl-PL");
            var formattedCurrentMonth = polishCulture
                .DateTimeFormat.GetMonthName(currentMonth.Month)
                .ToUpper() + " " + currentMonth.Year.ToString();

            var startOfMonth = currentMonth;
            var endOfMonth = new DateOnly(currentMonth.Year, currentMonth.Month, daysInMonth);

            var absences = await _context.Absences
                .Where(a => a.UserId == selectedUser.Id &&
                           a.EndDate >= startOfMonth.ToDateTime(TimeOnly.MinValue) &&
                           a.StartDate <= endOfMonth.ToDateTime(TimeOnly.MaxValue))
                .ToListAsync();

            var calendar = new Dictionary<string, DayInfo>();
            for (int day = 1; day <= daysInMonth; day++)
            {
                var dateObj = new DateOnly(currentMonth.Year, currentMonth.Month, day);
                var dayKey = day.ToString();
                calendar[dayKey] = new DayInfo
                {
                    DayOfWeek = polishCulture.DateTimeFormat.GetDayName(dateObj.ToDateTime(TimeOnly.MinValue).DayOfWeek).ToLower(),
                    Status = "",
                    IsToday = dateObj == today
                };
            }

            foreach (var absence in absences)
            {
                var absenceStartDate = DateOnly.FromDateTime(absence.StartDate);
                var absenceEndDate = DateOnly.FromDateTime(absence.EndDate);
                
                var startDay = absenceStartDate.Month == currentMonth.Month ? absenceStartDate.Day : 1;
                var endDay = absenceEndDate.Month == currentMonth.Month ? absenceEndDate.Day : daysInMonth;
                
                var letter = GetAbsenceLetter(absence.Reason);

                for (int day = startDay; day <= endDay; day++)
                {
                    var dayKey = day.ToString();
                    if (calendar.ContainsKey(dayKey))
                    {
                        calendar[dayKey].Status = letter;
                    }
                }
            }

            var users = await _context.Users
                .Select(u => new UserSummary { Id = u.Id, Name = u.Name })
                .ToListAsync();

            return new CalendarResponse
            {
                Calendar = calendar,
                CurrentMonth = currentMonth.ToString("yyyy-MM-dd"),
                FormattedCurrentMonth = formattedCurrentMonth,
                MonthOffset = monthOffset,
                Users = users,
                SelectedUserId = selectedUser.Id
            };
        }

        public async Task<AbsenceResponse> CreateAbsence(User user, AbsenceCreateDto absenceData)
        {
            var absence = new Absence
            {
                UserId = user.Id,
                StartDate = absenceData.StartDate.ToDateTime(TimeOnly.MinValue),
                EndDate = absenceData.EndDate.ToDateTime(TimeOnly.MinValue),
                Reason = absenceData.Reason,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Absences.Add(absence);
            await _context.SaveChangesAsync();

            return new AbsenceResponse
            {
                SuccessMessage = "Nieobecność została dodana.",
                Absence = new AbsenceDto
                {
                    Id = absence.Id,
                    StartDate = DateOnly.FromDateTime(absence.StartDate),
                    EndDate = DateOnly.FromDateTime(absence.EndDate),
                    Reason = absence.Reason
                }
            };
        }
    }
}
