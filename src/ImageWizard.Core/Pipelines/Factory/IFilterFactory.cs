// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Processing;

public interface IFilterFactory
{
    abstract static IEnumerable<IFilterAction> Create();
}
