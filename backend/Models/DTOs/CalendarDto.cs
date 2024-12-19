using System.Text.Json.Serialization;

namespace backendCRM.Models.DTOs
{
    public class DayInfo
    {
        [JsonPropertyName("day_of_week")]
        public string DayOfWeek { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("is_today")]
        public bool IsToday { get; set; }
    }

    public class CalendarResponse
    {
        public CalendarResponse()
        {
            Calendar = new Dictionary<string, DayInfo>();
            Users = new List<UserSummary>();
        }

        [JsonPropertyName("calendar")]
        public Dictionary<string, DayInfo> Calendar { get; set; }

        [JsonPropertyName("currentMonth")]
        public string CurrentMonth { get; set; } = string.Empty;

        [JsonPropertyName("formattedCurrentMonth")]
        public string FormattedCurrentMonth { get; set; } = string.Empty;

        [JsonPropertyName("monthOffset")]
        public int MonthOffset { get; set; }

        [JsonPropertyName("users")]
        public List<UserSummary> Users { get; set; }

        [JsonPropertyName("selectedUserId")]
        public long SelectedUserId { get; set; }
    }

    public class AbsenceCreateDto
    {
        [JsonPropertyName("start_date")]
        public required DateOnly StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public required DateOnly EndDate { get; set; }

        [JsonPropertyName("reason")]
        public required string Reason { get; set; }
    }

    public class AbsenceResponse
    {
        [JsonPropertyName("successMessage")]
        public string SuccessMessage { get; set; } = string.Empty;

        [JsonPropertyName("absence")]
        public required AbsenceDto Absence { get; set; }
    }

    public class AbsenceDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("start_date")]
        public DateOnly StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateOnly EndDate { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
    }
}
