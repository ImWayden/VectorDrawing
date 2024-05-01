using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input.Manipulations;
using System.Windows.Navigation;

namespace VectorDrawing.Classes
{
	public class Box
	{
		public Point Top_Left_Corner;
		public Point Top_Right_Corner;
		public Point Bottom_Left_Corner;
		public Point Bottom_Right_Corner;

		public Box() { }
		public Box(Point TL, Point TR, Point BL, Point BR)
		{
			Top_Left_Corner = TL;
			Top_Right_Corner = TR;
			Bottom_Left_Corner = BL;
			Bottom_Right_Corner = BR;
		}

		public bool IsInPlan(Point P1)
		{
			if(P1.X < Bottom_Left_Corner.X || P1.X > Top_Right_Corner.X)
				return false;
			else if(P1.Y < Bottom_Left_Corner.Y || P1.Y > Top_Right_Corner.Y)
				return false;
			return true;
		}
		private bool ccw(Point A, Point B, Point C)
		{
			return (C.Y - A.Y) * (B.X - A.X) > (B.Y - A.Y) * (C.X - A.X);
		}



		/* https://bryceboe.com/2006/10/23/line-segment-intersection-algorithm */
		public bool line_intersect(Point A, Point B, Point C, Point D)
		{
			return ccw(A, C, D) != ccw(B, C, D) && ccw(A, B, C) != ccw(A, B, D);
		}


		public bool box_lines_intersect(Box T2)
		{
			if(line_intersect(Top_Left_Corner, Bottom_Right_Corner, T2.Top_Right_Corner, T2.Bottom_Left_Corner) || line_intersect(Top_Left_Corner, Bottom_Right_Corner, T2.Top_Left_Corner, T2.Bottom_Right_Corner))
				return true;
			if (line_intersect(Top_Right_Corner, Bottom_Left_Corner, T2.Top_Right_Corner, T2.Bottom_Left_Corner) || line_intersect(Top_Right_Corner, Bottom_Left_Corner, T2.Top_Left_Corner, T2.Bottom_Right_Corner))
				return true;
			return false;
		}

		public bool IsIntersecting(Box T2)
		{
			if(IsInPlan(T2.Top_Left_Corner) || IsInPlan(T2.Top_Right_Corner) || IsInPlan(T2.Bottom_Left_Corner) || IsInPlan(T2.Bottom_Right_Corner))
				return true;
			else if(T2.IsInPlan(Top_Left_Corner) || T2.IsInPlan(Top_Right_Corner) || T2.IsInPlan(Bottom_Left_Corner) || T2.IsInPlan(Bottom_Right_Corner))
				return true;
			else if(box_lines_intersect(T2) || T2.box_lines_intersect(this))
				return true;
			return false;
		}
	}
}
