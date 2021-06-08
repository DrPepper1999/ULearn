using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Manipulation
{
	public static class VisualizerTask
	{
		public static double X = 220;
		public static double Y = -100;
		public static double Alpha = 0.05;
		public static double Wrist = 2 * Math.PI / 3;
		public static double Elbow = 3 * Math.PI / 4;
		public static double Shoulder = Math.PI / 2;

		public static Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
		public static Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
		public static Pen ManipulatorPen = new Pen(Color.Black, 3);
		public static Brush JointBrush = Brushes.Gray;

		public static void KeyDown(Form form, KeyEventArgs key)
		{
			// TODO: Добавьте реакцию на QAWS и пересчитывать Wrist
			form.Invalidate(); // 
            switch (key.KeyCode)
            {
				case Keys.Q:
					Shoulder++;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
				case Keys.A:
					Shoulder--;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
				case Keys.W:
					Elbow++;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
				case Keys.S:
					Elbow--;
					Wrist = -Alpha - Shoulder - Elbow;
					break;
			}
        }


		public static void MouseMove(Form form, MouseEventArgs e)
		{
			// TODO: Измените X и Y пересчитав координаты (e.X, e.Y) в логические.
			UpdateManipulator();
			form.Invalidate();
			PointF logicPointf = ConvertWindowToMath(new PointF(e.X, e.Y), GetShoulderPos(form));
			X = logicPointf.X;
			Y = logicPointf.Y;
		}

		public static void MouseWheel(Form form, MouseEventArgs e)
		{
			// TODO: Измените Alpha, используя e.Delta — размер прокрутки колеса мыши

			UpdateManipulator();
			form.Invalidate();
			Alpha += e.Delta;
		}

		public static void UpdateManipulator()
		{
			// Вызовите ManipulatorTask.MoveManipulatorTo и обновите значения полей Shoulder, Elbow и Wrist, 
			// если они не NaN. Это понадобится для последней задачи.
			
			var newAngle = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);

			if (!(double.IsNaN(newAngle[0]) || double.IsNaN(newAngle[1]) || double.IsNaN(newAngle[2])))
			{
				Shoulder = newAngle[0];
				Elbow = newAngle[1];
				Wrist = newAngle[2];
			}

		}

		public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
		{
			var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

			graphics.DrawString(
                $"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}", 
                new Font(SystemFonts.DefaultFont.FontFamily, 12), 
                Brushes.DarkRed, 
                10, 
                10);
			DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);

			// Нарисуйте сегменты манипулятора методом graphics.DrawLine используя ManipulatorPen.
			// Нарисуйте суставы манипулятора окружностями методом graphics.FillEllipse используя JointBrush.
			// Не забудьте сконвертировать координаты из логических в оконные
		}

		private static void DrawReachableZone(
            Graphics graphics, 
            Brush reachableBrush, 
            Brush unreachableBrush,
            PointF shoulderPos, 
            PointF[] joints)
		{
			var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
			var rmax = Manipulator.UpperArm + Manipulator.Forearm;
			var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
			var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
			var windowZero = ConvertMathToWindow(new PointF(0, 0), shoulderPos);
			var mathCircles	= AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);
			var windowCircle1 = ConvertMathToWindow(mathCircles[0], shoulderPos);
			var windowCircle2 = ConvertMathToWindow(mathCircles[1], shoulderPos);

			graphics.FillEllipse(reachableBrush, windowCenter.X - rmax, windowCenter.Y - rmax, 2 * rmax, 2 * rmax);
			graphics.FillEllipse(unreachableBrush, windowCenter.X - rmin, windowCenter.Y - rmin, 2 * rmin, 2 * rmin);

			graphics.FillEllipse(JointBrush, windowZero.X-15, windowZero.Y-15, 30, 30);
			graphics.DrawLine(ManipulatorPen, windowZero, ConvertMathToWindow(mathCircles[0],shoulderPos));
			graphics.FillEllipse(JointBrush, windowCircle1.X - 15, windowCircle1.Y - 15, 30, 30);
			graphics.DrawLine(ManipulatorPen, windowCircle1, ConvertMathToWindow(mathCircles[1], shoulderPos));
			graphics.FillEllipse(JointBrush, windowCircle2.X - 15, windowCircle2.Y - 15, 30, 30);
			graphics.DrawLine(ManipulatorPen, windowCircle2, ConvertMathToWindow(mathCircles[2], shoulderPos));
		}

		public static PointF GetShoulderPos(Form form)
		{
			return new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);
		}

		public static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos)
		{
			return new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
		}

		public static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos)
		{
			return new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
		}

	}
}