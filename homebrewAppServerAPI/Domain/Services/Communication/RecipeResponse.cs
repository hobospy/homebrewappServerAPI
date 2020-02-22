using homebrewAppServerAPI.Domain.Models;

namespace homebrewAppServerAPI.Domain.Services.Communication
{
    public class RecipeResponse : BaseResponse
    {
        public Recipe Recipe { get; private set; }

        private RecipeResponse(bool success, string message, Recipe recipe) : base(success, message)
        {
            Recipe = recipe;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="brew">Saved brew.</param>
        /// <returns>Response.</returns>
        public RecipeResponse(Recipe recipe) : this(true, string.Empty, recipe) { }

        /// <summary>
        /// Creates an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public RecipeResponse(string message) : this(false, message, null) { }
    }
}
