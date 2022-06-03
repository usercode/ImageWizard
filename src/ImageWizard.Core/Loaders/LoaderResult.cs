// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

/// <summary>
/// LoaderResult
/// </summary>
public readonly struct LoaderResult
{
    public static LoaderResult Success(OriginalData originalData) => new LoaderResult(LoaderResultState.Success, originalData ?? throw new ArgumentNullException(nameof(originalData)));

    public static LoaderResult NotModified() => new LoaderResult(LoaderResultState.NotModified, null);

    public static LoaderResult Failed() => new LoaderResult(LoaderResultState.Failed, null);

    private LoaderResult(LoaderResultState state, OriginalData? result)
    {
        State = state;
        Result = result;
    }

    /// <summary>
    /// State
    /// </summary>
    public LoaderResultState State { get; }

    /// <summary>
    /// Result
    /// </summary>
    //[MemberNotNullWhen(LoaderResultState.Success, nameof(State))]
    public OriginalData? Result { get; }
}
