using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace homebrewAppServerAPI.Resources
{
    public class UpdateRecipeResource
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public double ExpectedABV { get; set; }

        [Required]
        public int WaterProfileID { get; set; }

        public List<IngredientResource> Ingredients { get; set; }

        public List<RecipeStepResource> Steps { get; set; }
    }
}
