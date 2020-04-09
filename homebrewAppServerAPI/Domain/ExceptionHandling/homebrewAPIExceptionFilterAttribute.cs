using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Net;

namespace homebrewAppServerAPI.Domain.ExceptionHandling
{
    public class homebrewAPIExceptionFilter : IExceptionFilter
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public homebrewAPIExceptionFilter(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            homebrewAPIError hbError = null;

            if (context.Exception is homebrewAPIException hbAPIException)
            {
                context.Exception = null;
                hbError = new homebrewAPIError(hbAPIException.Message);
                //hbError.Errors = hbAPIException.Errors;

                context.HttpContext.Response.StatusCode = (int)hbAPIException.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                hbError = new homebrewAPIError("Unauthorized Access");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (context.Exception is NullReferenceException)
            {
                hbError = new homebrewAPIError("Null reference");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
#if USE_SQLITE
                var message = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
#else
                var message = "Unknown error occurred";
                string stack = null;
#endif
                hbError = new homebrewAPIError(message);
                hbError.detail = stack;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Result = new JsonResult(hbError);
        }
    }
}