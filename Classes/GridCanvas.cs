using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;

namespace VectorDrawing.Classes
{
	internal class GridCanvas
	{
		Canvas canvas;
		Camera camera;

		public GridCanvas(Canvas canvas, Camera camera)
		{
			this.canvas = canvas;
			this.camera = camera;
		}
		public void Draw_Grid()
		{
			if (camera == null)
				throw new NullReferenceException("Camera Null");
			Application.Current.Dispatcher.Invoke(() =>
			{
				canvas.Children.Clear();
			});

			double opacity;
			Brush color;
			for (double x = Math.Floor(camera.left); x <= camera.right; x += camera.step)
			{
				Point screenStartPoint = camera.CamToPlan(new Point(x, camera.top), new Plan2D(canvas.ActualWidth, canvas.ActualHeight));
				Point screenEndPoint = camera.CamToPlan(new Point(x, camera.bottom), new Plan2D(canvas.ActualWidth, canvas.ActualHeight));
				opacity = Utils.CalculateOpacity(x * camera.scale_factor);
				if (screenStartPoint.X < 0 || screenEndPoint.X > canvas.ActualWidth)
					continue;
				if (Math.Round(x, camera.deepness) == 0)
					color = Brushes.Red;
				else
					color = Brushes.Gray;
				Utils.AddLineToCanvas(canvas, screenStartPoint, screenEndPoint, color, 1.0, opacity);
			}
			for (double y = Math.Floor(camera.bottom); y <= camera.top; y += camera.step)
			{
				Point screenStartPoint = camera.CamToPlan(new Point(camera.left, y), new Plan2D(canvas.ActualWidth, canvas.ActualHeight));
				Point screenEndPoint = camera.CamToPlan(new Point(camera.right, y), new Plan2D(canvas.ActualWidth, canvas.ActualHeight));

				if (screenStartPoint.Y < 0 || screenEndPoint.Y > canvas.ActualHeight)
					continue;
				if (Math.Round(y, camera.deepness) == 0)
					color = Brushes.Red;
				else
					color = Brushes.Gray;
				opacity = Utils.CalculateOpacity(y * camera.scale_factor);
				Utils.AddLineToCanvas(canvas, screenStartPoint, screenEndPoint, color, 1.0, opacity);
			}
		}
	}


}
