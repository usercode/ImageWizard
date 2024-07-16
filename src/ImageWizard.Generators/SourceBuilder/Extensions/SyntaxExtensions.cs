// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator;

public static class SyntaxExtensions
{
    public static string GetString(this ExpressionSyntax expression)
    {
        if (expression is LiteralExpressionSyntax literal)
        {
            return literal.Token.Text;
        }
        else if (expression is InvocationExpressionSyntax invocation) //nameof(..)
        {
            if (invocation.ArgumentList.Arguments[0].Expression is IdentifierNameSyntax identifier)
            {
                return identifier.Identifier.ValueText;
            }
        }
        else if (expression is TypeOfExpressionSyntax typeOfExpressionSyntax)
        {
            return typeOfExpressionSyntax.Type.ToString();
        }
        else
        {
            return expression.ToString();
        }

        return string.Empty;
    }

    public static string[] GetFirstAttributeParameters(this TypeDeclarationSyntax typeSyntax, string attributeName)
    {
        return typeSyntax.AttributeLists
                                                  .SelectMany(x => x.Attributes)
                                                  .Where(x => x.Name.ToString() == attributeName)
                                                  .Select(x => x.ArgumentList?.Arguments[0].Expression.GetString())
                                                  .Where(x => x != null)
                                                  .Cast<string>()
                                                  .ToArray();
    }

    public static T GetParentSyntax<T>(this SyntaxNode methodDeclaration)
        where T : SyntaxNode
    {
        SyntaxNode? current = methodDeclaration.Parent;
        while (current != null && !(current is T))
        {
            current = current.Parent;
        }

        return current as T;
    }

    public static string GetNamespace(this BaseTypeDeclarationSyntax syntax)
    {
        // If we don't have a namespace at all we'll return an empty string
        // This accounts for the "default namespace" case
        string nameSpace = string.Empty;

        // Get the containing syntax node for the type declaration
        // (could be a nested type, for example)
        SyntaxNode? potentialNamespaceParent = syntax.Parent;

        // Keep moving "out" of nested classes etc until we get to a namespace
        // or until we run out of parents
        while (potentialNamespaceParent != null &&
                potentialNamespaceParent is not NamespaceDeclarationSyntax
                && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        // Build up the final namespace by looping until we no longer have a namespace declaration
        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            // We have a namespace. Use that as the type
            nameSpace = namespaceParent.Name.ToString();

            // Keep moving "out" of the namespace declarations until we 
            // run out of nested namespace declarations
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                {
                    break;
                }

                // Add the outer namespace as a prefix to the final namespace
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }
        }

        // return the final namespace
        return nameSpace;
    }
}
