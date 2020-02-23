using System.Collections.Generic;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ETypeOfBeer Type { get; set; }
        public string Description { get; set; }

        public IList<Brew> Brews { get; set; }
    }
}
