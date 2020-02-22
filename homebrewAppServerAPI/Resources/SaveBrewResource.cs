using System.ComponentModel.DataAnnotations;

namespace homebrewAppServerAPI.Resources
{
    public class SaveBrewResource
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public double ABV { get; set; }
    }
}
