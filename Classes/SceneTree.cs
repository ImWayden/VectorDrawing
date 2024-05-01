using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDrawing.Classes
{
	public class SceneTree
	{
		public List<Nodes_Layers> Layers;
		public SceneTree() {
			Layers = [new Nodes_Layers()];
		}
	}
}
