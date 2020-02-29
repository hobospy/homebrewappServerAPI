using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Brew
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime BrewDate { get; set; }
        public string TastingNotes { get; set; }
        public double ABV { get; set; }
        public bool BrewFavourite { get; set; } = false;

        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}