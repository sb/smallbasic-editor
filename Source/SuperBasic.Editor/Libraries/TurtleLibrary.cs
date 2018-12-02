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

            /*
            this.Set_X(0);
            this.Set_Y(0);
            this.Set_Angle(0);
            this.Set_Speed(1);
            */
        }

        public decimal Get_Angle() => this.plugin.Angle;

        public void Set_Angle(decimal value) => this.plugin.Angle = value;

        public decimal Get_Speed() => this.plugin.Speed;

        public void Set_Speed(decimal value) => this.plugin.Speed = value;

        public decimal Get_X() => this.plugin.X;

        public void Set_X(decimal value) => this.plugin.X = value;

        public decimal Get_Y() => this.plugin.Y;

        public void Set_Y(decimal value) => this.plugin.Y = value;

        public void Hide() => this.plugin.Hide();

        public void Show() => this.plugin.Show();

        public void PenDown() => this.isPenDown = true;

        public void PenUp() => this.isPenDown = false;

        public void TurnLeft() => this.Turn(-90);

        public void TurnRight() => this.Turn(90);

        public void Turn(decimal angle)
        {
            decimal newAngle = this.Get_Angle() + angle;
            this.plugin.RotateTo(newAngle);
            this.Set_Angle(newAngle);
        }

        public void Move(decimal distance)
        {
            double distanceDouble = (double)distance;
            double degrees = (double)this.Get_Angle() / 180 * Math.PI;
            decimal newX = this.Get_X() + (decimal)(distanceDouble * Math.Sin(degrees));
            decimal newY = this.Get_Y() - (decimal)(distanceDouble * Math.Cos(degrees));

            this.plugin.MoveTo(newY, newY, this.isPenDown);
            this.Set_X(newX);
            this.Set_Y(newY);
        }

        public void MoveTo(decimal x, decimal y)
        {
            decimal currentX = this.Get_X();
            decimal currentY = this.Get_Y();
            double distanceSquared = (double)(((x - currentX) * (x - currentX)) + ((y - currentY) * (y - currentY)));
            if (distanceSquared > 0)
            {
                double distance = Math.Sqrt(distanceSquared);
                double degrees = Math.Acos((double)(currentY - y) / distance) * 180 / Math.PI;
                if (x < currentX)
                {
                    degrees = 360 - degrees;
                }

                degrees -= (double)this.Get_Angle() % 360;
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
