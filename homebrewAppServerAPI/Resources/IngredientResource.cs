using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Resources
{
    public class IngredientResource
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}
