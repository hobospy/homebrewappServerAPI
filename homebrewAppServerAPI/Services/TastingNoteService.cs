using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Domain.Services.Communication;
using homebrewAppServerAPI.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Services
{
    public class TastingNoteService: ITastingNoteService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITastingNoteRepository _tastingNoteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TastingNoteService(ITastingNoteRepository tastingNoteRepository, IUnitOfWork unitOfWork)
        {
            this._tastingNoteRepository = tastingNoteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TastingNoteResponse> SaveAsync(TastingNote tastingNote)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with tasting note {tastingNote.Note}");

            try
            {
                var newTastingNote = new TastingNote();
                newTastingNote.Note = tastingNote.Note;
                newTastingNote.Date = tastingNote.Date;
                newTastingNote.BrewID = tastingNote.BrewID;

                var storedTastingNote = await _tastingNoteRepository.AddAsync(newTastingNote);

                if (storedTastingNote != null)
                {
                    storedTastingNote = await _tastingNoteRepository.FindByIdAsync(storedTastingNote.ID);
                }

                return new TastingNoteResponse(storedTastingNote);
            }
            catch (Exception ex)
            {
                var msg = "An error occurred when saving the tasting note";
                msg += tastingNote != null ? $" ({tastingNote.Note}: {ex.Message}" : $": {ex.Message}";
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", msg);
            }
        }

        public async Task<TastingNoteResponse> UpdateAsync(int id, TastingNote tastingNote)
        {
            var existingTastingNote = await _tastingNoteRepository.FindByIdAsync(id);

            if (existingTastingNote == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to update tasting note, can't find a tasting note with ID: {id}");
            }

            existingTastingNote.Note = tastingNote.Note;
            existingTastingNote.Date = tastingNote.Date;

            try
            {
                _tastingNoteRepository.Update(existingTastingNote);
                await _unitOfWork.CompleteAsync();

                return new TastingNoteResponse(existingTastingNote);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when updating the tasting note ({existingTastingNote.Note}): {ex.Message}");
            }
        }

        public async Task<TastingNoteResponse> PatchAsync(int id, JsonPatchDocument<TastingNote> patch)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with id {id}");

            if (patch == null)
            {
                var errorMsg = "Unable to patch tasting note, patch information is null";

                log.Debug(errorMsg);
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch tasting note, patch information is null");
            }

            var existingTastingNote = await _tastingNoteRepository.FindByIdAsync(id);

            if (existingTastingNote == null)
            {
                log.Debug($"Unable to find a tasting note with id {id}");
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch tasting note, can't find a tasting note with ID: {id}");
            }

            try
            {
                log.Debug($"Patching tasting note {existingTastingNote.Note} [{existingTastingNote.ID}]");
                patch.ApplyTo(existingTastingNote);
                var updatedTastingNote = await _tastingNoteRepository.Update(existingTastingNote);

                return new TastingNoteResponse(updatedTastingNote);
            }
            catch (Exception ex)
            {
                log.Debug($"Error caught when updating the tasting note {existingTastingNote.Note}[{existingTastingNote.ID}] {Helper.GetCurrentMethod()}," +
                    $" {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "No inner exception")}");

                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when patching the tasting note ({existingTastingNote.Note}): {ex.Message}");
            }
        }

        public async Task<TastingNoteResponse> DeleteAsync(int id)
        {
            var existingTastingNote = await _tastingNoteRepository.FindByIdAsync(id);

            if (existingTastingNote == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to delete tasting note, can't find a tasting note with ID: {id}");
            }

            try
            {
                _tastingNoteRepository.Remove(existingTastingNote);
                await _unitOfWork.CompleteAsync();

                return new TastingNoteResponse(existingTastingNote);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when deleting the tasting note: {ex.Message}");
            }
        }
    }
}
