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
using System.Web.Http.Cors;

namespace homebrewAppServerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BrewController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IBrewService _brewService;
        private readonly IMapper _mapper;

        public BrewController(IBrewService brewService, IMapper mapper)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            _brewService = brewService;
            _mapper = mapper;
        }

        // GET: Brew/Summary
        [HttpGet]
        [Route("Summary")]
        public async Task<IEnumerable<BrewResource>> GetAllBrewsAsync()
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            var brews = await _brewService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Brew>, IEnumerable<BrewResource>>(brews);
            return resources;
        }

        // Get Brew
        [HttpGet("{id}")]
        public async Task<BrewResource> GetBrewAsync(int id)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            var brewResponse = await _brewService.GetAsync(id);

            if (!brewResponse.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a brew with the ID: {id}");
            }

            var resource = _mapper.Map<Brew, BrewResource>(brewResponse.Brew);
            return resource;
        }

        // POST Brew
        [HttpPost]
        public async Task<IActionResult> PostBrewAsync([FromBody] SaveBrewResource resource)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            if (!ModelState.IsValid)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Supplied data invalid: ${ModelState.GetErrorMessages()}");
            }

            var brew = _mapper.Map<SaveBrewResource, Brew>(resource);
            var result = await _brewService.SaveAsync(brew);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to save changes: ${result.Message}");
            }

            var brewResource = _mapper.Map<Brew, BrewResource>(result.Brew);
            return Ok(brewResource);
        }

        // PUT Brew
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrewAsync(int id, [FromBody] SaveBrewResource resource)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            if (!ModelState.IsValid)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Supplied data invalid: ${ModelState.GetErrorMessages()}");
            }

            var brew = _mapper.Map<SaveBrewResource, Brew>(resource);
            var result = await _brewService.UpdateAsync(id, brew);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to save changes: ${result.Message}");
            }

            var brewResource = _mapper.Map<Brew, BrewResource>(result.Brew);
            return Ok(brewResource);
        }

        // PATCH Brew
        [HttpPatch("{id}")]
        //[Consumes(MediaTypes.Application.JsonPatch)]
        public async Task<IActionResult> PatchBrewAsync(int id, [FromBody] JsonPatchDocument<Brew> patch)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            if (!ModelState.IsValid)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Supplied data invalid: ${ModelState.GetErrorMessages()}");
            }

            var result = await _brewService.PatchAsync(id, patch);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch changes: ${result.Message}");
            }

            var brewResource = _mapper.Map<Brew, BrewResource>(result.Brew);
            return Ok(brewResource);
        }

        // DELETE Brew
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrewAsync(int id)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            var result = await _brewService.DeleteAsync(id);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to delete brew with ID: {id} due to : ${result.Message}");
            }

            var brewResource = _mapper.Map<Brew, BrewResource>(result.Brew);
            return Ok(brewResource);
        }
    }
}
