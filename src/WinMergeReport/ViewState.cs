namespace com.github.Tobotobo.DotnetWinMergeReport;

public record ViewState(
    int CursorLeft,
    int CursorTop,
    int Value,
    int MaxValue,
    int SpinnerIndex,
    int MaxProgreessCount = 30
);