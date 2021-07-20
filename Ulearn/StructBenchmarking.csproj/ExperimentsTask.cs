using System.Collections.Generic;

namespace StructBenchmarking
{ 
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount) // �������_1
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            for (int i = 16; i <= 512; i = i*2)
            {
                var classArr = new ClassArrayCreationTask(i);// �������_1
                classesTimes.Add(new ExperimentResult(i, benchmark.MeasureDurationInMs(classArr,
                    repetitionsCount)));

                var structArr = new StructArrayCreationTask(i);// �������_2
                structuresTimes.Add(new ExperimentResult(i, benchmark.MeasureDurationInMs(structArr,
                    repetitionsCount)));
            }
            //...

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)// �������_2
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            //...
            for (int i = 16; i <= 512; i *= 2)
            {
                var classArr = new MethodCallWithClassArgumentTask(i);// �������_1
                classesTimes.Add(new ExperimentResult(i, benchmark.MeasureDurationInMs(classArr,
                    repetitionsCount)));

                var structArr = new MethodCallWithStructArgumentTask(i);// �������_2
                structuresTimes.Add(new ExperimentResult(i, benchmark.MeasureDurationInMs(structArr,
                    repetitionsCount)));
            }

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

    }
}