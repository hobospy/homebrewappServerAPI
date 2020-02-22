﻿using homebrewAppServerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace homebrewAppServerAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Brew> Brews { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Recipe>().ToTable("Recipes");
            builder.Entity<Recipe>().HasKey(p => p.ID);
            builder.Entity<Recipe>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Recipe>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Recipe>().Property(p => p.Type).IsRequired();
            builder.Entity<Recipe>().Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Entity<Recipe>().HasMany(p => p.Brews).WithOne(p => p.Recipe).HasForeignKey(p => p.RecipeID);

            builder.Entity<Recipe>().HasData
                (
                    new Recipe
                    {
                        ID = 1000,
                        Name = "Brothers Kolsch Ripoff",
                        Type = "Kolsch",
                        Description = "Kolsch is a unique style in that it is fermented with ale yeast, but then finished with lagering. The result is the best of both worlds: A light easy drinking pale that finishes crisp and clean. The most basic way to separate beers into different styles is by yeast. By far, the two largest groups are ales and lagers."
                    },
                    new Recipe
                    {
                        ID = 1001,
                        Name = "Amarillo SMaSH",
                        Type = "APA",
                        Description = "Characterized by floral, fruity, citrus-like, piney, resinous American hops, the American pale ale is a medium-bodied beer with low to medium caramel, and carries with it a toasted maltiness."
                    },
                    new Recipe
                    {
                        ID = 1002,
                        Name = "Raspberry Brown Porter",
                        Type = "Porter",
                        Description = "A moderate-strength brown beer with a restrained roasty character and bitterness. May have a range of roasted flavors, generally without burnt qualities, and often has a chocolate-caramel-malty profile."
                    },
                    new Recipe
                    {
                        ID = 1003,
                        Name = "Pepper Saison",
                        Type = "Saison",
                        Description = "Saison (French, \"season,\" French pronunciation: ​[sɛ. zɔ̃]) is a pale ale that is highly carbonated, fruity, spicy, and often bottle conditioned. It was historically brewed with low alcohol levels, but modern productions of the style have moderate to high levels of alcohol."
                    },
                    new Recipe
                    {
                        ID = 1004,
                        Name = "White Peach Sour",
                        Type = "Sour",
                        Description = "Sour beer is beer which has an intentionally acidic, tart, or sour taste. Traditional sour beer styles include Belgian lambics, gueuze, and Flanders red ale."
                    }
                );

            builder.Entity<Brew>().ToTable("Brews");
            builder.Entity<Brew>().HasKey(p => p.ID);
            builder.Entity<Brew>().Property(p => p.ID).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Brew>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Brew>().Property(p => p.ABV).IsRequired();

            builder.Entity<Brew>().HasData
                (
                    new Brew { ID = 1000, Name = "Brothers Kolsch Ripoff I", ABV = 5.5, RecipeID = 1000, Rating = 4.7 },
                    new Brew { ID = 1001, Name = "Brothers Kolsch Ripoff II", ABV = 5.2, RecipeID = 1000, Rating = 4.8 },
                    new Brew { ID = 1002, Name = "Amarillo SMaSH I", ABV = 4.7, RecipeID = 1001 }
                );
        }
    }
}
