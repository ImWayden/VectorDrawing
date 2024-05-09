using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using VectorDrawing.Classes;


namespace VectorDrawing.Model
{
	internal class GridBitmapManager : BitmapManagerBase
	{
		public GridBitmapManager(WriteableBitmap GridBitmap, Camera camera)
		{
			this.Bitmap = GridBitmap;
			this.camera = camera;
			
			pen = new Pen(Brushes.Gray, 1.0);
		}
		public void Draw_Grid()
		{
			if (camera == null)
				throw new NullReferenceException("Camera Null");
			drawingVisual = new DrawingVisual();
			drawingContext = drawingVisual.RenderOpen();
			Bitmap.Clear(Colors.Transparent);
			double opacity;
			Color color;
			for (double x = Math.Floor(camera.left); x <= Math.Ceiling(camera.right); x += camera.step)
			{
				Point screenStartPoint = camera.CamToPlan(new Point(x, camera.top), new Plan2D(Bitmap.Width, Bitmap.Height));
				Point screenEndPoint = camera.CamToPlan(new Point(x, camera.bottom), new Plan2D(Bitmap.Width, Bitmap.Height));
				opacity = Utils.CalculateOpacity(x * camera.scale_factor);
				if (screenStartPoint.X < 0 || screenEndPoint.X > Bitmap.Width)
					continue;
				if (Math.Round(x, camera.deepness) == 0)
					color = Colors.Red;
				else
					color = Colors.Gray;
				//drawingContext.DrawLine(pen, screenStartPoint, screenEndPoint);
				Bitmap.DrawLineAa((int)screenStartPoint.X, (int)screenStartPoint.Y, (int)screenEndPoint.X, (int)screenEndPoint.Y, Utils.AdjustOpacity(color, opacity), 1);
			}
			for (double y = Math.Floor(camera.bottom); y <= Math.Ceiling(camera.top); y += camera.step)
			{
				Point screenStartPoint = camera.CamToPlan(new Point(camera.left, y), new Plan2D(Bitmap.Width, Bitmap.Height));
				Point screenEndPoint = camera.CamToPlan(new Point(camera.right, y), new Plan2D(Bitmap.Width, Bitmap.Height));

				if (screenStartPoint.Y < 0 || screenEndPoint.Y > Bitmap.Height)
					continue;
				if (Math.Round(y, camera.deepness) == 0)
					color = Colors.Red;
				else
					color = Colors.Gray;
				opacity = Utils.CalculateOpacity(y * camera.scale_factor);
				Bitmap.DrawLineAa((int)screenStartPoint.X, (int)screenStartPoint.Y, (int)screenEndPoint.X, (int)screenEndPoint.Y,Utils.AdjustOpacity(color, opacity), 1);
			}
			//drawingContext.Close();
		}
		//could do some multithreading somewhere
		public override void Update()
		{
			Bitmap.Lock();
			Draw_Grid();
			Bitmap.Unlock();
		}
	}
}
