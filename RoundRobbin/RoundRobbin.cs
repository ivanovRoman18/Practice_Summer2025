using System.Collections.Generic;


public class RoundRobin : IScheduler
{
    private readonly Queue<ICommand> _commands = new();
    private readonly object _lock = new();

    public bool HasCommand()
    {
        lock (_lock)
        {
            return _commands.Count > 0;
        }
    }

    public ICommand Select()
    {
        lock (_lock)
        {
            return _commands.Dequeue();
        }
    }

    public void Add(ICommand cmd)
    {
        lock (_lock)
        {
            _commands.Enqueue(cmd);
        }
    }
}
