using AutoMapper;
using WebApiCore.Models.DTOs;
using WebApiCore.Models;

namespace WebApiCore.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalParkDto, NationalPark>().ReverseMap();
            CreateMap<TrailDto, Trail>().ReverseMap();
        }
    }
}
