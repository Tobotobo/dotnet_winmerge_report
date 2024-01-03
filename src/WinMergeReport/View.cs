using System.Data;

namespace com.github.Tobotobo.DotnetWinMergeReport;

public class View
{
    private static readonly String[] spinner = ["⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏"];

    public static void Update(ViewState viewState)
    {
        Console.SetCursorPosition(viewState.CursorLeft, viewState.CursorTop);

        // スピナー
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(spinner[viewState.SpinnerIndex % spinner.Length]);
            Console.ResetColor();
        }

        // プログレスバー
        {
            var progress = viewState.Value * viewState.MaxProgreessCount / viewState.MaxValue;
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(new String('█', progress));
            Console.ResetColor();
            Console.Write(new String('.', viewState.MaxProgreessCount - progress));
            Console.Write("]");
            Console.ResetColor();
        }

        // パーセンテージ
        {
            Console.Write($" {viewState.Value * 100 / viewState.MaxValue}%");
        }

        // 現在値
        {
            Console.Write($" ({viewState.Value}/{viewState.MaxValue})");
        }
    }

    // public void Start()
    // {
    //     var cursorTop = Console.CursorTop;
    //     var cursorLeft = Console.CursorLeft;

    //     _task = Task.Run(() =>
    //     {
    //         Console.CursorVisible = false;
    //         while (Value < MaxValue)
    //         {
    //             // Console.Clear();
    //             // カーソル位置を先頭に戻す
    //             Console.SetCursorPosition(cursorLeft, cursorTop);
    //             // Console.SetCursorPosition(0, 0);
    //             if (Update != null)
    //             {
    //                 Update(this);
    //             }
    //             // if (Header != null)
    //             // {
    //             //     Header(this, false);
    //             // }
    //             Update_();
    //             // if (Footer != null)
    //             // {
    //             //     Footer(this, false);
    //             // }
    //             Thread.Sleep(100);
    //         }
    //         // Console.SetCursorPosition(cursorLeft, cursorTop);
    //         // Console.SetCursorPosition(0, 0);
    //         // if (Header != null)
    //         // {
    //         //     Header(this, true);
    //         // }
    //         // Update_();
    //         // if (Footer != null)
    //         // {
    //         //     Footer(this, true);
    //         // }
    //         Console.CursorVisible = true;
    //     });
    // }

    // private void Update_()
    // {

    // }
}