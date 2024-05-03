using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VectorDrawing.Classes
{

	static public class Utils
	{
		static public void AddLineToCanvas(Canvas canvas, Point p1, Point p2, Brush color, double thickness, double opacity)
		{
			Line line = new Line();
			line.Opacity = opacity;
			line.Stroke = color;
			line.StrokeThickness = thickness;
			line.X1 = p1.X;
			line.X2 = p2.X;
			line.Y1 = p1.Y;
			line.Y2 = p2.Y;
			canvas.Children.Add(line);
		}

		static public double CalculateOpacity(double coordinate)
		{
			double value = Math.Round(coordinate, 5) % 1.0;
			if (value == 0)
				return 0.5;
			else
				return 0.2;
		}


	}
}
