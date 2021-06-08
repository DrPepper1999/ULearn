using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowPos = new PointF((float)(Manipulator.UpperArm * Math.Cos(shoulder)), (float)(Manipulator.UpperArm * Math.Sin(shoulder)));
            var wristPos = new PointF((float)(elbowPos.X + Manipulator.Forearm*Math.Cos(shoulder+elbow + Math.PI)), (float)(elbowPos.Y + Manipulator.Forearm * Math.Sin(shoulder + elbow + Math.PI)));
            var palmEndPos = new PointF((float)(wristPos.X + Manipulator.Palm*Math.Cos(shoulder+elbow+wrist + Math.PI*2)), (float)(wristPos.Y + Manipulator.Palm * Math.Sin(shoulder + elbow + wrist + Math.PI*2)));
            return new PointF[]
            {    
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            Assert.AreEqual(Manipulator.UpperArm, Math.Sqrt(joints[0].X * joints[0].X + joints[0].Y * joints[0].Y));
            Assert.AreEqual(Manipulator.Forearm, Math.Round(Math.Sqrt(Math.Pow(joints[1].X - joints[0].X, 2) + Math.Pow(joints[1].Y - joints[0].Y, 2))));
            Assert.AreEqual(Manipulator.Palm, Math.Round(Math.Sqrt(Math.Pow(joints[2].X - joints[1].X, 2) + Math.Pow(joints[2].Y - joints[1].Y, 2))));
        }
    }
}