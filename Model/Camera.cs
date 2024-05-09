
using System.Windows;
using System.Windows.Media.Media3D;
using VectorDrawing.Classes;

namespace VectorDrawing.Model
{
	//camera placed over a virtual 2Dplan
	// the camera got it's own 2Dplan wich is placed on the virtual plane it's the part of the virtual plan that the camera sees if it makes sens
	public class Camera
	{
		//pos in cam plan
		public System.Windows.Vector Position { get; set; }
		public double Speed { get; set; }
		public double ZoomFactor { get; set; }

		Plan2D Cam_Plane;
		public double Scale { get; set; }

		//colision box of the camera
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
		//could be named ScreenToCam it transform a point from the screen(screen could be any plane tho) into cam coordinates
		public Point PlanToCam(Point ScreenPos, Plan2D Plan)
		{
			double relativeX = ScreenPos.X / Plan.Width;
			double relativeY = ScreenPos.Y / Plan.Height;

			double planX = Position.X + (relativeX - 0.5) * Cam_Plane.Width;
			double planY = Position.Y + (relativeY - 0.5) * Cam_Plane.Height;
			return new Point(planX, planY);
		}
		//transforme a point in the camplane to a point in screenplane 
		public Point CamToPlan(Point cam_plane_Point, Plan2D Plan)
		{

			double screenWidth = Plan.Width;
			double screenHeight = Plan.Height;


			double screenX = (cam_plane_Point.X - Position.X) * Scale + screenWidth / 2;
			double screenY = (cam_plane_Point.Y - Position.Y) * Scale + screenHeight / 2;

			return new Point(screenX, screenY);
		}

		public void Zoom_Manager(int delta, Point MousePos, Plan2D ScreenPlan)
		{
			Point InPlanMousePos = PlanToCam(MousePos, ScreenPlan);
			Vector CameraMov;

			if (delta > 0)
			{
				Scale *= 1 + ZoomFactor;
				if (Scale >= 99999)
					Scale = 99999;
			}
			else if (delta < 0)
			{
				Scale /= 1 + ZoomFactor;
				if (Scale <= 5)
					Scale = 5;
			}
			Update(ScreenPlan.Height, ScreenPlan.Width);
			Point NewInPlanMousePos = PlanToCam(MousePos, ScreenPlan);
			CameraMov = InPlanMousePos - NewInPlanMousePos;
			Position += CameraMov;
			deepness = SetDeepness();
			scale_factor = Math.Pow(10, deepness - 2);
			step = 1 / (scale_factor * 10);
		}
	}
}
