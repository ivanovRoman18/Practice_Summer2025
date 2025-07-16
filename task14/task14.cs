using System;
using System.Threading;
using System.Threading.Tasks;

namespace task14
{
    public class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            if (threadsNumber <= 0 || step <= 0)
                throw new ArgumentException("Threads number and step must be positive.");

            double totalResult = 0.0;
            object lockObj = new object();
            double intervalLength = b - a;
            double subIntervalLength = intervalLength / threadsNumber;

            Parallel.For(0, threadsNumber, i =>
            {
                double start = a + i * subIntervalLength;
                double end = (i == threadsNumber - 1) ? b : start + subIntervalLength;
                double localResult = 0.0;

                for (double x = start; x < end; x += step)
                {
                    double xNext = Math.Min(x + step, end);
                    localResult += (function(x) + function(xNext)) * (xNext - x) / 2.0;
                }

                lock (lockObj)
                {
                    totalResult += localResult;
                }
            });

            return totalResult;
        }
    }
}
