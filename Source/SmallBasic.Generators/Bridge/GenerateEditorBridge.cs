// <copyright file="GenerateEditorBridge.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Generators.Bridge
{
    using SmallBasic.Utilities;

    public sealed class GenerateEditorBridge : BaseConverterTask<BridgeTypeCollection>
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

            this.Line("namespace SmallBasic.Editor");
            this.Brace();

            this.Line("using System.Threading.Tasks;");
            this.Line("using Microsoft.JSInterop;");
            this.Line("using SmallBasic.Utilities.Bridge;");
            this.Blank();

            this.GenerateInteropType(model);
            this.Unbrace();
        }

        private void GenerateInteropType(BridgeTypeCollection model)
        {
            this.Line("internal static class Bridge");
            this.Brace();

            foreach (BridgeType type in model)
            {
                this.Line($"public static class {type.Name}");
                this.Brace();

                for (int i = 0; i < type.Methods.Count; i++)
                {
                    if (i > 0)
                    {
                        this.Blank();
                    }

                    Method method = type.Methods[i];

                    string returnType = method.OutputType.IsDefault() ? "Task" : $"Task<{method.OutputType}>";
                    string parameters = method.InputType.IsDefault() ? string.Empty : $"{method.InputType} {method.InputName}";
                    this.Line($"public static {(method.OutputType.IsDefault() ? "async " : string.Empty)}{returnType} {method.Name}({parameters})");
                    this.Brace();

                    string arguments = $@"""Bridge.{type.Name}.{method.Name}""";
                    if (!method.InputType.IsDefault())
                    {
                        arguments = $"{arguments}, {method.InputName}";
                    }

                    if (method.OutputType.IsDefault())
                    {
                        this.Line($@"await JSRuntime.Current.InvokeAsync<bool>({arguments}).ConfigureAwait(false);");
                    }
                    else
                    {
                        this.Line($@"return JSRuntime.Current.InvokeAsync<{method.OutputType}>({arguments});");
                    }

                    this.Unbrace();
                }

                this.Unbrace();
            }

            this.Unbrace();
        }
    }
}
