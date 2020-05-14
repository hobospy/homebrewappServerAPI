using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public enum ETypeOfBeer : short
    {
        [Description("Light lager")]
        LightLager,
        [Description("Pilsner")]
        Pilsner,
        [Description("European amber lager")]
        EurAmberLager,
        [Description("Dark lager")]
        DarkLager,
        [Description("Bock")]
        Bock,
        [Description("Light hybrid beer")]
        LightHybridBeer,
        [Description("Amber hybrid beer")]
        AmberHybridBeer,
        [Description("English pale ale")]
        EngPaleAle,
        [Description("Scottish and Irish ale")]
        ScotIrishAle,
        [Description("American ale")]
        AmericanAle,
        [Description("English brown ale")]
        EngBrownAle,
        [Description("Porter")]
        Porter,
        [Description("Stout")]
        Stout,
        [Description("India pale ale (IPA)")]
        IPA,
        [Description("German wheat and rye beer")]
        GerWheatRyeBeer,
        [Description("Belgian and French ale")]
        BelFrAle,
        [Description("Sour ale")]
        Sour,
        [Description("Belgian strong ale")]
        BelStrongAle,
        [Description("Strong ale")]
        StrongAle,
        [Description("Fruit beer")]
        FruitBeer,
        [Description(@"Spice/herb/vegetable beer")]
        SpiceHerbVegBeer,
        [Description("Smoke flavoured and wood-aged beer")]
        SmokeAgedBeer,
        [Description("Speciality beer")]
        Speciality,
        [Description("Kolsch and altbier")]
        KolschAlt
    }

    public enum ETypeOfIngredient
    {
        [Description("Grains")]
        Grains,
        [Description("Hops")]
        Hops,
        [Description("Adjuncts")]
        Adjuncts
    }

    public enum EUnitOfMeasure
    {
        [Description("kg")]
        kilo = 0,
        [Description("g")]
        gram,
        [Description("l")]
        litre = 100,
        [Description("ml")]
        millilitre
    }
}
