// <copyright file="TurtleLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;

    public interface ITurtlePlugin
    {
        decimal Angle { get; set; }

        decimal Speed { get; set; }

        decimal X { get; set; }

        decimal Y { get; set; }

        void Hide();

        void Show();

        void MoveTo(decimal x, decimal y, bool isPenDown);

        void RotateTo(decimal angle);
    }

    internal sealed class TurtleLibrary : ITurtleLibrary
    {
        private readonly ITurtlePlugin plugin;

        private bool isPenDown;

        public TurtleLibrary(ITurtlePlugin plugin)
        {
            this.plugin = plugin;

            this.isPenDown = true;

            this.X = 0;
            this.Y = 0;
            this.Angle = 0;
            this.Speed = 1;
        }

        public decimal Angle
        {
            get => this.plugin.Angle;
            set => this.plugin.Angle = value;
        }

        public decimal Speed
        {
            get => this.plugin.Speed;
            set => this.plugin.Speed = value;
        }

        public decimal X
        {
            get => this.plugin.X;
            set => this.plugin.X = value;
        }

        public decimal Y
        {
            get => this.plugin.Y;
            set => this.plugin.Y = value;
        }

        public void Hide() => this.plugin.Hide();

        public void Show() => this.plugin.Show();

        public void PenDown() => this.isPenDown = true;

        public void PenUp() => this.isPenDown = false;

        public void TurnLeft() => this.Turn(-90);

        public void TurnRight() => this.Turn(90);

        public void Turn(decimal angle)
        {
            decimal newAngle = this.Angle + angle;
            this.plugin.RotateTo(newAngle);
            this.Angle = newAngle;
        }

        public void Move(decimal distance)
        {
            double distanceDouble = (double)distance;
            double degrees = (double)this.Angle / 180 * Math.PI;
            decimal newX = this.X + (decimal)(distanceDouble * Math.Sin(degrees));
            decimal newY = this.Y - (decimal)(distanceDouble * Math.Cos(degrees));

            this.plugin.MoveTo(newY, newY, this.isPenDown);
            this.X = newX;
            this.Y = newY;
        }

        public void MoveTo(decimal x, decimal y)
        {
            double distanceSquared = (double)(((x - this.X) * (x - this.X)) + ((y - this.Y) * (y - this.Y)));
            if (distanceSquared > 0)
            {
                double distance = Math.Sqrt(distanceSquared);
                double degrees = Math.Acos((double)(this.Y - y) / distance) * 180 / Math.PI;
                if (x < this.X)
                {
                    degrees = 360 - degrees;
                }

                degrees -= (double)this.Angle % 360;
                if (degrees > 180)
                {
                    degrees -= 360;
                }

                this.Turn((decimal)degrees);
                this.Move((decimal)distance);
            }
        }
    }
}
