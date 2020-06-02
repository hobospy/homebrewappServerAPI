using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;

namespace homebrewAppServerAPI.Resources
{
    public class RecipeResource
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Favourite { get; set; }
        public double ExpectedABV { get; set; }
        public WaterProfileResource WaterProfile { get; set; }

        public List<IngredientResource> Ingredients { get; set; }


        public List<RecipeStep> Steps { get; set; }
    }
}
