// <copyright file="LibrariesCollection.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;

    public sealed class LibrariesCollection : IEngineLibraries, IDisposable
    {
        private readonly ControlsLibrary controls;
        private readonly GraphicsWindowLibrary graphicsWindow;
        private readonly TextWindowLibrary textWindow;
        private readonly ShapesLibrary shapes;
        private readonly TimerLibrary timer;

        public LibrariesCollection()
        {
            this.Array = new ArrayLibrary();
            this.Clock = new ClockLibrary();
            this.controls = new ControlsLibrary();
            this.Desktop = new DesktopLibrary();
            this.Dictionary = new DictionaryLibrary();
            this.File = new FileLibrary();
            this.Flickr = new FlickrLibrary();
            this.graphicsWindow = new GraphicsWindowLibrary(this);
            this.ImageList = new ImageListLibrary(null);
            this.Math = new MathLibrary();
            this.Mouse = new MouseLibrary();
            this.Network = new NetworkLibrary();
            this.Program = new ProgramLibrary();
            this.shapes = new ShapesLibrary();
            this.Sound = new SoundLibrary();
            this.Stack = new StackLibrary();
            this.Text = new TextLibrary();
            this.textWindow = new TextWindowLibrary();
            this.timer = new TimerLibrary();
            this.Turtle = new TurtleLibrary();
        }

        public IArrayLibrary Array { get; private set; }

        public IClockLibrary Clock { get; private set; }

        public IControlsLibrary Controls => this.controls;

        public IDesktopLibrary Desktop { get; private set; }

        public IDictionaryLibrary Dictionary { get; private set; }

        public IFileLibrary File { get; private set; }

        public IFlickrLibrary Flickr { get; private set; }

        public IGraphicsWindowLibrary GraphicsWindow => this.graphicsWindow;

        public IImageListLibrary ImageList { get; private set; }

        public IMathLibrary Math { get; private set; }

        public IMouseLibrary Mouse { get; private set; }

        public INetworkLibrary Network { get; private set; }

        public IProgramLibrary Program { get; private set; }

        public IShapesLibrary Shapes => this.shapes;

        public ISoundLibrary Sound { get; private set; }

        public IStackLibrary Stack { get; private set; }

        public ITextLibrary Text { get; private set; }

        public ITextWindowLibrary TextWindow => this.textWindow;

        public ITimerLibrary Timer => this.timer;

        public ITurtleLibrary Turtle { get; private set; }

        public void Dispose()
        {
            this.graphicsWindow.Dispose();
            this.textWindow.Dispose();
            this.timer.Dispose();
        }

        internal void ClearShapes() => this.shapes.Clear();

        internal void ClearControls() => this.controls.Clear();
    }
}
