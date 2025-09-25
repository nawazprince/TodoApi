using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class Todo
    {
        [Key]
        public int TodoId { get; set; }

        [Required(ErrorMessage = "Title is required."), MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        // Foreign Key
        [Required]
        public int UserId { get; set; }
        // Navigation property
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
