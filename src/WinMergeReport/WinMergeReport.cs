using com.github.Tobotobo.DotnetWinMergeRapper;

namespace com.github.Tobotobo.DotnetWinMergeReport;

public static class WinMergeReport
{
    public static TasksRunner<CompareTask> CreateCompareTasksRunner(
        string winMergeUExePath,
        string leftPath,
        string rightPath,
        string outputPath)
    {
        ValidateFilePath(winMergeUExePath, nameof(winMergeUExePath));
        ValidateDirPath(leftPath, nameof(leftPath));
        ValidateDirPath(rightPath, nameof(rightPath));
        ValidateDirPath(outputPath, nameof(outputPath));

        // フルパスに変換
        winMergeUExePath = Path.GetFullPath(winMergeUExePath);
        leftPath = Path.GetFullPath(leftPath);
        rightPath = Path.GetFullPath(rightPath);
        outputPath = Path.GetFullPath(outputPath);

        // 指定したフォルダの中にあるファイルの一覧を取得する
        var compareFiles = GetCompareFiles(leftPath, rightPath);

        // WinMergeの設定
        var commandLineOptionsBase = new CommandLineOptions
        {
            E = true,
            Minimize = true,
            NonInteractive = true,
            U = true,
            EnableExitCode = true,
            S_ = true,
        };
        var iniFileSettings = new IniFileSettings
        {
            { "ReportFiles/IncludeFileCmpReport", "1" },
            { "ReportFiles/ReportType", "2" },
            { "Settings/DirViewExpandSubdirs", "1" },
        };

        // 比較タスクの作成
        var tasksRunner = new TasksRunner<CompareTask>();
        foreach (var compareFile in compareFiles.Values)
        {
            if (!compareFile.LeftExists || !compareFile.RightExists)
            {
                continue;
            }

            var task = CreateCompareTask(
                compareFile,
                outputPath,
                winMergeUExePath,
                commandLineOptionsBase,
                iniFileSettings);

            tasksRunner.Add(task);
        }

        return tasksRunner;
    }

    private static CompareTask CreateCompareTask(
        CompareFile compareFile,
        string outputPath,
        string winMergeUExePath,
        CommandLineOptions commandLineOptionsBase,
        IniFileSettings iniFileSettings)
    {
        var outputFilePath = Path.Combine(outputPath, compareFile.RelativePath.Replace('\\', '_').Replace('/', '_') + ".html");
        var commandLineOptions = commandLineOptionsBase with
        {
            LeftPath = compareFile.LeftPath,
            RightPath = compareFile.RightPath,
            OR = outputFilePath,
        };
        var process = WinMergeRapper.Create(winMergeUExePath, commandLineOptions, iniFileSettings);
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        var task = new CompareTask(compareFile, process, () =>
        {
            process.Start();
            process.WaitForExit();
        });

        return task;
    }

    private static CompareFiles GetCompareFiles(string leftPath, string rightPath)
    {
        static void addFiles(string path, Action<string, string> action)
        {
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                action(path, file);
            }
        }
        var compareFiles = new CompareFiles();
        addFiles(leftPath, (basePath, fullPath) =>
        {
            compareFiles.AddLeft(basePath, fullPath);
        });
        addFiles(rightPath, (basePath, fullPath) =>
        {
            compareFiles.AddRight(basePath, fullPath);
        });
        return compareFiles;
    }

    private static void ValidateFilePath(string path, string paramName)
    {
        if (path == null) throw new ArgumentNullException(paramName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }
    }

    private static void ValidateDirPath(string path, string paramName)
    {
        if (path == null) throw new ArgumentNullException(paramName);
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException(path);
        }
    }
}