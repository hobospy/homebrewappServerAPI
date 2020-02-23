using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Extensions;
using homebrewAppServerAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace homebrewAppServerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:3000/", headers: "*", methods: "*")]
    public class BrewController : ControllerBase
    {
        private readonly IBrewService _brewService;
        private readonly IMapper _mapper;

        public BrewController(IBrewService brewService, IMapper mapper)
        {
            _brewService = brewService;

            _mapper = mapper;
        }

        // GET: Brew/Summary
        [HttpGet]
        [Route("Summary")]
        public async Task<IEnumerable<BrewResource>> GetAllBrewsAsync()
        {
            var brews = await _brewService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Brew>, IEnumerable<BrewResource>>(brews);

            return resources;
        }

        // POST Recipe
        [HttpPost]
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

        // PUT Recipe
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrewAsync(int id, [FromBody] SaveBrewResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var brew = _mapper.Map<SaveBrewResource, Brew>(resource);
            var result = await _brewService.UpdateAsync(id, brew);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var brewResource = _mapper.Map<Brew, BrewResource>(result.Brew);
            return Ok(brewResource);
        }

        // DELETE Recipe
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrewAsync(int id)
        {
            var result = await _brewService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var brewResource = _mapper.Map<Brew, BrewResource>(result.Brew);
            return Ok(brewResource);
        }
    }
}
