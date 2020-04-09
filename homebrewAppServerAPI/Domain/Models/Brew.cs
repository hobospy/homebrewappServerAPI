using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Brew
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime BrewDate { get; set; }

        [StringLength(1000)]
        public string TastingNotes { get; set; }

        [Required]
        public double ABV { get; set; }


        public bool BrewFavourite { get; set; } = false;

        [ForeignKey("RecipeID")]
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}