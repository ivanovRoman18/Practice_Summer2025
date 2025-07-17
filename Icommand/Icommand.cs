public interface ICommand
{
    void Execute();
}

public class TestCommand(int id) : ICommand
{
    public int Counter { get; private set; } = 0;
    private readonly int _id = id;

    public void Execute()
    {
        Console.WriteLine($"Поток {_id} вызов {++Counter}");
    }
}
