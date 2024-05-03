using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace VectorDrawing.Classes
{
	public class Camera
	{
		//pos in cam plan
		public System.Windows.Vector Position { get; set; }
		public double Speed { get; set; }
		public double ZoomFactor { get; set; }

		Plan2D Cam_Plane;
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
			Cam_Plane = new Plan2D(0,0);
			box = new Box();
			Position = new System.Windows.Vector(0,0);
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
			Cam_Plane.Height = height / Scale;
			Cam_Plane.Width = width / Scale;
			Update_Edges();
		}
		public void Update_Edges()
		{
			left = Position.X - Cam_Plane.Width / 2;
			right = Position.X + Cam_Plane.Width / 2;
			top = Position.Y + Cam_Plane.Height / 2;
			bottom = Position.Y - Cam_Plane.Height / 2;
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
			if (Scale >= 5000)
				return (4);
			else if (Scale >= 500)
				return (3);
			else if (Scale >= 50)
				return (2);
			else
				return (1);
		}

		public Point PlanToCam(Point ScreenPos, Plan2D Plan)
		{
			double relativeX = ScreenPos.X / Plan.Width;
			double relativeY = ScreenPos.Y / Plan.Height;

			double planX = Position.X + (relativeX - 0.5) * Cam_Plane.Width;
			double planY = Position.Y + (relativeY - 0.5) * Cam_Plane.Height;
			return new Point(planX, planY);
		}
		//transforme point to cam coordinate
		public Point CamToPlan(Point cam_plane_Point, Plan2D Plan)
		{

			double screenWidth = Plan.Width;
			double screenHeight = Plan.Height;


			double screenX = (cam_plane_Point.X - Position.X) * Scale + screenWidth / 2;
			double screenY = (cam_plane_Point.Y - Position.Y) * Scale + screenHeight / 2;

			return new Point(screenX, screenY);
		}
	}
}
