using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mapping
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Mapping between domain and dto
            //Not explicitly mentioned because Properties are same
            //Region is Model

            //for get, get by id
            CreateMap<Region, RegionDto>().ReverseMap();

            //for post
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();

            //UpdateRegionRequestDto name from controller
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

        }
    }
}
