// <copyright file="MathLibrary.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System;

    internal static partial class Libraries
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        private static decimal Execute_Math_Abs(decimal number) => Math.Abs(number);

        private static decimal Execute_Math_ArcCos(decimal cosValue) => (decimal)Math.Acos((double)cosValue);

        private static decimal Execute_Math_ArcSin(decimal sinValue) => (decimal)Math.Asin((double)sinValue);

        private static decimal Execute_Math_ArcTan(decimal tanValue) => (decimal)Math.Atan((double)tanValue);

        private static decimal Execute_Math_Ceiling(decimal number) => Math.Ceiling(number);

        private static decimal Execute_Math_Cos(decimal angle) => (decimal)Math.Cos((double)angle);

        private static decimal Execute_Math_Floor(decimal number) => Math.Floor(number);

        private static decimal Execute_Math_GetDegrees(decimal angle) => (180 * angle / (decimal)Math.PI) % 360;

        private static decimal Execute_Math_GetRadians(decimal angle) => (angle % 360) * (decimal)Math.PI / 180;

        private static decimal Execute_Math_GetRandomNumber(decimal maxNumber) => Random.Next((int)Math.Max(1, maxNumber) + 1);

        private static decimal Execute_Math_Log(decimal number) => (decimal)Math.Log10((double)number);

        private static decimal Execute_Math_Max(decimal number1, decimal number2) => Math.Max(number1, number2);

        private static decimal Execute_Math_Min(decimal number1, decimal number2) => Math.Min(number1, number2);

        private static decimal Execute_Math_NaturalLog(decimal number) => (decimal)Math.Log((double)number);

        private static decimal Execute_Math_Power(decimal baseNumber, decimal exponent) => (decimal)Math.Pow((double)baseNumber, (double)exponent);

        private static decimal Execute_Math_Remainder(decimal dividend, decimal divisor) => divisor == 0 ? 0 : (dividend % divisor);

        private static decimal Execute_Math_Round(decimal number) => Math.Round(number);

        private static decimal Execute_Math_Sin(decimal angle) => (decimal)Math.Sin((double)angle);

        private static decimal Execute_Math_SquareRoot(decimal number) => (decimal)Math.Sqrt((double)number);

        private static decimal Execute_Math_Tan(decimal angle) => (decimal)Math.Tan((double)angle);

        private static decimal Get_Math_Pi() => (decimal)Math.PI;
    }
}
