using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Ingredient
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public ETypeOfIngredient Type { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public EUnitOfMeasure Unit { get; set; }

        [ForeignKey("RecipeID")]
        public int RecipeID { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Recipe Recipe { get; set; }
        //public Dictionary<string, double> Grains { get; set; }
        //public Dictionary<string, double> Hops { get; set; }
        //public Dictionary<string, double> Adjuncts { get; set; }
    }
}
