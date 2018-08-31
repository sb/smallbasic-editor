// <copyright file="SyntaxNodes.Generated.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

/// <summary>
/// This file is auto-generated by a build task. It shouldn't be edited by hand.
/// </summary>
namespace SuperBasic.Compiler.Parsing
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SuperBasic.Compiler.Scanning;

    internal abstract class BaseStatementSyntax : BaseSyntaxNode
    {
    }

    internal sealed class SubModuleStatementSyntax : BaseStatementSyntax
    {
        public SubModuleStatementSyntax(Token subToken, Token nameToken, StatementBlockSyntax statements, Token endSubToken)
        {
            Debug.Assert(!ReferenceEquals(subToken, null), "'subToken' must not be null.");
            Debug.Assert(subToken.Kind == TokenKind.Sub, "'subToken' must have a TokenKind of 'Sub'.");
            Debug.Assert(!ReferenceEquals(nameToken, null), "'nameToken' must not be null.");
            Debug.Assert(nameToken.Kind == TokenKind.Identifier, "'nameToken' must have a TokenKind of 'Identifier'.");
            Debug.Assert(!ReferenceEquals(statements, null), "'statements' must not be null.");
            Debug.Assert(!ReferenceEquals(endSubToken, null), "'endSubToken' must not be null.");
            Debug.Assert(endSubToken.Kind == TokenKind.EndSub, "'endSubToken' must have a TokenKind of 'EndSub'.");

            this.SubToken = subToken;
            this.NameToken = nameToken;
            this.Statements = statements;
            this.EndSubToken = endSubToken;
        }

        public Token SubToken { get; private set; }

        public Token NameToken { get; private set; }

        public StatementBlockSyntax Statements { get; private set; }

        public Token EndSubToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Statements;
            }
        }
    }

    internal sealed class StatementBlockSyntax : BaseStatementSyntax
    {
        public StatementBlockSyntax(IReadOnlyList<BaseStatementSyntax> statements)
        {
            Debug.Assert(!ReferenceEquals(statements, null), "'statements' must not be null.");

            this.Statements = statements;
        }

        public IReadOnlyList<BaseStatementSyntax> Statements { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                foreach (var child in this.Statements)
                {
                    yield return child;
                }
            }
        }
    }

    internal sealed class IfPartSyntax : BaseSyntaxNode
    {
        public IfPartSyntax(Token ifToken, BaseExpressionSyntax expression, Token thenToken, StatementBlockSyntax statements)
        {
            Debug.Assert(!ReferenceEquals(ifToken, null), "'ifToken' must not be null.");
            Debug.Assert(ifToken.Kind == TokenKind.If, "'ifToken' must have a TokenKind of 'If'.");
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");
            Debug.Assert(!ReferenceEquals(thenToken, null), "'thenToken' must not be null.");
            Debug.Assert(thenToken.Kind == TokenKind.Then, "'thenToken' must have a TokenKind of 'Then'.");
            Debug.Assert(!ReferenceEquals(statements, null), "'statements' must not be null.");

            this.IfToken = ifToken;
            this.Expression = expression;
            this.ThenToken = thenToken;
            this.Statements = statements;
        }

        public Token IfToken { get; private set; }

        public BaseExpressionSyntax Expression { get; private set; }

        public Token ThenToken { get; private set; }

        public StatementBlockSyntax Statements { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
                yield return this.Statements;
            }
        }
    }

    internal sealed class ElseIfPartSyntax : BaseSyntaxNode
    {
        public ElseIfPartSyntax(Token elseIfToken, BaseExpressionSyntax expression, Token thenToken, StatementBlockSyntax statements)
        {
            Debug.Assert(!ReferenceEquals(elseIfToken, null), "'elseIfToken' must not be null.");
            Debug.Assert(elseIfToken.Kind == TokenKind.ElseIf, "'elseIfToken' must have a TokenKind of 'ElseIf'.");
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");
            Debug.Assert(!ReferenceEquals(thenToken, null), "'thenToken' must not be null.");
            Debug.Assert(thenToken.Kind == TokenKind.Then, "'thenToken' must have a TokenKind of 'Then'.");
            Debug.Assert(!ReferenceEquals(statements, null), "'statements' must not be null.");

            this.ElseIfToken = elseIfToken;
            this.Expression = expression;
            this.ThenToken = thenToken;
            this.Statements = statements;
        }

        public Token ElseIfToken { get; private set; }

        public BaseExpressionSyntax Expression { get; private set; }

        public Token ThenToken { get; private set; }

        public StatementBlockSyntax Statements { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
                yield return this.Statements;
            }
        }
    }

    internal sealed class ElsePartSyntax : BaseSyntaxNode
    {
        public ElsePartSyntax(Token elseToken, StatementBlockSyntax statements)
        {
            Debug.Assert(!ReferenceEquals(elseToken, null), "'elseToken' must not be null.");
            Debug.Assert(elseToken.Kind == TokenKind.Else, "'elseToken' must have a TokenKind of 'Else'.");
            Debug.Assert(!ReferenceEquals(statements, null), "'statements' must not be null.");

            this.ElseToken = elseToken;
            this.Statements = statements;
        }

        public Token ElseToken { get; private set; }

        public StatementBlockSyntax Statements { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Statements;
            }
        }
    }

    internal sealed class IfStatementSyntax : BaseStatementSyntax
    {
        public IfStatementSyntax(IfPartSyntax ifPart, IReadOnlyList<ElseIfPartSyntax> elseIfParts, ElsePartSyntax elsePartOpt, Token endIfToken)
        {
            Debug.Assert(!ReferenceEquals(ifPart, null), "'ifPart' must not be null.");
            Debug.Assert(!ReferenceEquals(elseIfParts, null), "'elseIfParts' must not be null.");
            Debug.Assert(!ReferenceEquals(endIfToken, null), "'endIfToken' must not be null.");
            Debug.Assert(endIfToken.Kind == TokenKind.EndIf, "'endIfToken' must have a TokenKind of 'EndIf'.");

            this.IfPart = ifPart;
            this.ElseIfParts = elseIfParts;
            this.ElsePartOpt = elsePartOpt;
            this.EndIfToken = endIfToken;
        }

        public IfPartSyntax IfPart { get; private set; }

        public IReadOnlyList<ElseIfPartSyntax> ElseIfParts { get; private set; }

        public ElsePartSyntax ElsePartOpt { get; private set; }

        public Token EndIfToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.IfPart;
                foreach (var child in this.ElseIfParts)
                {
                    yield return child;
                }

                if (!ReferenceEquals(this.ElsePartOpt, null))
                {
                    yield return this.ElsePartOpt;
                }
            }
        }
    }

    internal sealed class WhileStatementSyntax : BaseStatementSyntax
    {
        public WhileStatementSyntax(Token whileToken, BaseExpressionSyntax expression, StatementBlockSyntax statements, Token endWhileToken)
        {
            Debug.Assert(!ReferenceEquals(whileToken, null), "'whileToken' must not be null.");
            Debug.Assert(whileToken.Kind == TokenKind.While, "'whileToken' must have a TokenKind of 'While'.");
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");
            Debug.Assert(!ReferenceEquals(statements, null), "'statements' must not be null.");
            Debug.Assert(!ReferenceEquals(endWhileToken, null), "'endWhileToken' must not be null.");
            Debug.Assert(endWhileToken.Kind == TokenKind.EndWhile, "'endWhileToken' must have a TokenKind of 'EndWhile'.");

            this.WhileToken = whileToken;
            this.Expression = expression;
            this.Statements = statements;
            this.EndWhileToken = endWhileToken;
        }

        public Token WhileToken { get; private set; }

        public BaseExpressionSyntax Expression { get; private set; }

        public StatementBlockSyntax Statements { get; private set; }

        public Token EndWhileToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
                yield return this.Statements;
            }
        }
    }

    internal sealed class ForStepClauseSyntax : BaseSyntaxNode
    {
        public ForStepClauseSyntax(Token stepToken, BaseExpressionSyntax expression)
        {
            Debug.Assert(!ReferenceEquals(stepToken, null), "'stepToken' must not be null.");
            Debug.Assert(stepToken.Kind == TokenKind.Step, "'stepToken' must have a TokenKind of 'Step'.");
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");

            this.StepToken = stepToken;
            this.Expression = expression;
        }

        public Token StepToken { get; private set; }

        public BaseExpressionSyntax Expression { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
            }
        }
    }

    internal sealed class ForStatementSyntax : BaseStatementSyntax
    {
        public ForStatementSyntax(Token forToken, Token identifierToken, Token equalToken, BaseExpressionSyntax fromExpression, Token toToken, BaseExpressionSyntax toExpression, ForStepClauseSyntax stepClauseOpt, StatementBlockSyntax statements, Token endForToken)
        {
            Debug.Assert(!ReferenceEquals(forToken, null), "'forToken' must not be null.");
            Debug.Assert(forToken.Kind == TokenKind.For, "'forToken' must have a TokenKind of 'For'.");
            Debug.Assert(!ReferenceEquals(identifierToken, null), "'identifierToken' must not be null.");
            Debug.Assert(identifierToken.Kind == TokenKind.Identifier, "'identifierToken' must have a TokenKind of 'Identifier'.");
            Debug.Assert(!ReferenceEquals(equalToken, null), "'equalToken' must not be null.");
            Debug.Assert(equalToken.Kind == TokenKind.Equal, "'equalToken' must have a TokenKind of 'Equal'.");
            Debug.Assert(!ReferenceEquals(fromExpression, null), "'fromExpression' must not be null.");
            Debug.Assert(!ReferenceEquals(toToken, null), "'toToken' must not be null.");
            Debug.Assert(toToken.Kind == TokenKind.To, "'toToken' must have a TokenKind of 'To'.");
            Debug.Assert(!ReferenceEquals(toExpression, null), "'toExpression' must not be null.");
            Debug.Assert(!ReferenceEquals(statements, null), "'statements' must not be null.");
            Debug.Assert(!ReferenceEquals(endForToken, null), "'endForToken' must not be null.");
            Debug.Assert(endForToken.Kind == TokenKind.EndFor, "'endForToken' must have a TokenKind of 'EndFor'.");

            this.ForToken = forToken;
            this.IdentifierToken = identifierToken;
            this.EqualToken = equalToken;
            this.FromExpression = fromExpression;
            this.ToToken = toToken;
            this.ToExpression = toExpression;
            this.StepClauseOpt = stepClauseOpt;
            this.Statements = statements;
            this.EndForToken = endForToken;
        }

        public Token ForToken { get; private set; }

        public Token IdentifierToken { get; private set; }

        public Token EqualToken { get; private set; }

        public BaseExpressionSyntax FromExpression { get; private set; }

        public Token ToToken { get; private set; }

        public BaseExpressionSyntax ToExpression { get; private set; }

        public ForStepClauseSyntax StepClauseOpt { get; private set; }

        public StatementBlockSyntax Statements { get; private set; }

        public Token EndForToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.FromExpression;
                yield return this.ToExpression;
                if (!ReferenceEquals(this.StepClauseOpt, null))
                {
                    yield return this.StepClauseOpt;
                }

                yield return this.Statements;
            }
        }
    }

    internal sealed class LabelStatementSyntax : BaseStatementSyntax
    {
        public LabelStatementSyntax(Token labelToken, Token colonToken)
        {
            Debug.Assert(!ReferenceEquals(labelToken, null), "'labelToken' must not be null.");
            Debug.Assert(labelToken.Kind == TokenKind.Identifier, "'labelToken' must have a TokenKind of 'Identifier'.");
            Debug.Assert(!ReferenceEquals(colonToken, null), "'colonToken' must not be null.");
            Debug.Assert(colonToken.Kind == TokenKind.Colon, "'colonToken' must have a TokenKind of 'Colon'.");

            this.LabelToken = labelToken;
            this.ColonToken = colonToken;
        }

        public Token LabelToken { get; private set; }

        public Token ColonToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }

    internal sealed class GoToStatementSyntax : BaseStatementSyntax
    {
        public GoToStatementSyntax(Token goToToken, Token labelToken)
        {
            Debug.Assert(!ReferenceEquals(goToToken, null), "'goToToken' must not be null.");
            Debug.Assert(goToToken.Kind == TokenKind.GoTo, "'goToToken' must have a TokenKind of 'GoTo'.");
            Debug.Assert(!ReferenceEquals(labelToken, null), "'labelToken' must not be null.");
            Debug.Assert(labelToken.Kind == TokenKind.Identifier, "'labelToken' must have a TokenKind of 'Identifier'.");

            this.GoToToken = goToToken;
            this.LabelToken = labelToken;
        }

        public Token GoToToken { get; private set; }

        public Token LabelToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }

    internal sealed class UnrecognizedStatementSyntax : BaseStatementSyntax
    {
        public UnrecognizedStatementSyntax(Token unrecognizedToken)
        {
            Debug.Assert(!ReferenceEquals(unrecognizedToken, null), "'unrecognizedToken' must not be null.");

            this.UnrecognizedToken = unrecognizedToken;
        }

        public Token UnrecognizedToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }

    internal sealed class ExpressionStatementSyntax : BaseStatementSyntax
    {
        public ExpressionStatementSyntax(BaseExpressionSyntax expression)
        {
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");

            this.Expression = expression;
        }

        public BaseExpressionSyntax Expression { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
            }
        }
    }

    internal sealed class CommentStatementSyntax : BaseStatementSyntax
    {
        public CommentStatementSyntax(Token commentToken)
        {
            Debug.Assert(!ReferenceEquals(commentToken, null), "'commentToken' must not be null.");
            Debug.Assert(commentToken.Kind == TokenKind.Comment, "'commentToken' must have a TokenKind of 'Comment'.");

            this.CommentToken = commentToken;
        }

        public Token CommentToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }

    internal abstract class BaseExpressionSyntax : BaseSyntaxNode
    {
    }

    internal sealed class UnaryOperatorExpressionSyntax : BaseExpressionSyntax
    {
        public UnaryOperatorExpressionSyntax(Token operatorToken, BaseExpressionSyntax expression)
        {
            Debug.Assert(!ReferenceEquals(operatorToken, null), "'operatorToken' must not be null.");
            Debug.Assert(operatorToken.Kind == TokenKind.Minus, "'operatorToken' must have a TokenKind of 'Minus'.");
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");

            this.OperatorToken = operatorToken;
            this.Expression = expression;
        }

        public Token OperatorToken { get; private set; }

        public BaseExpressionSyntax Expression { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
            }
        }
    }

    internal sealed class BinaryOperatorExpressionSyntax : BaseExpressionSyntax
    {
        public BinaryOperatorExpressionSyntax(BaseExpressionSyntax leftExpression, Token operatorToken, BaseExpressionSyntax rightExpression)
        {
            Debug.Assert(!ReferenceEquals(leftExpression, null), "'leftExpression' must not be null.");
            Debug.Assert(!ReferenceEquals(operatorToken, null), "'operatorToken' must not be null.");
            Debug.Assert(operatorToken.Kind == TokenKind.Equal || operatorToken.Kind == TokenKind.NotEqual || operatorToken.Kind == TokenKind.Plus || operatorToken.Kind == TokenKind.Minus || operatorToken.Kind == TokenKind.Multiply || operatorToken.Kind == TokenKind.Divide || operatorToken.Kind == TokenKind.Colon || operatorToken.Kind == TokenKind.LessThan || operatorToken.Kind == TokenKind.GreaterThan || operatorToken.Kind == TokenKind.LessThanOrEqual || operatorToken.Kind == TokenKind.GreaterThanOrEqual, "'operatorToken' must have a TokenKind of 'Equal,NotEqual,Plus,Minus,Multiply,Divide,Colon,LessThan,GreaterThan,LessThanOrEqual,GreaterThanOrEqual'.");
            Debug.Assert(!ReferenceEquals(rightExpression, null), "'rightExpression' must not be null.");

            this.LeftExpression = leftExpression;
            this.OperatorToken = operatorToken;
            this.RightExpression = rightExpression;
        }

        public BaseExpressionSyntax LeftExpression { get; private set; }

        public Token OperatorToken { get; private set; }

        public BaseExpressionSyntax RightExpression { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.LeftExpression;
                yield return this.RightExpression;
            }
        }
    }

    internal sealed class ObjectAccessExpressionSyntax : BaseExpressionSyntax
    {
        public ObjectAccessExpressionSyntax(BaseExpressionSyntax baseExpression, Token dotToken, Token identifierToken)
        {
            Debug.Assert(!ReferenceEquals(baseExpression, null), "'baseExpression' must not be null.");
            Debug.Assert(!ReferenceEquals(dotToken, null), "'dotToken' must not be null.");
            Debug.Assert(dotToken.Kind == TokenKind.Dot, "'dotToken' must have a TokenKind of 'Dot'.");
            Debug.Assert(!ReferenceEquals(identifierToken, null), "'identifierToken' must not be null.");
            Debug.Assert(identifierToken.Kind == TokenKind.Identifier, "'identifierToken' must have a TokenKind of 'Identifier'.");

            this.BaseExpression = baseExpression;
            this.DotToken = dotToken;
            this.IdentifierToken = identifierToken;
        }

        public BaseExpressionSyntax BaseExpression { get; private set; }

        public Token DotToken { get; private set; }

        public Token IdentifierToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.BaseExpression;
            }
        }
    }

    internal sealed class ArrayAccessExpressionSyntax : BaseExpressionSyntax
    {
        public ArrayAccessExpressionSyntax(BaseExpressionSyntax baseExpression, Token leftBracketToken, BaseExpressionSyntax indexExpression, Token rightBracketToken)
        {
            Debug.Assert(!ReferenceEquals(baseExpression, null), "'baseExpression' must not be null.");
            Debug.Assert(!ReferenceEquals(leftBracketToken, null), "'leftBracketToken' must not be null.");
            Debug.Assert(leftBracketToken.Kind == TokenKind.LeftBracket, "'leftBracketToken' must have a TokenKind of 'LeftBracket'.");
            Debug.Assert(!ReferenceEquals(indexExpression, null), "'indexExpression' must not be null.");
            Debug.Assert(!ReferenceEquals(rightBracketToken, null), "'rightBracketToken' must not be null.");
            Debug.Assert(rightBracketToken.Kind == TokenKind.RightBracket, "'rightBracketToken' must have a TokenKind of 'RightBracket'.");

            this.BaseExpression = baseExpression;
            this.LeftBracketToken = leftBracketToken;
            this.IndexExpression = indexExpression;
            this.RightBracketToken = rightBracketToken;
        }

        public BaseExpressionSyntax BaseExpression { get; private set; }

        public Token LeftBracketToken { get; private set; }

        public BaseExpressionSyntax IndexExpression { get; private set; }

        public Token RightBracketToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.BaseExpression;
                yield return this.IndexExpression;
            }
        }
    }

    internal sealed class ArgumentSyntax : BaseExpressionSyntax
    {
        public ArgumentSyntax(BaseExpressionSyntax expression, Token commaTokenOpt)
        {
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");
            if (!ReferenceEquals(commaTokenOpt, null))
            {
                Debug.Assert(commaTokenOpt.Kind == TokenKind.Comma, "'commaTokenOpt' must have a TokenKind of 'Comma'.");
            }

            this.Expression = expression;
            this.CommaTokenOpt = commaTokenOpt;
        }

        public BaseExpressionSyntax Expression { get; private set; }

        public Token CommaTokenOpt { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
            }
        }
    }

    internal sealed class InvocationExpressionSyntax : BaseExpressionSyntax
    {
        public InvocationExpressionSyntax(BaseExpressionSyntax baseExpression, Token leftParenToken, IReadOnlyList<ArgumentSyntax> arguments, Token rightParenToken)
        {
            Debug.Assert(!ReferenceEquals(baseExpression, null), "'baseExpression' must not be null.");
            Debug.Assert(!ReferenceEquals(leftParenToken, null), "'leftParenToken' must not be null.");
            Debug.Assert(leftParenToken.Kind == TokenKind.LeftParen, "'leftParenToken' must have a TokenKind of 'LeftParen'.");
            Debug.Assert(!ReferenceEquals(arguments, null), "'arguments' must not be null.");
            Debug.Assert(!ReferenceEquals(rightParenToken, null), "'rightParenToken' must not be null.");
            Debug.Assert(rightParenToken.Kind == TokenKind.RightParen, "'rightParenToken' must have a TokenKind of 'RightParen'.");

            this.BaseExpression = baseExpression;
            this.LeftParenToken = leftParenToken;
            this.Arguments = arguments;
            this.RightParenToken = rightParenToken;
        }

        public BaseExpressionSyntax BaseExpression { get; private set; }

        public Token LeftParenToken { get; private set; }

        public IReadOnlyList<ArgumentSyntax> Arguments { get; private set; }

        public Token RightParenToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.BaseExpression;
                foreach (var child in this.Arguments)
                {
                    yield return child;
                }
            }
        }
    }

    internal sealed class ParenthesisExpressionSyntax : BaseExpressionSyntax
    {
        public ParenthesisExpressionSyntax(Token leftParenToken, BaseExpressionSyntax expression, Token rightParenToken)
        {
            Debug.Assert(!ReferenceEquals(leftParenToken, null), "'leftParenToken' must not be null.");
            Debug.Assert(leftParenToken.Kind == TokenKind.LeftParen, "'leftParenToken' must have a TokenKind of 'LeftParen'.");
            Debug.Assert(!ReferenceEquals(expression, null), "'expression' must not be null.");
            Debug.Assert(!ReferenceEquals(rightParenToken, null), "'rightParenToken' must not be null.");
            Debug.Assert(rightParenToken.Kind == TokenKind.RightParen, "'rightParenToken' must have a TokenKind of 'RightParen'.");

            this.LeftParenToken = leftParenToken;
            this.Expression = expression;
            this.RightParenToken = rightParenToken;
        }

        public Token LeftParenToken { get; private set; }

        public BaseExpressionSyntax Expression { get; private set; }

        public Token RightParenToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                yield return this.Expression;
            }
        }
    }

    internal sealed class IdentifierExpressionSyntax : BaseExpressionSyntax
    {
        public IdentifierExpressionSyntax(Token identifierToken)
        {
            Debug.Assert(!ReferenceEquals(identifierToken, null), "'identifierToken' must not be null.");
            Debug.Assert(identifierToken.Kind == TokenKind.Identifier, "'identifierToken' must have a TokenKind of 'Identifier'.");

            this.IdentifierToken = identifierToken;
        }

        public Token IdentifierToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }

    internal sealed class StringLiteralExpressionSyntax : BaseExpressionSyntax
    {
        public StringLiteralExpressionSyntax(Token stringToken)
        {
            Debug.Assert(!ReferenceEquals(stringToken, null), "'stringToken' must not be null.");
            Debug.Assert(stringToken.Kind == TokenKind.StringLiteral, "'stringToken' must have a TokenKind of 'StringLiteral'.");

            this.StringToken = stringToken;
        }

        public Token StringToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }

    internal sealed class NumberLiteralExpressionSyntax : BaseExpressionSyntax
    {
        public NumberLiteralExpressionSyntax(Token numberToken)
        {
            Debug.Assert(!ReferenceEquals(numberToken, null), "'numberToken' must not be null.");
            Debug.Assert(numberToken.Kind == TokenKind.NumberLiteral, "'numberToken' must have a TokenKind of 'NumberLiteral'.");

            this.NumberToken = numberToken;
        }

        public Token NumberToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }

    internal sealed class UnrecognizedExpressionSyntax : BaseExpressionSyntax
    {
        public UnrecognizedExpressionSyntax(Token unrecognizedToken)
        {
            Debug.Assert(!ReferenceEquals(unrecognizedToken, null), "'unrecognizedToken' must not be null.");

            this.UnrecognizedToken = unrecognizedToken;
        }

        public Token UnrecognizedToken { get; private set; }

        public override IEnumerable<BaseSyntaxNode> Children
        {
            get
            {
                return Enumerable.Empty<BaseSyntaxNode>();
            }
        }
    }
}
