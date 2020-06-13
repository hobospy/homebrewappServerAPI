using AutoMapper;
using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Resources;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            var recipes = await _recipeService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeResource>>(recipes);

            return resources;
        }

        // Get Recipe
        [HttpGet("{id}")]
        public async Task<RecipeResource> GetRecipeAsync(int id)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            var recipeResponse = await _recipeService.GetAsync(id);

            if (!recipeResponse.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a recipe with the ID: {id}");
            }

            var resource = _mapper.Map<Recipe, RecipeResource>(recipeResponse.Recipe);
            return resource;
        }

        // POST Recipe
        [HttpPost]
        public async Task<IActionResult> PostRecipeAsync([FromBody] UpdateRecipeResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var recipe = _mapper.Map<UpdateRecipeResource, Recipe>(resource);
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
        public async Task<IActionResult> PutRecipeAsync(int id, [FromBody] UpdateRecipeResource resource)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} for Recipe {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            RecipeResource recipeResult = null;

            try
            {
                var recipe = _mapper.Map<UpdateRecipeResource, Recipe>(resource);

                var result = await _recipeService.UpdateAsync(id, recipe);

                if (!result.Success)
                {
                    log.Debug($"Unable to update the recipe");
                    return BadRequest(result.Message);
                }

                log.Debug($"Mapping the updated recipe");
                recipeResult = _mapper.Map<Recipe, RecipeResource>(result.Recipe);
            }
            catch (System.Exception ex)
            {
                var str = ex.Message;
                log.Debug($"Error caught within {Helper.GetCurrentMethod()}, {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "No inner exception")}");
            }

            return Ok(recipeResult);
        }

        // PATCH Recipe
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchRecipeAsync(int id, [FromBody] JsonPatchDocument<Recipe> patch)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            if (!ModelState.IsValid)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Supplied data invalid: ${ModelState.GetErrorMessages()}");
            }

            var result = await _recipeService.PatchAsync(id, patch);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch changes: ${result.Message}");
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
