using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class WaterProfile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double LacticAcid { get; set; } = 0.0;
        public double Gypsum { get; set; } = 0.0;
        public double CalciumChloride { get; set; } = 0.0;
        public double EpsomSalt { get; set; } = 0.0;
        public double NonIodizedSalt { get; set; } = 0.0;
        public double BakingSoda { get; set; } = 0.0;
        [NotMapped]
        public Dictionary<string, double> AdditionalAduncts { get; set; }
    }
}
