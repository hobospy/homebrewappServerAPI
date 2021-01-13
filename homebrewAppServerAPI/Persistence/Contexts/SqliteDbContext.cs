using homebrewAppServerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace homebrewAppServerAPI.Persistence.Contexts
{
    public class SqliteDbContext : DbContext
    {
        public virtual DbSet<Brew> Brews { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeStep> RecipeSteps { get; set; }
        public virtual DbSet<WaterProfile> WaterProfiles { get; set; }
        public virtual DbSet<WaterProfileAddition> WaterProfileAdditons { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<TastingNote> TastingNotes { get; set; }
        public virtual DbSet<Timer> Timers { get; set; }

        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=C:\temp\homebrew.db");

            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
#if SEED_SQLITE_DATA
            if (builder != null)
            {
                builder.Entity<WaterProfile>().HasData
                    (
                        new WaterProfile
                        {
                            ID = 1000,
                            Name = "APA focused",
                            Description = "Soft water profile used to accentuate the hop profile"
                        },
                        new WaterProfile
                        {
                            ID = 1001,
                            Name = "Lager focused",
                            Description = "Minimal mineral addition to give a clean flavour to the beer"
                        },
                        new WaterProfile
                        {
                            ID = 1002,
                            Name = "Stout focused",
                            Description = "Used to accentuate both the malt and hops of the beer"
                        }
                    );

                builder.Entity<WaterProfileAddition>().HasData
                    (
                        new WaterProfileAddition
                        {
                            ID = 9000,
                            Name = "Lactic acid",
                            Amount = 6,
                            Unit = EUnitOfMeasure.millilitre,
                            WaterProfileID = 1000
                        },
                        new WaterProfileAddition
                        {
                            ID = 9001,
                            Name = "Gypsum",
                            Amount = 3.6,
                            Unit = EUnitOfMeasure.gram,
                            WaterProfileID = 1000
                        },
                        new WaterProfileAddition
                        {
                            ID = 9002,
                            Name = "Bicarbonate soda",
                            Amount = 3.6,
                            Unit = EUnitOfMeasure.gram,
                            WaterProfileID = 1000
                        },
                        new WaterProfileAddition
                        {
                            ID = 9003,
                            Name = "Epsom salt",
                            Amount = 3.6,
                            Unit = EUnitOfMeasure.gram,
                            WaterProfileID = 1000
                        }
                    );

                builder.Entity<Recipe>().HasData
                    (
                        new Recipe
                        {
                            ID = 2000,
                            Name = "Brothers Kolsch Ripoff",
                            Type = ETypeOfBeer.KolschAlt,
                            Description = "Kolsch is a unique style in that it is fermented with ale yeast, but then finished with lagering. The result is the best of both worlds: A light easy drinking pale that finishes crisp and clean. The most basic way to separate beers into different styles is by yeast. By far, the two largest groups are ales and lagers.",
                            Favourite = true,
                            WaterProfileID = 1001
                        },
                        new Recipe
                        {
                            ID = 2001,
                            Name = "Amarillo SMaSH",
                            Type = ETypeOfBeer.AmericanAle,
                            Description = "Characterized by floral, fruity, citrus-like, piney, resinous American hops, the American pale ale is a medium-bodied beer with low to medium caramel, and carries with it a toasted maltiness.",
                            Favourite = true,
                            WaterProfileID = 1000
                        },
                        new Recipe
                        {
                            ID = 2002,
                            Name = "Raspberry Brown Porter",
                            Type = ETypeOfBeer.Porter,
                            Description = "A moderate-strength brown beer with a restrained roasty character and bitterness. May have a range of roasted flavors, generally without burnt qualities, and often has a chocolate-caramel-malty profile.",
                            WaterProfileID = 1002
                        },
                        new Recipe
                        {
                            ID = 2003,
                            Name = "Pepper Saison",
                            Type = ETypeOfBeer.BelStrongAle,
                            Description = "Saison (French, \"season,\" French pronunciation: ​[sɛ. zɔ̃]) is a pale ale that is highly carbonated, fruity, spicy, and often bottle conditioned. It was historically brewed with low alcohol levels, but modern productions of the style have moderate to high levels of alcohol.",
                            WaterProfileID = 1001
                        },
                        new Recipe
                        {
                            ID = 2004,
                            Name = "White Peach Sour",
                            Type = ETypeOfBeer.Sour,
                            Description = "Sour beer is beer which has an intentionally acidic, tart, or sour taste. Traditional sour beer styles include Belgian lambics, gueuze, and Flanders red ale.",
                            Favourite = true,
                            WaterProfileID = 1001
                        }
                    );

                builder.Entity<Timer>().HasData
                    (
                    new Timer { ID = 1, Duration = 3705, Type = ETypeOfDuration.independent, RecipeStepID=9000 },
                    new Timer { ID = 2, Duration = 2, Type = ETypeOfDuration.independent, RecipeStepID = 9001 }
                    );

                builder.Entity<RecipeStep>().HasData(
                        new RecipeStep
                        {
                            ID = 9000,
                            Description = "Add grain and mash in",
                            RecipeID = 2001
                        },
                        new RecipeStep
                        {
                            ID = 9001,
                            Description = "Mash out and get to boil",
                            RecipeID = 2001
                        }
                    );

                builder.Entity<Brew>().HasData
                    (
                        new Brew
                        {
                            ID = 3000,
                            Name = "Brothers Kolsch Ripoff I",
                            BrewDate = new DateTime(2019, 11, 13),
                            ABV = 5.5,
                            RecipeID = 2000,
                            Rating = 2.3,
                            BrewedState = EBrewedState.brewed,
                        },
                        new Brew
                        {
                            ID = 3001,
                            Name = "Brothers Kolsch Ripoff II",
                            BrewDate = new DateTime(2019, 12, 24),
                            ABV = 4.9,
                            RecipeID = 2000,
                            Rating = 4.7,
                            BrewedState = EBrewedState.brewing,
                        },
                        new Brew
                        {
                            ID = 3002,
                            Name = "Amarillo SMaSH I",
                            BrewDate = new DateTime(2020, 02, 07),
                            ABV = 4.7,
                            RecipeID = 2001,
                            BrewedState = EBrewedState.brewing,
                        },
                        new Brew
                        {
                            ID = 3003,
                            Name = "Brothers Kolsch Ripoff III",
                            BrewDate = new DateTime(2020, 02, 21),
                            ABV = 4.7,
                            RecipeID = 2000,
                            Rating = 5.0,
                            BrewedState = EBrewedState.notBrewed,
                        }
                    );

                builder.Entity<TastingNote>().HasData
                    (
                        new TastingNote { ID = 2000, Note = "Not a million miles away from the real thing!", Date = DateTime.Now, BrewID = 3000 },
                        new TastingNote { ID = 2001, Note = "Yep, this one isn't great, there is an odd metalic taste associated with it.", Date = new DateTime(2019, 6, 24), BrewID = 3001 },
                        new TastingNote { ID = 2002, Note = "Cool, think I have found a house brew I can easily do and drink :)", Date = new DateTime(2020, 3, 2), BrewID = 3002 },
                        new TastingNote { ID = 2004, Note = "The taste of this improves after a few weeks", Date = new DateTime(2020, 3, 27), BrewID = 3002 },
                        new TastingNote { ID = 2003, Note = "Nice clean flavour with a reasonably strong aroma.  Clarity has improved over the past week", Date = DateTime.Now, BrewID = 3003 }
                    );

                builder.Entity<Ingredient>().HasData
                    (
                        new Ingredient {
                            ID = 7001,
                            Name = "Pale ale",
                            Type = ETypeOfIngredient.Grains,
                            Amount = 5.5,
                            Unit = EUnitOfMeasure.kilo,
                            RecipeStepID = 9000
                        },
                        new Ingredient
                        {
                            ID = 7002,
                            Name = "Wheat malt",
                            Type = ETypeOfIngredient.Grains,
                            Amount = 0.3,
                            Unit = EUnitOfMeasure.kilo,
                            RecipeStepID = 9000
                        },
                        new Ingredient
                        {
                            ID = 7003,
                            Name = "Light crystal malt",
                            Type = ETypeOfIngredient.Grains,
                            Amount = 0.2,
                            Unit = EUnitOfMeasure.kilo,
                            RecipeStepID = 9000
                        },
                        new Ingredient
                        {
                            ID = 7004,
                            Name = "Amarillo",
                            Type = ETypeOfIngredient.Hops,
                            Amount = 65,
                            Unit = EUnitOfMeasure.gram,
                            RecipeStepID = 9001
                        }
                    );
            }
#endif
        }
    }
}
