using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace VectorDrawing.Classes
{
	internal class ToolBar_Manager
	{
		private UndoManager undoManager;
		private ToolBar toolbar;

		public ToolBar_Manager(ToolBar toolBar) {
			this.toolbar = toolBar;
			undoManager = UndoManager.GetInstance();
			Init_Buttons();
		}

		private void Init_Buttons()
		{
			foreach (Button boutton in toolbar.Items)
				boutton.Click += ButtonClick;
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			Button boutton = sender as Button;

			if (boutton.Name == "UndoButton")
				undoManager.Undo();
			else if (boutton.Name == "RedoButton")
				undoManager.Redo();
		}
	}
}
