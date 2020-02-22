using System.ComponentModel.DataAnnotations;

namespace homebrewAppServerAPI.Resources
{
    public class SaveRecipeResource
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}
