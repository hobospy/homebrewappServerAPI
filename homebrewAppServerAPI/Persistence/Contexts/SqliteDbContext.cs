using homebrewAppServerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace homebrewAppServerAPI.Persistence.Contexts
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Brew> Brews { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<WaterProfile> WaterProfiles { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=homebrew.db");

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

                builder.Entity<Recipe>().HasData
                    (
                        new Recipe
                        {
                            ID = 2000,
                            Name = "Brothers Kolsch Ripoff",
                            Type = ETypeOfBeer.KolschAlt,
                            Description = "Kolsch is a unique style in that it is fermented with ale yeast, but then finished with lagering. The result is the best of both worlds: A light easy drinking pale that finishes crisp and clean. The most basic way to separate beers into different styles is by yeast. By far, the two largest groups are ales and lagers.",
                            Rating = 4.2,
                            WaterProfileID = 1001
                        },
                        new Recipe
                        {
                            ID = 2001,
                            Name = "Amarillo SMaSH",
                            Type = ETypeOfBeer.AmericanAle,
                            Description = "Characterized by floral, fruity, citrus-like, piney, resinous American hops, the American pale ale is a medium-bodied beer with low to medium caramel, and carries with it a toasted maltiness.",
                            Rating = 4.9,
                            WaterProfileID = 1000
                        },
                        new Recipe
                        {
                            ID = 2002,
                            Name = "Raspberry Brown Porter",
                            Type = ETypeOfBeer.Porter,
                            Description = "A moderate-strength brown beer with a restrained roasty character and bitterness. May have a range of roasted flavors, generally without burnt qualities, and often has a chocolate-caramel-malty profile.",
                            Rating = 0.8,
                            WaterProfileID = 1002
                        },
                        new Recipe
                        {
                            ID = 2003,
                            Name = "Pepper Saison",
                            Type = ETypeOfBeer.BelStrongAle,
                            Description = "Saison (French, \"season,\" French pronunciation: ​[sɛ. zɔ̃]) is a pale ale that is highly carbonated, fruity, spicy, and often bottle conditioned. It was historically brewed with low alcohol levels, but modern productions of the style have moderate to high levels of alcohol.",
                            Rating = 3.1,
                            WaterProfileID = 1001
                        },
                        new Recipe
                        {
                            ID = 2004,
                            Name = "White Peach Sour",
                            Type = ETypeOfBeer.Sour,
                            Description = "Sour beer is beer which has an intentionally acidic, tart, or sour taste. Traditional sour beer styles include Belgian lambics, gueuze, and Flanders red ale.",
                            Rating = 5.0,
                            WaterProfileID = 1001
                        }
                    );

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
                            BrewFavourite = true
                        },
                        new Brew
                        {
                            ID = 3001,
                            Name = "Brothers Kolsch Ripoff II",
                            BrewDate = new System.DateTime(2019, 12, 24),
                            ABV = 5.2,
                            TastingNotes = "Yep, this one isn't great, there is an odd metalic taste associated with it.",
                            RecipeID = 2000,
                            BrewFavourite = false
                        },
                        new Brew
                        {
                            ID = 3002,
                            Name = "Amarillo SMaSH I",
                            BrewDate = new System.DateTime(2020, 02, 07),
                            ABV = 4.7,
                            TastingNotes = "Cool, think I have found a house brew I can easily do and drink :)",
                            RecipeID = 2001
                        }
                    );
            }
#endif
        }
    }
}
