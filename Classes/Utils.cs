using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VectorDrawing.Classes
{

	static internal class Utils
	{
		static public bool IsInPlan(Point point, Point PositionMin, Point PositionMax)
		{
			if (Math.Max(PositionMin.X, Math.Min(point.X, PositionMax.X)) == point.X && Math.Max(PositionMin.Y, Math.Min(point.Y, PositionMax.Y)) == point.Y)
				return true;
			return false;
		}
		static public bool Rectangle_Overlap(Point R1Min, Point R1Max, Point R2Min, Point R2Nax)
		{

			return false;
		}
	}
}
