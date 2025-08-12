using Application.Dtos.Requests;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class UserRequestsProfile : Profile 
{
    public UserRequestsProfile()
    {
        CreateMap<AddUserProfileRequest, UserProfileEntity>()
            .ForMember(u => u.FirstName, opt => opt.MapFrom(u => u.Name));
        
        CreateMap<UpdateProfileRequest, UserProfileEntity>()
            .ForMember(u => u.FirstName, opt => opt.MapFrom(u => u.Name));
    }
}