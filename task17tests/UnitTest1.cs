using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

public class Task17Tests
{
    [Fact]
    public void HardStop_StopsImmediately()
    {
        var server = new ServerThread();
        var testCommand1Executed = false;
        var testCommand2Executed = false;

        server.AddCommand(new ActionCommand(() => testCommand1Executed = true));
        server.AddCommand(new ServerThread.HardStopCommand(server));
        server.AddCommand(new ActionCommand(() => testCommand2Executed = true));

        server.WaitForCompletion();

        Assert.True(testCommand1Executed);
        Assert.False(testCommand2Executed);
    }

    [Fact]
    public void SoftStop_StopsAfterQueueIsEmpty()
    {
        var server = new ServerThread();
        var testCommand1Executed = false;
        var testCommand2Executed = false;

        server.AddCommand(new ActionCommand(() => testCommand1Executed = true));
        server.AddCommand(new ServerThread.SoftStopCommand(server));
        server.AddCommand(new ActionCommand(() => testCommand2Executed = true));

        server.WaitForCompletion();

        Assert.True(testCommand1Executed);
        Assert.True(testCommand2Executed);
    }

    [Fact]
    public void HardStop_ThrowsIfCalledFromAnotherThread()
    {
        var server = new ServerThread();
        Assert.Throws<InvalidOperationException>(() => server.HardStop());
    }

    [Fact]
    public void SoftStop_ThrowsIfCalledFromAnotherThread()
    {
        var server = new ServerThread();
        Assert.Throws<InvalidOperationException>(() => server.SoftStop());
    }

    [Fact]
    public void ServerThread_ProcessesCommandsInOrder()
    {
        var server = new ServerThread();
        var results = new List<int>();

        server.AddCommand(new ActionCommand(() => results.Add(1)));
        server.AddCommand(new ActionCommand(() => results.Add(2)));
        server.AddCommand(new ActionCommand(() => results.Add(3)));
        server.AddCommand(new ServerThread.SoftStopCommand(server));

        server.WaitForCompletion();

        Assert.Equal(new[] { 1, 2, 3 }, results);
    }

    private class ActionCommand : ICommand
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action();
        }
    }
}
