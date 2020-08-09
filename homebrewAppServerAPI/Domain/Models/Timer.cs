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
    public class Timer
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public long Duration { get; set; }

        [Required]
        public ETypeOfDuration Type { get; set; }

        public int RecipeStepID { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("RecipeStepID")]
        public RecipeStep RecipeStep { get; set; }
    }
}
