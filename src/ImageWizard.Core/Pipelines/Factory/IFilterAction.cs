// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Processing;

public interface IFilterAction
{
    string Name { get; }

    bool TryExecute(IServiceProvider serviceProvider, string input, FilterContext filterContext);
}
