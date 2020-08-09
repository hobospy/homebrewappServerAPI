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
            CreateMap<TastingNote, TastingNoteResource>();

            CreateMap<TastingNote, NewTastingNoteResource>();

            CreateMap<Ingredient, IngredientResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()))
                .ForMember(src => src.Unit,
                       opt => opt.MapFrom(src => src.Unit.ToDescriptionString()));

            CreateMap<WaterProfileAddition, WaterProfileAdditionResource>()
                .ForMember(src => src.Unit,
                           opt => opt.MapFrom(src => src.Unit.ToDescriptionString()));

            CreateMap<WaterProfile, WaterProfileResource>();

            CreateMap<Timer, TimerResource>()
                .ForMember(dest => dest.Type,
                            opt => opt.MapFrom(src => src.Type.ToDescriptionString()));

            CreateMap<RecipeStep, RecipeStepResource>();

            CreateMap<Recipe, RecipeResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()))
                .ForMember(src => src.Steps,
                           opt => opt.MapFrom(src => src.Steps));

            CreateMap<Brew, BrewResource>()
                .ForMember(src => src.TastingNotes,
                           opt => opt.MapFrom(src => src.TastingNotes))
                .ForMember(src => src.Recipe,
                           opt => opt.MapFrom(src => src.Recipe));
        }
    }
}
