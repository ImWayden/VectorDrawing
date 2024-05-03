using System.Diagnostics;
using System.Runtime.CompilerServices;
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
using VectorDrawing.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VectorDrawing
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ICommand UndoCommand { get; set; }
		private bool isloaded = false;
		private ConteneurCanvas ConteneurCanvas;
		private ToolBox toolBox;
		private ToolBar_Manager toolBar;
		public MainWindow()
		{
			Debug.WriteLine("Main");
			InitializeComponent();
/*			BitmapViewModel Drawing_bitmap = new BitmapViewModel();
			
			Drawing_Canvas.DataContext = Drawing_bitmap;

			BitmapViewModel Grid_Bitmap = new BitmapViewModel();
			Grid_Canvas.DataContext = Grid_Bitmap;
*/

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
			Initialze_ToolBar();
			Initialize_Canvas();
			Initialize_ToolBox();
			isloaded = true;
			//throw new NotImplementedException();
		}
		private void Initialze_ToolBar()
		{
			 toolBar = new ToolBar_Manager(MyToolBar);
		}
		private void Initialize_Canvas()
		{
			ConteneurCanvas = new ConteneurCanvas(Drawing_Canvas, Grid_Canvas, Conteneur_Grid);
		}

		private void Initialize_ToolBox()
		{
			toolBox = new ToolBox(ToolBoxPanel);
			toolBox.ToolSelected += ConteneurCanvas.Swap_ActiveTool;
		}

	}
}