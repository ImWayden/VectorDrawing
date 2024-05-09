using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDrawing.Model;
using VectorDrawing.MVVM;

namespace VectorDrawing.ViewModel
{
    internal class SceneTreeViewModel : ViewModelBase
    {
        public ObservableCollection<Nodes_Layers> _layers { get; set; }
		private Nodes_Layers _active_layer;

		public RelayCommand<object> AddNewLayerCommand => new RelayCommand<object>(execute => AddNewLayer());
		public RelayCommand<object> RemoveLayerCommand => new RelayCommand<object>(execute => RemoveLayer(), canExecute => ActiveLayer != null && Layers.Count > 1);
		public RelayCommand<object> MoveLayerUpCommand => new RelayCommand<object>(execute => MoveLayerUp(), canExecute => ActiveLayer != null && ActiveLayer != Layers.First());
		public RelayCommand<object> MoveLayerDownCommand => new RelayCommand<object>(execute => MoveLayerDown(), canExecute => ActiveLayer != null && ActiveLayer != Layers.Last());
		public ObservableCollection<Nodes_Layers> Layers
		{
			get { return _layers; }
			set { _layers = value; OnPropertyChanged(); }
		}

		public Nodes_Layers ActiveLayer
		{
			get { return _active_layer; }
			set { _active_layer = value; OnPropertyChanged();}
		}

		public SceneTreeViewModel()
        {
			Layers = new ObservableCollection<Nodes_Layers>();
			AddNewLayer();
			ActiveLayer = Layers.First();
		}
		public void AddNewLayer()
		{
			// Créer un nouveau Layer et l'ajouter à la collection
			var newLayer = new Nodes_Layers(); // Assurez-vous de créer un nouvel objet Layer selon votre implémentation
			Layers.Add(newLayer);
		}

		public void RemoveLayer()
		{
			Layers.Remove(ActiveLayer);
			ActiveLayer = Layers.First();
		}

		public void MoveLayerUp()
		{
			int currentIndex = Layers.IndexOf(ActiveLayer);
			Layers.Move(currentIndex, currentIndex - 1);
		}

		public void MoveLayerDown()
		{
			int currentIndex = Layers.IndexOf(ActiveLayer);
			Layers.Move(currentIndex, currentIndex + 1);
		}

		public void LayerToggleVisible()
		{
			ActiveLayer.IsVisible = !ActiveLayer.IsVisible;
		}
	}
}
