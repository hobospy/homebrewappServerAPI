using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public List<RecipeStep> Steps { get; set; }
        public double Rating { get; set; } = 0.0;
        public ETypeOfBeer Type { get; set; }

        public int WaterID { get; set; }
        public WaterProfile WaterProfile { get; set; }

        public IList<Ingredient> Ingredients { get; set; }
        public IList<Brew> Brews { get; set; }
    }
}
