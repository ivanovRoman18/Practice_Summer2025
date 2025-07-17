using System;
using System.Threading;
using Xunit;

public class Task18Tests
{
    [Fact]
    public void LongRunningCommand_ExecutesInChunks()
    {
        var scheduler = new RoundRobin();
        using var server = new ServerThread(scheduler);

        var longCommand = new LongRunningCommand(3);
        server.AddCommand(longCommand);

        Thread.Sleep(100);

        Assert.Equal(0, longCommand.RemainingExecutions);
    }

    [Fact]
    public void MixedCommands_ExecuteProperly()
    {
        var scheduler = new RoundRobin();
        using var server = new ServerThread(scheduler);

        var longCommand = new LongRunningCommand(2);
        var simpleCommand = new SimpleCommand();

        server.AddCommand(longCommand);
        server.AddCommand(simpleCommand);

        Thread.Sleep(150);

        Assert.True(simpleCommand.Completed);
        Assert.Equal(0, longCommand.RemainingExecutions);
    }

    [Fact]
    public void RoundRobin_Fairness()
    {
        var scheduler = new RoundRobin();
        using var server = new ServerThread(scheduler);

        var cmd1 = new LongRunningCommand(3);
        var cmd2 = new LongRunningCommand(3);

        scheduler.Add(cmd1);
        scheduler.Add(cmd2);

        Thread.Sleep(200);

        Assert.InRange(Math.Abs(cmd1.ExecutionCount - cmd2.ExecutionCount), 0, 1);
    }

    private class LongRunningCommand : ICommand
    {
        public int RemainingExecutions { get; private set; }
        public int ExecutionCount { get; private set; }

        public LongRunningCommand(int executions)
        {
            RemainingExecutions = executions;
        }

        public void Execute()
        {
            Thread.Sleep(10);
            ExecutionCount++;
            RemainingExecutions--;
        }

        public bool IsCompleted() => RemainingExecutions <= 0;
    }

    private class SimpleCommand : ICommand
    {
        public bool Completed { get; private set; }

        public void Execute()
        {
            Completed = true;
        }

        public bool IsCompleted() => Completed;
    }
}

