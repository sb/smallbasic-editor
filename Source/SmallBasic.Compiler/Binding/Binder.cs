// <copyright file="Binder.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Compiler.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SmallBasic.Compiler.Diagnostics;
    using SmallBasic.Compiler.Parsing;
    using SmallBasic.Compiler.Runtime;
    using SmallBasic.Compiler.Scanning;
    using SmallBasic.Utilities;

    internal sealed class Binder
    {
        private readonly DiagnosticBag diagnostics;
        private readonly bool isRunningOnDesktop;
        private readonly IReadOnlyCollection<string> definedSubModules;

        public Binder(StatementBlockSyntax syntaxTree, DiagnosticBag diagnostics, bool isRunningOnDesktop)
        {
            this.diagnostics = diagnostics;
            this.isRunningOnDesktop = isRunningOnDesktop;
            this.definedSubModules = new SubModuleNamesCollector(syntaxTree).Names;

            var mainModule = new List<BaseBoundStatement>();
            var subModules = new Dictionary<string, BoundSubModule>();

            foreach (var syntax in syntaxTree.Body)
            {
                switch (syntax)
                {
                    case SubModuleStatementSyntax subModuleStatement:
                        {
                            var body = this.BindStatementBlock(subModuleStatement.Body);
                            var subModule = new BoundSubModule(subModuleStatement, subModuleStatement.NameToken.Text, body);

                            if (subModules.ContainsKey(subModule.Name))
                            {
                                this.diagnostics.ReportTwoSubModulesWithTheSameName(subModuleStatement.NameToken.Range, subModule.Name);
                            }
                            else
                            {
                                subModules.Add(subModule.Name, subModule);
                            }

                            break;
                        }

                    default:
                        {
                            var statement = this.BindStatementOpt(syntax);
                            if (!statement.IsDefault())
                            {
                                mainModule.Add(statement);
                            }

                            break;
                        }
                }
            }

            this.MainModule = new BoundStatementBlock(syntaxTree, mainModule);
            this.CheckForLabelErrors(this.MainModule);

            this.SubModules = subModules;
            foreach (var subModule in this.SubModules.Values)
            {
                this.CheckForLabelErrors(subModule.Body);
            }
        }

        public BoundStatementBlock MainModule { get; private set; }

        public IReadOnlyDictionary<string, BoundSubModule> SubModules { get; private set; }

        private void CheckForLabelErrors(BoundStatementBlock module)
        {
            var labelsCollector = new LabelDefinitionsCollector(this.diagnostics, module);
            var gotoChecker = new GoToUndefinedLabelChecker(this.diagnostics, labelsCollector.Labels, module);
        }

        private BaseBoundStatement BindStatementOpt(BaseStatementSyntax syntax)
        {
            switch (syntax)
            {
                case StatementBlockSyntax statementBlock: return this.BindStatementBlock(statementBlock);
                case IfStatementSyntax ifStatement: return this.BindIfStatement(ifStatement);
                case WhileStatementSyntax whileStatement: return this.BindWhileStatement(whileStatement);
                case ForStatementSyntax forStatement: return this.BindForStatement(forStatement);
                case ExpressionStatementSyntax expressionStatement: return this.BindExpressionStatement(expressionStatement);

                case LabelStatementSyntax labelStatement: return new BoundLabelStatement(labelStatement, labelStatement.LabelToken.Text);
                case GoToStatementSyntax goToStatement: return new BoundGoToStatement(goToStatement, goToStatement.LabelToken.Text);

                case UnrecognizedStatementSyntax unrecognizedStatement: return null;
                case CommentStatementSyntax commentStatement: return null;

                default: throw ExceptionUtilities.UnexpectedValue(syntax);
            }
        }

        private BoundStatementBlock BindStatementBlock(StatementBlockSyntax syntax)
        {
            var statements = new List<BaseBoundStatement>();

            foreach (var child in syntax.Body)
            {
                var statement = this.BindStatementOpt(child);

                if (!statement.IsDefault())
                {
                    statements.Add(statement);
                }
            }

            return new BoundStatementBlock(syntax, statements);
        }

        private BoundIfStatement BindIfStatement(IfStatementSyntax syntax)
        {
            BaseBoundExpression ifExpression = this.BindExpression(syntax.IfPart.Condition);
            BoundStatementBlock ifBody = this.BindStatementBlock(syntax.IfPart.Body);
            BoundIfPart ifPart = new BoundIfPart(syntax.IfPart, ifExpression, ifBody);

            List<BoundElseIfPart> elseIfParts = new List<BoundElseIfPart>();
            foreach (var part in syntax.ElseIfParts)
            {
                BaseBoundExpression elseIfExpression = this.BindExpression(part.Condition);
                BoundStatementBlock elseIfBody = this.BindStatementBlock(part.Body);
                elseIfParts.Add(new BoundElseIfPart(part, elseIfExpression, elseIfBody));
            }

            BoundElsePart elsePart = null;
            if (!syntax.ElsePartOpt.IsDefault())
            {
                BoundStatementBlock elseBody = this.BindStatementBlock(syntax.ElsePartOpt.Body);
                elsePart = new BoundElsePart(syntax.ElsePartOpt, elseBody);
            }

            return new BoundIfStatement(syntax, ifPart, elseIfParts, elsePart);
        }

        private BoundWhileStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            BaseBoundExpression expression = this.BindExpression(syntax.Condition);
            BoundStatementBlock body = this.BindStatementBlock(syntax.Body);

            return new BoundWhileStatement(syntax, expression, body);
        }

        private BoundForStatement BindForStatement(ForStatementSyntax syntax)
        {
            string identifier = syntax.IdentifierToken.Text;

            BaseBoundExpression fromExpression = this.BindExpression(syntax.FromExpression);
            BaseBoundExpression toExpression = this.BindExpression(syntax.ToExpression);

            BaseBoundExpression stepExpression = null;
            if (!syntax.StepClauseOpt.IsDefault())
            {
                stepExpression = this.BindExpression(syntax.StepClauseOpt.Expression);
            }

            BoundStatementBlock body = this.BindStatementBlock(syntax.Body);

            return new BoundForStatement(syntax, identifier, fromExpression, toExpression, stepExpression, body);
        }

        private BaseBoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
            BaseBoundExpression expression = this.BindExpression(syntax.Expression, expectsValue: false);

            if (expression.HasErrors)
            {
                return new BoundInvalidExpressionStatement(syntax, expression);
            }

            switch (expression)
            {
                case BoundBinaryExpression assignment when assignment.Kind == TokenKind.Equal:
                    {
                        return this.BindAssignmentExpressionStatement(syntax, assignment);
                    }

                case BoundLibraryMethodInvocationExpression methodInvocation:
                    {
                        return new BoundLibraryMethodInvocationStatement(syntax, methodInvocation);
                    }

                case BoundSubModuleInvocationExpression subModuleInvocation:
                    {
                        return new BoundSubModuleInvocationStatement(syntax, subModuleInvocation);
                    }
            }

            if (expression.HasValue)
            {
                this.diagnostics.ReportUnassignedExpressionStatement(syntax.Range);
            }
            else
            {
                this.diagnostics.ReportInvalidExpressionStatement(syntax.Range);
            }

            return new BoundInvalidExpressionStatement(syntax, expression);
        }

        private BaseBoundStatement BindAssignmentExpressionStatement(ExpressionStatementSyntax syntax, BoundBinaryExpression assignment)
        {
            switch (assignment.Left)
            {
                case BoundVariableExpression variable:
                    {
                        return new BoundVariableAssignmentStatement(syntax, variable, assignment.Right);
                    }

                case BoundArrayAccessExpression arrayAccess:
                    {
                        return new BoundArrayAssignmentStatement(syntax, arrayAccess, assignment.Right);
                    }

                case BoundLibraryPropertyExpression property:
                    {
                        if (Libraries.Types[property.Library.Name].Properties[property.Name].Setter.IsDefault())
                        {
                            this.diagnostics.ReportPropertyHasNoSetter(property.Syntax.Range, property.Library.Name, property.Name);
                        }

                        return new BoundPropertyAssignmentStatement(syntax, property, assignment.Right);
                    }

                case BoundLibraryEventExpression @event:
                    {
                        if (assignment.Right is BoundSubModuleExpression subModule)
                        {
                            return new BoundEventAssignmentStatement(syntax, @event, subModule.Name);
                        }
                        else
                        {
                            this.diagnostics.ReportAssigningNonSubModuleToEvent(@event.Syntax.Range);
                            return new BoundInvalidExpressionStatement(syntax, assignment);
                        }
                    }

                default:
                    {
                        this.diagnostics.ReportUnassignedExpressionStatement(syntax.Range);
                        return new BoundInvalidExpressionStatement(syntax, assignment);
                    }
            }
        }

        private BaseBoundExpression BindExpression(BaseExpressionSyntax syntax, bool expectsValue = true)
        {
            switch (syntax)
            {
                case UnaryOperatorExpressionSyntax unaryOperatorExpression: return this.BindUnaryOperatorExpression(unaryOperatorExpression);
                case BinaryOperatorExpressionSyntax binaryOperatorExpression: return this.BindBinaryOperatorExpression(binaryOperatorExpression);
                case ArrayAccessExpressionSyntax arrayAccessExpression: return this.BindArrayAccessExpression(arrayAccessExpression);
                case ParenthesisExpressionSyntax parenthesisExpression: return this.BindParenthesisExpression(parenthesisExpression);
                case StringLiteralExpressionSyntax stringLiteralExpression: return BindStringLiteralExpression(stringLiteralExpression);
                case NumberLiteralExpressionSyntax numberLiteralExpression: return this.BindNumberLiteralExpression(numberLiteralExpression);

                case ObjectAccessExpressionSyntax objectAccessExpression: return this.BindObjectAccessExpression(objectAccessExpression, expectsValue);
                case InvocationExpressionSyntax invocationExpression: return this.BindInvocationExpression(invocationExpression, expectsValue);
                case IdentifierExpressionSyntax identifierExpression: return this.BindIdentifierExpression(identifierExpression, expectsValue);

                case UnrecognizedExpressionSyntax unrecognizedExpression: return new BoundInvalidExpression(unrecognizedExpression, hasValue: true, hasErrors: true);

                default: throw ExceptionUtilities.UnexpectedValue(syntax);
            }
        }

        private BoundUnaryExpression BindUnaryOperatorExpression(UnaryOperatorExpressionSyntax syntax)
        {
            BaseBoundExpression expression = this.BindExpression(syntax.Expression);

            switch (syntax.OperatorToken.Kind)
            {
                case TokenKind.Minus:
                    return new BoundUnaryExpression(syntax, hasValue: true, expression.HasErrors, syntax.OperatorToken.Kind, expression);
                default:
                    throw ExceptionUtilities.UnexpectedValue(syntax.OperatorToken.Kind);
            }
        }

        private BoundBinaryExpression BindBinaryOperatorExpression(BinaryOperatorExpressionSyntax syntax)
        {
            BaseBoundExpression left = this.BindExpression(syntax.Left);
            BaseBoundExpression right = this.BindExpression(syntax.Right, expectsValue: !(left is BoundLibraryEventExpression));

            switch (syntax.OperatorToken.Kind)
            {
                case TokenKind.Or:
                case TokenKind.And:
                case TokenKind.NotEqual:
                case TokenKind.Equal:
                case TokenKind.LessThan:
                case TokenKind.GreaterThan:
                case TokenKind.LessThanOrEqual:
                case TokenKind.GreaterThanOrEqual:
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Multiply:
                case TokenKind.Divide:
                    return new BoundBinaryExpression(syntax, hasValue: true, left.HasErrors || right.HasErrors, syntax.OperatorToken.Kind, left, right);
                default:
                    throw ExceptionUtilities.UnexpectedValue(syntax.OperatorToken.Kind);
            }
        }

        private BaseBoundExpression BindArrayAccessExpression(ArrayAccessExpressionSyntax syntax)
        {
            BaseBoundExpression baseExpression = this.BindExpression(syntax.BaseExpression);
            BaseBoundExpression indexExpression = this.BindExpression(syntax.IndexExpression);

            string arrayName;
            List<BaseBoundExpression> indices = new List<BaseBoundExpression>();
            bool hasErrors = baseExpression.HasErrors || indexExpression.HasErrors;

            switch (baseExpression)
            {
                case BoundArrayAccessExpression arrayAccess:
                    {
                        arrayName = arrayAccess.Name;
                        indices.AddRange(arrayAccess.Indices);
                        indices.Add(indexExpression);
                        break;
                    }

                case BoundVariableExpression variable:
                    {
                        arrayName = variable.Name;
                        indices.Add(indexExpression);
                        break;
                    }

                default:
                    {
                        if (!hasErrors)
                        {
                            hasErrors = true;
                            this.diagnostics.ReportUnsupportedArrayBaseExpression(syntax.BaseExpression.Range);
                        }

                        return new BoundInvalidExpression(syntax, hasValue: true, hasErrors);
                    }
            }

            return new BoundArrayAccessExpression(syntax, hasValue: true, hasErrors, arrayName, indices);
        }

        private BoundParenthesisExpression BindParenthesisExpression(ParenthesisExpressionSyntax syntax)
        {
            BaseBoundExpression expression = this.BindExpression(syntax.Expression);
            return new BoundParenthesisExpression(syntax, hasValue: true, expression.HasErrors, expression);
        }

        private static BoundStringLiteralExpression BindStringLiteralExpression(StringLiteralExpressionSyntax syntax)
        {
            string value = syntax.StringToken.Text;
            if (value.Length < 1 || value[0] != '"')
            {
                throw new InvalidOperationException($"String literal '{value}' should have never been parsed without a starting double quotes.");
            }

            value = value.Substring(1);
            if (value.Length > 0 && value[value.Length - 1] == '"')
            {
                value = value.Substring(0, value.Length - 1);
            }

            return new BoundStringLiteralExpression(syntax, hasValue: true, hasErrors: false, value);
        }

        private BaseBoundExpression BindNumberLiteralExpression(NumberLiteralExpressionSyntax syntax)
        {
            string value = syntax.NumberToken.Text;

            if (decimal.TryParse(value, out decimal result))
            {
                return new BoundNumberLiteralExpression(syntax, hasValue: true, hasErrors: false, result);
            }

            this.diagnostics.ReportValueIsNotANumber(syntax.Range, value);
            return new BoundInvalidExpression(syntax, hasValue: true, hasErrors: true);
        }

        private BaseBoundExpression BindObjectAccessExpression(ObjectAccessExpressionSyntax syntax, bool expectsValue)
        {
            BaseBoundExpression baseExpression = this.BindExpression(syntax.BaseExpression, expectsValue: false);
            string identifier = syntax.IdentifierToken.Text;
            bool hasErrors = baseExpression.HasErrors;

            if (baseExpression is BoundLibraryTypeExpression libraryTypeExpression)
            {
                Library library = Libraries.Types[libraryTypeExpression.Name];

                if (library.Properties.TryGetValue(identifier, out Property property))
                {
                    bool noGetter = property.Getter.IsDefault();

                    if (!hasErrors)
                    {
                        if (expectsValue && noGetter)
                        {
                            hasErrors = true;
                            this.diagnostics.ReportExpectedExpressionWithAValue(syntax.Range);
                        }
                        else if (property.IsDeprecated)
                        {
                            hasErrors = true;
                            this.diagnostics.ReportLibraryMemberDeprecatedFromOlderVersion(syntax.Range, library.Name, property.Name);
                        }
                        else if (property.NeedsDesktop && !this.isRunningOnDesktop)
                        {
                            hasErrors = true;
                            this.diagnostics.ReportLibraryMemberNeedsDesktop(syntax.Range, library.Name, property.Name);
                        }
                    }

                    return new BoundLibraryPropertyExpression(syntax, hasValue: !noGetter, hasErrors, libraryTypeExpression, identifier);
                }

                if (library.Methods.TryGetValue(identifier, out Method method))
                {
                    if (!hasErrors)
                    {
                        if (expectsValue)
                        {
                            hasErrors = true;
                            this.diagnostics.ReportExpectedExpressionWithAValue(syntax.Range);
                        }
                        else if (method.IsDeprecated)
                        {
                            hasErrors = true;
                            this.diagnostics.ReportLibraryMemberDeprecatedFromOlderVersion(syntax.Range, library.Name, method.Name);
                        }
                        else if (method.NeedsDesktop && !this.isRunningOnDesktop)
                        {
                            hasErrors = true;
                            this.diagnostics.ReportLibraryMemberNeedsDesktop(syntax.Range, library.Name, method.Name);
                        }
                    }

                    return new BoundLibraryMethodExpression(syntax, hasValue: false, hasErrors, libraryTypeExpression, identifier);
                }

                if (library.Events.ContainsKey(identifier))
                {
                    return new BoundLibraryEventExpression(syntax, hasValue: false, hasErrors, libraryTypeExpression, identifier);
                }

                this.diagnostics.ReportLibraryMemberNotFound(syntax.Range, library.Name, identifier);
                return new BoundInvalidExpression(syntax, hasValue: true, hasErrors: true);
            }
            else
            {
                this.diagnostics.ReportUnsupportedDotBaseExpression(syntax.BaseExpression.Range);
                return new BoundInvalidExpression(syntax, hasValue: true, hasErrors: true);
            }
        }

        private BaseBoundExpression BindInvocationExpression(InvocationExpressionSyntax syntax, bool expectsValue)
        {
            BaseBoundExpression baseExpression = this.BindExpression(syntax.BaseExpression, expectsValue: false);
            bool hasErrors = baseExpression.HasErrors;
            List<BaseBoundExpression> arguments = new List<BaseBoundExpression>();

            foreach (ArgumentSyntax arg in syntax.Arguments)
            {
                BaseBoundExpression argument = this.BindExpression(arg.Expression);
                hasErrors |= argument.HasErrors;
                arguments.Add(argument);
            }

            switch (baseExpression)
            {
                case BoundLibraryMethodExpression libraryMethod:
                    {
                        Method method = Libraries.Types[libraryMethod.Library.Name].Methods[libraryMethod.Name];

                        if (!hasErrors)
                        {
                            if (arguments.Count != method.Parameters.Count)
                            {
                                hasErrors = true;
                                this.diagnostics.ReportUnexpectedArgumentsCount(syntax.Range, arguments.Count, method.Parameters.Count);
                            }
                            else if (expectsValue && !method.ReturnsValue)
                            {
                                hasErrors = true;
                                this.diagnostics.ReportExpectedExpressionWithAValue(syntax.Range);
                            }
                        }

                        return new BoundLibraryMethodInvocationExpression(syntax, method.ReturnsValue, hasErrors, libraryMethod, arguments);
                    }

                case BoundSubModuleExpression subModule:
                    {
                        if (!hasErrors)
                        {
                            if (arguments.Count != 0)
                            {
                                hasErrors = true;
                                this.diagnostics.ReportUnexpectedArgumentsCount(syntax.Range, arguments.Count, 0);
                            }
                            else if (expectsValue)
                            {
                                hasErrors = true;
                                this.diagnostics.ReportExpectedExpressionWithAValue(syntax.Range);
                            }
                        }

                        return new BoundSubModuleInvocationExpression(syntax, hasValue: false, hasErrors, subModule.Name);
                    }

                default:
                    {
                        this.diagnostics.ReportUnsupportedInvocationBaseExpression(syntax.Range);
                        return new BoundInvalidExpression(syntax, hasValue: true, hasErrors: true);
                    }
            }
        }

        private BaseBoundExpression BindIdentifierExpression(IdentifierExpressionSyntax syntax, bool expectsValue)
        {
            bool hasErrors = false;
            string name = syntax.IdentifierToken.Text;

            if (Libraries.Types.TryGetValue(name, out Library library))
            {
                if (expectsValue)
                {
                    hasErrors = true;
                    this.diagnostics.ReportExpectedExpressionWithAValue(syntax.Range);
                }

                return new BoundLibraryTypeExpression(syntax, hasValue: false, hasErrors, name);
            }
            else if (this.definedSubModules.Contains(name))
            {
                if (expectsValue)
                {
                    hasErrors = true;
                    this.diagnostics.ReportExpectedExpressionWithAValue(syntax.Range);
                }

                return new BoundSubModuleExpression(syntax, hasValue: false, hasErrors, name);
            }
            else
            {
                return new BoundVariableExpression(syntax, hasValue: true, hasErrors, name);
            }
        }
    }
}
