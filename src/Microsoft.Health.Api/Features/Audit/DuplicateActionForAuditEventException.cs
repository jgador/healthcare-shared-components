// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Microsoft.Health.Api.Features.Audit;

public class DuplicateActionForAuditEventException : Exception
{
    public DuplicateActionForAuditEventException()
    {
    }

    public DuplicateActionForAuditEventException(string message)
        : base(message)
    {
    }

    public DuplicateActionForAuditEventException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DuplicateActionForAuditEventException(string controllerName, string actionName)
        : base(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateActionForAuditEvent, controllerName, actionName))
    {
    }
}
