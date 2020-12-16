using homebrewAppServerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace homebrewAppServerAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<TastingNote> TastingNotes { get; set; }
        public DbSet<Brew> Brews { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<WaterProfile> WaterProfiles { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Timer> Timers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder != null)
            {
                base.OnModelCreating(builder);

                builder.Entity<WaterProfile>().ToTable("WaterProfiles");
                builder.Entity<WaterProfile>().HasKey(p => p.ID);
                builder.Entity<WaterProfile>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<WaterProfile>().Property(p => p.Name).IsRequired().HasMaxLength(50);
                //builder.Entity<WaterProfile>().HasMany(p => p.Recipes).WithOne(p => p.WaterProfile).HasForeignKey(p => p.WaterProfileID);

                builder.Entity<WaterProfile>().HasData
                    (
                        new WaterProfile
                        {
                            ID = 1000,
                            Name = "APA focused"
                        },
                        new WaterProfile
                        {
                            ID = 1001,
                            Name = "Lager focused"
                        },
                        new WaterProfile
                        {
                            ID = 1002,
                            Name = "Stout focused"
                        }
                    );

                builder.Entity<Recipe>().ToTable("Recipes");
                builder.Entity<Recipe>().HasKey(p => p.ID);
                builder.Entity<Recipe>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<Recipe>().Property(p => p.Name).IsRequired().HasMaxLength(50);
                builder.Entity<Recipe>().Property(p => p.Type).IsRequired();
                builder.Entity<Recipe>().Property(p => p.Description).IsRequired().HasMaxLength(500);
                builder.Entity<Recipe>().HasMany(p => p.Brews).WithOne(p => p.Recipe).HasForeignKey(p => p.RecipeID);
                builder.Entity<Recipe>().HasMany(p => p.Steps).WithOne(p => p.Recipe).HasForeignKey(p => p.RecipeID);
                //builder.Entity<Recipe>().HasMany(p => p.Ingredients).WithOne(p => p.Recipe).HasForeignKey(p => p.RecipeID);

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

                builder.Entity<Brew>().ToTable("Brews");
                builder.Entity<Brew>().HasKey(p => p.ID);
                builder.Entity<Brew>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<Brew>().Property(p => p.Name).IsRequired().HasMaxLength(50);
                builder.Entity<Brew>().Property(p => p.BrewDate).IsRequired();
                builder.Entity<Brew>().Property(p => p.ABV).IsRequired();
                builder.Entity<Brew>().HasMany(p => p.TastingNotes).WithOne(p => p.Brew).HasForeignKey(p => p.BrewID);

                builder.Entity<Brew>().HasData
                    (
                        new Brew
                        {
                            ID = 3000,
                            Name = "Brothers Kolsch Ripoff I",
                            BrewDate = new System.DateTime(2019, 11, 13),
                            ABV = 5.5,
                            RecipeID = 2000,
                            Rating = 2.3,
                            BrewedState = EBrewedState.brewed,
                        },
                        new Brew
                        {
                            ID = 3001,
                            Name = "Brothers Kolsch Ripoff II",
                            BrewDate = new System.DateTime(2019, 12, 24),
                            ABV = 5.08,
                            RecipeID = 2000,
                            Rating = 4.7,
                            BrewedState = EBrewedState.brewing,
                        },
                        new Brew
                        {
                            ID = 3002,
                            Name = "Amarillo SMaSH I",
                            BrewDate = new System.DateTime(2020, 02, 07),
                            ABV = 4.7,
                            RecipeID = 2001,
                            BrewedState = EBrewedState.brewing,
                        },
                        new Brew
                        {
                            ID = 3003,
                            Name = "Brothers Kolsch Ripoff III",
                            BrewDate = new System.DateTime(2020, 2, 8),
                            ABV = 4.7,
                            RecipeID = 2000,
                            Rating = 5.0,
                            BrewedState = EBrewedState.notBrewed,
                        }
                    );

                builder.Entity<TastingNote>().ToTable("TastingNotes");
                builder.Entity<TastingNote>().HasKey(p => p.ID);
                builder.Entity<TastingNote>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<TastingNote>().Property(p => p.Note).IsRequired();
                builder.Entity<TastingNote>().Property(p => p.Date).IsRequired();
                builder.Entity<TastingNote>().HasData
                    (
                        new TastingNote { ID = 2000, Note = "Not a million miles away from the real thing!", Date = DateTime.Now, BrewID = 3000 },
                        new TastingNote { ID = 2001, Note = "Yep, this one isn't great, there is an odd metalic taste associated with it.", Date = new DateTime(2019, 6, 24), BrewID = 3001 },
                        new TastingNote { ID = 2002, Note = "Cool, think I have found a house brew I can easily do and drink :)", Date = new DateTime(2020, 3, 2), BrewID = 3002 },
                        new TastingNote { ID = 2004, Note = "The taste of this improves after a few weeks", Date = new DateTime(2020, 3, 27), BrewID = 3002 },
                        new TastingNote { ID = 2003, Note = "Nice clean flavour with a reasonably strong aroma.  Clarity has improved over the past week", Date = DateTime.Now, BrewID = 3003 }
                    );

                builder.Entity<Ingredient>().ToTable("Ingredients");
                builder.Entity<Ingredient>().HasKey(p => p.ID);
                builder.Entity<Ingredient>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<Ingredient>().Property(p => p.Type).IsRequired();
                builder.Entity<Ingredient>().Property(p => p.Name).IsRequired().HasMaxLength(50);
                builder.Entity<Ingredient>().Property(p => p.Amount).IsRequired();

                builder.Entity<Ingredient>().HasData
                    (
                        new Ingredient { ID = 5000, Type = ETypeOfIngredient.Grains, Name = "Pale ale malt", Amount = 5.1, RecipeStepID = 9001 },
                        new Ingredient { ID = 5001, Type = ETypeOfIngredient.Grains, Name = "Chocolate malt", Amount = 0.1, RecipeStepID = 9001 },
                        new Ingredient { ID = 5002, Type = ETypeOfIngredient.Hops, Name = "Amarillo", Amount = 0.04, RecipeStepID = 9000 }
                    );

                builder.Entity<Timer>().ToTable("Timers");
                builder.Entity<Timer>().HasKey(p => p.ID);
                builder.Entity<Timer>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<Timer>().Property(p => p.Duration).IsRequired();
                builder.Entity<Timer>().Property(p => p.Type).IsRequired();

                builder.Entity<Timer>().HasData (
                        new Timer { ID = 1, Duration = 3705, Type = ETypeOfDuration.independent, RecipeStepID=9000 },
                        new Timer { ID = 2, Duration = 2, Type = ETypeOfDuration.independent, RecipeStepID=9001 }
                    );

                builder.Entity<RecipeStep>().ToTable("RecipeSteps");
                builder.Entity<RecipeStep>().HasKey(p => p.ID);
                builder.Entity<RecipeStep>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<RecipeStep>().Property(p => p.Description).IsRequired().HasMaxLength(500);

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
            }
        }
    }
}
