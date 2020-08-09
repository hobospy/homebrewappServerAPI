using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;

namespace homebrewAppServerAPI.Resources
{
    public class RecipeStepResource
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public List<IngredientResource> Ingredients { get; set; }
        public TimerResource Timer { get; set; }
    }
}
