using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VectorDrawing.Model
{
	public class BitmapModel
	{
		private WriteableBitmap _bitmap;

		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
			set { _bitmap = value; }
		}

		public BitmapModel() { }
	}
}
