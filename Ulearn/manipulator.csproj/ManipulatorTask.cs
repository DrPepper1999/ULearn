using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            // Используйте поля Forearm, UpperArm, Palm класса Manipulator
            //x - (cos(alpha) * palm)
            //y + (sin(alpha)*palm)
            var wristX = x - (Math.Cos(alpha) * Manipulator.Palm);
            var wristY = y + (Math.Sin(alpha) * Manipulator.Palm);
            var lineShoulderWrist = Math.Sqrt(wristX*wristX + wristY*wristY);
            var sholder = TriangleTask.GetABAngle(lineShoulderWrist, Manipulator.UpperArm, Manipulator.Forearm) + Math.Atan2(wristY, wristX);
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm,Manipulator.Forearm,lineShoulderWrist);
            var wrist = -alpha - sholder - elbow;
            var posManipulator = AnglesToCoordinatesTask.GetJointPositions(sholder, elbow, wrist);

            if (posManipulator[2].X == x && posManipulator[2].Y == y)
            {
                return new[] { sholder, elbow, wrist };
            }
            else
            {
                return new[] { double.NaN, double.NaN, double.NaN };
            }
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            Random random = new Random();
            
        }
    }
}