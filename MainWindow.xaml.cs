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
using VectorDrawing.Model;
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
		private Model.Camera camera;

		private WriteableBitmap grid_bmp;
		private GridViewModel Grid_VM;

		private WriteableBitmap drawing_bmp;
		private DrawingViewModel Drawing_VM;

		private SceneTreeViewModel SceneTree_VM;
		private ToolBarViewModel ToolBar_VM;
		private ToolBoxViewModel ToolBox_VM;

		private MainViewModel MainViewModel;

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
			Grid_VM.UpdateBitmap();
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("loaded");
			camera = new Model.Camera();
			camera.Update(Conteneur_Grid.ActualHeight, Conteneur_Grid.ActualWidth);
			Initialize_ViewModels();
			isloaded = true;
			//throw new NotImplementedException();
		}
		// i don't like the fact that the part responsible to draw on the bitmap is coupled with the scenetreeviewmodel 
		private void Initialize_ViewModels()
		{
			SceneTree_VM = new SceneTreeViewModel();
			grid_bmp = new WriteableBitmap((int)Conteneur_Grid.ActualWidth, (int)Conteneur_Grid.ActualHeight, 96, 96, PixelFormats.Pbgra32, null);
			Grid_VM = new GridViewModel(grid_bmp, camera);
			drawing_bmp = new WriteableBitmap((int)Conteneur_Grid.ActualWidth, (int)Conteneur_Grid.ActualHeight, 96, 96, PixelFormats.Pbgra32, null);
			Drawing_VM = new DrawingViewModel(drawing_bmp, camera, SceneTree_VM); 
			Grid_Layer.DataContext = Grid_VM;
			Drawing_Layer.DataContext = Drawing_VM;
			ToolBox_VM = new ToolBoxViewModel(Drawing_VM, SceneTree_VM, camera, Grid_VM);
			ToolBoxPanel.DataContext = ToolBox_VM;
			MainViewModel = new MainViewModel(Grid_VM, Drawing_VM, SceneTree_VM, ToolBox_VM, camera);
			Conteneur_Grid.DataContext = MainViewModel;
			Conteneur_Grid.MouseMove += MainViewModel.OnMouseMove;
			Conteneur_Grid.MouseLeftButtonDown += MainViewModel.OnLeftButtonDown;
			Conteneur_Grid.MouseLeftButtonUp += MainViewModel.OnLeftButtonUp;
			Conteneur_Grid.MouseRightButtonDown += MainViewModel.OnRightButtonDown;
			Conteneur_Grid.MouseRightButtonUp += MainViewModel.OnRightButtonUp;
			Conteneur_Grid.MouseWheel += MainViewModel.OnMouseWheel;
			Conteneur_Grid.SizeChanged += MainViewModel.OnSizeChanged;
			MyToolBar.DataContext = new SaveManager(SceneTree_VM,Drawing_VM);
			RedoButton.DataContext = UndoManager.GetInstance();
			UndoButton.DataContext = UndoManager.GetInstance();
			Init_Keybinds();
		}
		private void Init_Keybinds() 
		{
			var undoBinding = new KeyBinding();
			undoBinding.Command = UndoManager.GetInstance().UndoCommand;
			undoBinding.Key = Key.Z;
			undoBinding.Modifiers = ModifierKeys.Control;

			var redoBinding = new KeyBinding();
			redoBinding.Command = UndoManager.GetInstance().RedoCommand;
			redoBinding.Key = Key.Y;
			redoBinding.Modifiers = ModifierKeys.Control;

			// Ajoutez les KeyBindings à la collection InputBindings de la fenêtre
			InputBindings.Add(undoBinding);
			InputBindings.Add(redoBinding);
		}
	}
}