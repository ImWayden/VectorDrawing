using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDrawing.Classes
{
	public class Plan2D
	{
		public double Width { get; set; }
		public double Height { get; set; }

		public Plan2D(double width, double height)
		{
			Width = width;
			Height = height;
		}
	}
}
