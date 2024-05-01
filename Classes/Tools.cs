using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace VectorDrawing.Classes
{
	internal abstract class Tools
	{
		protected Canvas _canvas;
		protected DrawingCanvas _drawingCanvas;
		protected Classes.Camera _camera;
		protected bool IsRightMouseBouttonDown;
		protected bool IsLeftMouseBouttonDown;

		public Camera Camera
		{ 
			get { return _camera; }
			set { _camera = value; }
		}
		public Canvas Canvas
		{
			get {return  _canvas;}
			set { _canvas = value; }
		}

		public DrawingCanvas DrawingCanvas { 
			get { return _drawingCanvas; } 
			set { _drawingCanvas = value; }
		}
		public virtual void OnMouseMove(object sender, MouseEventArgs e) { }
		public virtual void OnMouseWheel(Canvas canvas) { }
		public virtual void OnRightMouseButtonDown(object sender, MouseButtonEventArgs e) { }
		public virtual void OnRightMouseButtonUp(object sender, MouseButtonEventArgs e) { }
		public virtual void OnLeftMouseButtonDown(object sender, MouseButtonEventArgs e) { } 
		public virtual void OnLeftMouseButtonUp(object sender, MouseButtonEventArgs e) { }
	}
}
