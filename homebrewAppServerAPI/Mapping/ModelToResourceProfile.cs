using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Resources;
using System.Collections.Generic;

namespace homebrewAppServerAPI.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Ingredient, IngredientResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()))
                .ForMember(src => src.Unit,
                           opt => opt.MapFrom(src => src.Unit.ToDescriptionString()));

            CreateMap<WaterProfile, WaterProfileResource>();

            CreateMap<Recipe, RecipeResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()))
                .ForMember(src => src.Ingredients,
                           opt => opt.MapFrom(src => src.Ingredients));

            CreateMap<Brew, BrewResource>();
        }
    }
}
