// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Microsoft.Health.Api.Registration;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Use health checks (extension method). Register the response as json.
    /// </summary>
    /// <param name="app">Application builder instance.</param>
    /// <param name="healthCheckPathString">Health check path string.</param>
    public static void UseHealthChecksExtension(this IApplicationBuilder app, string healthCheckPathString)
    {
        app.UseHealthChecksExtension(healthCheckPathString, null);
    }

    /// <summary>
    /// Use health checks (extension method). Register the response as json.
    /// </summary>
    /// <param name="app">Application builder instance.</param>
    /// <param name="healthCheckPathString">Health check path string.</param>
    /// <param name="predicate">A predicate that is used to filter the set of health checks executed.</param>
    public static void UseHealthChecksExtension(this IApplicationBuilder app, string healthCheckPathString, Func<HealthCheckRegistration, bool> predicate)
    {
        app.UseHealthChecks(new PathString(healthCheckPathString), new HealthCheckOptions
        {
            Predicate = predicate,
            ResponseWriter = async (httpContext, healthReport) =>
            {
                var response = JsonConvert.SerializeObject(
                    new
                    {
                        overallStatus = healthReport.Status.ToString(),
                        details = healthReport.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = Enum.GetName(typeof(HealthStatus), entry.Value.Status),
                            description = entry.Value.Description,
                            data = entry.Value.Data,
                        }),
                    });

                httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                await httpContext.Response.WriteAsync(response).ConfigureAwait(false);
            },
        });
    }
}
