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

		static public Point ScreenToPlan(Point ScreenPos, Canvas canvas, Camera camera)
		{
			double relativeX = ScreenPos.X / canvas.ActualWidth;
			double relativeY = ScreenPos.Y / canvas.ActualHeight;

			double planX = camera.Position.X + (relativeX - 0.5) * camera.Width;
			double planY = camera.Position.Y + (relativeY - 0.5) * camera.Height;
			return new Point(planX, planY);
		}

		static public Point PlanToScreen(Point planPoint, Canvas canvas, Camera camera)
		{

			double screenWidth = canvas.ActualWidth; // Largeur de l'écran
			double screenHeight = canvas.ActualHeight; // Hauteur de l'écran

			// Calculer la position du point sur l'écran en fonction de la position et du zoom de la caméra
			double screenX = (planPoint.X - camera.Position.X) * camera.Scale + screenWidth / 2;
			double screenY = (planPoint.Y - camera.Position.Y) * camera.Scale + screenHeight / 2;

			// Retourner les coordonnées du point converties en coordonnées de l'écran
			return new Point(screenX, screenY);
		}
	}
}
