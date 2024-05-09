using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VectorDrawing.Model;
using VectorDrawing.MVVM;

namespace VectorDrawing.ViewModel
{
	internal class GridViewModel : ViewModelBase
	{
		private ImageSource _bitmap;
		public ImageSource Bitmap
		{
			get { return _bitmap; }
			set
			{
				_bitmap = value;
				OnPropertyChanged("Bitmap");
			}
		}

		private GridBitmapManager _gridBitmapManager;

		public GridViewModel(WriteableBitmap gridBitmap, Camera camera)
		{
			_gridBitmapManager = new GridBitmapManager(gridBitmap, camera);
			UpdateBitmap();
		}

		public void OnSizeChanged(int width, int height)
		{
			_gridBitmapManager.Bitmap = _gridBitmapManager.Bitmap.Resize(width, height, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
			UpdateBitmap();
		}

		public void UpdateBitmap()
		{
			_gridBitmapManager.Update();
			Bitmap = _gridBitmapManager.Bitmap;
		}

	}
}
