using Core.Dto;
using Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var result=await Result.FailAsync(ex.Message.ToStringList());
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }
        }
    }
}
