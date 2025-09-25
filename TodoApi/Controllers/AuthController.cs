using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using TodoApi.DTOs;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponse<SignUpResponseDto>>> Signup([FromBody] SignUpDto model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join("; ",
                        ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage));
                var errorResponse = new ApiResponse<object>
                {
                    StatusCode = 400,
                    ErrorMessage = errorMessage
                };
                return BadRequest(errorResponse);
            }


            if (await _userRepository.IsEmailExistAsync(model.Email))
            {
                var errorResponse = new ApiResponse<object>
                {
                    StatusCode = 409,
                    ErrorMessage = "Email already exists."
                };

                return Conflict(errorResponse);
            }

            User user = _mapper.Map<User>(model);
            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            user.PasswordSalt = hmac.Key;

            await _userRepository.AddAsync(user);

            SignUpResponseDto responseDto = _mapper.Map<SignUpResponseDto>(user);

            var succesResponse = new ApiResponse<SignUpResponseDto>
            {
                StatusCode = 201,
                Data = responseDto
            };
            return Created("", succesResponse);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<SignUpResponseDto>>> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join("; ",
                        ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage));
                var errorResponse = new ApiResponse<object>
                {
                    StatusCode = 400,
                    ErrorMessage = errorMessage
                };
                return BadRequest(errorResponse);
            }

            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user is null)
            {
                var errorResponse = new ApiResponse<object>
                {
                    StatusCode = 401,
                    ErrorMessage = "Invalid email or password."
                };

                return Unauthorized(errorResponse);
            }

            ////check password.
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

            if (!user.PasswordHash.SequenceEqual(computedHash))
            {
                var errorResponse = new ApiResponse<object>
                {
                    StatusCode = 401,
                    ErrorMessage = "Invalid email or password."
                };

                return Unauthorized(errorResponse);
            }

            var LoginResponseDto = new LoginResponseDto { Email = model.Email, Token = _tokenService.CreateToken(user) };

            return Ok(new ApiResponse<LoginResponseDto>
            {
                StatusCode = 200,
                Data = LoginResponseDto
            });
        }
    }
}
