using AsterismWay.Data.Entities;
using AsterismWay.Data.Entities.CategoryEntity;
using AsterismWay.Data.Entities.FrequencyEntity;
using AsterismWay.DTOs;
using AutoMapper;

namespace AsterismWay
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<SelectedEvents, SelectedEventsDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Frequency, FrequencyDto>().ReverseMap();
        }
    }
}
