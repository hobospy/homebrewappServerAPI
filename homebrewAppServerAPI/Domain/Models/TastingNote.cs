using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace homebrewAppServerAPI.Domain.Models
{
    public class TastingNote
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Note { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int BrewID { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("BrewID")]
        public Brew Brew { get; set; }
    }
}
