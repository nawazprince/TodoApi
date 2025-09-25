using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using TodoApi.DAL;
using TodoApi.DTOs;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TodoRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedResult<TodoDto>> GetAsync(int userId, PageParams pageParams)
        {
            IQueryable<Todo> query = _dbContext.Todos
                                                .Where(t => t.UserId == userId)
                                                .AsQueryable();

            //search
            if (!string.IsNullOrWhiteSpace(pageParams.SearchTerm))
            {
                string term = pageParams.SearchTerm.Trim();
                query = query.Where(t =>
                    t.Title.Contains(term) ||
                    (t.Description.Contains(term)));
            }

            //Sorting
            bool ascending = string.Equals(pageParams.SortDirection, "asc",
                                           StringComparison.OrdinalIgnoreCase);

            query = pageParams.SortColumn switch
            {
                1 => ascending ? query.OrderBy(t => t.Title)
                               : query.OrderByDescending(t => t.Title),
                2 => ascending ? query.OrderBy(t => t.Description)
                               : query.OrderByDescending(t => t.Description),
                3 => ascending ? query.OrderBy(t => t.IsCompleted)
                               : query.OrderByDescending(t => t.IsCompleted),
                _ => ascending ? query.OrderBy(t => t.TodoId)
                               : query.OrderByDescending(t => t.TodoId)
            };

            //count
            long totalRows = await query.CountAsync();

            //Paging
            query = query
                .Skip(pageParams.Offset)
                .Take(pageParams.PageSize);

            // --- Projection / Mapping ---
            var data = await query
                .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TodoDto>
            {
                TotalRows = totalRows,
                PageData = data
            };
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _dbContext.Todos.FirstOrDefaultAsync(t => t.TodoId == id);
        }

        public async Task<Todo> AddAsync(Todo todo)
        {
            await _dbContext.Todos.AddAsync(todo);
            await _dbContext.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> UpdateAsync(Todo todo)
        {
            _dbContext.Update(todo);
            await _dbContext.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _dbContext.Todos.FindAsync(id);
            if (result == null)
                return false;

            _dbContext.Todos.Remove(result);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
