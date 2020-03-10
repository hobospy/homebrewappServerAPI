using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Ingredients
    {
        public Dictionary<string, double> Grains { get; set; }
        public Dictionary<string, double> Hops { get; set; }
        public Dictionary<string, double> Adjuncts { get; set; }
    }
}
