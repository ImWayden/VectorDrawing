using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDrawing.Model
{
	public class Nodes_Layers:Nodes
	{
		private bool isvisible;
		public Nodes_Layers() {
			isvisible = true;
		}

		public void Merge_Layer(Nodes_Layers Layer) {
			Compare_Corners(Layer);
			this.Childs.AddRange(Layer.Childs);
		}
	}
}
