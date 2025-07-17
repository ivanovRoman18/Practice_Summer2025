using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread : IDisposable
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;

        public HardStopCommand(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }

        public void Execute()
        {
            _serverThread.HardStop();
        }
    }

    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;

        public SoftStopCommand(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }

        public void Execute()
        {
            _serverThread.SoftStop();
        }
    }

    private readonly Thread _thread;
    private readonly BlockingCollection<ICommand> _commandQueue = new();
    private volatile bool _isRunning = true;
    private volatile bool _hardStopRequested = false;
    private readonly ManualResetEvent _stoppedEvent = new(false);

    public event Action<ICommand?, Exception?>? ExceptionHandler;

    public ServerThread()
    {
        _thread = new Thread(ProcessCommands)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        if (!_isRunning || _hardStopRequested)
            throw new InvalidOperationException("Server thread is not running");

        _commandQueue.Add(command);
    }

    private void ProcessCommands()
    {
        try
        {
            while (_isRunning && !_hardStopRequested)
            {
                var command = _commandQueue.Take();
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    ExceptionHandler?.Invoke(command, ex);
                }
            }

            // Обработать оставшиеся команды только после SoftStop
            if (!_hardStopRequested)
            {
                while (_commandQueue.TryTake(out var command))
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler?.Invoke(command, ex);
                    }
                }
            }
        }
        finally
        {
            _stoppedEvent.Set();
        }
    }

    public void SoftStop()
    {
        if (Thread.CurrentThread != _thread)
            throw new InvalidOperationException("SoftStop can only be called from the server thread itself");

        _isRunning = false;
    }

    public void HardStop()
    {
        if (Thread.CurrentThread != _thread)
            throw new InvalidOperationException("HardStop can only be called from the server thread itself");

        _hardStopRequested = true;
        _isRunning = false;
        _commandQueue.CompleteAdding();
    }

    public void WaitForCompletion()
    {
        _stoppedEvent.WaitOne();
    }

    public void Dispose()
    {
        _hardStopRequested = true;
        _isRunning = false;
        _commandQueue.CompleteAdding();
        WaitForCompletion();
        _commandQueue.Dispose();
        _stoppedEvent.Dispose();
        GC.SuppressFinalize(this);
    }
}
    