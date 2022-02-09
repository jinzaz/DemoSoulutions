using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace QuartzDemo.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private static ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate rd,ILogger logger)
        {
            _requestDelegate = rd;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context,ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context,Exception e)
        {
            if (e == null) return;
            _logger.LogError(e.Source,e.GetBaseException().ToString());
            await WriteExceptionAsync(context, e).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context,Exception e)
        {
            if (e is UnauthorizedAccessException)
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else if (e is Exception)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.Response.ContentType = "application/json";

            //await context.Response.WriteAsync(
            //    JsonConvert.SerializeObject(
            //        ReturnVerify.ReturnError("", e.GetBaseException().Message))).ConfigureAwait(false);
        }
    }
}
