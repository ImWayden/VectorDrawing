using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using VectorDrawing.MVVM;
using VectorDrawing.ViewModel;

namespace VectorDrawing.Model
{
	internal class SaveManager
	{
		DrawingViewModel DrawingViewModel;
		SceneTreeViewModel SceneTreeViewModel;
		TreeSerializer serializer;
		public RelayCommand<object> SaveCommand => new RelayCommand<object>(execute => OnSaveClicked());
		public RelayCommand<object> LoadCommand => new RelayCommand<object>(execute => OnLoadClicked());
		public SaveManager(SceneTreeViewModel sceneTree, DrawingViewModel drawingView)
		{ 
			DrawingViewModel = drawingView;
			SceneTreeViewModel = sceneTree;
			serializer = new TreeSerializer();
		}
		public void OnSaveClicked()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			Debug.WriteLine("SaveClicked");
			// Configurer la boîte de dialogue
			saveFileDialog.Filter = "Fichier json (*.json)|*.json|Tous les fichiers (*.*)|*.*";
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			// Afficher la boîte de dialogue et vérifier si l'utilisateur a appuyé sur OK
			if (saveFileDialog.ShowDialog() == true)
			{
				// Obtenir le chemin du fichier sélectionné par l'utilisateur
				string filePath = saveFileDialog.FileName;
				if (System.IO.File.Exists(filePath))
				{
					// Le fichier existe déjà, demander à l'utilisateur s'il souhaite l'écraser
					MessageBoxResult result = MessageBox.Show("Le fichier existe déjà. Voulez-vous l'écraser ?", "Confirmation", MessageBoxButton.YesNo);
					if (result == MessageBoxResult.No)
						return;
				}
				Save_To_json(filePath);
				// Écrire votre logique pour sauvegarder le fichier à l'emplacement spécifié
				// Exemple : File.WriteAllText(filePath, "Contenu du fichier à sauvegarder");
			}
		}

		public void OnLoadClicked()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			// Configurez les propriétés de OpenFileDialog
			openFileDialog.Title = "Sélectionnez un fichier";
			openFileDialog.Filter = "Fichier JSON (*.json)|*.json|Tous les fichiers (*.*)|*.*";
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			// Affichez la boîte de dialogue et vérifiez si l'utilisateur a sélectionné un fichier
			bool? result = openFileDialog.ShowDialog();
			if (result == true)
			{
				// L'utilisateur a sélectionné un fichier, récupérez le chemin du fichier
				string filePath = openFileDialog.FileName;
				string json = File.ReadAllText(filePath);
				SceneTreeViewModel.Layers = serializer.DeserializeNodesFromJson(json);
				// Faites quelque chose avec le chemin du fichier (par exemple, chargez les données à partir du fichier)
			}
			SceneTreeViewModel.ActiveLayer = SceneTreeViewModel.Layers[0];
			DrawingViewModel.UpdateBitmap();
		}
		public void Save_To_json(string filepath)
		{
			ObservableCollection<Nodes_Layers> layers = SceneTreeViewModel.Layers;
			string json = serializer.Serialize(layers);
			File.WriteAllText(filepath, json);
		}

	}
	public class TreeSerializer
	{
		public string Serialize(ObservableCollection<Nodes_Layers> layers)
		{
			return JsonConvert.SerializeObject(layers, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			});
		}

		public ObservableCollection<Nodes_Layers> DeserializeNodesFromJson(string json)
		{
			List<JObject> jsonObjects = JsonConvert.DeserializeObject<List<JObject>>(json);

			ObservableCollection<Nodes_Layers> nodes = new ObservableCollection<Nodes_Layers>();

			foreach (var jsonObject in jsonObjects)
			{
				string type = jsonObject["Type"].ToString();
				Nodes_Layers node;
				node = jsonObject.ToObject<Nodes_Layers>();
				((Nodes_Layers)node).Childs = DeserializeChilds(jsonObject["Childs"].ToString());
				nodes.Add(node as Nodes_Layers);
			}

			return nodes;
		}
		public List<Nodes> DeserializeChilds(string json)
		{
			List<JObject> jsonObjects = JsonConvert.DeserializeObject<List<JObject>>(json);

			List<Nodes> nodes = new List<Nodes>();

			foreach (var jsonObject in jsonObjects)
			{
				string type = jsonObject["Type"].ToString();
				Nodes node;
				switch (type)
				{
					case "Nodes_Layers":
						node = jsonObject.ToObject<Nodes_Layers>();
						// Désérialisez récursivement les propriétés imbriquées
						((Nodes_Layers)node).Childs = DeserializeChilds(jsonObject["Childs"].ToString());
						break;
					case "Nodes_Lines":
						node = jsonObject.ToObject<Nodes_Lines>();
						break;
					// Ajoutez d'autres cas pour d'autres types d'objets si nécessaire
					default:
						throw new ArgumentException($"Type inconnu : {type}");
				}
				nodes.Add(node);
			}

			return nodes;
		}
	}
}
