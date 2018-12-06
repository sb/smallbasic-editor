// <copyright file="GenerateClientBridge.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators.Bridge
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class GenerateClientBridge : BaseConverterTask<BridgeTypeCollection>
    {
        protected override void Generate(BridgeTypeCollection model)
        {
            this.Line($@"import * as fs from ""fs"";");
            this.Line($@"import * as os from ""os"";");
            this.Line($@"import * as path from ""path"";");
            this.Line($@"import * as child_process from ""child_process"";");
            this.Blank();

            this.Line($@"const communicationFilePath = path.join(os.tmpdir(), `bridge-comm-${{Math.random().toString().split(""."")[1]}}.json`);");
            this.Line(@"const bridgeBinaryPath = path.resolve(__dirname, ""_bridge"", ""SuperBasic.Bridge.dll"");");
            this.Blank();

            this.Line("(<any>global).Bridge = {");
            this.Indent();

            for (int i = 0; i < model.Count; i++)
            {
                BridgeType type = model[i];
                this.Line($"{type.Name}: {{");
                this.Indent();

                for (int j = 0; j < type.Methods.Count; j++)
                {
                    Method method = type.Methods[j];

                    string parameter = method.InputType.IsDefault() ? string.Empty : $"{method.InputName.ToLowerFirstChar()}: object";
                    string outputType = method.OutputType.IsDefault() ? "boolean" : "object";

                    this.Line($"{method.Name}: ({parameter}): {outputType} => {{");
                    this.Indent();

                    if (method.InputType.IsDefault())
                    {
                        if (method.OutputType.IsDefault())
                        {
                            this.Line($@"child_process.execFileSync(""dotnet"", [bridgeBinaryPath, ""{type.Name}"", ""{method.Name}""]);");
                            this.Line("return true;");
                        }
                        else
                        {
                            this.Line($@"child_process.execFileSync(""dotnet"", [bridgeBinaryPath, ""{type.Name}"", ""{method.Name}"", communicationFilePath]);");
                            this.Line(@"return JSON.parse(fs.readFileSync(communicationFilePath, ""utf8""));");
                        }
                    }
                    else
                    {
                        this.Line($@"fs.writeFileSync(communicationFilePath, JSON.stringify({method.InputName.ToLowerFirstChar()}));");
                        if (method.OutputType.IsDefault())
                        {
                            this.Line($@"child_process.execFileSync(""dotnet"", [bridgeBinaryPath, ""{type.Name}"", ""{method.Name}"", communicationFilePath]);");
                            this.Line("return true;");
                        }
                        else
                        {
                            this.Line($@"child_process.execFileSync(""dotnet"", [bridgeBinaryPath, ""{type.Name}"", ""{method.Name}"", communicationFilePath]);");
                            this.Line(@"return JSON.parse(fs.readFileSync(communicationFilePath, ""utf8""));");
                        }
                    }

                    this.Unindent();
                    this.Line($"}}{(j + 1 == type.Methods.Count ? string.Empty : ",")}");
                }

                this.Unindent();
                this.Line($"}}{(i + 1 == model.Count ? string.Empty : ",")}");
            }

            this.Unindent();
            this.Line("};");
        }
    }
}
