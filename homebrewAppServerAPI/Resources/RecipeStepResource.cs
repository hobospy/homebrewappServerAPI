using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Resources
{
    public class RecipeStepResource
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int Timer { get; set; }
    }
}
