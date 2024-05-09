using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VectorDrawing.Classes;
using VectorDrawing.Model;
using VectorDrawing.MVVM;

namespace VectorDrawing.ViewModel
{
	internal class MainViewModel : ViewModelBase
	{
		GridViewModel Grid_VM;
		DrawingViewModel Drawing_VM;
		SceneTreeViewModel SceneTree;
		ToolBoxViewModel ToolBox;
		UndoManager UndoManager;
		Camera camera;
        public MainViewModel(GridViewModel gridvm, DrawingViewModel drawvm, SceneTreeViewModel scene, ToolBoxViewModel toolbox, Camera camera)
        {
            Grid_VM = gridvm;
			Drawing_VM = drawvm;
			SceneTree = scene;
			ToolBox = toolbox;
			this.camera = camera;
			UndoManager = UndoManager.GetInstance();
        }

        public void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			camera.Update(e.NewSize.Height, e.NewSize.Width);
			Grid_VM.OnSizeChanged((int)e.NewSize.Width, (int)e.NewSize.Height);
			Drawing_VM.OnSizeChanged((int)e.NewSize.Width, (int)e.NewSize.Height);
		}

		public void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid != null)
			{
				camera.Zoom_Manager(e.Delta, e.GetPosition(grid), new Plan2D(grid.ActualWidth, grid.ActualHeight));
				Grid_VM.UpdateBitmap();
				Drawing_VM.UpdateBitmap();
			}
		}

		public void OnMouseMove(object sender, MouseEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid != null)
				ToolBox.ActiveTool.OnMouseMove(e.GetPosition(grid), grid.IsMouseCaptured);
		}

		public void OnLeftButtonDown(object sender,  MouseButtonEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid != null)
				ToolBox.ActiveTool.OnLeftButtonDown(e.GetPosition(grid));
		}
		public void OnLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid != null)
				ToolBox.ActiveTool.OnLeftButtonUp(e.GetPosition(grid));
		}
		public void OnRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid != null)
				ToolBox.ActiveTool.OnRightButtonDown(e.GetPosition(grid));
		}
		public void OnRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			Grid grid = sender as Grid;
			if (grid != null)
				ToolBox.ActiveTool.OnRightButtonUp(e.GetPosition(grid));
		}


	}
}
