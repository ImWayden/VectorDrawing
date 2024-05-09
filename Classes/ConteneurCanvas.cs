using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
/*
namespace VectorDrawing.Classes
{
	
	internal class ConteneurCanvas
	{
		DrawingCanvas drawingCanvas;
		GridCanvas gridcanvas;
		Grid conteneur;
		Camera camera;
		WriteableBitmap WriteableBitmap;
		private Tools ActiveTool;
		private Point startPoint;
		private Point Drawing_startPoint;
		private bool isloaded = false;
		private bool isRightMouseButtonDown = false;
		private bool isLeftMouseButtonDown = false;
		private SolidColorBrush drawingBrush = new SolidColorBrush(Colors.Black);
		private double drawingThickness = 2.0;
		public ConteneurCanvas(Canvas drawing, Canvas grid, Grid conteneur)
		{
			camera = new Camera();
			drawingCanvas = new DrawingCanvas(drawing, camera);
			gridcanvas = new GridCanvas(grid, camera);
			this.conteneur = conteneur;
			InitializeEvents();
			InitializeCamera();
			Initialize_Tool();
		}
		private void InitializeEvents()
		{
			conteneur.SizeChanged += Canvas_SizeChanged;
			conteneur.Loaded += Canvas_Loaded;
			conteneur.MouseMove += Canvas_MouseMove;
			conteneur.MouseWheel += Canvas_MouseWheel;
			conteneur.MouseRightButtonDown += Canvas_MouseRightButtonDown;
			conteneur.MouseRightButtonUp += Canvas_MouseRightButtonUp;
			conteneur.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
			conteneur.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
		}

		private void InitializeCamera()
		{
			camera.Update(conteneur.ActualHeight, conteneur.ActualWidth);
			Update_Canvas();
		}

		private void Initialize_Tool()
		{
			ActiveTool = new Tools_Line();
			ActiveTool.Camera = camera;
			ActiveTool.Canvas = drawingCanvas.canvas;
			ActiveTool.DrawingCanvas = drawingCanvas;
			ActiveTool.ConteneurCanvas = this;
		}

		private void Canvas_Loaded(object sender, RoutedEventArgs e)
		{
			//apparently never called
			isloaded = true;
		}
		private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			Debug.WriteLine("MouseWheel");
			Zoom_Manager(e.Delta, e.GetPosition(conteneur));
		}

		private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.RightButton == MouseButtonState.Pressed)
			{
				ActiveTool.OnRightMouseButtonDown(sender, e);
			}
		}
		private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				ActiveTool.OnLeftMouseButtonDown(sender, e);
			}
		}
		private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ActiveTool.OnLeftMouseButtonUp(sender, e);
			isLeftMouseButtonDown = false; // Réinitialiser le drapeau lorsque le bouton droit de la souris est relâché
		}
		private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			ActiveTool.OnRightMouseButtonUp(sender, e);
		}

		private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
		{

			camera.Update(conteneur.ActualHeight, conteneur.ActualWidth);
			Update_Canvas();
		}

		private void Canvas_MouseMove(object sender, MouseEventArgs e)
		{
			ActiveTool.OnMouseMove(sender, e);
		}

		public void Swap_ActiveTool(object sender, Tools tool)
		{
			Debug.WriteLine("omg dog");
			ActiveTool = tool;
			tool.Canvas = drawingCanvas.canvas;
			tool.Camera = camera;
			tool.DrawingCanvas = drawingCanvas;
			tool.ConteneurCanvas = this;
		}

		//porbably should be partially or totally moved into the camera class
		private void Zoom_Manager(int delta, Point MousePos)
		{
			Point InPlanMousePos = camera.PlanToCam(MousePos, new Plan2D(drawingCanvas.canvas.ActualWidth, drawingCanvas.canvas.ActualHeight));
			Vector CameraMov;

			if (delta > 0)
			{
				camera.Scale *= 1 + camera.ZoomFactor;
				if (camera.Scale >= 99999)
					camera.Scale = 99999;
			}
			else if (delta < 0)
			{
				camera.Scale /= 1 + camera.ZoomFactor;
				if (camera.Scale <= 5)
					camera.Scale = 5;
			}
			camera.Update(drawingCanvas.canvas.ActualHeight, drawingCanvas.canvas.ActualWidth);
			Point NewInPlanMousePos = camera.PlanToCam(MousePos, new Plan2D(drawingCanvas.canvas.ActualWidth, drawingCanvas.canvas.ActualHeight));
			CameraMov = InPlanMousePos - NewInPlanMousePos;
			camera.Position += CameraMov;
			camera.deepness = camera.SetDeepness();
			camera.scale_factor = Math.Pow(10, camera.deepness - 2);
			camera.step = 1 / (camera.scale_factor * 10);
			Update_Canvas();
		}

		public void Update_Canvas()
		{
			gridcanvas.Draw_Grid();
			drawingCanvas.Draw_Scene();
			camera.Update(conteneur.ActualHeight, conteneur.ActualWidth);
		}
	}
}
	*/