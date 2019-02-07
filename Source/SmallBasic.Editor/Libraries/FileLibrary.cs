// <copyright file="FileLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Utilities;
    using SmallBasic.Utilities.Bridge;

    internal sealed class FileLibrary : IFileLibrary
    {
        private const string SuccessResponse = "SUCCESS";
        private const string FailedResponse = "FAILED";

        private string lastError = string.Empty;

        public FileLibrary()
        {
        }

        public string Get_LastError() => this.lastError;

        public void Set_LastError(string value) => this.lastError = value;

        public Task<string> AppendContents(string filePath, string contents)
            => this.Execute(Bridge.File.AppendContents(new FileBridgeModels.PathAndContentsArgs(filePath, contents)));

        public Task<string> CopyFile(string sourceFilePath, string destinationFilePath)
            => this.Execute(Bridge.File.CopyFile(new FileBridgeModels.SourceAndDestinationArgs(sourceFilePath, destinationFilePath)));

        public Task<string> CreateDirectory(string directoryPath)
            => this.Execute(Bridge.File.CreateDirectory(directoryPath));

        public Task<string> DeleteDirectory(string directoryPath)
            => this.Execute(Bridge.File.DeleteDirectory(directoryPath));

        public Task<string> DeleteFile(string filePath)
            => this.Execute(Bridge.File.DeleteFile(filePath));

        public Task<BaseValue> GetDirectories(string directoryPath) => this.Execute(
            Bridge.File.GetDirectories(directoryPath),
            directories =>
            {
                int i = 1;
                return new ArrayValue(directories.ToDictionary(
                    value => (i++).ToString(CultureInfo.CurrentCulture),
                    value => StringValue.Create(value)));
            });

        public Task<BaseValue> GetFiles(string directoryPath) => this.Execute(
            Bridge.File.GetFiles(directoryPath),
            files =>
            {
                int i = 1;
                return new ArrayValue(files.ToDictionary(
                    value => (i++).ToString(CultureInfo.CurrentCulture),
                    value => StringValue.Create(value)));
            });

        public Task<BaseValue> GetTemporaryFilePath()
            => this.Execute(Bridge.File.GetTemporaryFilePath(), StringValue.Create);

        public Task<string> InsertLine(string filePath, decimal lineNumber, string contents)
            => this.Execute(Bridge.File.InsertLine(new FileBridgeModels.PathAndLineAndContentsArgs(filePath, lineNumber, contents)));

        public Task<BaseValue> ReadContents(string filePath)
            => this.Execute(Bridge.File.ReadContents(filePath), StringValue.Create);

        public Task<BaseValue> ReadLine(string filePath, decimal lineNumber)
            => this.Execute(Bridge.File.ReadLine(new FileBridgeModels.PathAndLineArgs(filePath, lineNumber)), StringValue.Create);

        public Task<string> WriteContents(string filePath, string contents)
            => this.Execute(Bridge.File.WriteContents(new FileBridgeModels.PathAndContentsArgs(filePath, contents)));

        public Task<string> WriteLine(string filePath, decimal lineNumber, string contents)
            => this.Execute(Bridge.File.WriteLine(new FileBridgeModels.PathAndLineAndContentsArgs(filePath, lineNumber, contents)));

        private async Task<string> Execute(Task<FileBridgeModels.Result> action)
        {
            var result = await action.ConfigureAwait(false);
            if (result.Success)
            {
                this.lastError = string.Empty;
                return SuccessResponse;
            }
            else
            {
                this.lastError = result.Error;
                return FailedResponse;
            }
        }

        private async Task<BaseValue> Execute<T>(Task<FileBridgeModels.Result<T>> action, Func<T, BaseValue> converter)
        {
            var result = await action.ConfigureAwait(false);
            if (result.Success)
            {
                this.lastError = string.Empty;
                return converter(result.Value);
            }
            else
            {
                this.lastError = result.Error;
                return StringValue.Create(FailedResponse);
            }
        }
    }
}
