// <copyright file="OperatorTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Tests.Runtime
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using SmallBasic.Compiler;
    using SmallBasic.Compiler.Diagnostics;
    using Xunit;

    public sealed class OperatorTests
    {
        [Theory]
        // Adding numbers:
        [InlineData(@"1", @"2", "3")] // Number
        [InlineData(@"1", @"""2""", "3")] // Numeric String
        [InlineData(@"1", @"""a""", "1a")] // Char String
        [InlineData(@"1", @"""True""", "1True")] // Boolean
        [InlineData(@"3", @"ar", "31=2;")] // Array
        // Adding numeric strings:
        [InlineData(@"""5""", @"2", "7")] // Number
        [InlineData(@"""5""", @"""4""", "9")] // Numeric String
        [InlineData(@"""5""", @"""a""", "5a")] // Char String
        [InlineData(@"""5""", @"""False""", "5False")] // Boolean
        [InlineData(@"""5""", @"ar", "51=2;")] // Array
        // Adding char strings:
        [InlineData(@"""a""", @"2", "a2")] // Number
        [InlineData(@"""a""", @"""4""", "a4")] // Numeric String
        [InlineData(@"""a""", @"""a""", "aa")] // Char String
        [InlineData(@"""a""", @"""False""", "aFalse")] // Boolean
        [InlineData(@"""a""", @"ar", "a1=2;")] // Array
        // Adding booleans:
        [InlineData(@"""True""", @"2", "True2")] // Number
        [InlineData(@"""True""", @"""4""", "True4")] // Numeric String
        [InlineData(@"""True""", @"""a""", "Truea")] // Char String
        [InlineData(@"""True""", @"""False""", "TrueFalse")] // Boolean
        [InlineData(@"""True""", @"ar", "True1=2;")] // Array
        // Adding arrays:
        [InlineData(@"ar", @"2", "1=2;2")] // Number
        [InlineData(@"ar", @"""4""", "1=2;4")] // Numeric String
        [InlineData(@"ar", @"""a""", "1=2;a")] // Char String
        [InlineData(@"ar", @"""False""", "1=2;False")] // Boolean
        [InlineData(@"ar", @"ar", "1=2;1=2;")] // Array
        public Task ItEvaluatesAdditionOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} + {right}", result);
        }

        [Theory]
        // Subtracting numbers:
        [InlineData(@"1", @"5", "-4")] // Number
        [InlineData(@"1", @"""4""", "-3")] // Numeric String
        [InlineData(@"1", @"""a""", "1")] // Char String
        [InlineData(@"1", @"""False""", "1")] // Boolean
        [InlineData(@"1", @"ar", "1")] // Array
        // Subtracting numeric strings:
        [InlineData(@"""2""", @"5", "-3")] // Number
        [InlineData(@"""2""", @"""4""", "-2")] // Numeric String
        [InlineData(@"""2""", @"""a""", "2")] // Char String
        [InlineData(@"""2""", @"""False""", "2")] // Boolean
        [InlineData(@"""2""", @"ar", "2")] // Array
        // Subtracting char strings:
        [InlineData(@"""a""", @"5", "-5")] // Number
        [InlineData(@"""a""", @"""4""", "-4")] // Numeric String
        [InlineData(@"""a""", @"""a""", "0")] // Char String
        [InlineData(@"""a""", @"""False""", "0")] // Boolean
        [InlineData(@"""a""", @"ar", "0")] // Array
        // Subtracting booleans:
        [InlineData(@"""True""", @"5", "-5")] // Number
        [InlineData(@"""True""", @"""4""", "-4")] // Numeric String
        [InlineData(@"""True""", @"""a""", "0")] // Char String
        [InlineData(@"""True""", @"""False""", "0")] // Boolean
        [InlineData(@"""True""", @"ar", "0")] // Array
        // Subtracting arrays:
        [InlineData(@"ar", @"5", "-5")] // Number
        [InlineData(@"ar", @"""4""", "-4")] // Numeric String
        [InlineData(@"ar", @"""a""", "0")] // Char String
        [InlineData(@"ar", @"""False""", "0")] // Boolean
        [InlineData(@"ar", @"ar", "0")] // Array
        public Task ItEvaluatesSubtractionOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} - {right}", result);
        }

        [Theory]
        // Multiplying numbers:
        [InlineData(@"2", @"5", "10")] // Number
        [InlineData(@"2", @"""4""", "8")] // Numeric String
        [InlineData(@"2", @"""a""", "0")] // Char String
        [InlineData(@"2", @"""False""", "0")] // Boolean
        [InlineData(@"2", @"ar", "0")] // Array
        // Multiplying numeric strings:
        [InlineData(@"""2""", @"5", "10")] // Number
        [InlineData(@"""2""", @"""4""", "8")] // Numeric String
        [InlineData(@"""2""", @"""a""", "0")] // Char String
        [InlineData(@"""2""", @"""False""", "0")] // Boolean
        [InlineData(@"""2""", @"ar", "0")] // Array
        // Multiplying char strings:
        [InlineData(@"""a""", @"5", "0")] // Number
        [InlineData(@"""a""", @"""4""", "0")] // Numeric String
        [InlineData(@"""a""", @"""a""", "0")] // Char String
        [InlineData(@"""a""", @"""False""", "0")] // Boolean
        [InlineData(@"""a""", @"ar", "0")] // Array
        // Multiplying booleans:
        [InlineData(@"""True""", @"5", "0")] // Number
        [InlineData(@"""True""", @"""4""", "0")] // Numeric String
        [InlineData(@"""True""", @"""a""", "0")] // Char String
        [InlineData(@"""True""", @"""False""", "0")] // Boolean
        [InlineData(@"""True""", @"ar", "0")] // Array
        // Multiplying arrays:
        [InlineData(@"ar", @"5", "0")] // Number
        [InlineData(@"ar", @"""4""", "0")] // Numeric String
        [InlineData(@"ar", @"""a""", "0")] // Char String
        [InlineData(@"ar", @"""False""", "0")] // Boolean
        [InlineData(@"ar", @"ar", "0")] // Array
        public Task ItEvaluatesMultiplicationOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} * {right}", result);
        }

        [Theory]
        // Dividing numbers:
        [InlineData(@"10", @"5", "2")] // Number
        [InlineData(@"10", @"0", "10")] // Number (zero)
        [InlineData(@"10", @"""4""", "2.5")] // Numeric String
        [InlineData(@"10", @"""0""", "10")] // Numeric String (zero)
        [InlineData(@"10", @"""a""", "10")] // Char String
        [InlineData(@"10", @"""False""", "10")] // Boolean
        [InlineData(@"10", @"ar", "10")] // Array
        // Dividing numeric strings:
        [InlineData(@"""10""", @"5", "2")] // Number
        [InlineData(@"""10""", @"0", "10")] // Number (zero)
        [InlineData(@"""10""", @"""4""", "2.5")] // Numeric String
        [InlineData(@"""10""", @"""0""", "10")] // Numeric String (zero)
        [InlineData(@"""10""", @"""a""", "10")] // Char String
        [InlineData(@"""10""", @"""False""", "10")] // Boolean
        [InlineData(@"""10""", @"ar", "10")] // Array
        // Dividing char strings:
        [InlineData(@"""a""", @"5", "0")] // Number
        [InlineData(@"""a""", @"0", "0")] // Number (zero)
        [InlineData(@"""a""", @"""4""", "0")] // Numeric String
        [InlineData(@"""a""", @"""0""", "0")] // Numeric String (zero)
        [InlineData(@"""a""", @"""a""", "0")] // Char String
        [InlineData(@"""a""", @"""False""", "0")] // Boolean
        [InlineData(@"""a""", @"ar", "0")] // Array
        // Dividing booleans:
        [InlineData(@"""True""", @"5", "0")] // Number
        [InlineData(@"""True""", @"0", "0")] // Number (zero)
        [InlineData(@"""True""", @"""4""", "0")] // Numeric String
        [InlineData(@"""True""", @"""0""", "0")] // Numeric String (zero)
        [InlineData(@"""True""", @"""a""", "0")] // Char String
        [InlineData(@"""True""", @"""False""", "0")] // Boolean
        [InlineData(@"""True""", @"ar", "0")] // Array
        // Dividing arrays:
        [InlineData(@"ar", @"5", "0")] // Number
        [InlineData(@"ar", @"0", "0")] // Number (zero)
        [InlineData(@"ar", @"""4""", "0")] // Numeric String
        [InlineData(@"ar", @"""0""", "0")] // Numeric String (zero)
        [InlineData(@"ar", @"""a""", "0")] // Char String
        [InlineData(@"ar", @"""False""", "0")] // Boolean
        [InlineData(@"ar", @"ar", "0")] // Array
        public Task ItEvaluatesDivisionOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} / {right}", result);
        }

        [Theory]
        [InlineData(@"5", @"-5")] // Number
        [InlineData(@"""5""", @"-5")] // Numeric string
        [InlineData(@"""a""", @"0")] // Char string
        [InlineData(@"""True""", @"0")] // Boolean
        [InlineData(@"ar", @"0")] // Array
        public Task ItEvaluatesUnaryMinus(string expression, string result)
        {
            return EvaluateExpression($"-{expression}", result);
        }

        [Theory]
        // Numbers equal:
        [InlineData(@"1", @"1", "True")] // Number (True)
        [InlineData(@"1", @"2", "False")] // Number (False)
        [InlineData(@"1", @"""1""", "True")] // Numeric String (True)
        [InlineData(@"1", @"""2""", "False")] // Numeric String (False)
        [InlineData(@"1", @"""a""", "False")] // Char String
        [InlineData(@"1", @"""True""", "False")] // Boolean
        [InlineData(@"1", @"ar", "False")] // Array
        // Numeric strings equal:
        [InlineData(@"""1""", @"1", "True")] // Number (True)
        [InlineData(@"""1""", @"2", "False")] // Number (False)
        [InlineData(@"""1""", @"""1""", "True")] // Numeric String (True)
        [InlineData(@"""1""", @"""2""", "False")] // Numeric String (False)
        [InlineData(@"""1""", @"""a""", "False")] // Char String
        [InlineData(@"""1""", @"""True""", "False")] // Boolean
        [InlineData(@"""1""", @"ar", "False")] // Array
        // Char strings equal:
        [InlineData(@"""a""", @"2", "False")] // Number
        [InlineData(@"""a""", @"""4""", "False")] // Numeric String
        [InlineData(@"""a""", @"""a""", "True")] // Char String (True)
        [InlineData(@"""a""", @"""b""", "False")] // Char String (False)
        [InlineData(@"""a""", @"""False""", "False")] // Boolean
        [InlineData(@"""a""", @"ar", "False")] // Array
        // Booleans equal:
        [InlineData(@"""True""", @"2", "False")] // Number
        [InlineData(@"""True""", @"""4""", "False")] // Numeric String
        [InlineData(@"""True""", @"""a""", "False")] // Char String
        [InlineData(@"""True""", @"""True""", "True")] // Boolean (True)
        [InlineData(@"""True""", @"""False""", "False")] // Boolean (False)
        [InlineData(@"""True""", @"ar", "False")] // Array
        // Arrays equal:
        [InlineData(@"ar", @"2", "False")] // Number
        [InlineData(@"ar", @"""4""", "False")] // Numeric String
        [InlineData(@"ar", @"""a""", "False")] // Char String
        [InlineData(@"ar", @"""False""", "False")] // Boolean
        [InlineData(@"ar", @"ar", "True")] // Array
        public Task ItEvaluatesEqualOperator(string left, string right, string result)
        {
            return EvaluateExpression($"({left} = {right})", result);
        }

        [Theory]
        // Numbers not equal:
        [InlineData(@"1", @"1", "False")] // Number (False)
        [InlineData(@"1", @"2", "True")] // Number (True)
        [InlineData(@"1", @"""1""", "False")] // Numeric String (False)
        [InlineData(@"1", @"""2""", "True")] // Numeric String (True)
        [InlineData(@"1", @"""a""", "True")] // Char String
        [InlineData(@"1", @"""True""", "True")] // Boolean
        [InlineData(@"1", @"ar", "True")] // Array
        // Numeric not strings equal:
        [InlineData(@"""1""", @"1", "False")] // Number (False)
        [InlineData(@"""1""", @"2", "True")] // Number (True)
        [InlineData(@"""1""", @"""1""", "False")] // Numeric String (False)
        [InlineData(@"""1""", @"""2""", "True")] // Numeric String (True)
        [InlineData(@"""1""", @"""a""", "True")] // Char String
        [InlineData(@"""1""", @"""True""", "True")] // Boolean
        [InlineData(@"""1""", @"ar", "True")] // Array
        // Char strings not equal:
        [InlineData(@"""a""", @"2", "True")] // Number
        [InlineData(@"""a""", @"""4""", "True")] // Numeric String
        [InlineData(@"""a""", @"""a""", "False")] // Char String (False)
        [InlineData(@"""a""", @"""b""", "True")] // Char String (True)
        [InlineData(@"""a""", @"""False""", "True")] // Boolean
        [InlineData(@"""a""", @"ar", "True")] // Array
        // Booleans not equal:
        [InlineData(@"""True""", @"2", "True")] // Number
        [InlineData(@"""True""", @"""4""", "True")] // Numeric String
        [InlineData(@"""True""", @"""a""", "True")] // Char String
        [InlineData(@"""True""", @"""True""", "False")] // Boolean (False)
        [InlineData(@"""True""", @"""False""", "True")] // Boolean (True)
        [InlineData(@"""True""", @"ar", "True")] // Array
        // Arrays not equal:
        [InlineData(@"ar", @"2", "True")] // Number
        [InlineData(@"ar", @"""4""", "True")] // Numeric String
        [InlineData(@"ar", @"""a""", "True")] // Char String
        [InlineData(@"ar", @"""False""", "True")] // Boolean
        [InlineData(@"ar", @"ar", "False")] // Array
        public Task ItEvaluatesNotEqualOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} <> {right}", result);
        }

        [Theory]
        // Numbers smaller than:
        [InlineData(@"1", @"0", "False")] // Number (smaller than)
        [InlineData(@"1", @"1", "False")] // Number (equal)
        [InlineData(@"1", @"2", "True")] // Number (greater than)
        [InlineData(@"1", @"""0""", "False")] // Numeric String (smaller than)
        [InlineData(@"1", @"""1""", "False")] // Numeric String (equal)
        [InlineData(@"1", @"""2""", "True")] // Numeric String (greater than)
        [InlineData(@"-1", @"""a""", "True")] // Char String (smaller than)
        [InlineData(@"0", @"""a""", "False")] // Char String (equal)
        [InlineData(@"1", @"""a""", "False")] // Char String (greater than)
        [InlineData(@"-1", @"""True""", "True")] // Boolean (smaller than)
        [InlineData(@"0", @"""True""", "False")] // Boolean (equal)
        [InlineData(@"1", @"""True""", "False")] // Boolean (greater than)
        [InlineData(@"-1", @"ar", "True")] // Array (smaller than)
        [InlineData(@"0", @"ar", "False")] // Array (equal)
        [InlineData(@"1", @"ar", "False")] // Array (greater than)
        // Numeric strings smaller than:
        [InlineData(@"""1""", @"0", "False")] // Number (smaller than)
        [InlineData(@"""1""", @"1", "False")] // Number (equal)
        [InlineData(@"""1""", @"2", "True")] // Number (greater than)
        [InlineData(@"""1""", @"""0""", "False")] // Numeric String (smaller than)
        [InlineData(@"""1""", @"""1""", "False")] // Numeric String (equal)
        [InlineData(@"""1""", @"""2""", "True")] // Numeric String (greater than)
        [InlineData(@"""-1""", @"""a""", "True")] // Char String (smaller than)
        [InlineData(@"""0""", @"""a""", "False")] // Char String (equal)
        [InlineData(@"""1""", @"""a""", "False")] // Char String (greater than)
        [InlineData(@"""-1""", @"""True""", "True")] // Boolean (smaller than)
        [InlineData(@"""0""", @"""True""", "False")] // Boolean (equal)
        [InlineData(@"""1""", @"""True""", "False")] // Boolean (greater than)
        [InlineData(@"""-1""", @"ar", "True")] // Array (smaller than)
        [InlineData(@"""0""", @"ar", "False")] // Array (equal)
        [InlineData(@"""1""", @"ar", "False")] // Array (greater than)
        // Char strings smaller than:
        [InlineData(@"""a""", @"-1", "False")] // Number (smaller than)
        [InlineData(@"""a""", @"0", "False")] // Number (equal)
        [InlineData(@"""a""", @"1", "True")] // Number (greater than)
        [InlineData(@"""a""", @"""-1""", "False")] // Numeric String (smaller than)
        [InlineData(@"""a""", @"""0""", "False")] // Numeric String (equal)
        [InlineData(@"""a""", @"""1""", "True")] // Numeric String (greater than)
        [InlineData(@"""a""", @"""a""", "False")] // Char String
        [InlineData(@"""a""", @"""False""", "False")] // Boolean
        [InlineData(@"""a""", @"ar", "False")] // Array
        // Booleans smaller than:
        [InlineData(@"""True""", @"-1", "False")] // Number (smaller than)
        [InlineData(@"""True""", @"0", "False")] // Number (equal)
        [InlineData(@"""True""", @"1", "True")] // Number (greater than)
        [InlineData(@"""True""", @"""-1""", "False")] // Numeric String (smaller than)
        [InlineData(@"""True""", @"""0""", "False")] // Numeric String (equal)
        [InlineData(@"""True""", @"""1""", "True")] // Numeric String (greater than)
        [InlineData(@"""True""", @"""a""", "False")] // Char String
        [InlineData(@"""True""", @"""False""", "False")] // Boolean
        [InlineData(@"""True""", @"ar", "False")] // Array
        // Arrays smaller than:
        [InlineData(@"ar", @"-1", "False")] // Number (smaller than)
        [InlineData(@"ar", @"0", "False")] // Number (equal)
        [InlineData(@"ar", @"1", "True")] // Number (greater than)
        [InlineData(@"ar", @"""-1""", "False")] // Numeric String (smaller than)
        [InlineData(@"ar", @"""0""", "False")] // Numeric String (equal)
        [InlineData(@"ar", @"""1""", "True")] // Numeric String (greater than)
        [InlineData(@"ar", @"""a""", "False")] // Char String
        [InlineData(@"ar", @"""False""", "False")] // Boolean
        [InlineData(@"ar", @"ar", "False")] // Array
        public Task ItEvaluatesLessThanOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} < {right}", result);
        }

        [Theory]
        // Numbers smaller than or equal:
        [InlineData(@"1", @"0", "False")] // Number (smaller than)
        [InlineData(@"1", @"1", "True")] // Number (equal)
        [InlineData(@"1", @"2", "True")] // Number (greater than)
        [InlineData(@"1", @"""0""", "False")] // Numeric String (smaller than)
        [InlineData(@"1", @"""1""", "True")] // Numeric String (equal)
        [InlineData(@"1", @"""2""", "True")] // Numeric String (greater than)
        [InlineData(@"-1", @"""a""", "True")] // Char String (smaller than)
        [InlineData(@"0", @"""a""", "True")] // Char String (equal)
        [InlineData(@"1", @"""a""", "False")] // Char String (greater than)
        [InlineData(@"-1", @"""True""", "True")] // Boolean (smaller than)
        [InlineData(@"0", @"""True""", "True")] // Boolean (equal)
        [InlineData(@"1", @"""True""", "False")] // Boolean (greater than)
        [InlineData(@"-1", @"ar", "True")] // Array (smaller than)
        [InlineData(@"0", @"ar", "True")] // Array (equal)
        [InlineData(@"1", @"ar", "False")] // Array (greater than)
        // Numeric strings smaller than or equal:
        [InlineData(@"""1""", @"0", "False")] // Number (smaller than)
        [InlineData(@"""1""", @"1", "True")] // Number (equal)
        [InlineData(@"""1""", @"2", "True")] // Number (greater than)
        [InlineData(@"""1""", @"""0""", "False")] // Numeric String (smaller than)
        [InlineData(@"""1""", @"""1""", "True")] // Numeric String (equal)
        [InlineData(@"""1""", @"""2""", "True")] // Numeric String (greater than)
        [InlineData(@"""-1""", @"""a""", "True")] // Char String (smaller than)
        [InlineData(@"""0""", @"""a""", "True")] // Char String (equal)
        [InlineData(@"""1""", @"""a""", "False")] // Char String (greater than)
        [InlineData(@"""-1""", @"""True""", "True")] // Boolean (smaller than)
        [InlineData(@"""0""", @"""True""", "True")] // Boolean (equal)
        [InlineData(@"""1""", @"""True""", "False")] // Boolean (greater than)
        [InlineData(@"""-1""", @"ar", "True")] // Array (smaller than)
        [InlineData(@"""0""", @"ar", "True")] // Array (equal)
        [InlineData(@"""1""", @"ar", "False")] // Array (greater than)
        // Char strings smaller than or equal:
        [InlineData(@"""a""", @"-1", "False")] // Number (smaller than)
        [InlineData(@"""a""", @"0", "True")] // Number (equal)
        [InlineData(@"""a""", @"1", "True")] // Number (greater than)
        [InlineData(@"""a""", @"""-1""", "False")] // Numeric String (smaller than)
        [InlineData(@"""a""", @"""0""", "True")] // Numeric String (equal)
        [InlineData(@"""a""", @"""1""", "True")] // Numeric String (greater than)
        [InlineData(@"""a""", @"""a""", "True")] // Char String
        [InlineData(@"""a""", @"""False""", "True")] // Boolean
        [InlineData(@"""a""", @"ar", "True")] // Array
        // Booleans smaller than or equal:
        [InlineData(@"""True""", @"-1", "False")] // Number (smaller than)
        [InlineData(@"""True""", @"0", "True")] // Number (equal)
        [InlineData(@"""True""", @"1", "True")] // Number (greater than)
        [InlineData(@"""True""", @"""-1""", "False")] // Numeric String (smaller than)
        [InlineData(@"""True""", @"""0""", "True")] // Numeric String (equal)
        [InlineData(@"""True""", @"""1""", "True")] // Numeric String (greater than)
        [InlineData(@"""True""", @"""a""", "True")] // Char String
        [InlineData(@"""True""", @"""False""", "True")] // Boolean
        [InlineData(@"""True""", @"ar", "True")] // Array
        // Arrays smaller than or equal:
        [InlineData(@"ar", @"-1", "False")] // Number (smaller than)
        [InlineData(@"ar", @"0", "True")] // Number (equal)
        [InlineData(@"ar", @"1", "True")] // Number (greater than)
        [InlineData(@"ar", @"""-1""", "False")] // Numeric String (smaller than)
        [InlineData(@"ar", @"""0""", "True")] // Numeric String (equal)
        [InlineData(@"ar", @"""1""", "True")] // Numeric String (greater than)
        [InlineData(@"ar", @"""a""", "True")] // Char String
        [InlineData(@"ar", @"""False""", "True")] // Boolean
        [InlineData(@"ar", @"ar", "True")] // Array
        public Task ItEvaluatesLessThanOrEqualOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} <= {right}", result);
        }

        [Theory]
        // Numbers greater than:
        [InlineData(@"1", @"0", "True")] // Number (smaller than)
        [InlineData(@"1", @"1", "False")] // Number (equal)
        [InlineData(@"1", @"2", "False")] // Number (greater than)
        [InlineData(@"1", @"""0""", "True")] // Numeric String (smaller than)
        [InlineData(@"1", @"""1""", "False")] // Numeric String (equal)
        [InlineData(@"1", @"""2""", "False")] // Numeric String (greater than)
        [InlineData(@"-1", @"""a""", "False")] // Char String (smaller than)
        [InlineData(@"0", @"""a""", "False")] // Char String (equal)
        [InlineData(@"1", @"""a""", "True")] // Char String (greater than)
        [InlineData(@"-1", @"""True""", "False")] // Boolean (smaller than)
        [InlineData(@"0", @"""True""", "False")] // Boolean (equal)
        [InlineData(@"1", @"""True""", "True")] // Boolean (greater than)
        [InlineData(@"-1", @"ar", "False")] // Array (smaller than)
        [InlineData(@"0", @"ar", "False")] // Array (equal)
        [InlineData(@"1", @"ar", "True")] // Array (greater than)
        // Numeric strings greater than:
        [InlineData(@"""1""", @"0", "True")] // Number (smaller than)
        [InlineData(@"""1""", @"1", "False")] // Number (equal)
        [InlineData(@"""1""", @"2", "False")] // Number (greater than)
        [InlineData(@"""1""", @"""0""", "True")] // Numeric String (smaller than)
        [InlineData(@"""1""", @"""1""", "False")] // Numeric String (equal)
        [InlineData(@"""1""", @"""2""", "False")] // Numeric String (greater than)
        [InlineData(@"""-1""", @"""a""", "False")] // Char String (smaller than)
        [InlineData(@"""0""", @"""a""", "False")] // Char String (equal)
        [InlineData(@"""1""", @"""a""", "True")] // Char String (greater than)
        [InlineData(@"""-1""", @"""True""", "False")] // Boolean (smaller than)
        [InlineData(@"""0""", @"""True""", "False")] // Boolean (equal)
        [InlineData(@"""1""", @"""True""", "True")] // Boolean (greater than)
        [InlineData(@"""-1""", @"ar", "False")] // Array (smaller than)
        [InlineData(@"""0""", @"ar", "False")] // Array (equal)
        [InlineData(@"""1""", @"ar", "True")] // Array (greater than)
        // Char strings greater than:
        [InlineData(@"""a""", @"-1", "True")] // Number (smaller than)
        [InlineData(@"""a""", @"0", "False")] // Number (equal)
        [InlineData(@"""a""", @"1", "False")] // Number (greater than)
        [InlineData(@"""a""", @"""-1""", "True")] // Numeric String (smaller than)
        [InlineData(@"""a""", @"""0""", "False")] // Numeric String (equal)
        [InlineData(@"""a""", @"""1""", "False")] // Numeric String (greater than)
        [InlineData(@"""a""", @"""a""", "False")] // Char String
        [InlineData(@"""a""", @"""False""", "False")] // Boolean
        [InlineData(@"""a""", @"ar", "False")] // Array
        // Booleans greater than:
        [InlineData(@"""True""", @"-1", "True")] // Number (smaller than)
        [InlineData(@"""True""", @"0", "False")] // Number (equal)
        [InlineData(@"""True""", @"1", "False")] // Number (greater than)
        [InlineData(@"""True""", @"""-1""", "True")] // Numeric String (smaller than)
        [InlineData(@"""True""", @"""0""", "False")] // Numeric String (equal)
        [InlineData(@"""True""", @"""1""", "False")] // Numeric String (greater than)
        [InlineData(@"""True""", @"""a""", "False")] // Char String
        [InlineData(@"""True""", @"""False""", "False")] // Boolean
        [InlineData(@"""True""", @"ar", "False")] // Array
        // Arrays greater than:
        [InlineData(@"ar", @"-1", "True")] // Number (smaller than)
        [InlineData(@"ar", @"0", "False")] // Number (equal)
        [InlineData(@"ar", @"1", "False")] // Number (greater than)
        [InlineData(@"ar", @"""-1""", "True")] // Numeric String (smaller than)
        [InlineData(@"ar", @"""0""", "False")] // Numeric String (equal)
        [InlineData(@"ar", @"""1""", "False")] // Numeric String (greater than)
        [InlineData(@"ar", @"""a""", "False")] // Char String
        [InlineData(@"ar", @"""False""", "False")] // Boolean
        [InlineData(@"ar", @"ar", "False")] // Array
        public Task ItEvaluatesGreaterThanOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} > {right}", result);
        }

        [Theory]
        // Numbers greater than or equal:
        [InlineData(@"1", @"0", "True")] // Number (smaller than)
        [InlineData(@"1", @"1", "True")] // Number (equal)
        [InlineData(@"1", @"2", "False")] // Number (greater than)
        [InlineData(@"1", @"""0""", "True")] // Numeric String (smaller than)
        [InlineData(@"1", @"""1""", "True")] // Numeric String (equal)
        [InlineData(@"1", @"""2""", "False")] // Numeric String (greater than)
        [InlineData(@"-1", @"""a""", "False")] // Char String (smaller than)
        [InlineData(@"0", @"""a""", "True")] // Char String (equal)
        [InlineData(@"1", @"""a""", "True")] // Char String (greater than)
        [InlineData(@"-1", @"""True""", "False")] // Boolean (smaller than)
        [InlineData(@"0", @"""True""", "True")] // Boolean (equal)
        [InlineData(@"1", @"""True""", "True")] // Boolean (greater than)
        [InlineData(@"-1", @"ar", "False")] // Array (smaller than)
        [InlineData(@"0", @"ar", "True")] // Array (equal)
        [InlineData(@"1", @"ar", "True")] // Array (greater than)
        // Numeric strings greater than or equal:
        [InlineData(@"""1""", @"0", "True")] // Number (smaller than)
        [InlineData(@"""1""", @"1", "True")] // Number (equal)
        [InlineData(@"""1""", @"2", "False")] // Number (greater than)
        [InlineData(@"""1""", @"""0""", "True")] // Numeric String (smaller than)
        [InlineData(@"""1""", @"""1""", "True")] // Numeric String (equal)
        [InlineData(@"""1""", @"""2""", "False")] // Numeric String (greater than)
        [InlineData(@"""-1""", @"""a""", "False")] // Char String (smaller than)
        [InlineData(@"""0""", @"""a""", "True")] // Char String (equal)
        [InlineData(@"""1""", @"""a""", "True")] // Char String (greater than)
        [InlineData(@"""-1""", @"""True""", "False")] // Boolean (smaller than)
        [InlineData(@"""0""", @"""True""", "True")] // Boolean (equal)
        [InlineData(@"""1""", @"""True""", "True")] // Boolean (greater than)
        [InlineData(@"""-1""", @"ar", "False")] // Array (smaller than)
        [InlineData(@"""0""", @"ar", "True")] // Array (equal)
        [InlineData(@"""1""", @"ar", "True")] // Array (greater than)
        // Char strings greater than or equal:
        [InlineData(@"""a""", @"-1", "True")] // Number (smaller than)
        [InlineData(@"""a""", @"0", "True")] // Number (equal)
        [InlineData(@"""a""", @"1", "False")] // Number (greater than)
        [InlineData(@"""a""", @"""-1""", "True")] // Numeric String (smaller than)
        [InlineData(@"""a""", @"""0""", "True")] // Numeric String (equal)
        [InlineData(@"""a""", @"""1""", "False")] // Numeric String (greater than)
        [InlineData(@"""a""", @"""a""", "True")] // Char String
        [InlineData(@"""a""", @"""False""", "True")] // Boolean
        [InlineData(@"""a""", @"ar", "True")] // Array
        // Booleans greater than or equal:
        [InlineData(@"""True""", @"-1", "True")] // Number (smaller than)
        [InlineData(@"""True""", @"0", "True")] // Number (equal)
        [InlineData(@"""True""", @"1", "False")] // Number (greater than)
        [InlineData(@"""True""", @"""-1""", "True")] // Numeric String (smaller than)
        [InlineData(@"""True""", @"""0""", "True")] // Numeric String (equal)
        [InlineData(@"""True""", @"""1""", "False")] // Numeric String (greater than)
        [InlineData(@"""True""", @"""a""", "True")] // Char String
        [InlineData(@"""True""", @"""False""", "True")] // Boolean
        [InlineData(@"""True""", @"ar", "True")] // Array
        // Arrays greater than or equal:
        [InlineData(@"ar", @"-1", "True")] // Number (smaller than)
        [InlineData(@"ar", @"0", "True")] // Number (equal)
        [InlineData(@"ar", @"1", "False")] // Number (greater than)
        [InlineData(@"ar", @"""-1""", "True")] // Numeric String (smaller than)
        [InlineData(@"ar", @"""0""", "True")] // Numeric String (equal)
        [InlineData(@"ar", @"""1""", "False")] // Numeric String (greater than)
        [InlineData(@"ar", @"""a""", "True")] // Char String
        [InlineData(@"ar", @"""False""", "True")] // Boolean
        [InlineData(@"ar", @"ar", "True")] // Array
        public Task ItEvaluatesGreaterThanOrEqualOperator(string left, string right, string result)
        {
            return EvaluateExpression($"{left} >= {right}", result);
        }

        [Theory]
        // Truth table:
        [InlineData(@"""True""", @"""True""", "True")]
        [InlineData(@"""True""", @"""False""", "False")]
        [InlineData(@"""False""", @"""False""", "False")]
        // Numbers:
        [InlineData(@"1", @"""True""", "False")]
        [InlineData(@"1", @"""False""", "False")]
        // Numeric Strings:
        [InlineData(@"""1""", @"""True""", "False")]
        [InlineData(@"""1""", @"""False""", "False")]
        // Char strings:
        [InlineData(@"""a""", @"""True""", "False")]
        [InlineData(@"""a""", @"""False""", "False")]
        // Arrays:
        [InlineData(@"ar", @"""True""", "False")]
        [InlineData(@"ar", @"""False""", "False")]
        public async Task ItEvaluatesAndOperator(string left, string right, string result)
        {
            await EvaluateExpression($"({left} and {right})", result).ConfigureAwait(false);
            await EvaluateExpression($"({right} and {left})", result).ConfigureAwait(false);
        }

        [Theory]
        // Truth table:
        [InlineData(@"""True""", @"""True""", "True")]
        [InlineData(@"""True""", @"""False""", "True")]
        [InlineData(@"""False""", @"""False""", "False")]
        // Numbers:
        [InlineData(@"1", @"""True""", "True")]
        [InlineData(@"1", @"""False""", "False")]
        // Numeric Strings:
        [InlineData(@"""1""", @"""True""", "True")]
        [InlineData(@"""1""", @"""False""", "False")]
        // Char strings:
        [InlineData(@"""a""", @"""True""", "True")]
        [InlineData(@"""a""", @"""False""", "False")]
        // Arrays:
        [InlineData(@"ar", @"""True""", "True")]
        [InlineData(@"ar", @"""False""", "False")]
        public async Task ItEvaluatesOrOperator(string left, string right, string result)
        {
            await EvaluateExpression($"({left} or {right})", result).ConfigureAwait(false);
            await EvaluateExpression($"({right} or {left})", result).ConfigureAwait(false);
        }

        private static Task EvaluateExpression(string expression, string result)
        {
            return new SmallBasicCompilation($@"
ar[1] = 2
x = {expression}").VerifyRuntime(memoryContents: $@"
ar = 1=2;
x = {result}");
        }
    }
}
