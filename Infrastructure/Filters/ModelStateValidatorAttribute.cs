using System.Collections.Generic;
using System.Linq;
using Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Infrastructure.Filters
{
    public class ModelStateValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = context.ModelState.GetModelErrors();
                context.Result = new OkObjectResult(new Result(false, message));
            }
        }
    }

    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// gets error list
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static List<string> GetModelErrors(this ModelStateDictionary dict)
        {
            var modelErrors = dict.Keys.SelectMany(k => dict[k].Errors)
                .Select(m => m.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();
            return modelErrors;
        }
    }
    }
