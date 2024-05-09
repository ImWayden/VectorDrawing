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
				return 0.8;
			else
				return 0.6;
		}

		public static Color AdjustOpacity(Color color, double opacityFactor)
		{
			// Assurez-vous que le facteur d'opacité est compris entre 0 et 1
			opacityFactor = Math.Max(0.0, Math.Min(1.0, opacityFactor));

			// Appliquer le facteur d'opacité à la valeur alpha de la couleur
			byte newAlpha = (byte)(color.A * opacityFactor);

			// Appliquer le facteur d'opacité aux composantes RGB de la couleur en tenant compte de la nouvelle valeur alpha
			byte newR = (byte)((color.R / 255.0) * newAlpha);
			byte newG = (byte)((color.G / 255.0) * newAlpha);
			byte newB = (byte)((color.B / 255.0) * newAlpha);

			// Retourner une nouvelle couleur avec les composantes RVB ajustées et la nouvelle valeur alpha
			return Color.FromArgb(color.A, newR, newG, newB);
		}
	}
}
