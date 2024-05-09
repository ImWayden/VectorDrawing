using System.Windows;
using System.Windows.Input;
using VectorDrawing.ViewModel;
using VectorDrawing.Classes;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace VectorDrawing.Model
{
    internal class Tools_Line : Tools, IToolsAction
	{
		private double thickness;
		private Point startPoint;
		private Nodes_Lines line;
		private Nodes_Layers Active_Layer;
		private Point P1;
		private Point P2;
		private int actual_Index;
		private Action_AddObject action;

		private SceneTreeViewModel Scene;
		private DrawingViewModel DrawingVM;
		private GridViewModel GridVM;

		public Tools_Line(DrawingViewModel drawingvm, SceneTreeViewModel scene, Model.Camera camera, GridViewModel gridvm)
		{
			DrawingVM = drawingvm;
			Scene = scene;
			GridVM = gridvm;
			this.Camera = camera;
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
			Active_Layer.Add_Object(line);
			action = new Action_AddObject(line, Scene.ActiveLayer, new DUpdate_Canvas(DrawingVM.UpdateBitmap));
		}

		public void StartLine(Point MousePos)
		{
			Active_Layer = Scene.ActiveLayer;
			Create_Line(MousePos);
		}

		public void FinishLine()
		{
			Active_Layer.Compare_Corners(line);
			UndoManager.GetInstance().Add_Action(action);
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
			//should be optimized by having a tmp bitmap superposed to already drawn things since i don't need to redraw everything
			//only what im adding 
			DrawingVM.UpdateBitmap();
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
