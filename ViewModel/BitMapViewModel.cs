using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using VectorDrawing.Model;
using VectorDrawing.MVVM;

namespace VectorDrawing.ViewModel
{
	internal class BitmapViewModel : ViewModelBase
	{
		private BitmapModel _bitmapModel;

		public BitmapModel BitmapModel
		{
			get { return _bitmapModel; }
			set
			{
				if (_bitmapModel != value)
				{
					_bitmapModel = value;
					OnPropertyChanged();
				}
			}
		}

		public BitmapViewModel()
		{
			_bitmapModel = new BitmapModel();
		}

		// Méthode pour modifier le bitmap et déclencher la notification de changement
		public void UpdateBitmap(WriteableBitmap newBitmap)
		{
			BitmapModel.Bitmap = newBitmap;
		}

	}
}
