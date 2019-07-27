// <copyright file="FileBridge.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Bridge
{
    using System;
    using System.IO;
    using System.Linq;
    using SmallBasic.Utilities.Bridge;

    internal sealed class FileBridge : IFileBridge
    {
        public FileBridgeModels.Result AppendContents(FileBridgeModels.PathAndContentsArgs args) => Execute(() =>
        {
            var filePath = Environment.ExpandEnvironmentVariables(args.FilePath);
            File.AppendAllText(filePath, args.Contents);
        });

        public FileBridgeModels.Result CopyFile(FileBridgeModels.SourceAndDestinationArgs args) => Execute(() =>
        {
            var sourceFilePath = Environment.ExpandEnvironmentVariables(args.SourceFilePath);
            var destinationFilePath = Environment.ExpandEnvironmentVariables(args.DestinationFilePath);

            if (Directory.Exists(destinationFilePath))
            {
                var fileName = Path.GetFileName(sourceFilePath);
                destinationFilePath = Path.Combine(destinationFilePath, fileName);
            }

            File.Copy(sourceFilePath, destinationFilePath);
        });

        public FileBridgeModels.Result CreateDirectory(string directoryPath) => Execute(() =>
        {
            directoryPath = Environment.ExpandEnvironmentVariables(directoryPath);
            Directory.CreateDirectory(directoryPath);
        });

        public FileBridgeModels.Result DeleteDirectory(string directoryPath) => Execute(() =>
        {
            directoryPath = Environment.ExpandEnvironmentVariables(directoryPath);
            Directory.Delete(directoryPath, recursive: true);
        });

        public FileBridgeModels.Result DeleteFile(string filePath) => Execute(() =>
        {
            filePath = Environment.ExpandEnvironmentVariables(filePath);
            File.Delete(filePath);
        });

        public FileBridgeModels.Result<string[]> GetDirectories(string directoryPath) => Execute(() =>
        {
            directoryPath = Environment.ExpandEnvironmentVariables(directoryPath);
            return Directory.GetDirectories(directoryPath);
        });

        public FileBridgeModels.Result<string[]> GetFiles(string directoryPath) => Execute(() =>
        {
            directoryPath = Environment.ExpandEnvironmentVariables(directoryPath);
            return Directory.GetFiles(directoryPath);
        });

        public FileBridgeModels.Result<string> GetTemporaryFilePath()
            => Execute(Path.GetTempFileName);

        public FileBridgeModels.Result InsertLine(FileBridgeModels.PathAndLineAndContentsArgs args) => Execute(() =>
        {
            var filePath = Environment.ExpandEnvironmentVariables(args.FilePath);
            var lines = File.ReadAllLines(filePath).ToList();
            var lineNumber = Math.Min(lines.Count, Math.Max(0, ((int)args.LineNumber) - 1));

            lines.Insert(lineNumber, args.Contents);
            File.WriteAllLines(filePath, lines);
        });

        public FileBridgeModels.Result<string> ReadContents(string filePath) => Execute(() =>
        {
            filePath = Environment.ExpandEnvironmentVariables(filePath);
            return File.ReadAllText(filePath);
        });

        public FileBridgeModels.Result<string> ReadLine(FileBridgeModels.PathAndLineArgs args) => Execute(() =>
        {
            var filePath = Environment.ExpandEnvironmentVariables(args.FilePath);
            var lines = File.ReadAllLines(filePath).ToList();
            var lineNumber = Math.Min(lines.Count - 1, Math.Max(0, ((int)args.LineNumber) - 1));

            return lines[lineNumber];
        });

        public FileBridgeModels.Result WriteContents(FileBridgeModels.PathAndContentsArgs args) => Execute(() =>
        {
            var filePath = Environment.ExpandEnvironmentVariables(args.FilePath);
            File.WriteAllText(filePath, args.Contents);
        });

        public FileBridgeModels.Result WriteLine(FileBridgeModels.PathAndLineAndContentsArgs args) => Execute(() =>
        {
            var filePath = Environment.ExpandEnvironmentVariables(args.FilePath);
            var lines = File.ReadAllLines(filePath).ToList();
            var lineNumber = Math.Max(0, ((int)args.LineNumber) - 1);

            if (lineNumber < lines.Count)
            {
                lines[lineNumber] = args.Contents;
            }
            else
            {
                lines.Add(args.Contents);
            }

            File.WriteAllLines(filePath, lines);
        });

        private static FileBridgeModels.Result Execute(Action action)
        {
            try
            {
                action();
                return new FileBridgeModels.Result(success: true, string.Empty);
            }
            catch (Exception ex)
            {
                return new FileBridgeModels.Result(success: false, ex.Message);
            }
        }

        private static FileBridgeModels.Result<T> Execute<T>(Func<T> action)
        {
            try
            {
                T value = action();
                return new FileBridgeModels.Result<T>(success: true, value, error: string.Empty);
            }
            catch (Exception ex)
            {
                return new FileBridgeModels.Result<T>(success: false, value: default, error: ex.Message);
            }
        }
    }
}
