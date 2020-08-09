using homebrewAppServerAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services.Communication
{
    public class TastingNoteResponse : BaseResponse
    {
        public TastingNote TastingNote { get; private set; }

        private TastingNoteResponse(bool success, string message, TastingNote tastingNote) : base(success, message)
        {
            TastingNote = tastingNote;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="tastingNote">Saved tasting note.</param>
        /// <returns>Response.</returns>
        public TastingNoteResponse(TastingNote tastingNote) : this(true, string.Empty, tastingNote) { }

        /// <summary>
        /// Creates an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public TastingNoteResponse(string message) : this(false, message, null) { }
    }
}
