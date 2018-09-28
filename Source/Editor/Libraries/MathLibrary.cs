// <copyright file="MathLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Editor.Libraries
{
    using System;
    using SuperBasic.Compiler.Runtime;

    internal sealed class MathLibrary : IMathLibrary
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        public decimal Pi => (decimal)Math.PI;

        public decimal Abs(decimal number) => Math.Abs(number);

        public decimal ArcCos(decimal cosValue) => (decimal)Math.Acos((double)cosValue);

        public decimal ArcSin(decimal sinValue) => (decimal)Math.Asin((double)sinValue);

        public decimal ArcTan(decimal tanValue) => (decimal)Math.Atan((double)tanValue);

        public decimal Ceiling(decimal number) => Math.Ceiling(number);

        public decimal Cos(decimal angle) => (decimal)Math.Cos((double)angle);

        public decimal Floor(decimal number) => Math.Floor(number);

        public decimal GetDegrees(decimal angle) => (180 * angle / (decimal)Math.PI) % 360;

        public decimal GetRadians(decimal angle) => (angle % 360) * (decimal)Math.PI / 180;

        public decimal GetRandomNumber(decimal maxNumber) => Random.Next((int)Math.Max(1, maxNumber) + 1);

        public decimal Log(decimal number) => (decimal)Math.Log10((double)number);

        public decimal Max(decimal number1, decimal number2) => Math.Max(number1, number2);

        public decimal Min(decimal number1, decimal number2) => Math.Min(number1, number2);

        public decimal NaturalLog(decimal number) => (decimal)Math.Log((double)number);

        public decimal Power(decimal baseNumber, decimal exponent) => (decimal)Math.Pow((double)baseNumber, (double)exponent);

        public decimal Remainder(decimal dividend, decimal divisor) => divisor == 0 ? 0 : (dividend % divisor);

        public decimal Round(decimal number) => Math.Round(number);

        public decimal Sin(decimal angle) => (decimal)Math.Sin((double)angle);

        public decimal SquareRoot(decimal number) => (decimal)Math.Sqrt((double)number);

        public decimal Tan(decimal angle) => (decimal)Math.Tan((double)angle);
    }
}
