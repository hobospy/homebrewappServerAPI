using homebrewAppServerAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services.Communication
{
    public class SaveBrewResponse : BaseResponse
    {
        public Brew Brew { get; private set; }

        private SaveBrewResponse(bool success, string message, Brew brew) : base(success, message)
        {
            Brew = brew;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="brew">Saved brew.</param>
        /// <returns>Response.</returns>
        public SaveBrewResponse(Brew brew) : this(true, string.Empty, brew) { }

        /// <summary>
        /// Creates an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public SaveBrewResponse(string message) : this(false, message, null) { }
    }
}
