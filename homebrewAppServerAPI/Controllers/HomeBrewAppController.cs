using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
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

        public HomeBrewAppController(IBrewService brewService)
        {
            _brewService = brewService;
        }

        // GET: HomeBrewApp
        [HttpGet]
        [Route("BrewSummary")]
        public async Task<IEnumerable<Brew>> GetAllAsync()
        {
            var brews = await _brewService.ListAsync();
            return brews;
        }
    }
}
