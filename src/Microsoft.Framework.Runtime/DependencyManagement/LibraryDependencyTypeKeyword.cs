// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Framework.Runtime
{
    public class LibraryDependencyTypeKeyword
    {
        private static readonly List<LibraryDependencyTypeKeyword> _rareKeywords = new List<LibraryDependencyTypeKeyword>(16);

        public static readonly LibraryDependencyTypeKeyword Default = new LibraryDependencyTypeKeyword(
                "default",
                flagsToAdd: LibraryDependencyTypeFlag.MainReference |
                            LibraryDependencyTypeFlag.MainSource |
                            LibraryDependencyTypeFlag.MainExport |
                            LibraryDependencyTypeFlag.RuntimeComponent |
                            LibraryDependencyTypeFlag.BecomesNupkgDependency,
                flagsToRemove: LibraryDependencyTypeFlag.None);

        public static readonly LibraryDependencyTypeKeyword Private = new LibraryDependencyTypeKeyword(
                "private",
                flagsToAdd: LibraryDependencyTypeFlag.MainReference |
                            LibraryDependencyTypeFlag.MainSource |
                            LibraryDependencyTypeFlag.RuntimeComponent |
                            LibraryDependencyTypeFlag.BecomesNupkgDependency,
                flagsToRemove: LibraryDependencyTypeFlag.None);

        public static readonly LibraryDependencyTypeKeyword Dev = new LibraryDependencyTypeKeyword(
                "dev",
                flagsToAdd: LibraryDependencyTypeFlag.DevComponent,
                flagsToRemove: LibraryDependencyTypeFlag.None);

        public static readonly LibraryDependencyTypeKeyword Build = new LibraryDependencyTypeKeyword(
                "build",
                flagsToAdd: LibraryDependencyTypeFlag.MainSource |
                            LibraryDependencyTypeFlag.PreprocessComponent,
                flagsToRemove: LibraryDependencyTypeFlag.None);

        public static readonly LibraryDependencyTypeKeyword Preprocess = new LibraryDependencyTypeKeyword(
                "preprocess",
                flagsToAdd: LibraryDependencyTypeFlag.PreprocessReference,
                flagsToRemove: LibraryDependencyTypeFlag.None);


        private readonly string _value;
        private readonly LibraryDependencyTypeFlag _flagsToAdd;
        private readonly LibraryDependencyTypeFlag _flagsToRemove;

        public LibraryDependencyTypeFlag FlagsToAdd
        {
            get { return _flagsToAdd; }
        }

        public LibraryDependencyTypeFlag FlagsToRemove
        {
            get { return _flagsToRemove; }
        }

        private const int DefaultKeywordLength = 5;

        static LibraryDependencyTypeKeyword()
        {
            Default = new LibraryDependencyTypeKeyword(
                "default",
                flagsToAdd: LibraryDependencyTypeFlag.MainReference |
                            LibraryDependencyTypeFlag.MainSource |
                            LibraryDependencyTypeFlag.MainExport |
                            LibraryDependencyTypeFlag.RuntimeComponent |
                            LibraryDependencyTypeFlag.BecomesNupkgDependency,
                flagsToRemove: LibraryDependencyTypeFlag.None);

            Private = new LibraryDependencyTypeKeyword(
                "private",
                flagsToAdd: LibraryDependencyTypeFlag.MainReference |
                            LibraryDependencyTypeFlag.MainSource |
                            LibraryDependencyTypeFlag.RuntimeComponent |
                            LibraryDependencyTypeFlag.BecomesNupkgDependency,
                flagsToRemove: LibraryDependencyTypeFlag.None);

            Dev = new LibraryDependencyTypeKeyword(
                "dev",
                flagsToAdd: LibraryDependencyTypeFlag.DevComponent,
                flagsToRemove: LibraryDependencyTypeFlag.None);

            Build = new LibraryDependencyTypeKeyword(
                "build",
                flagsToAdd: LibraryDependencyTypeFlag.MainSource |
                            LibraryDependencyTypeFlag.PreprocessComponent,
                flagsToRemove: LibraryDependencyTypeFlag.None);

            Preprocess = new LibraryDependencyTypeKeyword("preprocess",
                flagsToAdd: LibraryDependencyTypeFlag.PreprocessReference,
                flagsToRemove: LibraryDependencyTypeFlag.None);
        }

        private LibraryDependencyTypeKeyword(
            string value,
            LibraryDependencyTypeFlag flagsToAdd,
            LibraryDependencyTypeFlag flagsToRemove)
        {
            _value = value;
            _flagsToAdd = flagsToAdd;
            _flagsToRemove = flagsToRemove;
        }

        internal static LibraryDependencyTypeKeyword Declare(
            string keyword,
            LibraryDependencyTypeFlag flagsToAdd,
            LibraryDependencyTypeFlag flagsToRemove)
        {
            var kw = new LibraryDependencyTypeKeyword(keyword, flagsToAdd, flagsToRemove);
            _rareKeywords.Add(kw);
            return kw;
        }

        internal static LibraryDependencyTypeKeyword Parse(string keyword)
        {
            switch (keyword)
            {
                case "default":
                    return Default;
                case "private":
                    return Private;
                case "dev":
                    return Dev;
                case "build":
                    return Build;
                case "preprocess":
                    return Preprocess;
                default:
                    break;
            }

            PopulateRareKeywords();

            foreach (var kw in _rareKeywords)
            {
                if (kw._value == keyword)
                {
                    return kw;
                }
            }

            throw new Exception(string.Format("TODO: unknown keyword {0}", keyword));
        }

        private static void PopulateRareKeywords()
        {
            lock (_rareKeywords)
            {
                if (_rareKeywords.Count == 0)
                {
                    DeclareOnOff("MainReference", LibraryDependencyTypeFlag.MainReference, LibraryDependencyTypeFlag.None);
                    DeclareOnOff("MainSource", LibraryDependencyTypeFlag.MainSource, LibraryDependencyTypeFlag.None);
                    DeclareOnOff("MainExport", LibraryDependencyTypeFlag.MainExport, LibraryDependencyTypeFlag.None);
                    DeclareOnOff("PreprocessReference", LibraryDependencyTypeFlag.PreprocessReference, LibraryDependencyTypeFlag.None);

                    DeclareOnOff("RuntimeComponent", LibraryDependencyTypeFlag.RuntimeComponent, LibraryDependencyTypeFlag.None);
                    DeclareOnOff("DevComponent", LibraryDependencyTypeFlag.DevComponent, LibraryDependencyTypeFlag.None);
                    DeclareOnOff("PreprocessComponent", LibraryDependencyTypeFlag.PreprocessComponent, LibraryDependencyTypeFlag.None);
                    DeclareOnOff("BecomesNupkgDependency", LibraryDependencyTypeFlag.BecomesNupkgDependency, LibraryDependencyTypeFlag.None);
                }
            }
        }

        private static void DeclareOnOff(string name, LibraryDependencyTypeFlag flag, LibraryDependencyTypeFlag emptyFlags)
        {
            Declare(name,
                flagsToAdd: flag,
                flagsToRemove: emptyFlags);

            Declare(
                name + "-off",
                flagsToAdd: emptyFlags,
                flagsToRemove: flag);
        }
    }
}
