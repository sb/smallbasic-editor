// <copyright file="MemoryExplorer.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Pages.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.AspNetCore.Blazor.Components;
    using SmallBasic.Compiler;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Components.Toolbox;
    using SmallBasic.Editor.Interop;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Utilities;
    using SmallBasic.Utilities.Resources;

    public sealed class MemoryExplorer : SmallBasicComponent
    {
        private bool isExpanded = false;

        [Parameter]
        private AsyncEngine Engine { get; set; }

        internal static void Inject(TreeComposer composer, AsyncEngine engine)
        {
            composer.Inject<MemoryExplorer>(new Dictionary<string, object>
            {
                { nameof(MemoryExplorer.Engine), engine }
            });
        }

        protected override void OnInit()
        {
            this.Engine.ExecutedStep += this.StateHasChanged;
        }

        protected override void ComposeTree(TreeComposer composer)
        {
            composer.Element("memory-explorer", body: () =>
            {
                composer.Element(
                    name: this.isExpanded ? "content-area-expanded" : "content-area-contracted",
                    body: () =>
                    {
                        this.ComposeHeader(composer);

                        composer.Element("member-area", body: () =>
                        {
                            if (this.isExpanded)
                            {
                                this.ComposeVariables(composer);
                                this.ComposeCallStack(composer);
                            }
                        });
                    });
            });
        }

        private void ComposeHeader(TreeComposer composer)
        {
            composer.Element("header-area", body: () =>
            {
                composer.Element(
                    name: "logo-area",
                    events: new TreeComposer.Events
                    {
                        OnClick = args => this.isExpanded = !this.isExpanded
                    },
                    body: () =>
                    {
                        composer.Element("logo");
                    });

                composer.Element("empty-scroll-area");

                if (this.isExpanded)
                {
                    composer.Element("title-area", body: () => composer.Text(EditorResources.MemoryExplorer_Title));
                    composer.Element(
                        name: "minimize-button",
                        events: new TreeComposer.Events
                        {
                            OnClick = args => this.isExpanded = false
                        },
                        body: () =>
                        {
                            composer.Element("angle-left");
                        });
                }
            });
        }

        private void ComposeVariables(TreeComposer composer)
        {
            composer.Element("variables-member", body: () =>
            {
                composer.Element("member-title-area", body: () =>
                {
                    composer.Element("logo-area", body: () => composer.Element("logo"));
                    composer.Element("title-text", body: () => composer.Text(EditorResources.MemoryExplorer_Variables));
                });

                composer.Element("member-table-header", body: () =>
                {
                    composer.Element("left-text", body: () => composer.Text(EditorResources.MemoryExplorer_Name));
                    composer.Element("right-text", body: () => composer.Text(EditorResources.MemoryExplorer_Value));
                });

                this.ComposeOnlyIfNotRunning(composer, showEvenIfTerminated: true, body: () =>
                {
                    var memory = this.Engine.GetSnapshot().Memory;

                    if (memory.Any())
                    {
                        composer.Element("variables-block", body: () =>
                        {
                            foreach (var variable in memory)
                            {
                                composer.Element("variable", body: () =>
                                {
                                    composer.Element("name-cell", body: () =>
                                    {
                                        composer.Element("icon-container", body: () =>
                                        {
                                            switch (variable.Value)
                                            {
                                                case StringValue stringValue:
                                                case BooleanValue booleanValue:
                                                    composer.Element("string-type-icon");
                                                    break;
                                                case NumberValue numberValue:
                                                    composer.Element("number-type-icon");
                                                    break;
                                                case ArrayValue arrayValue:
                                                    composer.Element("array-type-icon");
                                                    break;
                                                default:
                                                    throw ExceptionUtilities.UnexpectedValue(variable.Value);
                                            }
                                        });

                                        composer.Element("name-container", body: () => composer.Text(variable.Key));
                                    });

                                    composer.Element("value-cell", body: () => composer.Text(variable.Value.ToDisplayString()));
                                });
                            }
                        });
                    }
                });
            });
        }

        private void ComposeCallStack(TreeComposer composer)
        {
            composer.Element("call-stack-member", body: () =>
            {
                composer.Element("member-title-area", body: () =>
                {
                    composer.Element("logo-area", body: () => composer.Element("logo"));
                    composer.Element("title-text", body: () => composer.Text(EditorResources.MemoryExplorer_CallStack));
                });

                composer.Element("member-table-header", body: () =>
                {
                    composer.Element("left-text", body: () => composer.Text(EditorResources.MemoryExplorer_Line));
                    composer.Element("right-text", body: () => composer.Text(EditorResources.MemoryExplorer_Module));
                });

                this.ComposeOnlyIfNotRunning(composer, showEvenIfTerminated: false, body: () =>
                {
                    composer.Element("call-stack", body: () =>
                    {
                        composer.Element("blue-box");
                        composer.Element("container-box", body: () =>
                        {
                            bool firstFrame = true;
                            foreach (var frame in this.Engine.GetSnapshot().ExecutionStack.Reverse())
                            {
                                string name;
                                if (firstFrame)
                                {
                                    firstFrame = false;
                                    name = "stack-frame-highlighted";
                                }
                                else
                                {
                                    name = "stack-frame";
                                }

                                composer.Element(name, body: () =>
                                {
                                    var line = frame.CurrentSourceLine + 1; // Monaco editor is one-based
                                    composer.Element("line-cell", body: () => composer.Text(line.ToString(CultureInfo.CurrentCulture)));
                                    composer.Element("module-name-cell", body: () => composer.Text(frame.Module.Name));
                                });
                            }
                        });
                    });
                });
            });
        }

        private void ComposeOnlyIfNotRunning(TreeComposer composer, bool showEvenIfTerminated, Action body)
        {
            switch (this.Engine.State)
            {
                case ExecutionState.BlockedOnNumberInput:
                case ExecutionState.BlockedOnStringInput:
                case ExecutionState.Paused:
                    {
                        body();
                        break;
                    }

                case ExecutionState.Running:
                    {
                        composer.Element("running-block", body: () =>
                        {
                            composer.Element("icon-container", body: () => composer.Element("program-running-icon"));
                            composer.Element("text-container", body: () => composer.Text(EditorResources.MemoryExplorer_ProgramRunning));
                        });

                        break;
                    }

                case ExecutionState.Terminated:
                    {
                        if (showEvenIfTerminated)
                        {
                            body();
                        }
                        else
                        {
                            composer.Element("running-block", body: () =>
                            {
                                composer.Element("icon-container", body: () => composer.Element("program-ended-icon"));
                                composer.Element("text-container", body: () => composer.Text(EditorResources.MemoryExplorer_ProgramEnded));
                            });
                        }

                        break;
                    }

                default:
                    {
                        throw ExceptionUtilities.UnexpectedValue(this.Engine.State);
                    }
            }
        }
    }
}
