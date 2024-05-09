using System.Windows;
using System.Windows.Input;
using VectorDrawing.ViewModel;
using VectorDrawing.Classes;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System;

namespace VectorDrawing.Model
{
    internal class Tools_MultiLine : Tools, IToolsAction
	{
		private double thickness;
		private Point startPoint;
		private Nodes_Lines line;
		private Nodes_Layers tmp_Layer;
		private Nodes_Layers Active_Layer;
		private Point P1;
		private Point P2;
		private int actual_Index;
		private List<Action_AddObject> actions;
		private SceneTreeViewModel Scene;
		private DrawingViewModel DrawingVM;
		private GridViewModel GridVM;

		public Tools_MultiLine(DrawingViewModel drawingvm, SceneTreeViewModel scene, Model.Camera camera, GridViewModel gridvm)
		{
			DrawingVM = drawingvm;
			Scene = scene;
			GridVM = gridvm;
			this.Camera = camera;
			tmp_Layer = new Nodes_Layers();
			actions = new List<Action_AddObject>();
			actual_Index = -1;
		}
		private void Send_Actions()
		{
			foreach (Action_AddObject action in actions)
				UndoManager.GetInstance().Add_Action(action);
			actions.Clear();
		}
		public void Camera_MovStart(Point startPoint)
		{
			this.startPoint = startPoint;
		}

		//potentiels probleme de decallage dans le calque
		private void Create_Line(Point MousePos)
		{
			Bitmap = DrawingVM.Bitmap as WriteableBitmap;
			P1 = Camera.PlanToCam(MousePos, new Plan2D(Bitmap.Width, Bitmap.Height));
			P1.X = Math.Round(P1.X, Camera.deepness - 1);
			P1.Y = Math.Round(P1.Y, Camera.deepness - 1);
			P2 = P1;
			line = new Nodes_Lines(P1, P2);
			tmp_Layer.Add_Object(line);
			actual_Index++;
			actions.Add(new Action_AddObject(line, Scene.ActiveLayer, new DUpdate_Canvas(DrawingVM.UpdateBitmap)));
		}

		public void StartLine(Point MousePos)
		{
			Active_Layer = Scene.ActiveLayer;
			Active_Layer.Add_Object(tmp_Layer);
			Create_Line(MousePos);
		}

		public void FinishLine()
		{
			Active_Layer.Remove_Object(tmp_Layer);
			Active_Layer.Merge_Layer(tmp_Layer as Nodes_Layers);
			Active_Layer.Compare_Corners(tmp_Layer);
			tmp_Layer.Childs.Clear();
			actual_Index = -1;
			Send_Actions();
		}
		public void Update_Movement(Point endPoint)
		{
			Vector delta = new Vector(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
			Camera.Position -= delta / Camera.Scale;
			startPoint = endPoint;
			//could be moved idk i need it here to update but i could put it inside MainViewModel
			Camera.Update(DrawingVM.Bitmap.Height, DrawingVM.Bitmap.Width);
			GridVM.UpdateBitmap();
			DrawingVM.UpdateBitmap();
		}
		public void Update_Line(Point endpoint)
		{
			//if (IsLeftMouseBouttonDown && isMouseCaptured)
			Point point = Camera.PlanToCam(endpoint, new Plan2D(Bitmap.Width, Bitmap.Height));
			point.X = Math.Round(point.X, Camera.deepness - 1);
			point.Y = Math.Round(point.Y, Camera.deepness - 1);
			line.P2 = point;
			if (point != P1)
			{
				DrawingVM.UpdateBitmap();
				Create_Line(endpoint);
			}
		}

		public override void OnLeftButtonUp(Point MousePos)
		{
			FinishLine();
			IsLeftMouseBouttonDown = false;
		}

		public override void OnLeftButtonDown(Point MousePos)
		{
			IsLeftMouseBouttonDown = true;
			StartLine(MousePos);
		}

		public override void OnRightButtonUp(Point MousePos)
		{
			IsRightMouseBouttonDown = false;
		}

		public override void OnRightButtonDown(Point MousePos)
		{
			IsRightMouseBouttonDown = true;
			Camera_MovStart(MousePos);
		}

		public override void OnMouseMove(Point MousePos, bool IsMouseCaptured)
		{

			if (IsLeftMouseBouttonDown)
				Update_Line(MousePos);
			else if (IsRightMouseBouttonDown)
				Update_Movement(MousePos);
		}
	}
}
