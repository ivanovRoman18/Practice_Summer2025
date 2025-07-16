using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ScottPlot;

class Program
{
    static double Function(double x) => Math.Sin(x);

    static double SingleThreadIntegral(double a, double b, double step)
    {
        double sum = 0;
        for (double x = a; x < b; x += step)
            sum += Function(x) * step;
        return sum;
    }

    static double MultiThreadIntegral(double a, double b, double step, int threads)
    {
        double total = 0;
        object lockObj = new object();

        Parallel.For(0, threads, new ParallelOptions { MaxDegreeOfParallelism = threads }, i =>
        {
            double localSum = 0;
            double start = a + i * (b - a) / threads;
            double end = start + (b - a) / threads;

            for (double x = start; x < end; x += step)
                localSum += Function(x) * step;

            lock (lockObj) { total += localSum; }
        });

        return total;
    }

    static double MeasureTime(Func<double> action, int iterations = 10, int warmup = 3)
    {   
        for (int i = 0; i < warmup; i++)
            action();

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            action();
        sw.Stop();

        return sw.Elapsed.TotalMilliseconds / iterations;
    }

    static void Main()
    {
        const double a = -100, b = 100;
        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4 };
        int[] threadCounts = { 1, 2, 4, 6, 8, 12, 16 };

        
        double optimalStep = steps[0];
        double minTime = double.MaxValue;

        foreach (double step in steps)
        {
            double time = MeasureTime(() => SingleThreadIntegral(a, b, step), 20, 5);
            Console.WriteLine($"Шаг {step}: {time:F4} мс");

            if (time < minTime)
            {
                optimalStep = step;
                minTime = time;
            }
        }

        
        double[] times = new double[threadCounts.Length];
        int optimalThreads = 1;
        double bestTime = double.MaxValue;

        for (int i = 0; i < threadCounts.Length; i++)
        {
            int threads = threadCounts[i];
            times[i] = MeasureTime(() => MultiThreadIntegral(a, b, optimalStep, threads), 20, 5);
            Console.WriteLine($"{threads} потоков: {times[i]:F4} мс");

            if (times[i] < bestTime)
            {
                bestTime = times[i];
                optimalThreads = threads;
            }
        }

      
        double singleTime = MeasureTime(() => SingleThreadIntegral(a, b, optimalStep), 50, 10);
        double multiTime = MeasureTime(() => MultiThreadIntegral(a, b, optimalStep, optimalThreads), 50, 10);
        double speedup = (singleTime - multiTime) / singleTime * 100;

   
        var plt = new Plot();
        plt.Add.Bars(threadCounts.Select(x => (double)x).ToArray(), times);
        plt.XLabel("Количество потоков");
        plt.YLabel("Время выполнения (мс)");
        plt.Title("Зависимость времени от числа потоков");
        plt.SavePng("performance.png", 800, 600);

        string report = $@"
Результаты оптимизации для ∫sin(x)dx на [{-100}, {100}]

1. Оптимальные параметры:
   - Шаг интегрирования: {optimalStep}
   - Оптимальное число потоков: {optimalThreads}

2. Производительность:
   - Однопоточное время: {singleTime:F4} мс
   - Многопоточное время: {multiTime:F4} мс
   - Ускорение: {speedup:F2}%

3. Вывод:
   Многопоточная версия {(speedup >= 15 ? "быстрее" : "медленнее")} однопоточной на {Math.Abs(speedup):F2}%
   {(speedup >= 15 ? "Соответствует критериям" : "Требует оптимизации")}";

        File.WriteAllText("report.txt", report);
        Console.WriteLine(report);
    }
}
