using Application.Dtos.Requests;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class FilterRequestsProfile : Profile
{
    public FilterRequestsProfile()
    {
        CreateMap<UpdateFilterRequest, UserFilterEntity>();

    }
}