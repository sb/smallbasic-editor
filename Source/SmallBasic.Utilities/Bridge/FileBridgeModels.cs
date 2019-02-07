// <copyright file="FileBridgeModels.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Utilities.Bridge
{
    public static class FileBridgeModels
    {
        public sealed class Result
        {
            public Result()
            {
            }

            public Result(bool success, string error)
            {
                this.Success = success;
                this.Error = error;
            }

            public bool Success { get; private set; }

            public string Error { get; private set; }
        }

        public sealed class Result<T>
        {
            public Result()
            {
            }

            public Result(bool success, T value, string error)
            {
                this.Success = success;
                this.Value = value;
                this.Error = error;
            }

            public bool Success { get; private set; }

            public T Value { get; private set; }

            public string Error { get; private set; }
        }

        public sealed class PathAndContentsArgs
        {
            public PathAndContentsArgs()
            {
            }

            public PathAndContentsArgs(string filePath, string contents)
            {
                this.FilePath = filePath;
                this.Contents = contents;
            }

            public string FilePath { get; private set; }

            public string Contents { get; private set; }
        }

        public sealed class SourceAndDestinationArgs
        {
            public SourceAndDestinationArgs()
            {
            }

            public SourceAndDestinationArgs(string sourceFilePath, string destinationFilePath)
            {
                this.SourceFilePath = sourceFilePath;
                this.DestinationFilePath = destinationFilePath;
            }

            public string SourceFilePath { get; private set; }

            public string DestinationFilePath { get; private set; }
        }

        public sealed class PathAndLineAndContentsArgs
        {
            public PathAndLineAndContentsArgs()
            {
            }

            public PathAndLineAndContentsArgs(string filePath, decimal lineNumber, string contents)
            {
                this.FilePath = filePath;
                this.LineNumber = lineNumber;
                this.Contents = contents;
            }

            public string FilePath { get; private set; }

            public decimal LineNumber { get; private set; }

            public string Contents { get; set; }
        }

        public sealed class PathAndLineArgs
        {
            public PathAndLineArgs()
            {
            }

            public PathAndLineArgs(string filePath, decimal lineNumber)
            {
                this.FilePath = filePath;
                this.LineNumber = lineNumber;
            }

            public string FilePath { get; private set; }

            public decimal LineNumber { get; private set; }
        }
    }
}
