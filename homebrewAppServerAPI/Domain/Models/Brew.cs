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
        public string TastingNotes { get; set; }
        public double ABV { get; set; }
        public double Rating { get; set; }

        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}