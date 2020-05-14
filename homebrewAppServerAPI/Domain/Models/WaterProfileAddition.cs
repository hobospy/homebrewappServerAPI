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
    public class WaterProfileAddition
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public EUnitOfMeasure Unit { get; set; }

        [ForeignKey("WaterProfileID")]
        public int WaterProfileID { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public WaterProfile WaterProfile { get; set; }
    }
}
