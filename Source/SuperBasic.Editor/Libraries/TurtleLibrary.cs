// <copyright file="TurtleLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SuperBasic.Compiler.Runtime;
    using SuperBasic.Editor.Components;
    using SuperBasic.Editor.Libraries.Graphics;
    using SuperBasic.Editor.Libraries.Shapes;

    internal sealed class TurtleLibrary : ITurtleLibrary
    {
        private readonly LibrariesCollection libraries;
        private readonly List<LineGraphicsObject> lines;
        private readonly TurtleShape turtle;

        private bool isInitialized;
        private bool isPenDown;
        private decimal speed;

        public TurtleLibrary(LibrariesCollection libraries)
        {
            this.libraries = libraries;
            this.lines = new List<LineGraphicsObject>();
            this.turtle = new TurtleShape(libraries.Styles)
            {
                IsVisible = false
            };

            this.isInitialized = false;
            this.isPenDown = false;

            this.Set_X(320);
            this.Set_Y(240);
            this.Set_Angle(0);
            this.Set_Speed(5);
        }

        public decimal Get_Angle() => this.turtle.Angle;

        public void Set_Angle(decimal value) => this.turtle.Angle = value;

        public decimal Get_Speed() => this.speed;

        public void Set_Speed(decimal value) => this.speed = Math.Max(1, Math.Min(10, value));

        public decimal Get_X() => this.turtle.TranslateX + (this.turtle.Width / 2);

        public void Set_X(decimal value) => this.turtle.TranslateX = value - (this.turtle.Width / 2);

        public decimal Get_Y() => this.turtle.TranslateY + (this.turtle.Height / 2);

        public void Set_Y(decimal value) => this.turtle.TranslateY = value - (this.turtle.Height / 2);

        public void Hide()
        {
            this.Initialize();
            this.turtle.IsVisible = false;
        }

        public Task Move(decimal distance)
        {
            this.Initialize();

            decimal angle = this.Get_Angle() / 180 * (decimal)Math.PI;
            decimal duration = this.speed == 10
                ? 5
                : Math.Abs(distance * 320 / (this.speed * this.speed));

            decimal newY = this.Get_Y() - (decimal)((double)distance * Math.Cos((double)angle));
            decimal newX = this.Get_X() + (decimal)((double)distance * Math.Sin((double)angle));

            List<Task> tasks = new List<Task>();

            if (this.isPenDown)
            {
                var line = new LineGraphicsObject(
                    this.Get_X(),
                    this.Get_Y(),
                    this.Get_X() + 1,
                    this.Get_Y() + 1,
                    this.libraries.Styles);

                this.lines.Add(line);

                tasks.Add(line.Animate(newX, newY, duration));
            }

            // Animation does not understand the offset. Add it manually:
            tasks.Add(this.turtle.AnimateTranslation(newX - (this.turtle.Width / 2), newY - (this.turtle.Height / 2), duration));
            return Task.WhenAll(tasks);
        }

        public async Task MoveTo(decimal x, decimal y)
        {
            decimal distanceSquared = ((x - this.Get_X()) * (x - this.Get_X())) + ((y - this.Get_Y()) * (y - this.Get_Y()));

            if (distanceSquared == 0)
            {
                return;
            }

            decimal distance = (decimal)Math.Sqrt((double)distanceSquared);
            decimal angle = (decimal)Math.Acos((double)((this.Get_Y() - y) / distance)) * 180 / (decimal)Math.PI;

            if (x < this.Get_X())
            {
                angle = 360 - angle;
            }

            angle -= (this.Get_Angle() % 360);

            if (angle > 180)
            {
                angle -= 360;
            }

            await this.Turn(angle).ConfigureAwait(false);
            await this.Move(distance).ConfigureAwait(false);
        }

        public void PenDown()
        {
            this.Initialize();
            this.isPenDown = true;
        }

        public void PenUp()
        {
            this.Initialize();
            this.isPenDown = false;
        }

        public void Show()
        {
            this.Initialize();
            this.turtle.IsVisible = true;
        }

        public Task Turn(decimal angle)
        {
            this.Initialize();

            decimal newAngle = this.Get_Angle() + angle;
            decimal duration = this.speed == 10
                ? 5
                : Math.Abs((angle * 200 / (this.speed * this.speed)));

            return this.turtle.AnimateAngle(newAngle, duration);
        }

        public Task TurnLeft() => this.Turn(-90);

        public Task TurnRight() => this.Turn(90);

        internal void Clear()
        {
            this.Hide();
            this.lines.Clear();
        }

        internal void ComposeTree(TreeComposer composer)
        {
            foreach (var line in this.lines)
            {
                line.ComposeTree(composer);
            }

            this.turtle.ComposeTree(composer);
        }

        private void Initialize()
        {
            if (!this.isInitialized)
            {
                this.turtle.IsVisible = true;
                this.isPenDown = true;
                this.isInitialized = true;
            }
        }
    }
}
