namespace com.github.Tobotobo.DotnetWinMergeReport;

public class CompareFile(string relativePath)
{
    public string RelativePath { get; } = relativePath;
    public string? LeftPath { get; set; } = null;
    public string? RightPath { get; set; } = null;

    public bool LeftExists { get => LeftPath != null; }
    public bool RightExists { get => RightPath != null; }

    public string? CompareResult { get; set; } = null;

    public override string ToString()
    {
        return $"{(LeftExists ? "L" : " ")}{(RightExists ? "R" : " ")}:{RelativePath}";
    }
}