﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using EnsureThat;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Health.Blob.Configs;
using Microsoft.Health.Blob.Features.Storage;

namespace Microsoft.Health.Blob.Features.Health;

/// <summary>
/// Performs health checks on blob storage.
/// </summary>
public class BlobHealthCheck : IHealthCheck
{
    private readonly BlobServiceClient _client;
    private readonly BlobContainerConfiguration _blobContainerConfiguration;
    private readonly IBlobClientTestProvider _testProvider;
    private readonly ILogger<BlobHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlobHealthCheck"/> class.
    /// </summary>
    /// <param name="client">The cloud blob client factory.</param>
    /// <param name="namedBlobContainerConfigurationAccessor">The IOptions accessor to get a named container configuration version.</param>
    /// <param name="containerConfigurationName">Name to get corresponding container configuration.</param>
    /// <param name="testProvider">The test provider.</param>
    /// <param name="logger">The logger.</param>
    public BlobHealthCheck(
        BlobServiceClient client,
        IOptionsSnapshot<BlobContainerConfiguration> namedBlobContainerConfigurationAccessor,
        string containerConfigurationName,
        IBlobClientTestProvider testProvider,
        ILogger<BlobHealthCheck> logger)
    {
        EnsureArg.IsNotNull(client, nameof(client));
        EnsureArg.IsNotNull(namedBlobContainerConfigurationAccessor, nameof(namedBlobContainerConfigurationAccessor));
        EnsureArg.IsNotNullOrWhiteSpace(containerConfigurationName, nameof(containerConfigurationName));
        EnsureArg.IsNotNull(testProvider, nameof(testProvider));
        EnsureArg.IsNotNull(logger, nameof(logger));

        _client = client;
        _blobContainerConfiguration = namedBlobContainerConfigurationAccessor.Get(containerConfigurationName);
        _testProvider = testProvider;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Performing health check.");
        await _testProvider.PerformTestAsync(_client, _blobContainerConfiguration, cancellationToken).ConfigureAwait(false);
        return HealthCheckResult.Healthy("Successfully connected.");
    }
}
