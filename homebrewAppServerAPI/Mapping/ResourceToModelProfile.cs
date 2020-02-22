using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Resources;

namespace homebrewAppServerAPI.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveBrewResource, Brew>();
            CreateMap<SaveRecipeResource, Recipe>();
        }
    }
}
