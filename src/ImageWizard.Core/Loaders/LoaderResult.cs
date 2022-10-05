// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// LoaderResult
/// </summary>
public readonly struct LoaderResult
{
    public static LoaderResult Success(OriginalData originalData) => new LoaderResult(LoaderResultState.Success, originalData ?? throw new ArgumentNullException(nameof(originalData)));

    public static LoaderResult NotModified() => new LoaderResult(LoaderResultState.NotModified, null);

    public static LoaderResult NotFound() => new LoaderResult(LoaderResultState.NotFound, null);

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
