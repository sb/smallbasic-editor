// <copyright file="LibrariesCollection.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Editor.Libraries
{
    using System;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Editor.Libraries.Utilities;
    using SmallBasic.Editor.Store;

    public sealed class LibrariesCollection : IEngineLibraries, IDisposable
    {
        public LibrariesCollection()
        {
            this.Styles = new GraphicsWindowStyles(
                penWidth: 2,
                penColor: PredefinedColors.GetHexColor("Black"),
                brushColor: PredefinedColors.GetHexColor("SlateBlue"),
                fontBold: true,
                fontItalic: false,
                fontName: "Roboto",
                fontSize: 12);

            this.Array = new ArrayLibrary();
            this.Clock = new ClockLibrary();
            this.Controls = new ControlsLibrary();
            this.Desktop = new DesktopLibrary();
            this.Dictionary = new DictionaryLibrary();
            this.File = new FileLibrary();
            this.Flickr = new FlickrLibrary();
            this.GraphicsWindow = new GraphicsWindowLibrary(this);
            this.ImageList = new ImageListLibrary(this);
            this.Math = new MathLibrary();
            this.Mouse = new MouseLibrary(this);
            this.Network = new NetworkLibrary();
            this.Program = new ProgramLibrary();
            this.Shapes = new ShapesLibrary(this);
            this.Sound = new SoundLibrary();
            this.Stack = new StackLibrary();
            this.Text = new TextLibrary();
            this.TextWindow = new TextWindowLibrary();
            this.Timer = new TimerLibrary();
            this.Turtle = new TurtleLibrary(this);
        }

        IArrayLibrary IEngineLibraries.Array => this.Array;

        IClockLibrary IEngineLibraries.Clock => this.Clock;

        IControlsLibrary IEngineLibraries.Controls => this.Controls;

        IDesktopLibrary IEngineLibraries.Desktop => this.Desktop;

        IDictionaryLibrary IEngineLibraries.Dictionary => this.Dictionary;

        IFileLibrary IEngineLibraries.File => this.File;

        IFlickrLibrary IEngineLibraries.Flickr => this.Flickr;

        IGraphicsWindowLibrary IEngineLibraries.GraphicsWindow => this.GraphicsWindow;

        IImageListLibrary IEngineLibraries.ImageList => this.ImageList;

        IMathLibrary IEngineLibraries.Math => this.Math;

        IMouseLibrary IEngineLibraries.Mouse => this.Mouse;

        INetworkLibrary IEngineLibraries.Network => this.Network;

        IProgramLibrary IEngineLibraries.Program => this.Program;

        IShapesLibrary IEngineLibraries.Shapes => this.Shapes;

        ISoundLibrary IEngineLibraries.Sound => this.Sound;

        IStackLibrary IEngineLibraries.Stack => this.Stack;

        ITextLibrary IEngineLibraries.Text => this.Text;

        ITextWindowLibrary IEngineLibraries.TextWindow => this.TextWindow;

        ITimerLibrary IEngineLibraries.Timer => this.Timer;

        ITurtleLibrary IEngineLibraries.Turtle => this.Turtle;

        internal GraphicsWindowStyles Styles { get; set; }

        internal ArrayLibrary Array { get; private set; }

        internal ClockLibrary Clock { get; private set; }

        internal ControlsLibrary Controls { get; private set; }

        internal DesktopLibrary Desktop { get; private set; }

        internal DictionaryLibrary Dictionary { get; private set; }

        internal FileLibrary File { get; private set; }

        internal FlickrLibrary Flickr { get; private set; }

        internal GraphicsWindowLibrary GraphicsWindow { get; private set; }

        internal ImageListLibrary ImageList { get; private set; }

        internal MathLibrary Math { get; private set; }

        internal MouseLibrary Mouse { get; private set; }

        internal NetworkLibrary Network { get; private set; }

        internal ProgramLibrary Program { get; private set; }

        internal ShapesLibrary Shapes { get; private set; }

        internal SoundLibrary Sound { get; private set; }

        internal StackLibrary Stack { get; private set; }

        internal TextLibrary Text { get; private set; }

        internal TextWindowLibrary TextWindow { get; private set; }

        internal TimerLibrary Timer { get; private set; }

        internal TurtleLibrary Turtle { get; private set; }

        public void Dispose()
        {
            this.GraphicsWindow.Dispose();
            this.TextWindow.Dispose();
            this.Mouse.Dispose();
            this.Timer.Dispose();
        }
    }
}
