﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace AspNetCore.ApiVersioning.Configuration
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly OpenApiInfo _openApiInfo;
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, OpenApiInfo openApiInfo)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _openApiInfo = openApiInfo ?? throw new ArgumentNullException(nameof(openApiInfo));
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                if (_openApiInfo is null)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }
                else
                {
                    options.SwaggerDoc(description.GroupName, _openApiInfo);
                }
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "HookTrigger API",
                Version = description.ApiVersion.ToString(),
                Description = "HookTrigger API with Swagger, Swashbuckle, and API versioning.",
                Contact = new OpenApiContact() { Name = "Mihai Dumitru", Email = "devs@codescu.com" },
                TermsOfService = new Uri("https://opensource.org/licenses/MIT"),
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}