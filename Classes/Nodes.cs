using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VectorDrawing.Classes
{
	public abstract class Nodes
	{
		public Box box;

		public List<Nodes> Childs;
		public Nodes() {
			Childs = [];
			box = new Box();
			box.Top_Right_Corner = new Point(double.NegativeInfinity, double.NegativeInfinity);
			box.Bottom_Left_Corner = new Point(double.PositiveInfinity, double.PositiveInfinity);
			box.Top_Left_Corner = new Point(double.PositiveInfinity, double.NegativeInfinity);
			box.Bottom_Right_Corner = new Point(double.NegativeInfinity, double.PositiveInfinity);
		}

		public virtual void Determine_Top_Right_Corner() {
			foreach(Nodes n in Childs)
			{
				box.Top_Right_Corner.X = Math.Max(box.Top_Right_Corner.X, n.box.Top_Right_Corner.X);
				box.Top_Right_Corner.Y = Math.Max(box.Top_Right_Corner.Y, n.box.Top_Right_Corner.Y);
			}
		}
		public virtual void Determine_Bottom_Left_Corner() { 
			foreach (Nodes n in Childs)
			{
				box.Bottom_Left_Corner.X = Math.Min(box.Bottom_Left_Corner.X, n.box.Bottom_Left_Corner.X);
				box.Bottom_Left_Corner.Y = Math.Min(box.Bottom_Left_Corner.Y, n.box.Bottom_Left_Corner.Y);
			}
		}
		public virtual void Determine_Top_Left_Corner()
		{
			box.Top_Left_Corner.X = box.Bottom_Left_Corner.X;
			box.Top_Left_Corner.Y = box.Top_Right_Corner.Y;
		}
		public virtual void Determine_Bottom_Right_Corner()
		{
			box.Bottom_Right_Corner.X = box.Top_Right_Corner.X; 
			box.Bottom_Right_Corner.Y = box.Bottom_Left_Corner.Y;
		}

		public void Compare_Corners(Nodes n)
		{
			if (Childs.Count == 0)
			{
				box.Top_Right_Corner = n.box.Top_Right_Corner;
				box.Bottom_Left_Corner = n.box.Bottom_Left_Corner;
			}
			else
			{
				box.Top_Right_Corner.X = Math.Max(box.Top_Right_Corner.X, n.box.Top_Right_Corner.X);
				box.Top_Right_Corner.Y = Math.Max(box.Top_Right_Corner.Y, n.box.Top_Right_Corner.Y);
				box.Bottom_Left_Corner.X = Math.Min(box.Bottom_Left_Corner.X, n.box.Bottom_Left_Corner.X);
				box.Bottom_Left_Corner.Y = Math.Min(box.Bottom_Left_Corner.Y, n.box.Bottom_Left_Corner.Y);
			}
			Determine_Top_Left_Corner();
			Determine_Bottom_Right_Corner();
		}

		public void Add_Object(Nodes n) {
			Compare_Corners(n);
			Childs.Add(n);
		}

		public void Add_Object_At(Nodes n, int index)
		{
			Compare_Corners(n);
			Childs.Insert(index,n);
		}
		public void Remove_Object(Nodes n) {
			Childs.Remove(n);
			if (n.box.Top_Right_Corner.X == box.Top_Right_Corner.X || n.box.Top_Right_Corner.Y == box.Top_Right_Corner.Y)
				Determine_Top_Right_Corner();
			if (n.box.Bottom_Left_Corner.X == box.Bottom_Left_Corner.X || n.box.Bottom_Left_Corner.Y == box.Bottom_Left_Corner.Y)
				Determine_Bottom_Left_Corner();
			Determine_Top_Left_Corner();
			Determine_Bottom_Right_Corner();
		}

		public void Update_Corners()
		{
			Determine_Top_Right_Corner();
			Determine_Bottom_Left_Corner();
			Determine_Bottom_Right_Corner();
			Determine_Top_Left_Corner();
		}
	}
}
