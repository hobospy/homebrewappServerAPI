using homebrewAppServerAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services.Communication
{
    public class WaterProfileResponse : BaseResponse
    {
        public WaterProfile WaterProfile { get; private set; }

        public WaterProfileResponse(bool success, string message, WaterProfile waterProfile) : base(success, message)
        {
            WaterProfile = waterProfile;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="waterProfile">Saved water profile.</param>
        /// <returns>Response.</returns>
        public WaterProfileResponse(WaterProfile waterProfile) : this(true, string.Empty, waterProfile) { }

        /// <summary>
        /// Creates an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public WaterProfileResponse(string message) : this(false, message, null) { }
    }
}
