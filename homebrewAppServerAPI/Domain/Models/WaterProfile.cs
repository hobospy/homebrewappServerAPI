using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class WaterProfile
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<WaterProfileAddition> Additions { get; set; }

        //[Required]
        //public double LacticAcid { get; set; } = 0.0;

        //[Required]
        //public double Gypsum { get; set; } = 0.0;

        //[Required]
        //public double CalciumChloride { get; set; } = 0.0;

        //[Required]
        //public double EpsomSalt { get; set; } = 0.0;

        //[Required]
        //public double NonIodizedSalt { get; set; } = 0.0;

        //[Required]
        //public double BakingSoda { get; set; } = 0.0;

        //[NotMapped]
        //public Dictionary<string, double> AdditionalAduncts { get; set; }

        //[JsonIgnore]
        //[IgnoreDataMember]
        //public IList<Recipe> Recipes { get; set; }
    }
}
