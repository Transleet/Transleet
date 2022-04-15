﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orleans.Concurrency;

namespace Transleet.Models
{
    [Immutable]
    public record EntryNotification(Guid Id, Entry? Item);
}
