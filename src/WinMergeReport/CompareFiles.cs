namespace com.github.Tobotobo.DotnetWinMergeReport;

public class CompareFiles : Dictionary<string, CompareFile>
{

    public CompareFile AddLeft(string basePath, string fullPath)
    {
        var relativePath = GetRelativePath(basePath, fullPath);
        if (!this.TryGetValue(relativePath, out var compareFile))
        {
            compareFile = new CompareFile(relativePath);
            this.Add(relativePath, compareFile);
        }
        compareFile.LeftPath = fullPath;
        return compareFile;
    }

    public CompareFile AddRight(string basePath, string fullPath)
    {
        var relativePath = GetRelativePath(basePath, fullPath);
        if (!this.TryGetValue(relativePath, out var compareFile))
        {
            compareFile = new CompareFile(relativePath);
            this.Add(relativePath, compareFile);
        }
        compareFile.RightPath = fullPath;
        return compareFile;
    }

    private static string GetRelativePath(string uri1, string uri2)
    {
        var u1 = new Uri(uri1);
        var u2 = new Uri(uri2);
        var relativeUri = u1.MakeRelativeUri(u2);
        var relativePath = relativeUri.ToString();
        var index = relativePath.IndexOf('/');
        if (index == -1) return relativePath;
        relativePath = relativePath[(index + 1)..];
        return relativePath;
    }
}