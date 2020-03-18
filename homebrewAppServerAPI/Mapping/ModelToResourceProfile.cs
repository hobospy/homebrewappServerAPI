﻿using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Resources;

namespace homebrewAppServerAPI.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Ingredient, IngredientResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()));

            CreateMap<WaterProfile, WaterProfileResource>();

            CreateMap<Recipe, RecipeResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()));

            CreateMap<Brew, BrewResource>();
        }
    }
}
