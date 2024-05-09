using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace VectorDrawing.Model
{
	internal abstract class BitmapManagerBase
	{
		protected WriteableBitmap _bitmap;
		protected DrawingVisual drawingVisual;
		protected DrawingContext drawingContext;
		protected Camera camera;
		protected Pen pen;

		public WriteableBitmap Bitmap
		{
			get { return _bitmap; }
			set { _bitmap = value; }
		}
		public void Render(WriteableBitmap bitmap, Visual visual)
		{
			if (bitmap == null || visual == null)
			{
				throw new ArgumentNullException();
			}
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bitmap.Width, (int)bitmap.Height, 96, 96, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(visual);
			bitmap.Lock();
			renderTargetBitmap.CopyPixels(new Int32Rect(0, 0, renderTargetBitmap.PixelWidth, renderTargetBitmap.PixelHeight), bitmap.BackBuffer, bitmap.BackBufferStride * bitmap.PixelHeight, bitmap.BackBufferStride);
			bitmap.Unlock();
		}

		public virtual void Update() {}
	}
}
