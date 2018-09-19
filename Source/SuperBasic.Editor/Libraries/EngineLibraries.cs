// <copyright file="EngineLibraries.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests
{
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Libraries;

    public sealed class EngineLibraries : IEngineLibraries
    {
        public EngineLibraries()
        {
            // TODO: implement missing libraries
            this.Array = new ArrayLibrary();
            this.Clock = new ClockLibrary();
            // this.Controls = new ControlsLibrary();
            // this.GraphicsWindow = new GraphicsWindowLibrary();
            // this.ImageList = new ImageListLibrary();
            this.Math = new MathLibrary();
            this.Program = new ProgramLibrary();
            // this.Shapes = new ShapesLibrary();
            this.Stack = new StackLibrary();
            this.Text = new TextLibrary();
            // this.TextWindow = new TextWindowLibrary();
            this.Timer = new TimerLibrary();
            // this.Turtle = new TurtleLibrary();
        }

        public IArrayLibrary Array { get; private set; }

        public IClockLibrary Clock { get; private set; }

        public IControlsLibrary Controls { get; private set; }

        public IGraphicsWindowLibrary GraphicsWindow { get; private set; }

        public IImageListLibrary ImageList { get; private set; }

        public IMathLibrary Math { get; private set; }

        public IProgramLibrary Program { get; private set; }

        public IShapesLibrary Shapes { get; private set; }

        public IStackLibrary Stack { get; private set; }

        public ITextLibrary Text { get; private set; }

        public ITextWindowLibrary TextWindow { get; private set; }

        public ITimerLibrary Timer { get; private set; }

        public ITurtleLibrary Turtle { get; private set; }
    }
}
