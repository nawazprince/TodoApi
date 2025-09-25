using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoRepository
    {
        Task<Todo> AddAsync(Todo todo);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<TodoDto>> GetAsync(int userId, PageParams pageParams);
        Task<Todo> GetByIdAsync(int id);
        Task<Todo> UpdateAsync(Todo todo);
    }
}