using System.Text.Json.Serialization;

namespace backend.Models.DTOs
{
    public class CurrentSessionOut
    {
        [JsonPropertyName("start_time")]
        public string? StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public string? EndTime { get; set; }

        [JsonPropertyName("elapsed_time")]
        public string ElapsedTime { get; set; } = "00:00:00";

        [JsonPropertyName("count_time")]
        public int CountTime { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
}
