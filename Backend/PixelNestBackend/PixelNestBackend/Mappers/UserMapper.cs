using AutoMapper;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;

namespace PixelNestBackend.Mappers
{
    
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            
        }
    }
}
