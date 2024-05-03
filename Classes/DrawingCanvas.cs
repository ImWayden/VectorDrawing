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
		public Canvas canvas;
		private Classes.Camera camera;
		public Nodes_Layers ActiveLayer;
		public SceneTree Scene;

		public DrawingCanvas(Canvas canvas, Camera camera)
		{
			this.canvas = canvas;
			this.camera = camera;
			Scene = new SceneTree();
			ActiveLayer = Scene.Layers[0];
		}

		public void Draw_Scene()
		{

			canvas.Children.Clear();

			foreach (Nodes Layer in Scene.Layers)
			{
				if(camera.box.IsIntersecting(Layer.box))
					recursive_draw(Layer);
			}
		}

		public void Draw_Line(Nodes n)
		{
			Nodes_Lines line = n as Nodes_Lines;
			Point P1 = camera.CamToPlan(line.P1, new Plan2D(canvas.ActualWidth, canvas.ActualHeight));
			Point P2 = camera.CamToPlan(line.P2, new Plan2D(canvas.ActualWidth, canvas.ActualHeight));
			Utils.AddLineToCanvas(canvas, P1, P2, Brushes.White, 1.0, 1);
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
	}
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
