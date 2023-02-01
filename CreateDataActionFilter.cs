using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace RetroGamesApi
{
    public class CreateDataActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            /*
             * https://stackoverflow.com/questions/50689315/how-is-httpcontext-traceidentifier-generated-in-net-core
             */

            DataSentinel sentinel = new DataSentinel(context.HttpContext);
            sentinel.CreateDataFileIfNotExists();
        }
    }
}
