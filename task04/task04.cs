namespace task04;

public interface ISpaceship
{
    void MoveForward();      // Движение вперед
    void Rotate(int angle);  // Поворот на угол (градусы)
    void Fire();             // Выстрел ракетой
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
}

public class Cruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;

    public void MoveForward()
    {
        Console.WriteLine($"Cruiser moving forward at speed: {Speed}");
    }

    public void Rotate(int angle)
    {
        Console.WriteLine($"Cruiser rotating {angle} degrees.");
    }

    public void Fire()
    {
        Console.WriteLine($"Cruiser shoots power: {FirePower}");
    }
}

public class Fighter : ISpaceship
{
    public int Speed => 100;
    public int FirePower => 50;

    public void MoveForward()
    {
        Console.WriteLine($"Fighter moving forward at speed: {Speed}");
    }

    public void Rotate(int angle)
    {
        Console.WriteLine($"Fighter rotating {angle} degrees.");
    }

    public void Fire()
    {
        Console.WriteLine($"Fighter shoots power: {FirePower}");
    }
    
}