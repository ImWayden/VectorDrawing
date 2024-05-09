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
		private bool _update;
		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
			set { _bitmap = value; }
		}
		public bool Update
		{
			get { return _update; }
			set {
				if (_update == true)
					_update = false;
				else if(_update == false)
					_update = true;
			}
		}

		public BitmapModel() { }


	}
}
