// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.DocNET.Filters.Base;

public abstract class DocNETFilter : Filter<DocNETFilterContext>
{
    public DocNETFilter()
    {

    }

    public override string Namespace => "docnet";

}
