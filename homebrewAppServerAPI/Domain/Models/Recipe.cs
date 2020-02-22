using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public IEnumerable<Brew> Brews { get; set; }
    }
}
