using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("users")]
    public class User
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(191)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(191)]
        public required string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        [Required]
        [MaxLength(191)]
        public required string Password { get; set; }

        [MaxLength(100)]
        public string? RememberToken { get; set; }

        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}
