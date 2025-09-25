using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public class TodoDto
    {
        public int TodoId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoCreateDto
    {
        [Required(ErrorMessage = "Title is required."), MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; }
    }

    public class TodoUpdateDto
    {
        [Required(ErrorMessage = "Title is required."), MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
