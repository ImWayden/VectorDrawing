using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDrawing.Classes
{
	public interface IAction
	{
		public void Undo();
		public void Redo();
	}

	public class Action_AddObject : IAction 
	{

		Nodes Object;
		Nodes Layer;
		int index;

		public Action_AddObject(Nodes n, Nodes layer)
		{
			Object = n; 
			Layer = layer;
		}

		public void Undo()
		{
			index = Layer.Childs.IndexOf(Object);
			Layer.Remove_Object(Object);
		}
		public void Redo()
		{
			Layer.Add_Object_At(Object, index);
		}
	}

	public class Action_MergeLayer : IAction
	{

		Nodes_Layers Layer1;
		Nodes_Layers Layer2;
		Nodes_Layers Final_Layer;
		SceneTree SceneTree;
		int index1;
		int index2;


		public Action_MergeLayer(Nodes_Layers Layer1, Nodes_Layers Layer2, SceneTree scene, int index1, int index2)
		{
			this.Layer1 = Layer1;
			this.Layer2 = Layer2;
			this.index1 = index1;
			this.index2 = index2;
			SceneTree = scene;
		}

		public void Undo()
		{
			SceneTree.Layers.Remove(Final_Layer);
			SceneTree.Layers.Insert(index1,Layer1);
			SceneTree.Layers.Insert(index2,Layer2);
		}
		public void Redo()
		{
			SceneTree.Layers.Remove(Layer1);
			SceneTree.Layers.Remove(Layer2);
			SceneTree.Layers.Insert(index1, Final_Layer);
		}
	}

	public class UndoManager
	{
		private Stack<IAction> UndoActions;
		private Stack<IAction> RedoActions;
		private static UndoManager _instance;
		public UndoManager()
		{
			UndoActions = new Stack<IAction>();
			RedoActions = new Stack<IAction>();
		}

		public static UndoManager GetInstance()
		{
			if(_instance == null)
			{
				_instance = new UndoManager();
			}
			return _instance;
		}


		public void Add_Action(IAction action)
		{
			UndoActions.Push(action);
			RedoActions.Clear();
		}

		public void Undo()
		{
			IAction action = UndoActions.Pop();
			action.Undo();
			UndoActions.Push(action);
		}

		public void Redo()
		{
			IAction action = RedoActions.Pop();
			action.Redo();
			UndoActions.Push(action);
		}
	}

	enum ActionType
	{
		AddObject,
		RemoveObject,
		CreateLayer,
		RemoveLayer,
		MoveLayer
	}

}
