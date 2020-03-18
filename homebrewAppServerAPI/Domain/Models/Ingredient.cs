using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Ingredient
    {
        public int ID { get; set; }
        public ETypeOfIngredient Type { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }

        public int RecipeID { get; set; }
        [JsonIgnore]
        public Recipe Recipe { get; set; }
        //public Dictionary<string, double> Grains { get; set; }
        //public Dictionary<string, double> Hops { get; set; }
        //public Dictionary<string, double> Adjuncts { get; set; }
    }
}
