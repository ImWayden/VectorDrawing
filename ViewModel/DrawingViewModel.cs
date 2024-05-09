using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VectorDrawing.Model;
using VectorDrawing.MVVM;

namespace VectorDrawing.ViewModel
{
	internal class DrawingViewModel : ViewModelBase
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

		private DrawingBitmapManager _drawingBitmapManager;

		public DrawingBitmapManager DrawingBitmapManager
		{
			get { return this._drawingBitmapManager; }
			set { this._drawingBitmapManager = value;}
		}
		public DrawingViewModel(WriteableBitmap DrawingBitmap, Camera camera, SceneTreeViewModel Scene)
		{
			_drawingBitmapManager = new DrawingBitmapManager(DrawingBitmap, Scene, camera);
			UpdateBitmap();
		}

		public void OnSizeChanged(int width, int height)
		{
			_drawingBitmapManager.Bitmap = _drawingBitmapManager.Bitmap.Resize(width, height, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
			UpdateBitmap();
		}

		public void UpdateBitmap()
		{
			_drawingBitmapManager.Update();
			Bitmap = _drawingBitmapManager.Bitmap;
		}
	}
}
