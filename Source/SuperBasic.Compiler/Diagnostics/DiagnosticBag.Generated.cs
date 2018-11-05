// <copyright file="DiagnosticBag.Generated.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

/// <summary>
/// This file is auto-generated by a build task. It shouldn't be edited by hand.
/// </summary>
namespace SuperBasic.Compiler.Diagnostics
{
    using System.Collections.Generic;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Utilities;

    internal sealed class DiagnosticBag
    {
        private readonly List<Diagnostic> builder = new List<Diagnostic>();

        public IReadOnlyList<Diagnostic> Contents => this.builder;

        public void ReportUnrecognizedCharacter(TextRange range, char character)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnrecognizedCharacter, range, character.ToDisplayString()));
        }

        public void ReportUnterminatedStringLiteral(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnterminatedStringLiteral, range));
        }

        public void ReportUnexpectedTokenFound(TextRange range, TokenKind found, TokenKind expected)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnexpectedTokenFound, range, found.ToDisplayString(), expected.ToDisplayString()));
        }

        public void ReportUnexpectedEndOfStream(TextRange range, TokenKind expected)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnexpectedEndOfStream, range, expected.ToDisplayString()));
        }

        public void ReportUnexpectedStatementInsteadOfNewLine(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnexpectedStatementInsteadOfNewLine, range));
        }

        public void ReportUnexpectedTokenInsteadOfStatement(TextRange range, TokenKind found)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnexpectedTokenInsteadOfStatement, range, found.ToDisplayString()));
        }

        public void ReportTwoSubModulesWithTheSameName(TextRange range, string name)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.TwoSubModulesWithTheSameName, range, name.ToDisplayString()));
        }

        public void ReportTwoLabelsWithTheSameName(TextRange range, string label)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.TwoLabelsWithTheSameName, range, label.ToDisplayString()));
        }

        public void ReportGoToUndefinedLabel(TextRange range, string label)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.GoToUndefinedLabel, range, label.ToDisplayString()));
        }

        public void ReportPropertyHasNoSetter(TextRange range, string library, string property)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.PropertyHasNoSetter, range, library.ToDisplayString(), property.ToDisplayString()));
        }

        public void ReportAssigningNonSubModuleToEvent(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.AssigningNonSubModuleToEvent, range));
        }

        public void ReportUnassignedExpressionStatement(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnassignedExpressionStatement, range));
        }

        public void ReportInvalidExpressionStatement(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.InvalidExpressionStatement, range));
        }

        public void ReportUnsupportedArrayBaseExpression(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnsupportedArrayBaseExpression, range));
        }

        public void ReportValueIsNotANumber(TextRange range, string value)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.ValueIsNotANumber, range, value.ToDisplayString()));
        }

        public void ReportUnsupportedDotBaseExpression(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnsupportedDotBaseExpression, range));
        }

        public void ReportExpectedExpressionWithAValue(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.ExpectedExpressionWithAValue, range));
        }

        public void ReportLibraryMemberNotFound(TextRange range, string library, string member)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.LibraryMemberNotFound, range, library.ToDisplayString(), member.ToDisplayString()));
        }

        public void ReportUnexpectedArgumentsCount(TextRange range, int actualCount, int expectedCount)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnexpectedArgumentsCount, range, actualCount.ToDisplayString(), expectedCount.ToDisplayString()));
        }

        public void ReportUnsupportedInvocationBaseExpression(TextRange range)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.UnsupportedInvocationBaseExpression, range));
        }

        public void ReportLibraryMemberDeprecatedFromOlderVersion(TextRange range, string library, string member)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.LibraryMemberDeprecatedFromOlderVersion, range, library.ToDisplayString(), member.ToDisplayString()));
        }

        public void ReportLibraryMemberNeedsDesktop(TextRange range, string library, string member)
        {
            this.builder.Add(new Diagnostic(DiagnosticCode.LibraryMemberNeedsDesktop, range, library.ToDisplayString(), member.ToDisplayString()));
        }
    }
}
