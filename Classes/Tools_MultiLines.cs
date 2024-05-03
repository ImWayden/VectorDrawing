﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace VectorDrawing.Classes
{
	/*
	 * lot of problem with that it doubles the lines for some reasons and gets reseted when moving the cam
	 * i'll keep that for when i have the time to modifyu it
	 */
	internal class Tools_MultiLine : Tools
	{
		private double thickness;
		private Point startPoint;
		private Nodes_Lines line;
		private Nodes_Layers tmp_Layer;
		private Nodes_Layers Active_Layer;
		private Point P1;
		private Point P2;
		private int actual_Index;
		private List<Action_AddObject> actions;
		public Tools_MultiLine()
		{
			tmp_Layer = new Nodes_Layers();
			actions = new List<Action_AddObject>();
			actual_Index = -1;
		}

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
			P1 = Camera.PlanToCam(Position, new Plan2D(DrawingCanvas.canvas.ActualWidth, DrawingCanvas.canvas.ActualHeight));
			P1.X = Math.Round(P1.X, Camera.deepness - 1);
			P1.Y = Math.Round(P1.Y, Camera.deepness - 1);
			P2 = P1;
			line = new Nodes_Lines(P1, P2);
			tmp_Layer.Add_Object(line);
			actual_Index++;
			actions.Add(new Action_AddObject(line, DrawingCanvas.ActiveLayer, new DUpdate_Canvas(ConteneurCanvas.Update_Canvas)));
		}

		private void Send_Actions()
		{
			foreach (Action_AddObject action in actions)
				UndoManager.GetInstance().Add_Action(action);
			actions.Clear();
		}

		public override void OnLeftMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			Active_Layer = DrawingCanvas.ActiveLayer as Nodes_Layers;
			DrawingCanvas.ActiveLayer.Add_Object(tmp_Layer);
			Canvas.CaptureMouse();
			Create_Line(e.GetPosition(Canvas));
			IsLeftMouseBouttonDown = true;
		}

		public override void OnLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			Canvas.ReleaseMouseCapture();
			Active_Layer.Remove_Object(tmp_Layer);
			Active_Layer.Merge_Layer(tmp_Layer as Nodes_Layers);
			Active_Layer.Compare_Corners(tmp_Layer);
			tmp_Layer.Childs.Clear();
			actual_Index = -1;
			Send_Actions();
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
				Point point = Camera.PlanToCam(e.GetPosition(Canvas), new Plan2D(DrawingCanvas.canvas.ActualWidth, DrawingCanvas.canvas.ActualHeight));
				Nodes_Lines tmp_Line;
				tmp_Line = null;
				point.X = Math.Round(point.X, Camera.deepness - 1);
				point.Y = Math.Round(point.Y, Camera.deepness - 1);
				line.P2 = point;
				if (point != P1)
				{
					DrawingCanvas.Draw_Line(line);
					Create_Line(e.GetPosition(Canvas));
				}
			}
		}
	}
}
