// <copyright file="FileLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Globalization;
    using System.Linq;
    using SuperBasic.Compiler.Runtime;

    public interface IFilePlugin
    {
        void AppendContents(string filePath, string contents);

        void CopyFile(string sourceFilePath, string destinationFilePath);

        void CreateDirectory(string directoryPath);

        void DeleteDirectory(string directoryPath);

        void DeleteFile(string filePath);

        string[] GetDirectories(string directoryPath);

        string[] GetFiles(string directoryPath);

        string GetTemporaryFilePath();

        void InsertLine(string filePath, decimal lineNumber, string contents);

        string ReadContents(string filePath);

        string ReadLine(string filePath, decimal lineNumber);

        void WriteContents(string filePath, string contents);

        void WriteLine(string filePath, decimal lineNumber, string contents);
    }

    internal sealed class FileLibrary : IFileLibrary
    {
        private readonly IFilePlugin plugin;

        private string lastError;

        public FileLibrary(IFilePlugin plugin)
        {
            this.plugin = plugin;
            this.lastError = string.Empty;
        }

        public string Get_LastError() => this.lastError;

        public void Set_LastError(string value) => this.lastError = value;

        public string AppendContents(string filePath, string contents)
        {
            return this.ProcessOperation(() => this.plugin.AppendContents(filePath, contents));
        }

        public string CopyFile(string sourceFilePath, string destinationFilePath)
        {
            return this.ProcessOperation(() => this.plugin.CopyFile(sourceFilePath, destinationFilePath));
        }

        public string CreateDirectory(string directoryPath)
        {
            return this.ProcessOperation(() => this.CreateDirectory(directoryPath));
        }

        public string DeleteDirectory(string directoryPath)
        {
            return this.ProcessOperation(() => this.plugin.DeleteDirectory(directoryPath));
        }

        public string DeleteFile(string filePath)
        {
            return this.ProcessOperation(() => this.plugin.DeleteFile(filePath));
        }

        public ArrayValue GetDirectories(string directoryPath)
        {
            return this.ProcessOperation(() =>
            {
                int i = 1;
                string[] directories = this.plugin.GetDirectories(directoryPath);

                return new ArrayValue(directories.ToDictionary(
                    value => (i++).ToString(CultureInfo.CurrentCulture),
                    value => StringValue.Create(value)));
            }) ?? new ArrayValue();
        }

        public ArrayValue GetFiles(string directoryPath)
        {
            return this.ProcessOperation(() =>
            {
                int i = 1;
                string[] files = this.plugin.GetFiles(directoryPath);

                return new ArrayValue(files.ToDictionary(
                    value => (i++).ToString(CultureInfo.CurrentCulture),
                    value => StringValue.Create(value)));
            }) ?? new ArrayValue();
        }

        public string GetTemporaryFilePath()
        {
            return this.ProcessOperation(() => this.plugin.GetTemporaryFilePath()) ?? string.Empty;
        }

        public string InsertLine(string filePath, decimal lineNumber, string contents)
        {
            return this.ProcessOperation(() => this.plugin.InsertLine(filePath, lineNumber, contents));
        }

        public string ReadContents(string filePath)
        {
            return this.ProcessOperation(() => this.plugin.ReadContents(filePath)) ?? string.Empty;
        }

        public string ReadLine(string filePath, decimal lineNumber)
        {
            return this.ProcessOperation(() => this.plugin.ReadLine(filePath, lineNumber)) ?? string.Empty;
        }

        public string WriteContents(string filePath, string contents)
        {
            return this.ProcessOperation(() => this.plugin.WriteContents(filePath, contents));
        }

        public string WriteLine(string filePath, decimal lineNumber, string contents)
        {
            return this.ProcessOperation(() => this.plugin.WriteLine(filePath, lineNumber, contents));
        }

        private string ProcessOperation(Action operation)
        {
            this.lastError = string.Empty;

            try
            {
                operation();
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                this.lastError = ex.Message;
                return "FAILED";
            }
        }

        private TResult ProcessOperation<TResult>(Func<TResult> operation)
        {
            this.lastError = string.Empty;

            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                this.lastError = ex.Message;
                return default;
            }
        }
    }
}
