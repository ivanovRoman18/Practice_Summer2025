using ScottPlot;

var plt = new Plot();

// Данные для графика (поменяли местами оси)
double[] executionTimes = { 45.2, 23.8, 12.1, 7.9, 8.7, 9.2 }; // OX
double[] threadCounts = { 1, 2, 4, 8, 12, 16 };               // OY

// Добавляем данные (теперь executionTimes по OX, threadCounts по OY)
var scatter = plt.Add.Scatter(executionTimes, threadCounts);
scatter.Label = "Время выполнения";

// Подписи осей (поменяли местами)
plt.XLabel("Время выполнения (мс)");
plt.YLabel("Количество потоков");
plt.Title("Производительность многопоточного вычисления");

plt.ShowLegend();

// Сохраняем график
plt.SavePng("performance.png", 800, 600);

Console.WriteLine("График успешно сохранён!");
