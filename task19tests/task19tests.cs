using System;
using System.Threading;
using Xunit;

public class Task19Tests
{
    [Fact]
    public void TestCommands_ExecuteThreeTimesEach()
    {
        var scheduler = new RoundRobin();
        using var server = new ServerThread(scheduler);

        for (int i = 1; i <= 5; i++)
            server.AddCommand(new TestCommand(i));

        server.AddCommand(new ServerThread.HardStopCommand(server));
        server.WaitForCompletion();
    }

    [Fact]
    public void HardStop_ImmediatelyStopsExecution()
    {
        var scheduler = new RoundRobin();
        using var server = new ServerThread(scheduler);

        server.AddCommand(new InfiniteCommand());
        server.AddCommand(new ServerThread.HardStopCommand(server));
        server.WaitForCompletion();
    }

    private class InfiniteCommand : ICommand
    {
        public void Execute() => Thread.Sleep(100);
    }
}
