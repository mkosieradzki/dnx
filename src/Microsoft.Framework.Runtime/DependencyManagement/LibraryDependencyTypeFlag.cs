// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Framework.Runtime
{
    [Flags]
    public enum LibraryDependencyTypeFlag
    {
        None = 0,
        MainReference = 1,
        MainSource = 1 << 2,
        MainExport = 1 << 3,
        PreprocessReference = 1 << 4,
        RuntimeComponent = 1 << 5,
        DevComponent = 1 << 6,
        PreprocessComponent = 1 << 7,
        BecomesNupkgDependency = 1 << 8
    }
}
