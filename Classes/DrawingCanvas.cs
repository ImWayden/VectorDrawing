using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;

namespace VectorDrawing.Classes
{
	internal class DrawingCanvas
	{
		Canvas canvas;
		private Classes.Camera camera;
		private Point startPoint;
		private Point Drawing_startPoint;
		private bool isloaded = false;
		private bool isRightMouseButtonDown = false;
		private bool isLeftMouseButtonDown = false;
		private SolidColorBrush drawingBrush = new SolidColorBrush(Colors.Black);
		private double drawingThickness = 2.0;
		private Tools ActiveTool;
		public Nodes_Layers ActiveLayer;
		public SceneTree Scene;
		public DrawingCanvas(Canvas canvas) 
		{
			this.canvas = canvas;
			this.canvas.SizeChanged += Canvas_SizeChanged;
			this.canvas.Loaded += Canvas_Loaded;
			this.canvas.MouseMove += Canvas_MouseMove;
			this.canvas.MouseWheel += Canvas_MouseWheel;
			this.canvas.MouseRightButtonDown += Canvas_MouseRightButtonDown;
			this.canvas.MouseRightButtonUp += Canvas_MouseRightButtonUp;
			this.canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
			this.canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
			Scene = new SceneTree();
			InitializeCamera();
			ActiveTool = new Tools_Line();
			ActiveTool.Camera = camera;
			ActiveTool.Canvas = canvas;
			ActiveTool.DrawingCanvas = this;
			ActiveLayer = Scene.Layers[0];
		}
		private void AddLineToCanvas(Canvas canvas, Point p1, Point p2, Brush color, double thickness, double opacity)
		{
			Line line = new Line();
			line.Opacity = opacity;
			line.Stroke = color;
			line.StrokeThickness = 1;
			line.X1 = p1.X;
			line.X2 = p2.X;
			line.Y1 = p1.Y;
			line.Y2 = p2.Y;
			canvas.Children.Add(line);
		}

		public void Swap_ActiveTool(object sender, Tools tool)
		{
			Debug.WriteLine("omg dog");
			ActiveTool = tool;
			tool.Canvas = canvas;
			tool.Camera = camera;
			tool.DrawingCanvas = this;
		}
		private void InitializeCamera()
		{
			camera = new Classes.Camera();
			camera.Update(canvas.ActualHeight, canvas.ActualWidth);
			Update_Canvas();
		}

		private void Canvas_Loaded(object sender, RoutedEventArgs e)
		{
			//apparently never called
			isloaded = true;
		}
		private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			Debug.WriteLine("MouseWheel");
			Zoom_Manager(e.Delta, e.GetPosition(canvas));
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
		
			camera.Update(canvas.ActualHeight, canvas.ActualWidth);
			Update_Canvas();
		}

		private void Canvas_MouseMove(object sender, MouseEventArgs e)
		{
			ActiveTool.OnMouseMove(sender, e);
			
			/*			if (isLeftMouseButtonDown && canvas.IsMouseCaptured)
						{
							Point CurrentPoint = e.GetPosition(canvas);
							Drawing_startPoint = CurrentPoint;
							Draw_Grid();
						}*/
		}
		//could be moved out for more general usage
		public Point ScreenToPlan(Point ScreenPos, Canvas canvas, Camera camera)
		{
			double relativeX = ScreenPos.X / canvas.ActualWidth;
			double relativeY = ScreenPos.Y / canvas.ActualHeight;

			double planX = camera.Position.X + (relativeX - 0.5) * camera.Width;
			double planY = camera.Position.Y + (relativeY - 0.5) * camera.Height;
			return new Point(planX, planY);
		}

		public Point PlanToScreen(Point planPoint)
		{

			double screenWidth = canvas.ActualWidth; // Largeur de l'écran
			double screenHeight = canvas.ActualHeight; // Hauteur de l'écran

			// Calculer la position du point sur l'écran en fonction de la position et du zoom de la caméra
			double screenX = (planPoint.X - camera.Position.X) * camera.Scale + screenWidth / 2;
			double screenY = (planPoint.Y - camera.Position.Y) * camera.Scale + screenHeight / 2;

			// Retourner les coordonnées du point converties en coordonnées de l'écran
			return new Point(screenX, screenY);
		}

		private void Zoom_Manager(int delta, Point MousePos)
		{
			Point InPlanMousePos = ScreenToPlan(MousePos, canvas, camera);	
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
			// Ajuster la position de la caméra pour compenser le déplacement résultant
			camera.Update(canvas.ActualHeight, canvas.ActualWidth);
			Point NewInPlanMousePos = ScreenToPlan(MousePos, canvas, camera);
			CameraMov = InPlanMousePos - NewInPlanMousePos;
			camera.Position += CameraMov;
			camera.deepness = camera.SetDeepness();
			camera.scale_factor = Math.Pow(10, camera.deepness - 2);
			camera.step = 1 / (camera.scale_factor * 10);
			Update_Canvas();
		}

		private double CalculateOpacity(double coordinate)
		{
			double value = Math.Round(coordinate, 5) % 1.0;
			if (value == 0)
				return 0.5;
			else
				return 0.2;
		}
		/*		public void Draw_Grid()
				{
					if (camera == null)
						throw new NullReferenceException("Camera Null");
					canvas.Children.Clear();
					double visible_width = camera.right - camera.left;
					double visible_height = camera.top - camera.bottom;
					double relative_x;
					double relative_y;
					double pixel_x;
					double pixel_y;
					int deepness = SetDeepness();
					double scale_factor = Math.Pow(10, deepness - 2);
					double step = 1 / (scale_factor * 10);
					//double deepness = Math.Abs(Math.Floor(Math.Log10(camera.Scale)));
					double opacity = 1.0;
					Brush color;
					for (double x = Math.Floor(camera.left); x <= camera.right; x += step)
					{
						relative_x = x - camera.left;
						pixel_x = (relative_x / visible_width) * canvas.ActualWidth;
						if (pixel_x < 0 || pixel_x > canvas.ActualWidth)
							continue;
						if (Math.Round(x, deepness) == 0)
							color = Brushes.Red;
						else
							color = Brushes.Gray;
						opacity = CalculateOpacity(x * scale_factor);
						AddLineToCanvas(canvas, new Point(pixel_x, 0), new Point(pixel_x, canvas.ActualHeight), color, 1.0, opacity);
					}
					for (double y = Math.Floor(camera.bottom); y <= camera.top; y += step)
					{
						relative_y = y - camera.bottom;
						pixel_y = (relative_y / visible_height) * canvas.ActualHeight;
						if (pixel_y < 0 || pixel_y > canvas.ActualHeight)
							continue;
						if (Math.Round(y, deepness) == 0)
							color = Brushes.Red;
						else
							color = Brushes.Gray;
						opacity = CalculateOpacity(y * scale_factor);
						AddLineToCanvas(canvas, new Point(0, pixel_y), new Point(canvas.ActualWidth, pixel_y), color, 1.0, opacity);
					}
				}
		*/
		public void Draw_Grid()
		{
			if (camera == null)
				throw new NullReferenceException("Camera Null");
			canvas.Children.Clear();

			double opacity;
			Brush color;
			for (double x = Math.Floor(camera.left); x <= camera.right; x += camera.step)
			{
				Point screenStartPoint = PlanToScreen(new Point(x, camera.top));
				Point screenEndPoint = PlanToScreen(new Point(x, camera.bottom));
				opacity = CalculateOpacity(x * camera.scale_factor);
				if (screenStartPoint.X < 0 || screenEndPoint.X > canvas.ActualWidth)
					continue;
				if (Math.Round(x, camera.deepness) == 0)
					color = Brushes.Red;
				else
					color = Brushes.Gray;
				AddLineToCanvas(canvas, screenStartPoint, screenEndPoint, color, 1.0,opacity);
			}
			for (double y = Math.Floor(camera.bottom); y <= camera.top; y += camera.step)
			{
				Point screenStartPoint = PlanToScreen(new Point(camera.left, y));
				Point screenEndPoint = PlanToScreen(new Point(camera.right, y));
				
				if (screenStartPoint.Y < 0 || screenEndPoint.Y > canvas.ActualHeight)
					continue;
				if (Math.Round(y, camera.deepness) == 0)
					color = Brushes.Red;
				else
					color = Brushes.Gray;
				opacity = CalculateOpacity(y * camera.scale_factor);
				AddLineToCanvas(canvas, screenStartPoint, screenEndPoint, color, 1.0, opacity);
			}
		}

		public void Draw_Scene()
		{

			foreach (Nodes Layer in Scene.Layers)
			{
				if(camera.box.IsIntersecting(Layer.box))
					recursive_draw(Layer);
			}
		}

		public void Draw_Line(Nodes n)
		{
			Nodes_Lines line = n as Nodes_Lines;
			Point P1 = PlanToScreen(line.P1);
			Point P2 = PlanToScreen(line.P2);
			AddLineToCanvas(canvas, P1, P2, Brushes.White, 2.0, 1);
		}

		public void recursive_draw(Nodes Layer)
		{
			foreach(Nodes n in Layer.Childs)
			{
				if(camera.box.IsIntersecting(n.box))
				{
					if (n is Nodes_Lines)
						Draw_Line(n);
					else if (n is Nodes_Layers)
						recursive_draw(n);
				}
			}
		}

		public void Update_Canvas()		{
			Draw_Grid();
			Draw_Scene();
			camera.Update(canvas.ActualHeight, canvas.ActualWidth);
		}
	}
}
