using System.Text;
using System.CommandLine;

namespace com.github.Tobotobo.DotnetWinMergeReport;

public class App
{
    public int Run(string[] args)
    {
        // コンソールの出力エンコーディングをUTF-16に設定
        Console.OutputEncoding = Encoding.Unicode;

        var leftPathOption = new Option<string>(
            "--left-path",
            "左側で開くフォルダー")
        {
            IsRequired = true
        };
        var rightPathOption = new Option<string>(
            "--right-path",
            "右側で開くフォルダー")
        {
            IsRequired = true
        };
        var outputPathOption = new Option<string>(
            "--output-path",
            "レポートを出力するフォルダー")
        {
            IsRequired = true
        };
        var rootCommand = new RootCommand("WinMergeで指定されたフォルダーを比較しレポートを出力する") {
            leftPathOption,
            rightPathOption,
            outputPathOption,
        };
        rootCommand.SetHandler((leftPath, rightPath, outputPath) =>
        {
            var winMergeUExePath = Path.Combine(Environment.GetEnvironmentVariable("WINMERGE_PATH") ?? "", "WinMergeU.exe");
            RootHandler(winMergeUExePath, leftPath, rightPath, outputPath);
        }, leftPathOption, rightPathOption, outputPathOption);

        return rootCommand.Invoke(args);
    }

    private static void RootHandler(
        string winMergeUExePath,
        string leftPath,
        string rightPath,
        string outputPath)
    {
        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
        }
        Directory.CreateDirectory(outputPath);

        var compareTasksRunner = WinMergeReport.CreateCompareTasksRunner(
            winMergeUExePath,
            leftPath,
            rightPath,
            outputPath);

        var viewState = new ViewState(
            Console.CursorLeft,
            Console.CursorTop,
            0,
            compareTasksRunner.Count,
            0
        );

        bool updateView()
        {
            viewState = viewState! with
            {
                Value = compareTasksRunner!.CompletedCount,
                SpinnerIndex = viewState.SpinnerIndex + 1
            };
            View.Update(viewState);
            return compareTasksRunner.CompletedCount < compareTasksRunner.Count;
        }

        Console.CursorVisible = false; // コンソールのカーソルを非表示にする
        var updateViewTask = Task.Run(() =>
        {
            while (updateView())
            {
                Thread.Sleep(100);
            }
        });

        // 比較タスクの実行
        compareTasksRunner.Start(10).Wait();

        // 比較結果の最終表示
        updateViewTask.Wait();
        Console.CursorVisible = true; // コンソールのカーソルを表示する
    }





    // string GetRelativePath(string uri1, string uri2)
    // {
    //     var u1 = new Uri(uri1);
    //     var u2 = new Uri(uri2);
    //     var relativeUri = u1.MakeRelativeUri(u2);
    //     var relativePath = relativeUri.ToString();
    //     var index = relativePath.IndexOf('/');
    //     if (index == -1) return relativePath;
    //     relativePath = relativePath.Substring(index + 1);
    //     return relativePath;
    // }

}



