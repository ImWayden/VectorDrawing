using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace VectorDrawing.Classes
{
	internal class Tools_Line : Tools
	{
		private ConteneurCanvas conteneur;
		private double thickness;
		private Point startPoint;
		private Nodes_Lines line;
		private Nodes_Layers Active_Layer;
		private Point P1;
		private Point P2;
		private int actual_Index;
		private Action_AddObject action;
		public Tools_Line() {}

		public override void OnRightMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			Debug.WriteLine("RightClick");
			startPoint = e.GetPosition(Canvas);
			Canvas.CaptureMouse();
			IsRightMouseBouttonDown = true;
		}

		public override void OnRightMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			Canvas.ReleaseMouseCapture();
			IsRightMouseBouttonDown = false;
		}

		//potentiels probleme de decallage dans le calque
		private void Create_Line(Point Position)
		{
			P1 = Utils.ScreenToPlan(Position, Canvas, Camera);
			P1.X = Math.Round(P1.X, Camera.deepness - 1);
			P1.Y = Math.Round(P1.Y, Camera.deepness - 1);
			P2 = P1;
			line = new Nodes_Lines(P1, P2);
			Active_Layer.Add_Object(line);
			action = new Action_AddObject(line, DrawingCanvas.ActiveLayer);
		}

		public override void OnLeftMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			Active_Layer = DrawingCanvas.ActiveLayer as Nodes_Layers;
			Canvas.CaptureMouse();
			Create_Line(e.GetPosition(Canvas));
			IsLeftMouseBouttonDown = true;
		}

		public override void OnLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			Canvas.ReleaseMouseCapture();
			Active_Layer.Compare_Corners(line);
			UndoManager.GetInstance().Add_Action(action);
			IsLeftMouseBouttonDown = false;
		}
		public override void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (IsRightMouseBouttonDown && Canvas.IsMouseCaptured)
			{
				Point endPoint = e.GetPosition(Canvas);
				Vector delta = new Vector(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
				Camera.Position -= delta / Camera.Scale;
				startPoint = endPoint;
				ConteneurCanvas.Update_Canvas();
			}
			if (IsLeftMouseBouttonDown && Canvas.IsMouseCaptured)
			{
				Point point = Utils.ScreenToPlan(e.GetPosition(Canvas), Canvas, Camera);
				point.X = Math.Round(point.X, Camera.deepness - 1);
				point.Y = Math.Round(point.Y, Camera.deepness - 1);
				line.P2 = point;
				DrawingCanvas.Draw_Line(line);
			}
		}
	}
}
