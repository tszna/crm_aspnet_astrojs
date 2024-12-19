using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendCRM.Models
{
    [Table("time_sessions")]
    public class TimeSession
    {
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public TimeSpan? ElapsedTime { get; set; }

        public TimeSpan? FullElapsedTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
