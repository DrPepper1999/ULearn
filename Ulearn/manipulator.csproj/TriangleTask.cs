using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if ((a + b <= c) || a < 0 || b < 0 || c < 0)
                return double.NaN;

            return Math.Acos((a*a+b*b-c*c)/(2*a*b));
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, 1.0471975511)]
        [TestCase(3d, 3d, 3d, 1.0471975511)]
        [TestCase(1,1,5,double.NaN)]
        [TestCase(5,1,1,double.NaN)]
        [TestCase(1,5,1,double.NaN)]
        [TestCase(1.0d, 1.0d, 0.0d, 0.0d)]
        // добавьте ещё тестовых случаев!
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var actualAngle = TriangleTask.GetABAngle(a, b, c);
            Assert.AreEqual(expectedAngle, actualAngle);
        }
    }
}