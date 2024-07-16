// Copyright (c) usercode
// https://github.com/usercode/DragonFly
// MIT License

using System.Text;

namespace SourceGenerator;

public class SourceBuilder
{
    private StringBuilder Builder { get; } = new StringBuilder();

    private int Tabs { get; set; }

    public void AddTab() => Tabs++;

    public void RemoveTab() => Tabs--;

    public void AppendTabs()
    {
        for (var i = 0; i < Tabs; i++)
        {
            Builder.Append("    ");
        }
    }

    public void AppendLineBreak()
    {
        Builder.AppendLine();
    }

    public void Append(string source)
    {
        Builder.Append(source);
    }

    public void AppendLine(string source)
    {
        AppendTabs();

        Builder.AppendLine(source);
    }

    public void AppendLine()
    {
        AppendTabs();

        Builder.AppendLine();
    }

    public void AppendBlock(Action<SourceBuilder> action, bool command = false)
    {
        AppendLine("{");
        AddTab();

        action(this);

        RemoveTab();
        
        if (command)
        {
            AppendLine("};");
        }
        else
        {
            AppendLine("}");
        }
    }

    public void AppendTryCatch(Action<SourceBuilder> tryBlock, Action<SourceBuilder>? catchBlock = null, Action<SourceBuilder>? finallyBlock = null)
    {
        AppendLine("try");
        AppendBlock(tryBlock);

        if (catchBlock != null)
        {
            AppendLine("catch (Exception ex)");
            AppendBlock(catchBlock);
        }

        if (finallyBlock != null)
        {
            AppendLine("finally");
            AppendBlock(finallyBlock);
        }
    }

    public override string ToString() => Builder.ToString();
}
