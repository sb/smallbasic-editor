// <copyright file="UseIsDefaultHelper.cs" company="MIT License">
// Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SmallBasic.Analyzers
{
    using System;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseIsDefaultHelperAnalyzer : DiagnosticAnalyzer
    {
        public const string Title = "Use IsDefault Helper";
        public const string MessageFormat = "Use '.IsDefault()' helper for all null checks.";
        public const string Category = "SmallBasic.Analyzers";

        public const string IsNullId = "SB1001";
        public const string ReferenceEqualsArg1Id = "SB1002";
        public const string ReferenceEqualsArg2Id = "SB1003";
        public const string EqualsNullId = "SB1004";
        public const string NullEqualsId = "SB1005";
        public const string NotEqualsNullId = "SB1006";
        public const string NullNotEqualsId = "SB1007";

        private static readonly DiagnosticDescriptor IsNullRule = new DiagnosticDescriptor(IsNullId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        private static readonly DiagnosticDescriptor ReferenceEqualsArg1Rule = new DiagnosticDescriptor(ReferenceEqualsArg1Id, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        private static readonly DiagnosticDescriptor ReferenceEqualsArg2Rule = new DiagnosticDescriptor(ReferenceEqualsArg2Id, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        private static readonly DiagnosticDescriptor EqualsNullRule = new DiagnosticDescriptor(EqualsNullId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        private static readonly DiagnosticDescriptor NullEqualsRule = new DiagnosticDescriptor(NullEqualsId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        private static readonly DiagnosticDescriptor NotEqualsNullRule = new DiagnosticDescriptor(NotEqualsNullId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        private static readonly DiagnosticDescriptor NullNotEqualsRule = new DiagnosticDescriptor(NullNotEqualsId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
            IsNullRule,
            ReferenceEqualsArg1Rule,
            ReferenceEqualsArg2Rule,
            EqualsNullRule,
            NullEqualsRule,
            NotEqualsNullRule,
            NullNotEqualsRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeIsExpression, SyntaxKind.IsPatternExpression);
            context.RegisterSyntaxNodeAction(AnalyzeInvocationExpression, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeBinaryExpression, SyntaxKind.EqualsExpression, SyntaxKind.NotEqualsExpression);
        }

        private static void AnalyzeIsExpression(SyntaxNodeAnalysisContext context)
        {
            var isPattern = (IsPatternExpressionSyntax)context.Node;
            if (isPattern.Pattern is ConstantPatternSyntax constant && IsDefaultOrNull(constant.Expression))
            {
                context.ReportDiagnostic(Diagnostic.Create(IsNullRule, isPattern.GetLocation()));
            }
        }

        private static void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;
            if (invocation.Expression is IdentifierNameSyntax identifier && identifier.Identifier.Text == "ReferenceEquals")
            {
                var arguments = invocation.ArgumentList.Arguments;
                if (arguments.Count == 2)
                {
                    if (IsDefaultOrNull(arguments[0].Expression))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(ReferenceEqualsArg2Rule, invocation.GetLocation()));
                    }
                    else if (IsDefaultOrNull(arguments[1].Expression))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(ReferenceEqualsArg1Rule, invocation.GetLocation()));
                    }
                }
            }
        }

        private static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = (BinaryExpressionSyntax)context.Node;
            if (binaryExpression.IsKind(SyntaxKind.EqualsExpression))
            {
                if (IsDefaultOrNull(binaryExpression.Right))
                {
                    context.ReportDiagnostic(Diagnostic.Create(EqualsNullRule, binaryExpression.GetLocation()));
                }
                else if (IsDefaultOrNull(binaryExpression.Left))
                {
                    context.ReportDiagnostic(Diagnostic.Create(NullEqualsRule, binaryExpression.GetLocation()));
                }
            }
            else if (binaryExpression.IsKind(SyntaxKind.NotEqualsExpression))
            {
                if (IsDefaultOrNull(binaryExpression.Right))
                {
                    context.ReportDiagnostic(Diagnostic.Create(NotEqualsNullRule, binaryExpression.GetLocation()));
                }
                else if (IsDefaultOrNull(binaryExpression.Left))
                {
                    context.ReportDiagnostic(Diagnostic.Create(NullNotEqualsRule, binaryExpression.GetLocation()));
                }
            }
        }

        private static bool IsDefaultOrNull(ExpressionSyntax syntax)
        {
            return syntax.Kind() == SyntaxKind.NullLiteralExpression || syntax.Kind() == SyntaxKind.DefaultLiteralExpression;
        }
    }

    [Shared]
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UseIsDefaultHelperCodeFixProvider))]
    public class UseIsDefaultHelperCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
            UseIsDefaultHelperAnalyzer.IsNullId,
            UseIsDefaultHelperAnalyzer.ReferenceEqualsArg1Id,
            UseIsDefaultHelperAnalyzer.ReferenceEqualsArg2Id,
            UseIsDefaultHelperAnalyzer.EqualsNullId,
            UseIsDefaultHelperAnalyzer.NullEqualsId,
            UseIsDefaultHelperAnalyzer.NotEqualsNullId,
            UseIsDefaultHelperAnalyzer.NullNotEqualsId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.Single();
            var position = diagnostic.Location.SourceSpan.Start;

            var codeAction = CodeAction.Create(
                title: UseIsDefaultHelperAnalyzer.Title,
                createChangedDocument: token =>
                {
                    switch (diagnostic.Id)
                    {
                        case UseIsDefaultHelperAnalyzer.IsNullId:
                            return ApplyFix<IsPatternExpressionSyntax>(context.Document, position, isPattern => isPattern.Expression, token);
                        case UseIsDefaultHelperAnalyzer.ReferenceEqualsArg1Id:
                            return ApplyFix<InvocationExpressionSyntax>(context.Document, position, invocation => invocation.ArgumentList.Arguments[0].Expression, token);
                        case UseIsDefaultHelperAnalyzer.ReferenceEqualsArg2Id:
                            return ApplyFix<InvocationExpressionSyntax>(context.Document, position, invocation => invocation.ArgumentList.Arguments[1].Expression, token);
                        case UseIsDefaultHelperAnalyzer.EqualsNullId:
                            return ApplyFix<BinaryExpressionSyntax>(context.Document, position, binaryExpression => binaryExpression.Left, token);
                        case UseIsDefaultHelperAnalyzer.NullEqualsId:
                            return ApplyFix<BinaryExpressionSyntax>(context.Document, position, binaryExpression => binaryExpression.Right, token);
                        case UseIsDefaultHelperAnalyzer.NotEqualsNullId:
                            return ApplyFix<BinaryExpressionSyntax>(context.Document, position, binaryExpression => binaryExpression.Left, token, negated: true);
                        case UseIsDefaultHelperAnalyzer.NullNotEqualsId:
                            return ApplyFix<BinaryExpressionSyntax>(context.Document, position, binaryExpression => binaryExpression.Right, token, negated: true);
                        default:
                            throw new InvalidOperationException($"Cannot fix Diagnostic Id '{diagnostic.Id}'.");
                    }
                },
                equivalenceKey: UseIsDefaultHelperAnalyzer.Title);

            context.RegisterCodeFix(codeAction, diagnostic);
            return Task.CompletedTask;
        }

        private static async Task<Document> ApplyFix<TSyntaxNode>(Document document, int location, Func<TSyntaxNode, ExpressionSyntax> extractor, CancellationToken token, bool negated = false)
            where TSyntaxNode : SyntaxNode
        {
            var oldRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);
            var oldNode = oldRoot.FindToken(location).Parent.AncestorsAndSelf().OfType<TSyntaxNode>().Single();

            ExpressionSyntax newNode = SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                extractor(oldNode),
                SyntaxFactory.IdentifierName("IsDefault")));

            if (negated)
            {
                newNode = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, newNode);
            }

            var newRoot = oldRoot.ReplaceNode(oldNode, newNode);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
