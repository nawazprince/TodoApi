using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignUpDto, User>()
                 .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                 .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<User, SignUpResponseDto>();
            CreateMap<Todo, TodoDto>();
            CreateMap<TodoCreateDto, Todo>();
            CreateMap<TodoUpdateDto, Todo>();
        }
    }
}
