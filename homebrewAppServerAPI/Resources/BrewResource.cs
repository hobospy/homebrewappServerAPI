using System;
using System.Collections.Generic;

namespace homebrewAppServerAPI.Resources
{
    public class BrewResource
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime BrewDate { get; set; }
        public int BrewedState { get; set; }
        public string BrewingNotes { get; set; }
        public double ABV { get; set; }
        public List<TastingNoteResource> TastingNotes { get; set; }
        public double Rating { get; set; }

        public RecipeResource Recipe { get; set; }
    }
}
