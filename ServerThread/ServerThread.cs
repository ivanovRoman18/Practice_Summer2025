using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread : IDisposable
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _server;
        public HardStopCommand(ServerThread server) => _server = server;
        public void Execute() => _server.HardStop();
    }

    private readonly Thread _thread;
    private readonly BlockingCollection<ICommand> _commandQueue = new();
    private readonly IScheduler _scheduler;
    private volatile bool _isRunning = true;
    private readonly ManualResetEvent _stoppedEvent = new(false);

    public ServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _thread = new Thread(ProcessCommands) { IsBackground = true };
        _thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        if (!_isRunning) throw new InvalidOperationException("Server is stopping");
        _commandQueue.Add(command);
    }

    private void ProcessCommands()
    {
        try
        {
            while (_isRunning)
            {
                ProcessSchedulerCommands();
                ProcessNewCommands();
            }
        }
        finally
        {
            _stoppedEvent.Set();
        }
    }

    private void ProcessSchedulerCommands()
    {
        if (_scheduler.HasCommand())
        {
            var command = _scheduler.Select();
            ProcessCommand(command);
        }
    }

    private void ProcessNewCommands()
    {
        if (_commandQueue.TryTake(out var command, 100))
        {
            ProcessCommand(command);
        }
    }

    private void ProcessCommand(ICommand command)
    {
        command.Execute();

        if (command is IExecutionCountable countable && !countable.IsComplete)
        {
            _scheduler.Add(command);
        }
    }

    private void HardStop()
    {
        _isRunning = false;
        _commandQueue.CompleteAdding();
    }

    public void WaitForCompletion() => _stoppedEvent.WaitOne();

    public void Dispose()
    {
        HardStop();
        WaitForCompletion();
        _commandQueue.Dispose();
        _stoppedEvent.Dispose();
    }
}

public interface IExecutionCountable
{
    int ExecutionCount { get; }
    bool IsComplete { get; }
}
