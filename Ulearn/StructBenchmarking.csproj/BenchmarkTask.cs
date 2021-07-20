using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.
            task.Run();
            Stopwatch stopwatch = new Stopwatch();
            double result = 0;
            for (int i = 0; i < repetitionCount; i++)
            {
                stopwatch.Restart();
                stopwatch.Start();
                task.Run();
                stopwatch.Stop();
                result += stopwatch.ElapsedMilliseconds;
            }

            return result / repetitionCount;
        }

    }
    public class BulderTest : ITask
    {
        public void Run()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                stringBuilder.Append('a');
            }
        }
    }

    public class StringTest : ITask
    {
        public void Run()
        {
            string test = new string('a', 1000);
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            BulderTest bulderTest = new BulderTest();
            StringTest stringTest = new StringTest();
            Benchmark benchmark = new Benchmark();

            var secondTest = benchmark.MeasureDurationInMs(bulderTest, 10000);
            var firstTest = benchmark.MeasureDurationInMs(stringTest, 10000);
            Assert.Less(firstTest, secondTest);
        }
    }
}