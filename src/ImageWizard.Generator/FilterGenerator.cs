using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using SourceGenerator;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace Test;

[Generator]
public class MethodIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var methodDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
            "ImageWizard.Attributes.FilterAttribute",
                predicate: static (s, _) => s is MethodDeclarationSyntax,
                transform: static (ctx, _) => ctx).Collect();

        context.RegisterSourceOutput(methodDeclarations, Generate);
    }

    private void Generate(SourceProductionContext context, ImmutableArray<GeneratorAttributeSyntaxContext> generatorAttribute)
    {
        foreach (var group in generatorAttribute
                                        .GroupBy(x => ((MethodDeclarationSyntax)x.TargetNode).GetParentSyntax<ClassDeclarationSyntax>()))
        {
            string namespaceName = group.Key.GetNamespace();

            SourceBuilder builder = new SourceBuilder();
            builder.AddUsings("System.Globalization", "System.Text", "System.Text.RegularExpressions", "Microsoft.AspNetCore.WebUtilities", "ImageWizard", "ImageWizard.Attributes", "ImageWizard.Processing", "ImageWizard.Utils");
            builder.AddNamespace(namespaceName, x =>
            {                
                x.AddClass(Modifier.Public, group.Key.Identifier.Text, x =>
                {
                    x.AppendLine("public static IEnumerable<IFilterAction> Create()");
                    x.AppendBlock(x =>
                    {
                        x.AppendLine("return [ ");
                        foreach (var method in group)
                        {
                            IMethodSymbol methodSymbol = (IMethodSymbol)method.TargetSymbol;

                            string pattern = CreateParameterRegex(methodSymbol);
                            string parser = CreateParameterParser(methodSymbol);

                            x.AppendLine($"new FilterAction<{group.Key.Identifier.Text}>(\"{method.TargetSymbol.Name.ToLower()}\", new Regex(@\"{pattern}\"), (filter, group) => {{ {parser} }}),");
                        }
                        x.AppendLine("];");
                    });
                },
                isPartial: true,
                baseTypes: [ "IFilterFactory" ]);
            });

            context.AddSource($"{group.Key.Identifier.Text}.g.cs", builder.ToString());
        }

        //if (generatorAttribute.TargetNode is not MethodDeclarationSyntax methodDeclaration)
        //{
        //    return;
        //}

        //ClassDeclarationSyntax? classDeclarationSyntax = GetContainingClass(methodDeclaration);

        //if (classDeclarationSyntax == null)
        //{
        //    return;
        //}

        //context.AddSource($"{classDeclarationSyntax.Identifier.Text}_{methodDeclaration.Identifier.Text}_{C++}.g.cs", "");
    }

    private string CreateParameterRegex(IMethodSymbol methodSymbol)
    {
        List<ParameterItem> parameterItems = new List<ParameterItem>();

        foreach (IParameterSymbol parameterSymbol in methodSymbol.Parameters)
        {
            string p;

            if (parameterSymbol.Type.OriginalDefinition.TypeKind == TypeKind.Enum)
            {
                p = string.Join("|",
                                                parameterSymbol.Type
                                                .GetMembers()
                                                .Where(member => member.Kind is SymbolKind.Field)
                                                .Select(symbol => symbol.Name.ToLower())
                                                .ToArray());
            }
            else
            {
                p = parameterSymbol.Type.OriginalDefinition.SpecialType switch
                {
                    SpecialType.System_Byte
                    or SpecialType.System_Int16
                    or SpecialType.System_Int32
                    or SpecialType.System_Int64
                    => @"-?\d+",

                    SpecialType.System_Single
                    or SpecialType.System_Double
                    or SpecialType.System_Decimal
                    => @"-?\d+\.\d+",

                    SpecialType.System_Boolean => "True|False",
                    SpecialType.System_String => @"(('[^']*')|([A-Za-z0-9-_\s]+))",

                    _ => throw new Exception()
                };
            }

            parameterItems.Add(new ParameterItem()
            {
                Name = parameterSymbol.Name,
                Pattern = $@"(?<{parameterSymbol.Name}>{p})"
            });
        }

        return $@"^\({string.Join(",", parameterItems.Select(x => x.Pattern).ToArray())}\)$";
    }

    private string CreateParameterParser(IMethodSymbol methodSymbol)
    {
        StringBuilder builder = new StringBuilder();

        foreach (IParameterSymbol parameterSymbol in methodSymbol.Parameters)
        {
            string m;

            if (parameterSymbol.Type.OriginalDefinition.TypeKind == TypeKind.Enum)
            {
                string enumType = parameterSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                m = $"var {parameterSymbol.Name} = Enum.Parse<{enumType}>(group[\"{parameterSymbol.Name}\"].ValueSpan, true);";
            }
            else
            {
                ParameterType parameterType = parameterSymbol.Type.OriginalDefinition.SpecialType switch
                {
                    SpecialType.System_Byte => new ParameterType("byte", true, true, true),
                    SpecialType.System_Int16 => new ParameterType("short", true, true, true),
                    SpecialType.System_Int32 => new ParameterType("int", true, true, true),
                    SpecialType.System_Int64 => new ParameterType("long", true, true, true),
                    SpecialType.System_Single => new ParameterType("float", true, true, true),
                    SpecialType.System_Double => new ParameterType("double", true, true, true),
                    SpecialType.System_Decimal => new ParameterType("decimal", true, true, true),
                    SpecialType.System_Boolean => new ParameterType("bool", true, false, false),
                    SpecialType.System_String => new ParameterType("string", false, false, false),

                    _ => throw new Exception()
                };

                //use string
                if (parameterType.UseParsing == false)
                {
                    m = $"{parameterType.Name} {parameterSymbol.Name} = group[\"{parameterSymbol.Name}\"].Value;";
                }
                else
                {
                    //parse value
                    if (parameterType.UseCultureInfoForParsing)
                    {
                        m = $"{parameterType.Name} {parameterSymbol.Name} = {parameterType.Name}.Parse(group[\"{parameterSymbol.Name}\"].ValueSpan, CultureInfo.InvariantCulture);";
                    }
                    else
                    {
                        m = $"{parameterType.Name} {parameterSymbol.Name} = {parameterType.Name}.Parse(group[\"{parameterSymbol.Name}\"].ValueSpan);";
                    }

                    //check DPR attribute
                    if (parameterSymbol.GetAttributes() is ImmutableArray<AttributeData> attributes && attributes.Length > 0)
                    {
                        string dpr = attributes[0].AttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                        if (dpr == "global::ImageWizard.Attributes.DPRAttribute")
                        {
                            m += $"if (filter.Context.ProcessingContext.ClientHints.DPR != null) {{ {parameterSymbol.Name} = ({parameterType.Name}) ((double){parameterSymbol.Name} * filter.Context.ProcessingContext.ClientHints.DPR.Value); }}";
                        }
                    }
                }
            }

            builder.Append(m);
        }

        builder.Append($"filter.{methodSymbol.Name}({string.Join(",", methodSymbol.Parameters.Select(x => x.Name))});");

        return builder.ToString().Trim();
    }
}

class ParameterItem
{
    public string Name { get; set; }

    public string Pattern { get; set; }

    public string CodeLine { get; set; }   
}

class ParameterType
{
    public ParameterType(string name, bool useParsing, bool useCultureInfoForParsing, bool useDPR)
    {
        Name = name;
        UseParsing = useParsing;
        UseCultureInfoForParsing = useCultureInfoForParsing;
        UseDPR = useDPR;
    }

    public string Name { get; set; }
    public bool UseParsing { get; set; }
    public bool UseCultureInfoForParsing { get; set; }

    public bool UseDPR { get; set; }
}