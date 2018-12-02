// <copyright file="LibrariesCollection.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Components;

    public sealed class LibrariesCollection : IEngineLibraries
    {
        private TextWindowLibrary textWindowLibrary = new TextWindowLibrary();

        public LibrariesCollection()
        {
            this.Array = new ArrayLibrary();
            this.Clock = new ClockLibrary();
            this.Controls = new ControlsLibrary(null);
            this.Desktop = new DesktopLibrary(null);
            this.Dictionary = new DictionaryLibrary();
            this.File = new FileLibrary(null);
            this.Flickr = new FlickrLibrary();
            this.GraphicsWindow = new GraphicsWindowLibrary(null, null);
            this.ImageList = new ImageListLibrary(null);
            this.Math = new MathLibrary();
            this.Mouse = new MouseLibrary();
            this.Network = new NetworkLibrary(null, null);
            this.Program = new ProgramLibrary();
            this.Shapes = new ShapesLibrary(null, null);
            this.Sound = new SoundLibrary();
            this.Stack = new StackLibrary();
            this.Text = new TextLibrary();
            this.TextWindow = this.textWindowLibrary;
            this.Timer = new TimerLibrary();
            this.Turtle = new TurtleLibrary(null);
        }

        public IArrayLibrary Array { get; private set; }

        public IClockLibrary Clock { get; private set; }

        public IControlsLibrary Controls { get; private set; }

        public IDesktopLibrary Desktop { get; private set; }

        public IDictionaryLibrary Dictionary { get; private set; }

        public IFileLibrary File { get; private set; }

        public IFlickrLibrary Flickr { get; private set; }

        public IGraphicsWindowLibrary GraphicsWindow { get; private set; }

        public IImageListLibrary ImageList { get; private set; }

        public IMathLibrary Math { get; private set; }

        public IMouseLibrary Mouse { get; private set; }

        public INetworkLibrary Network { get; private set; }

        public IProgramLibrary Program { get; private set; }

        public IShapesLibrary Shapes { get; private set; }

        public ISoundLibrary Sound { get; private set; }

        public IStackLibrary Stack { get; private set; }

        public ITextLibrary Text { get; private set; }

        public ITextWindowLibrary TextWindow { get; private set; }

        public ITimerLibrary Timer { get; private set; }

        public ITurtleLibrary Turtle { get; private set; }

        internal void SetInputBuffer(string value) => this.textWindowLibrary.SetInputBuffer(value);

        internal void TerminateTextDisplay() => this.textWindowLibrary.TerminateTextDisplay();
    }
}
