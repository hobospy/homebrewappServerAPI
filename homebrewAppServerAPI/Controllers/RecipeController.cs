using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeService recipeService, IMapper mapper)
        {
            _recipeService = recipeService;

            _mapper = mapper;
        }

        // GET: Recipe/Summary
        [HttpGet]
        [Route("Summary")]
        public async Task<IEnumerable<RecipeResource>> GetAllRecipesAsync()
        {
            var recipes = await _recipeService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeResource>>(recipes);

            return resources;
        }

        // POST Recipe
        [HttpPost]
        public async Task<IActionResult> PostRecipeAsync([FromBody] SaveRecipeResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var recipe = _mapper.Map<SaveRecipeResource, Recipe>(resource);
            var result = await _recipeService.SaveAsync(recipe);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var recipeResource = _mapper.Map<Recipe, RecipeResource>(result.Recipe);
            return Ok(recipeResource);
        }

        // PUT Recipe
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeAsync(int id, [FromBody] SaveRecipeResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var recipe = _mapper.Map<SaveRecipeResource, Recipe>(resource);
            var result = await _recipeService.UpdateAsync(id, recipe);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var recipeResource = _mapper.Map<Recipe, RecipeResource>(result.Recipe);
            return Ok(recipeResource);
        }

        // DELETE Recipe
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeAsync(int id)
        {
            var result = await _recipeService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var recipeResource = _mapper.Map<Recipe, RecipeResource>(result.Recipe);
            return Ok(recipeResource);
        }
    }
}
