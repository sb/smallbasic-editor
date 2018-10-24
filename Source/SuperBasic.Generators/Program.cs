// <copyright file="Program.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators
{
    using System;
    using System.Globalization;
    using CommandLine;

    public class TaskOptions
    {
        [Option(longName: "task", Required = true, HelpText = "Task name to run")]
        public string TaskName { get; set; }

        [Option(longName: "input", Required = true, HelpText = "Path to input file")]
        public string InputFile { get; set; }

        [Option(longName: "output", Required = true, HelpText = "Path to output file")]
        public string OutputFile { get; set; }
    }

    public static class Program
    {
        public static int Main(string[] args) =>
            Parser.Default.ParseArguments<TaskOptions>(args)
            .MapResult(options =>
            {
                foreach (Type type in typeof(Program).Assembly.DefinedTypes)
                {
                    if (type.Name.ToLower(CultureInfo.CurrentCulture) == options.TaskName.Replace("-", string.Empty, StringComparison.CurrentCulture))
                    {
                        BaseGeneratorTask taskInstance = (BaseGeneratorTask)Activator.CreateInstance(type);
                        return taskInstance.Execute(options.InputFile, options.OutputFile);
                    }
                }

                Console.Error.WriteLine($"Cannot find a task with name '{options.TaskName}'.");
                return 1;
            },
            errors =>
            {
                foreach (Error error in errors)
                {
                    Console.Error.WriteLine(error.ToString());
                }

                return 1;
            });
    }
}
