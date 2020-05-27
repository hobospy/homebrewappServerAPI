using AutoMapper;
using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Resources;
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
                    return BadRequest(result.Message);
                }

                recipeResult = _mapper.Map<Recipe, RecipeResource>(result.Recipe);
            }
            catch (System.Exception ex)
            {
                var str = ex.Message;
            }

            return Ok(recipeResult);
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
