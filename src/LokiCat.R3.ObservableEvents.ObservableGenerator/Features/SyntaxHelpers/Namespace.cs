using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LokiCat.R3.ObservableEvents.ObservableGenerator.Features.SyntaxHelpers;

public static class Namespace
{
    public static string GetNamespace(SyntaxNode node) {
        return node.Ancestors()
                   .OfType<BaseNamespaceDeclarationSyntax>()
                   .FirstOrDefault()
                   ?.Name.ToString() ?? "Global";
    }
}