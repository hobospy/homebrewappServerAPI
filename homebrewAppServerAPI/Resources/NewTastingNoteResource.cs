using System;
using System.ComponentModel.DataAnnotations;

namespace homebrewAppServerAPI.Resources
{
    public class NewTastingNoteResource
    {
        [Required]
        public string Note { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int BrewID { get; set; }
    }
}
