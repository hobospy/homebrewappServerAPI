using AutoMapper;
using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace homebrewAppServerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IIngredientService _ingredientService;
        private readonly IMapper _mapper;

        public IngredientController(IIngredientService ingredientService, IMapper mapper)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            _ingredientService = ingredientService;
            _mapper = mapper;
        }

        // GET: Brew/Summary
        [HttpGet]
        [Route("Summary")]
        public async Task<IEnumerable<IngredientResource>> GetAllIngredientsAsync()
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            var ingredients = await _ingredientService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Ingredient>, IEnumerable<IngredientResource>>(ingredients);
            return resources;
        }

        // Get Brew
        [HttpGet("{id}")]
        public async Task<IngredientResource> GetIngredientAsync(int id)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            var ingredientResponse = await _ingredientService.GetAsync(id);

            if (!ingredientResponse.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a ingredient with the ID: {id}");
            }

            var resource = _mapper.Map<Ingredient, IngredientResource>(ingredientResponse.Ingredient);
            return resource;
        }
    }
}
