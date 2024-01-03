using System.Diagnostics;

namespace com.github.Tobotobo.DotnetWinMergeReport;

public class CompareTask(CompareFile compareFile, Process winMergeProcess, Action action) : Task(action)
{
    public CompareFile CompareFile { get; } = compareFile;
    public Process WinMergeProcess { get; } = winMergeProcess;
}