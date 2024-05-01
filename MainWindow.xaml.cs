using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VectorDrawing.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VectorDrawing
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool isloaded = false;
		private DrawingCanvas DrawingCanvas;
		private ToolBox toolBox;
		public MainWindow()
		{
			Debug.WriteLine("Main");
			InitializeComponent();
			Loaded += MainWindow_Loaded;
			SizeChanged += MainWindow_SizeChanged;
		}
	
		private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (!isloaded)
				return;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("loaded");
			Initialize_DrawingCanvas();
			Initialize_ToolBox();
			isloaded = true;
			//throw new NotImplementedException();
		}

		private void Initialize_DrawingCanvas()
		{
			DrawingCanvas = new DrawingCanvas(canvas);
		}

		private void Initialize_ToolBox()
		{
			toolBox = new ToolBox(ToolBoxPanel);
			toolBox.ToolSelected += DrawingCanvas.Swap_ActiveTool;
		}
	}
}