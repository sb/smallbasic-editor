// <copyright file="Program.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using CommandLine;

    [Verb("convert", HelpText = "Converts a model file into a code file.")]
    public class ConvertVerb
    {
        [Option(longName: "task", Required = true, HelpText = "Task name to run")]
        public string TaskName { get; set; }

        [Option(longName: "input", Required = true, HelpText = "Path to input model file")]
        public string InputFile { get; set; }

        [Option(longName: "output", Required = true, HelpText = "Path to output code file")]
        public string OutputFile { get; set; }
    }

    public static class Program
    {
        public static int Main(string[] args) =>
            CommandLine.Parser.Default.ParseArguments<ConvertVerb>(args).MapResult(
                (ConvertVerb verb) => FindAndRunTask<BaseConverterTask>(verb.TaskName, task => task.Execute(verb.InputFile, verb.OutputFile)),
                (IEnumerable<Error> errors) =>
                {
                    foreach (var error in errors)
                    {
                        Console.Error.WriteLine(error.ToString());
                    }

                    return 1;
                });

        private static int FindAndRunTask<TTask>(string taskName, Func<TTask, int> then)
        {
            foreach (Type type in typeof(Program).Assembly.DefinedTypes)
            {
                if (type.Name.ToLower(CultureInfo.CurrentCulture) == taskName.Replace("-", string.Empty, StringComparison.CurrentCulture))
                {
                    return then((TTask)Activator.CreateInstance(type));
                }
            }

            Console.Error.WriteLine($"Cannot find a task with name '{taskName}'.");
            return 1;
        }
    }
}
