using AutoMapper;
using GeocacheAPI.ViewModels;

namespace GeocacheAPI.Data
{
    public class GeocacheMappingProfile : Profile
    {
        public GeocacheMappingProfile()
        {
            CreateMap<Item, ItemViewModel>()
                .ReverseMap();

            CreateMap<Geocache, GeocacheViewModel>()
                .ReverseMap();

            CreateMap<Location, LocationViewModel>()
                .ReverseMap();
        }
        
    }
}
