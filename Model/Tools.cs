using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VectorDrawing.MVVM;
using System.Windows;




namespace VectorDrawing.Model
{

	public interface IToolsAction
	{
		void OnLeftButtonUp(Point MousePos);
		void OnLeftButtonDown(Point MousePos);
		void OnRightButtonUp(Point MousePos);
		void OnRightButtonDown(Point MousePos);
		void OnMouseMove(Point MousePos, bool IsMouseCaptured);
	}

	internal abstract class Tools : ViewModelBase , IToolsAction
	{
		protected WriteableBitmap _bitmap;
		protected Camera _camera;
		protected bool IsRightMouseBouttonDown;
		protected bool IsLeftMouseBouttonDown;
		private bool _isChecked = false;

		public Tools()
		{

		}
		public bool IsChecked
		{
			get { return _isChecked; }
			set { _isChecked = value; OnPropertyChanged(); }
		}
		public Camera Camera
		{ 
			get { return _camera; }
			set { _camera = value; }
		}
		
		public WriteableBitmap Bitmap
		{
			get {return _bitmap; }
			set { _bitmap = value; }
		}

		public abstract void OnLeftButtonDown(Point MousePos);
		public abstract void OnLeftButtonUp(Point MousePos);
		public abstract void OnRightButtonDown(Point MousePos);
		public abstract void OnRightButtonUp(Point MousePos);
		public abstract void OnMouseMove(Point MousePos, bool isMouseCaptured);
	}
}