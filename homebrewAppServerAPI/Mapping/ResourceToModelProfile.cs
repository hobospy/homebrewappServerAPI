using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Resources;
using System;
using System.Linq;

namespace homebrewAppServerAPI.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {

            CreateMap<TastingNoteResource, TastingNote>();

            CreateMap<NewTastingNoteResource, TastingNote>();

            CreateMap<IngredientResource, Ingredient>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(source => Enum.GetValues(typeof(ETypeOfIngredient))
                                                            .Cast<ETypeOfIngredient>()
                                                            .FirstOrDefault(v => string.Equals(v.ToDescriptionString(), source.Type, StringComparison.OrdinalIgnoreCase))))
                .ForMember(src => src.Unit,
                           opt => opt.MapFrom(source => Enum.GetValues(typeof(EUnitOfMeasure))
                                                            .Cast<EUnitOfMeasure>()
                                                            .FirstOrDefault(v => string.Equals(v.ToDescriptionString(), source.Unit, StringComparison.OrdinalIgnoreCase))));

            CreateMap<WaterProfileAdditionResource, WaterProfileAddition>()
                .ForMember(src => src.Unit,
                           opt => opt.MapFrom(source => Enum.GetValues(typeof(EUnitOfMeasure))
                                                            .Cast<EUnitOfMeasure>()
                                                            .FirstOrDefault(v => string.Equals(v.ToDescriptionString(), source.Unit, StringComparison.OrdinalIgnoreCase))));
            
            CreateMap<WaterProfileResource, WaterProfile>();

            CreateMap<TimerResource, Timer>()
                .ForMember(dest => dest.Type,
                           opt => opt.MapFrom(source => Enum.GetValues(typeof(ETypeOfDuration))
                                                            .Cast<ETypeOfDuration>()
                                                            .FirstOrDefault(v => string.Equals(v.ToDescriptionString(), source.Type, StringComparison.OrdinalIgnoreCase))));

            CreateMap<RecipeStepResource, RecipeStep>();

            CreateMap<RecipeResource, Recipe>();

            CreateMap<UpdateRecipeResource, Recipe>()
                .ForMember(r => r.Type,
                           op => op.MapFrom(source => Enum.GetValues(typeof(ETypeOfBeer))
                                                          .Cast<ETypeOfBeer>()
                                                          .FirstOrDefault(v => string.Equals(v.ToDescriptionString(), source.Type, StringComparison.OrdinalIgnoreCase))))
                .ForMember(src => src.Steps,
                           opt => opt.MapFrom(src => src.Steps));

            CreateMap<BrewResource, Brew>();

            CreateMap<NewBrewResource, Brew>();

            CreateMap<NewTastingNoteResource, TastingNote>();
        }
    }
}
