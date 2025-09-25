using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
