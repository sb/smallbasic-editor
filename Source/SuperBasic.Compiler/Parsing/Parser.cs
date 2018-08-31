// <copyright file="Parser.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Scanning;

    internal sealed class Parser
    {
        private const string MissingTokenText = "<?>";

        private static readonly TokenKind[] BinaryOperatorPrecedence =
        {
            TokenKind.Or,
            TokenKind.And,
            TokenKind.Equal,
            TokenKind.NotEqual,
            TokenKind.LessThan,
            TokenKind.GreaterThan,
            TokenKind.LessThanOrEqual,
            TokenKind.GreaterThanOrEqual,
            TokenKind.Plus,
            TokenKind.Minus,
            TokenKind.Multiply,
            TokenKind.Divide
        };

        private readonly IReadOnlyList<Token> tokens;
        private readonly DiagnosticBag diagnostics;

        private short index = 0;

        public Parser(IReadOnlyList<Token> tokens, DiagnosticBag diagnostics)
        {
            this.tokens = tokens;
            this.diagnostics = diagnostics;

            var statements = new List<BaseStatementSyntax>();

            while (this.index < this.tokens.Count)
            {
                switch (this.Peek())
                {
                    case TokenKind.Sub:
                        statements.Add(this.ParseSubModuleDeclaration());
                        break;
                    default:
                        statements.Add(this.ParseStatement());
                        break;
                }
            }

            this.SyntaxTree = new StatementBlockSyntax(statements);
        }

        public StatementBlockSyntax SyntaxTree { get; private set; }

        private SubModuleStatementSyntax ParseSubModuleDeclaration()
        {
            var subToken = this.Eat(TokenKind.Sub);
            var nameToken = this.Eat(TokenKind.Identifier);
            this.RunToEndOfLine();

            var statements = this.ParseStatementsExcept(TokenKind.Sub, TokenKind.EndSub);

            var endSubToken = this.Eat(TokenKind.EndSub);
            this.RunToEndOfLine();

            return new SubModuleStatementSyntax(subToken, nameToken, statements, endSubToken);
        }

        private StatementBlockSyntax ParseStatementsExcept(params TokenKind[] kinds)
        {
            List<BaseStatementSyntax> statements = new List<BaseStatementSyntax>();
            while (this.index < this.tokens.Count && !kinds.Contains(this.Peek()))
            {
                statements.Add(this.ParseStatement());
            }

            return new StatementBlockSyntax(statements);
        }

        private BaseStatementSyntax ParseStatement()
        {
            switch (this.Peek())
            {
                case TokenKind.If:
                    return this.ParseIfStatement();
                case TokenKind.For:
                    return this.ParseForStatement();
                case TokenKind.While:
                    return this.ParseWhileStatement();

                case TokenKind.Identifier:
                    if (this.index + 1 < this.tokens.Count && this.tokens[this.index + 1].Kind == TokenKind.Colon)
                    {
                        var labelToken = this.Eat(TokenKind.Identifier);
                        var colonToken = this.Eat(TokenKind.Colon);
                        this.RunToEndOfLine();
                        return new LabelStatementSyntax(labelToken, colonToken);
                    }
                    else
                    {
                        var expression = this.ParseBaseExpression();
                        this.RunToEndOfLine();
                        return new ExpressionStatementSyntax(expression);
                    }

                case TokenKind.GoTo:
                    var goToToken = this.Eat(TokenKind.GoTo);
                    var identifier = this.Eat(TokenKind.Identifier);
                    this.RunToEndOfLine();
                    return new GoToStatementSyntax(goToToken, identifier);

                case TokenKind.Comment:
                    var commentToken = this.Eat(TokenKind.Comment);
                    this.RunToEndOfLine();
                    return new CommentStatementSyntax(commentToken);

                case TokenKind foundKind:
                    var foundToken = this.Eat(foundKind);
                    this.RunToEndOfLine(reportErrors: false);

                    if (foundKind != TokenKind.Unrecognized)
                    {
                        this.diagnostics.ReportUnexpectedTokenInsteadOfStatement(foundToken.Range, foundToken.Kind);
                    }

                    return new UnrecognizedStatementSyntax(foundToken);
            }
        }

        private IfStatementSyntax ParseIfStatement()
        {
            var ifToken = this.Eat(TokenKind.If);
            var expression = this.ParseBaseExpression();
            var thenToken = this.Eat(TokenKind.Then);
            this.RunToEndOfLine();

            var statements = this.ParseStatementsExcept(TokenKind.ElseIf, TokenKind.Else, TokenKind.EndIf);
            var ifPart = new IfPartSyntax(ifToken, expression, thenToken, statements);

            var elseIfParts = new List<ElseIfPartSyntax>();
            while (this.index < this.tokens.Count && this.Peek() == TokenKind.ElseIf)
            {
                var elseIfToken = this.Eat(TokenKind.ElseIf);
                expression = this.ParseBaseExpression();
                thenToken = this.Eat(TokenKind.Then);
                this.RunToEndOfLine();

                statements = this.ParseStatementsExcept(TokenKind.ElseIf, TokenKind.Else, TokenKind.EndIf);
                elseIfParts.Add(new ElseIfPartSyntax(elseIfToken, expression, thenToken, statements));
            }

            ElsePartSyntax elsePartOpt = null;
            if (this.index < this.tokens.Count && this.Peek() == TokenKind.Else)
            {
                var elseToken = this.Eat(TokenKind.Else);
                this.RunToEndOfLine();

                statements = this.ParseStatementsExcept(TokenKind.ElseIf, TokenKind.Else, TokenKind.EndIf);
                elsePartOpt = new ElsePartSyntax(elseToken, statements);
            }

            var endIfToken = this.Eat(TokenKind.EndIf);
            this.RunToEndOfLine();
            return new IfStatementSyntax(ifPart, elseIfParts, elsePartOpt, endIfToken);
        }

        private ForStatementSyntax ParseForStatement()
        {
            var forToken = this.Eat(TokenKind.For);
            var identifierToken = this.Eat(TokenKind.Identifier);
            var equalToken = this.Eat(TokenKind.Equal);
            var fromExpression = this.ParseBaseExpression();
            var toToken = this.Eat(TokenKind.To);
            var toExpression = this.ParseBaseExpression();

            ForStepClauseSyntax stepClauseOpt = null;
            if (this.index < this.tokens.Count && this.Peek() == TokenKind.Step)
            {
                var stepToken = this.Eat(TokenKind.Step);
                var expression = this.ParseBaseExpression();

                stepClauseOpt = new ForStepClauseSyntax(stepToken, expression);
            }

            this.RunToEndOfLine();

            var statements = this.ParseStatementsExcept(TokenKind.EndFor);

            var endForToken = this.Eat(TokenKind.EndFor);
            this.RunToEndOfLine();

            return new ForStatementSyntax(forToken, identifierToken, equalToken, fromExpression, toToken, toExpression, stepClauseOpt, statements, endForToken);
        }

        private WhileStatementSyntax ParseWhileStatement()
        {
            var whileToken = this.Eat(TokenKind.While);
            var expression = this.ParseBaseExpression();
            this.RunToEndOfLine();

            var statements = this.ParseStatementsExcept(TokenKind.EndWhile);

            var endWhileToken = this.Eat(TokenKind.EndWhile);
            this.RunToEndOfLine();

            return new WhileStatementSyntax(whileToken, expression, statements, endWhileToken);
        }

        private BaseExpressionSyntax ParseBaseExpression()
        {
            return this.ParseBinaryOperator(precedence: 0);
        }

        private BaseExpressionSyntax ParseBinaryOperator(int precedence)
        {
            if (precedence >= BinaryOperatorPrecedence.Length)
            {
                return this.ParseUnaryOperator();
            }

            var expression = this.ParseBinaryOperator(precedence + 1);
            var expectedOperatorKind = BinaryOperatorPrecedence[precedence];

            while (this.index < this.tokens.Count && this.Peek() == expectedOperatorKind)
            {
                var operatorToken = this.Eat(expectedOperatorKind);
                var rightHandSide = this.ParseBinaryOperator(precedence + 1);

                expression = new BinaryOperatorExpressionSyntax(expression, operatorToken, rightHandSide);
            }

            return expression;
        }

        private BaseExpressionSyntax ParseUnaryOperator()
        {
            if (this.index < this.tokens.Count && this.Peek() == TokenKind.Minus)
            {
                var minusToken = this.Eat(TokenKind.Minus);
                var expression = this.ParseBaseExpression();

                return new UnaryOperatorExpressionSyntax(minusToken, expression);
            }

            return this.ParseCoreExpression();
        }

        private BaseExpressionSyntax ParseCoreExpression()
        {
            var expression = this.ParseTerminalExpression();

            while (this.index < this.tokens.Count)
            {
                switch (this.Peek())
                {
                    case TokenKind.Dot:
                        var dotToken = this.Eat(TokenKind.Dot);
                        var identifierToken = this.Eat(TokenKind.Identifier);
                        expression = new ObjectAccessExpressionSyntax(expression, dotToken, identifierToken);
                        break;

                    case TokenKind.LeftBracket:
                        var leftBracketToken = this.Eat(TokenKind.LeftBracket);
                        var indexExpression = this.ParseBaseExpression();
                        var rightBracketToken = this.Eat(TokenKind.RightBracket);
                        expression = new ArrayAccessExpressionSyntax(expression, leftBracketToken, indexExpression, rightBracketToken);
                        break;

                    case TokenKind.LeftParen:
                        var leftParenToken = this.Eat(TokenKind.LeftParen);
                        var arguments = this.ParseArguments();
                        var rightParenToken = this.Eat(TokenKind.RightParen);
                        expression = new InvocationExpressionSyntax(expression, leftParenToken, arguments, rightParenToken);
                        break;

                    default:
                        return expression;
                }
            }

            return expression;
        }

        private List<ArgumentSyntax> ParseArguments()
        {
            var arguments = new List<ArgumentSyntax>();
            BaseExpressionSyntax currentArgument = null;

            while (this.index < this.tokens.Count)
            {
                if (!ReferenceEquals(currentArgument, null))
                {
                    switch (this.Peek())
                    {
                        case TokenKind.Comma:
                            var commaTokenOpt = this.Eat(TokenKind.Comma);
                            arguments.Add(new ArgumentSyntax(currentArgument, commaTokenOpt));
                            currentArgument = null;
                            break;

                        case TokenKind.RightParen:
                            arguments.Add(new ArgumentSyntax(currentArgument, commaTokenOpt: null));
                            return arguments;

                        case TokenKind foundKind:
                            this.diagnostics.ReportUnexpectedTokenFound(this.tokens[this.index].Range, foundKind, TokenKind.Comma);
                            arguments.Add(new ArgumentSyntax(currentArgument, commaTokenOpt: null));
                            currentArgument = null;
                            break;
                    }
                }
                else if (this.Peek() == TokenKind.RightParen)
                {
                    return arguments;
                }
                else
                {
                    currentArgument = this.ParseBaseExpression();
                }
            }

            if (!ReferenceEquals(currentArgument, null))
            {
                arguments.Add(new ArgumentSyntax(currentArgument, commaTokenOpt: null));
            }

            return arguments;
        }

        private BaseExpressionSyntax ParseTerminalExpression()
        {
            if (this.index >= this.tokens.Count)
            {
                var range = this.tokens[this.tokens.Count - 1].Range;
                var missingToken = new Token(TokenKind.Identifier, MissingTokenText, range);

                this.diagnostics.ReportUnexpectedEndOfStream(range, missingToken.Kind);
                return new IdentifierExpressionSyntax(missingToken);
            }

            switch (this.Peek())
            {
                case TokenKind.Identifier:
                    var identifierToken = this.Eat(TokenKind.Identifier);
                    return new IdentifierExpressionSyntax(identifierToken);

                case TokenKind.NumberLiteral:
                    var numberToken = this.Eat(TokenKind.NumberLiteral);
                    return new NumberLiteralExpressionSyntax(numberToken);

                case TokenKind.StringLiteral:
                    var stringToken = this.Eat(TokenKind.StringLiteral);
                    return new StringLiteralExpressionSyntax(stringToken);

                case TokenKind.LeftParen:
                    var leftParenToken = this.Eat(TokenKind.LeftParen);
                    var expression = this.ParseBaseExpression();
                    var rightParenToken = this.Eat(TokenKind.RightParen);
                    return new ParenthesisExpressionSyntax(leftParenToken, expression, rightParenToken);

                case TokenKind foundKind:
                    var foundToken = this.Eat(foundKind);

                    if (foundKind != TokenKind.Unrecognized)
                    {
                        this.diagnostics.ReportUnexpectedTokenFound(foundToken.Range, foundKind, TokenKind.Identifier);
                    }

                    return new UnrecognizedExpressionSyntax(foundToken);
            }
        }

        private void RunToEndOfLine(bool reportErrors = true)
        {
            var currentLine = this.tokens[this.index - 1].Range.Start.Line;

            while (true)
            {
                if (this.index >= this.tokens.Count)
                {
                    break;
                }

                var currentToken = this.tokens[this.index];

                if (currentToken.Kind == TokenKind.Comment ||
                    currentToken.Kind == TokenKind.Unrecognized ||
                    currentLine < currentToken.Range.Start.Line)
                {
                    break;
                }

                if (reportErrors)
                {
                    this.diagnostics.ReportUnexpectedStatementInsteadOfNewLine(currentToken.Range);
                    reportErrors = false;
                }

                this.index++;
            }
        }

        private TokenKind Peek() => this.tokens[this.index].Kind;

        private Token Eat(TokenKind kind)
        {
            if (this.index < this.tokens.Count)
            {
                Token current = this.tokens[this.index];

                if (current.Kind == kind)
                {
                    this.index++;
                    return current;
                }
                else
                {
                    this.diagnostics.ReportUnexpectedTokenFound(current.Range, current.Kind, kind);
                    return new Token(kind, MissingTokenText, current.Range);
                }
            }
            else
            {
                var range = this.tokens[this.tokens.Count - 1].Range;
                this.diagnostics.ReportUnexpectedEndOfStream(range, kind);
                return new Token(kind, MissingTokenText, range);
            }
        }
    }
}
