namespace com.github.Tobotobo.DotnetWinMergeReport;

public class TasksRunner<T> : List<T> where T : Task
{
    // 実行中のタスク
    private IList<T> _runningTasks = [];
    public IReadOnlyCollection<T> RunningTasks { get => (IReadOnlyCollection<T>)_runningTasks; }

    // 完了したタスクの数
    public int CompletedCount { get; private set; }

    // 次に実行するタスクのインデックス
    private int _nextTaskIndex = 0;

    public Task Start(int thredCount = 1)
    {
        CompletedCount = 0;
        _nextTaskIndex = 0;
        _runningTasks.Clear();

        return Task.Run(() =>
        {
            while (true)
            {
                // 完了したタスクを削除
                var completedTasks = _runningTasks.Where(task => task.IsCompleted).ToList();
                foreach (var completedTask in completedTasks)
                {
                    _runningTasks.Remove(completedTask);
                    CompletedCount += 1;
                }

                // 全てのタスクが完了したら終了
                if (CompletedCount >= this.Count)
                {
                    break;
                }

                // 実行中のタスクが指定した数になるまで実行
                while (RunningTasks.Count < thredCount && _nextTaskIndex < this.Count)
                {
                    var task = this[_nextTaskIndex];
                    _nextTaskIndex += 1;
                    _runningTasks.Add(task);
                    if (!task.IsCompleted)
                    {
                        task.Start();
                    }
                }

                // 待機
                Thread.Sleep(100);
            }
        });
    }
}