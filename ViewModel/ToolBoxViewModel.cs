using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VectorDrawing.Model;
using VectorDrawing.MVVM;

namespace VectorDrawing.ViewModel
{
	internal class ToolBoxViewModel : ViewModelBase
	{
		private Tools _activetool;
		private Tools_Line _line_tool;
		private Tools_MultiLine _line_tool2;
		private Tools_MultiLine _line_tool3;

		public ObservableCollection<Tools> ToolsCollection { get; private set; }
		public RelayCommand<object> SwapToLineCommand=> new RelayCommand<object>(execute => swap_to_Line());
		public RelayCommand<string> SwapToolCommand => new RelayCommand<string>(parameter => SwapTool(parameter));

		public Tools_Line Line_Tool
		{
			get { return _line_tool; }	
			set { _line_tool = value; }
		}

		public Tools ActiveTool
		{
			get { return _activetool; }
			set { _activetool = value;OnPropertyChanged(); }
		}

		public ToolBoxViewModel(DrawingViewModel drawingvm, SceneTreeViewModel scene, Model.Camera camera, GridViewModel gridvm)
		{
			_line_tool = new Tools_Line (drawingvm, scene, camera, gridvm);
			_line_tool2 = new Tools_MultiLine(drawingvm, scene, camera, gridvm);
			_line_tool3 = new Tools_MultiLine(drawingvm, scene, camera, gridvm);
			ToolsCollection = new ObservableCollection<Tools>
			{
				_line_tool,
				_line_tool2,
				_line_tool3,
			};
			ActiveTool = _line_tool;
		}
		public void SwapTool(string arg)
		{
			switch (arg)
			{
				case "Line_Tool":
					Deactivate_Tool(ActiveTool);
					_line_tool.IsChecked = true;
					ActiveTool = _line_tool;
					break;
				case "Eraser_Tool":
					Deactivate_Tool(ActiveTool);
					_line_tool2.IsChecked = true;
					ActiveTool = _line_tool2;
					break;
				case "Selecteur_Tool":
					Deactivate_Tool(ActiveTool);
					_line_tool3.IsChecked = true;
					ActiveTool = _line_tool3;
					break;
			}
		}

		public void swap_to_Line()
		{
			Deactivate_Tool(ActiveTool);
			_line_tool.IsChecked = true;
			ActiveTool = _line_tool;
		}
		public void Deactivate_Tool(Tools tool)
		{
			tool.IsChecked = false;
		}
	}
}
