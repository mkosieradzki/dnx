// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Dnx.Runtime;

namespace Microsoft.Dnx.Compilation
{
    public interface ILibraryExportProvider
    {
        LibraryExport GetLibraryExport(CompilationTarget target);
    }
}
