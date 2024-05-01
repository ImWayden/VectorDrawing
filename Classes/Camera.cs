using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace VectorDrawing.Classes
{
	public class Camera
	{
		public System.Windows.Vector Position { get; set; }
		public double Speed { get; set; }
		public double ZoomFactor { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public double Scale { get; set; }

		public Box box;

		public int deepness;
		public double scale_factor;
		public double step;

		public double left;
		public double right;
		public double top;
		public double bottom;

		public Camera()
		{
			box = new Box();
			Position = new Vector(0,0);
			Speed = 0;
			ZoomFactor = 0.1;
			Scale = 15;
			deepness = SetDeepness();
			scale_factor = Math.Pow(10, deepness - 2);
			step = 1 / (scale_factor * 10);
		}

		public void Move(System.Windows.Vector delta)
		{
			Position += delta;
			Update_Edges();
		}
		public void Update(double height,  double width)
		{
			Height = height / Scale;
			Width = width / Scale;
			Update_Edges();
		}
		public void Update_Edges()
		{
			left = Position.X - Width / 2;
			right = Position.X + Width / 2;
			top = Position.Y + Height / 2;
			bottom = Position.Y - Height / 2;
			Update_Positions();
		}

		public void Update_Positions()
		{
			box.Top_Right_Corner = new Point(right, top);
			box.Bottom_Left_Corner = new Point(left, bottom);
			box.Bottom_Right_Corner = new Point(right, bottom);
			box.Top_Left_Corner = new Point(left, top);
		}

		public int SetDeepness()
		{
			if (Scale >= 10000)
				return (4);
			else if (Scale >= 1000)
				return (3);
			else if (Scale >= 100)
				return (2);
			else
				return (1);
		}
	}
}
