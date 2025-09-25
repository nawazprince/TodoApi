using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedinUserRepository _loggedinUserRepository;

        public TodosController(ITodoRepository todoRepository, IMapper mapper, ILoggedinUserRepository loggedinUserRepository)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
            _loggedinUserRepository = loggedinUserRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<TodoDto>>>> Get(
        [FromBody] PageParams pageParams)
        {
            var loggedinUser = await _loggedinUserRepository.GetLoggedinUser();
            var result = await _todoRepository.GetAsync(loggedinUser.UserId, pageParams);

            return Ok(new ApiResponse<PagedResult<TodoDto>>
            {
                StatusCode = 200,
                Data = result
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<TodoDto>>> GetById(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    ErrorMessage = "Not found."
                });
            }

            TodoDto todoDto = _mapper.Map<TodoDto>(todo);

            return Ok(new ApiResponse<TodoDto>
            {
                StatusCode = 200,
                Data = todoDto
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TodoDto>>> Post([FromBody] TodoCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    ErrorMessage = errors
                });
            }
            var loggedinUser = await _loggedinUserRepository.GetLoggedinUser();

            var todo = _mapper.Map<Todo>(model);
            todo.UserId = loggedinUser.UserId;

            var created = await _todoRepository.AddAsync(todo);

            var response = new ApiResponse<TodoDto>
            {
                StatusCode = 201,
                Data = _mapper.Map<TodoDto>(created)
            };

            return CreatedAtAction(nameof(GetById),
                new { id = created.TodoId },
                response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<TodoDto>>> Update(int id, [FromBody] TodoUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    ErrorMessage = errors
                });
            }

            var existing = await _todoRepository.GetByIdAsync(id);

            if (existing == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    ErrorMessage = "Not found."
                });
            }

            // update fields
            _mapper.Map(dto, existing);

            var todo = await _todoRepository.UpdateAsync(existing);
            var todoDto = _mapper.Map<TodoDto>(todo);

            return Ok(new ApiResponse<TodoDto>
            {
                StatusCode = 200,
                Data = todoDto
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var existing = await _todoRepository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    ErrorMessage = "Todo not found."
                });
            }

            bool success = await _todoRepository.DeleteAsync(id);
            return success
                ? Ok(new ApiResponse<object> { StatusCode = 200, Data = new { Message = "Deleted successfully" } })
                : StatusCode(500, new ApiResponse<object>
                { StatusCode = 500, ErrorMessage = "Failed to delete." });
        }
    }
}
