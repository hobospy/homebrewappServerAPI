using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeBrewAppController : ControllerBase
    {
        private readonly IBrewService _brewService;
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public HomeBrewAppController(IBrewService brewService, IRecipeService recipeService, IMapper mapper)
        {
            _brewService = brewService;
            _recipeService = recipeService;

            _mapper = mapper;
        }

        // GET: HomeBrewApp/BrewSummary
        [HttpGet]
        [Route("BrewSummary")]
        public async Task<IEnumerable<BrewResource>> GetAllBrewsAsync()
        {
            var brews = await _brewService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Brew>, IEnumerable<BrewResource>>(brews);
            
            return resources;
        }

        // GET: HomeBrewApp/RecipeSummary
        [HttpGet]
        [Route("RecipeSummary")]
        public async Task<IEnumerable<RecipeResource>> GetAllRecipesAsync()
        {
            var recipes = await _recipeService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeResource>>(recipes);

            return resources;
        }

        // POST HomeBrewApp
        [HttpPost]
        [Route("NewBrew")]
        public async Task<IActionResult> PostBrewAsync([FromBody] SaveBrewResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var brew = _mapper.Map<SaveBrewResource, Brew>(resource);
            var result = await _brewService.SaveAsync(brew);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var brewResource = _mapper.Map<Brew, BrewResource>(result.Brew);
            return Ok(brewResource);
        }
    }
}
