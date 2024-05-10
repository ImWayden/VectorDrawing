using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDrawing.Model;
using VectorDrawing.MVVM;

namespace VectorDrawing.ViewModel
{
	internal class ToolBarViewModel : ViewModelBase
	{
		public UndoManager undoManager;
		public SaveManager saveManager;
        public ToolBarViewModel()
        {
            undoManager = UndoManager.GetInstance();
        }
    }
}
