using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AspNetCore.ApiVersioning
{
    public static class SwaggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerr(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
                //c.RoutePrefix = swaggerOptions.RoutePrefix;

                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    c.DocExpansion(DocExpansion.None);
                }
            });

            return app;
        }
    }
}