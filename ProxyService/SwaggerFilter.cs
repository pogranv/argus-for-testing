using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class SwaggerFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        
        if (context.ApiDescription.RelativePath.Contains("{*path}"))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "path",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new OpenApiSchema { Type = "string" }
            });

            if (context.ApiDescription.HttpMethod == "POST" || 
                context.ApiDescription.HttpMethod == "PUT")
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                AdditionalPropertiesAllowed = true
                            }
                        }
                    }
                };
            }
        }
    }
}