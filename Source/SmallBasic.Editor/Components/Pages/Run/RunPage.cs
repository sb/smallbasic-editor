// <copyright file="RunPage.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Components.Pages.Run
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using SmallBasic.Compiler;
    using SmallBasic.Editor.Components.Display;
    using SmallBasic.Editor.Components.Layout;
    using SmallBasic.Editor.Components.Toolbox;
    using SmallBasic.Editor.Libraries;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Editor.Store;
    using SmallBasic.Utilities;
    using SmallBasic.Utilities.Resources;

    public sealed class RunPage : MainLayout, IDisposable
    {
        private readonly AsyncEngine engine = new AsyncEngine(isDebugging: false);

        private bool isInitialized;

        public static void Inject(TreeComposer composer)
        {
            composer.Inject<RunPage>();
        }

        public void Dispose()
        {
            this.engine.Dispose();
        }

        protected override void OnInit()
        {
            if (CompilationStore.Compilation.Diagnostics.Any())
            {
                NavigationStore.NagivateTo(NavigationStore.PageId.Edit);
                return;
            }
        }

        protected override void ComposeBody(TreeComposer composer)
        {
            composer.Element("run-page", body: () =>
            {
                EngineDisplay.Inject(composer, this.engine);
            });
        }

        protected override void ComposeLeftActions(TreeComposer composer)
        {
            Actions.Action(composer, "back", EditorResources.Actions_Back, () =>
            {
                NavigationStore.NagivateTo(NavigationStore.PageId.Edit);
                return Task.CompletedTask;
            });
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;
                await Task.Run(() => this.engine.StartLoop()).ConfigureAwait(false);
            }
        }
    }
}
