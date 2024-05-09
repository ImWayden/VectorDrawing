using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDrawing.Model
{
	public class Nodes_Layers:Nodes
	{
		private bool _isvisible;
		private Color _color;
		private string _name;

		public bool IsVisible
		{
			get{ return _isvisible; }
			set { _isvisible = value; }
		}
		
		public Color Color
		{
			get { return _color; }
			set { _color = value; }
		}
		//could add null value verification
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public Nodes_Layers() {
			Color = Color.White;
			Name = "Layer";
			IsVisible = true;
		}

		public void Merge_Layer(Nodes_Layers Layer) {
			Compare_Corners(Layer);
			this.Childs.AddRange(Layer.Childs);
		}
	}
}
