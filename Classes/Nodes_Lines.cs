using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VectorDrawing.Classes
{
	internal class Nodes_Lines : Nodes
	{
		private Point _P1;
		private Point _P2;

		public Point P1 {
			get
			{
				return _P1;
			}
			set {
				_P1 = value;
				Update_Corners();
			}
		}
		public Point P2
		{
			get
			{
				return _P2;
			}
			set
			{
				_P2 = value;
				Update_Corners();
			}
		}

		public Nodes_Lines(Point P1, Point P2) {
			this.P1 = P1;
			this.P2 = P2;
			Update_Corners();
		}
		public override void Determine_Top_Right_Corner()
		{
			box.Top_Right_Corner.X = Math.Max(box.Top_Right_Corner.X, Math.Max(P1.X, P2.X));
			box.Top_Right_Corner.Y = Math.Max(box.Top_Right_Corner.Y, Math.Max(P1.Y, P2.Y));
		}
		public override void Determine_Bottom_Left_Corner()
		{
			box.Bottom_Left_Corner.X = Math.Min(box.Bottom_Left_Corner.X, Math.Min(P1.X, P2.X));
			box.Bottom_Left_Corner.Y = Math.Min(box.Bottom_Left_Corner.Y, Math.Min(P1.Y, P2.Y));
		}
		
	}
}
