using System;
using System.Collections.Generic;
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

        public EBrewedState BrewedState { get; set; } = EBrewedState.notBrewed;

        [StringLength(2000)]
        public string BrewingNotes { get; set; }

        public IList<TastingNote> TastingNotes { get; set; }

        [Required]
        public double ABV { get; set; }

        public double Rating { get; set; } = 0.0;

        public int RecipeID { get; set; }
        [ForeignKey("RecipeID")]
        public Recipe Recipe { get; set; }
    }
}