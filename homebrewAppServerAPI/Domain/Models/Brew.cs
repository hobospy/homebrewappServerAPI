using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Brew
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime BrewDate { get; set; }

        [StringLength(1000)]
        public string TastingNotes { get; set; }


        public double ABV { get; set; }


        public bool BrewFavourite { get; set; } = false;

        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}