using homebrewAppServerAPI.Domain.Models;

namespace homebrewAppServerAPI.Domain.Services.Communication
{
    public class IngredientResponse : BaseResponse
    {
        public Ingredient Ingredient { get; private set; }

        private IngredientResponse(bool success, string message, Ingredient ingredient) : base(success, message)
        {
            Ingredient = ingredient;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="brew">Saved brew.</param>
        /// <returns>Response.</returns>
        public IngredientResponse(Ingredient ingredient) : this(true, string.Empty, ingredient) { }

        /// <summary>
        /// Creates an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public IngredientResponse(string message) : this(false, message, null) { }
    }
}
