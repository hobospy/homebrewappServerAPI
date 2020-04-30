﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace homebrewAppServerAPI.Domain.Models
{
    public class Recipe
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool Favourite { get; set; } = false;

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [JsonIgnore]
        public List<RecipeStep> Steps { get; set; }

        [Required]
        public ETypeOfBeer Type { get; set; }

        public IList<Ingredient> Ingredients { get; set; }
        public IList<Brew> Brews { get; set; }

        [ForeignKey("WaterProfileID")]
        public int WaterProfileID { get; set; }
        public WaterProfile WaterProfile { get; set; }
    }
}
