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
    public class TastingNoteController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITastingNoteService _tastingNoteService;
        private readonly IMapper _mapper;

        public TastingNoteController(ITastingNoteService tastingNoteService, IMapper mapper)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            _tastingNoteService = tastingNoteService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostTastingNoteAsync([FromBody] NewTastingNoteResource resource)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            if (!ModelState.IsValid)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Supplied data invalid: ${ModelState.GetErrorMessages()}");
            }

            var tastingNote = _mapper.Map<NewTastingNoteResource, TastingNote>(resource);
            var result = await _tastingNoteService.SaveAsync(tastingNote);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to save changes: ${result.Message}");
            }

            var NewTastingNoteResource = _mapper.Map<TastingNote, NewTastingNoteResource>(result.TastingNote);
            return Ok(NewTastingNoteResource);

            //return Ok();
        }

        // PUT TastingNote
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTastingNoteAsync(int id, [FromBody] NewTastingNoteResource resource)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            if (!ModelState.IsValid)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Supplied data invalid: ${ModelState.GetErrorMessages()}");
            }

            var tastingNote = _mapper.Map<NewTastingNoteResource, TastingNote>(resource);
            var result = await _tastingNoteService.UpdateAsync(id, tastingNote);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to save changes: ${result.Message}");
            }

            var tastingNoteResource = _mapper.Map<TastingNote, NewTastingNoteResource>(result.TastingNote);
            return Ok(tastingNoteResource);
        }

        // PATCH TastingNote
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTastingNoteAsync(int id, [FromBody] JsonPatchDocument<TastingNote> patch)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            if (!ModelState.IsValid)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Supplied data invalid: ${ModelState.GetErrorMessages()}");
            }

            var result = await _tastingNoteService.PatchAsync(id, patch);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch changes: ${result.Message}");
            }

            var tastingNoteResource = _mapper.Map<TastingNote, TastingNoteResource>(result.TastingNote);
            return Ok(tastingNoteResource);
        }

        // DELETE Tasting Note
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTastingNoteAsync(int id)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID: {id}");

            var result = await _tastingNoteService.DeleteAsync(id);

            if (!result.Success)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to delete tasting note with ID: {id} due to : ${result.Message}");
            }

            var tastingNoteResource = _mapper.Map<TastingNote, TastingNoteResource>(result.TastingNote);
            return Ok(tastingNoteResource);
        }
    }
}
