using homebrewAppServerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace homebrewAppServerAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Brew> Brews { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<WaterProfile> WaterProfiles { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

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
                builder.Entity<WaterProfile>().HasMany(p => p.Recipes).WithOne(p => p.WaterProfile).HasForeignKey(p => p.WaterProfileID);

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
                builder.Entity<Recipe>().HasMany(p => p.Ingredients).WithOne(p => p.Recipe).HasForeignKey(p => p.RecipeID);

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

                builder.Entity<Brew>().HasData
                    (
                        new Brew
                        {
                            ID = 3000,
                            Name = "Brothers Kolsch Ripoff I",
                            BrewDate = new System.DateTime(2019, 11, 13),
                            ABV = 5.5,
                            TastingNotes = "Not a million miles away from the real thing!",
                            RecipeID = 2000,
                            Rating = 2.3
                        },
                        new Brew
                        {
                            ID = 3001,
                            Name = "Brothers Kolsch Ripoff II",
                            BrewDate = new System.DateTime(2019, 12, 24),
                            ABV = 5.08,
                            TastingNotes = "Yep, this one isn't great, there is an odd metalic taste associated with it.",
                            RecipeID = 2000,
                            Rating = 4.7
                        },
                        new Brew
                        {
                            ID = 3002,
                            Name = "Amarillo SMaSH I",
                            BrewDate = new System.DateTime(2020, 02, 07),
                            ABV = 4.7,
                            TastingNotes = "Cool, think I have found a house brew I can easily do and drink :)",
                            RecipeID = 2001
                        },
                        new Brew
                        {
                            ID = 3003,
                            Name = "Brothers Kolsch Ripoff III",
                            BrewDate = new System.DateTime(2020, 2, 8),
                            ABV = 4.7,
                            TastingNotes = "Nice clean flavour with a reasonably strong aroma.  Clarity has improved over the past week",
                            RecipeID = 2000,
                            Rating = 5.0
                        }
                    );

                builder.Entity<Ingredient>().ToTable("Ingredients");
                builder.Entity<Ingredient>().HasKey(p => p.ID);
                builder.Entity<Ingredient>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
                builder.Entity<Ingredient>().Property(p => p.Type).IsRequired();
                builder.Entity<Ingredient>().Property(p => p.Name).IsRequired().HasMaxLength(50);
                builder.Entity<Ingredient>().Property(p => p.Amount).IsRequired();

                builder.Entity<Ingredient>().HasData
                    (
                        new Ingredient { ID = 5000, Type = ETypeOfIngredient.Grains, Name = "Pale ale malt", Amount = 5.1, RecipeID = 2000 },
                        new Ingredient { ID = 5001, Type = ETypeOfIngredient.Grains, Name = "Chocolate malt", Amount = 0.1, RecipeID = 2000 },
                        new Ingredient { ID = 5002, Type = ETypeOfIngredient.Hops, Name = "Amarillo", Amount = 0.04, RecipeID = 2000 }
                    );
            }
        }
    }
}
