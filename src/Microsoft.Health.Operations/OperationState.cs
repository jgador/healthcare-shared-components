﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Microsoft.Health.Operations;

/// <summary>
/// Represents the state of a long-running operation.
/// </summary>
/// <typeparam name="T">The type used to denote the category of operation.</typeparam>
public class OperationState<T>
{
    /// <summary>
    /// Gets or sets the operation ID.
    /// </summary>
    /// <value>The unique ID that denotes a particular operation.</value>
    public Guid OperationId { get; init; }

    /// <summary>
    /// Gets or sets the category of the operation.
    /// </summary>
    public T? Type { get; init; }

    /// <summary>
    /// Gets or sets the date and time the operation was started.
    /// </summary>
    /// <value>The <see cref="DateTime"/> when the operation was started.</value>
    public DateTime CreatedTime { get; init; }

    /// <summary>
    /// Gets or sets the last date and time the state was updated.
    /// </summary>
    /// <value>The last <see cref="DateTime"/> when the operation state was updated.</value>
    public DateTime LastUpdatedTime { get; init; }

    /// <summary>
    /// Gets or sets the execution status of the operation.
    /// </summary>
    public OperationStatus Status { get; init; }

    /// <summary>
    /// Gets the percentage of work that has been completed by the operation.
    /// </summary>
    /// <value>An optional integer ranging from 0 to 100 if supported.</value>
    public int? PercentComplete { get; init; }

    /// <summary>
    /// Gets the optional collection of resources locations that the operation is creating or manipulating.
    /// </summary>
    /// <remarks>
    /// The set of resources may change until the <see cref="Status"/> indicates completion.
    /// </remarks>
    /// <value>A collection of resource IDs, or <see langword="null"/> if there are no targeted resources.</value>
    public IReadOnlyCollection<Uri>? Resources { get; init; }

    /// <summary>
    /// Gets the optional results of the operation.
    /// </summary>
    /// <remarks>
    /// The results may change over time as the operation continues execution.
    /// </remarks>
    /// <value>
    /// An object whose type depends on the <see cref="OperationState{T}.Type"/> if specified;
    /// otherwise <see langword="null"/>.
    /// </value>
    public object? Results { get; init; }
}
