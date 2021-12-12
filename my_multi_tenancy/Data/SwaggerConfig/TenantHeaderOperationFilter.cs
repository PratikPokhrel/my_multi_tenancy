using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Core.Constants;

namespace my_multi_tenancy.Data.SwaggerConfig
{
    public class AddSwaggerHeaderParameter : IOperationFilter
    {

        /// <summary>
        /// Implementing interface
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = DefaultConstants.TenantId,
            //    In = ParameterLocation.Header,
            //    Required = false,
            //    AllowEmptyValue = false,
            //    Schema = new OpenApiSchema
            //    {
            //        Type = "String",
            //        Title = "Tenant Id"
            //    }
            //});
        }
    }
}
