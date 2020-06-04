using homebrewAppServerAPI.Domain.Models;

namespace homebrewAppServerAPI.Domain.Services.Communication
{
    public class RecipeStepResponse : BaseResponse
    {
        public RecipeStep RecipeStep { get; private set; }

        private RecipeStepResponse(bool success, string message, RecipeStep recipeStep) : base(success, message)
        {
            RecipeStep = recipeStep;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="brew">Saved brew.</param>
        /// <returns>Response.</returns>
        public RecipeStepResponse(RecipeStep recipeStep) : this(true, string.Empty, recipeStep) { }

        /// <summary>
        /// Creates an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public RecipeStepResponse(string message) : this(false, message, null) { }
    }
}
