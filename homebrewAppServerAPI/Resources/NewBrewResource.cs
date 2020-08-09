using System;
using System.ComponentModel.DataAnnotations;

namespace homebrewAppServerAPI.Resources
{
    public class NewBrewResource
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int RecipeID { get; set; }

        [DataType(DataType.Date)]
        public DateTime BrewDate { get; set; }
    }
}
