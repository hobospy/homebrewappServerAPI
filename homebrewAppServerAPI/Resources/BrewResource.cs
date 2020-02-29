using System;

namespace homebrewAppServerAPI.Resources
{
    public class BrewResource
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime BrewDate { get; set; }
        public double ABV { get; set; }
        public string TastingNotes { get; set; }
        public bool BrewFavourite { get; set; }

        public RecipeResource Recipe { get; set; }
    }
}
