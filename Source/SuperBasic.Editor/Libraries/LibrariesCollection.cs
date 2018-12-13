// <copyright file="LibrariesCollection.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Libraries.Utilities;

    public sealed class LibrariesCollection : IEngineLibraries, IDisposable
    {
        private readonly ArrayLibrary array;
        private readonly ClockLibrary clock;
        private readonly ControlsLibrary controls;
        private readonly DesktopLibrary desktop;
        private readonly DictionaryLibrary dictionary;
        private readonly FileLibrary file;
        private readonly FlickrLibrary flickr;
        private readonly GraphicsWindowLibrary graphicsWindow;
        private readonly ImageListLibrary imageList;
        private readonly MathLibrary math;
        private readonly MouseLibrary mouse;
        private readonly NetworkLibrary network;
        private readonly ProgramLibrary program;
        private readonly ShapesLibrary shapes;
        private readonly SoundLibrary sound;
        private readonly StackLibrary stack;
        private readonly TextLibrary text;
        private readonly TextWindowLibrary textWindow;
        private readonly TimerLibrary timer;
        private readonly TurtleLibrary turtle;

        public LibrariesCollection()
        {
            this.Styles = new GraphicsWindowStyles(
                penWidth: 2,
                penColor: PredefinedColors.GetHexColor("Black"),
                brushColor: PredefinedColors.GetHexColor("SlateBlue"),
                fontBold: false,
                fontItalic: false,
                fontName: "Roboto",
                fontSize: 12);

            this.array = new ArrayLibrary();
            this.clock = new ClockLibrary();
            this.controls = new ControlsLibrary();
            this.desktop = new DesktopLibrary();
            this.dictionary = new DictionaryLibrary();
            this.file = new FileLibrary();
            this.flickr = new FlickrLibrary();
            this.graphicsWindow = new GraphicsWindowLibrary(this);
            this.imageList = new ImageListLibrary(null);
            this.math = new MathLibrary();
            this.mouse = new MouseLibrary();
            this.network = new NetworkLibrary();
            this.program = new ProgramLibrary();
            this.shapes = new ShapesLibrary(this);
            this.sound = new SoundLibrary();
            this.stack = new StackLibrary();
            this.text = new TextLibrary();
            this.textWindow = new TextWindowLibrary();
            this.timer = new TimerLibrary();
            this.turtle = new TurtleLibrary();
        }

        public IArrayLibrary Array => this.array;

        public IClockLibrary Clock => this.clock;

        public IControlsLibrary Controls => this.controls;

        public IDesktopLibrary Desktop => this.desktop;

        public IDictionaryLibrary Dictionary => this.dictionary;

        public IFileLibrary File => this.file;

        public IFlickrLibrary Flickr => this.flickr;

        public IGraphicsWindowLibrary GraphicsWindow => this.graphicsWindow;

        public IImageListLibrary ImageList => this.imageList;

        public IMathLibrary Math => this.math;

        public IMouseLibrary Mouse => this.mouse;

        public INetworkLibrary Network => this.network;

        public IProgramLibrary Program => this.program;

        public IShapesLibrary Shapes => this.shapes;

        public ISoundLibrary Sound => this.sound;

        public IStackLibrary Stack => this.stack;

        public ITextLibrary Text => this.text;

        public ITextWindowLibrary TextWindow => this.textWindow;

        public ITimerLibrary Timer => this.timer;

        public ITurtleLibrary Turtle => this.turtle;

        internal GraphicsWindowStyles Styles { get; set; }

        public void Dispose()
        {
            this.graphicsWindow.Dispose();
            this.textWindow.Dispose();
            this.timer.Dispose();
        }

        internal void ClearShapes() => this.shapes.Clear();

        internal void ClearControls() => this.controls.Clear();

        internal void ClearTurtle() => this.turtle.Clear();
    }
}
