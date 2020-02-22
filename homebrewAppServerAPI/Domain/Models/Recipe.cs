using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Recipe
    {

        //TODO: Make Type an enum (based off of the brewing guidelines), guess we should also include Other as an option
        // then update the ModelToResourceProfile file to ensure we get the descriptive value
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public IEnumerable<Brew> Brews { get; set; }
    }
}
