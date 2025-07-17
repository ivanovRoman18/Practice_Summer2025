using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread : IDisposable
{
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
        if (!_isRunning)
            throw new InvalidOperationException("Server thread is not running");

        _commandQueue.Add(command);
    }

    private void ProcessCommands()
    {
        try
        {
            while (_isRunning)
            {
                if (_scheduler.HasCommand())
                {
                    var command = _scheduler.Select();
                    command.Execute();

                    if (!command.IsCompleted())
                    {
                        _scheduler.Add(command);
                    }
                    continue;
                }

                if (_commandQueue.TryTake(out var newCommand, 100))
                {
                    newCommand.Execute();

                    if (!newCommand.IsCompleted())
                    {
                        _scheduler.Add(newCommand);
                    }
                }
            }

            while (_scheduler.HasCommand())
            {
                var command = _scheduler.Select();
                command.Execute();
            }
        }
        finally
        {
            _stoppedEvent.Set();
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _commandQueue.CompleteAdding();
    }

    public void WaitForCompletion() => _stoppedEvent.WaitOne();

    public void Dispose()
    {
        Stop();
        WaitForCompletion();
        _commandQueue.Dispose();
        _stoppedEvent.Dispose();
    }
}
