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
    public class RecipeStep
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Description { get; set; }
        public int Timer { get; set; }


        public int RecipeID { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("RecipeID")]
        public Recipe Recipe { get; set; }
    }
}
