using System.Text.Json.Serialization;

namespace backendCRM.Models.DTOs
{
    public class DailySummary
    {
        [JsonPropertyName("time")]
        public string Time { get; set; } = "-";

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }

    public class UserSummary
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }

    public class WeeklySummaryResponse
    {
        public WeeklySummaryResponse()
        {
            DailySummary = new Dictionary<string, DailySummary>();
            WeeklyTotal = "00:00:00";
            Users = new List<UserSummary>();
        }

        [JsonPropertyName("dailySummary")]
        public Dictionary<string, DailySummary> DailySummary { get; set; }

        [JsonPropertyName("weeklyTotal")]
        public string WeeklyTotal { get; set; }

        [JsonPropertyName("weekOffset")]
        public int WeekOffset { get; set; }

        [JsonPropertyName("users")]
        public List<UserSummary> Users { get; set; }

        [JsonPropertyName("selectedUserId")]
        public long SelectedUserId { get; set; }
    }
}
