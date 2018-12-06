// <copyright file="GenerateBridgeExecution.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Bridge
{
    using SuperBasic.Utilities;

    public sealed class GenerateBridgeExecution : BaseConverterTask<BridgeTypeCollection>
    {
        protected override void Generate(BridgeTypeCollection model)
        {
            foreach (BridgeType type in model)
            {
                foreach (Method method in type.Methods)
                {
                    if (method.InputName.IsDefault() ^ method.InputType.IsDefault())
                    {
                        this.LogError($"Method {type.Name}.{method.Name} must specify either both or neither {nameof(method.InputName)} and {nameof(method.InputType)}");
                    }
                }
            }

            this.Line("namespace SuperBasic.Bridge");
            this.Brace();

            this.Line("using System.Diagnostics;");
            this.Line("using Newtonsoft.Json;");
            this.Line("using SuperBasic.Utilities;");
            this.Line("using SuperBasic.Utilities.Bridge;");
            this.Blank();

            this.GenerateBridgeInterfaces(model);
            this.GenerateeExecution(model);

            this.Unbrace();
        }

        private void GenerateBridgeInterfaces(BridgeTypeCollection model)
        {
            foreach (BridgeType type in model)
            {
                this.Line($"internal interface I{type.Name}Bridge");
                this.Brace();

                for (var i = 0; i < type.Methods.Count; i++)
                {
                    if (i > 0)
                    {
                        this.Blank();
                    }

                    Method method = type.Methods[i];
                    string returnType = method.OutputType.IsDefault() ? "void" : method.OutputType;
                    string parameters = method.InputType.IsDefault() ? string.Empty : $"{method.InputType} {method.InputName}";
                    this.Line($"{returnType} {method.Name}({parameters});");
                }

                this.Unbrace();
            }
        }

        private void GenerateeExecution(BridgeTypeCollection model)
        {
            this.Line("internal static class BridgeExecution");
            this.Brace();

            foreach (BridgeType type in model)
            {
                this.Line($"private static readonly I{type.Name}Bridge {type.Name} = new {type.Name}Bridge();");
                this.Blank();
            }

            this.Line("public static void Run(string[] args)");
            this.Brace();
            this.Line(@"Debug.Assert(args.Length >= 2, ""Only intended for bridge calls"");");
            this.Blank();
            this.Line("string type = args[0];");
            this.Line("string method = args[1];");
            this.Line("string filePath = args.Length > 2 ? args[2] : null;");
            this.Blank();

            this.Line("switch (type)");
            this.Brace();

            foreach (BridgeType type in model)
            {
                this.Line($@"case ""{type.Name}"":");
                this.Brace();

                this.Line("switch (method)");
                this.Brace();

                foreach (Method method in type.Methods)
                {
                    this.Line($@"case ""{method.Name}"":");
                    this.Brace();

                    if (method.InputType.IsDefault())
                    {
                        if (method.OutputType.IsDefault())
                        {
                            this.Line($"{type.Name}.{method.Name}();");
                        }
                        else
                        {
                            this.Line($"{method.OutputType} output = {type.Name}.{method.Name}();");
                            this.Line("System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(output));");
                        }
                    }
                    else
                    {
                        this.Line($"{method.InputType} input = JsonConvert.DeserializeObject<{method.InputType}>(System.IO.File.ReadAllText(filePath));");
                        if (method.OutputType.IsDefault())
                        {
                            this.Line($"{type.Name}.{method.Name}(input);");
                        }
                        else
                        {
                            this.Line($"{method.OutputType} output = {type.Name}.{method.Name}(input);");
                            this.Line("System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(output));");
                        }
                    }

                    this.Line("break;");
                    this.Unbrace();
                }

                this.Line("default:");
                this.Brace();
                this.Line("throw ExceptionUtilities.UnexpectedValue(method);");
                this.Unbrace();
                this.Unbrace();

                this.Line("break;");
                this.Unbrace();
            }

            this.Line("default:");
            this.Brace();
            this.Line("throw ExceptionUtilities.UnexpectedValue(type);");
            this.Unbrace();
            this.Unbrace();
            this.Unbrace();
            this.Unbrace();
        }
    }
}
