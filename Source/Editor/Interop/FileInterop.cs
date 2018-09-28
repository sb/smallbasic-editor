// <copyright file="FileInterop.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using SuperBasic.Editor.Libraries;

    internal class FileInterop : IFileInterop
    {
        private LibrariesCollection libraries;

        public FileInterop(LibrariesCollection libraries)
        {
            this.libraries = libraries;
        }

        public Task ReportFileError(string error)
        {
            this.libraries.File.LastError = error;
            return Task.CompletedTask;
        }
    }
}
