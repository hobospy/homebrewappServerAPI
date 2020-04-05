using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.ExceptionHandling
{
    public class homebrewAPIError
    {
        public string message { get; set; }
        public bool isError { get; set; }
        public string detail { get; set; }
        //public ValidationErrorCollection errors { get; set; }

        public homebrewAPIError(string message)
        {
            this.message = message;
            isError = true;
        }

        public homebrewAPIError(ModelStateDictionary  modelState)
        {
            this.isError = true;

            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                message = "Please correct teh specified errors and try again.";
            }
        }
    }
}
