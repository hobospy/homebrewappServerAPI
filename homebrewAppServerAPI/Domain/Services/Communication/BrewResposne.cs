using homebrewAppServerAPI.Domain.Models;

namespace homebrewAppServerAPI.Domain.Services.Communication
{
    public class BrewResponse : BaseResponse
    {
        public Brew Brew { get; private set; }

        private BrewResponse(bool success, string message, Brew brew) : base(success, message)
        {
            Brew = brew;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="brew">Saved brew.</param>
        /// <returns>Response.</returns>
        public BrewResponse(Brew brew) : this(true, string.Empty, brew) { }

        /// <summary>
        /// Creates an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public BrewResponse(string message) : this(false, message, null) { }
    }
}
