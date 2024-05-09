using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VectorDrawing.MVVM;
using VectorDrawing.ViewModel;

namespace VectorDrawing.Model
{
    public interface IAction
    {
        public void Undo();
        public void Redo();
    }

    public delegate void DUpdate_Canvas();
    public class Action_AddObject : IAction
    {
        DUpdate_Canvas update_Canvas;
        Nodes Object;
        Nodes Layer;
        int index;

        public Action_AddObject(Nodes n, Nodes layer, DUpdate_Canvas update_Canvas)
        {
            Object = n;
            Layer = layer;
            this.update_Canvas = update_Canvas;
        }

        public void Undo()
        {
            index = Layer.Childs.IndexOf(Object);
            Layer.Remove_Object(Object);
			update_Canvas();
        }
        public void Redo()
        {
            Layer.Add_Object_At(Object, index);
			update_Canvas();
        }
    }

	 class Action_MergeLayer : IAction
    {

        Nodes_Layers Layer1;
        Nodes_Layers Layer2;
        Nodes_Layers Final_Layer;
        SceneTreeViewModel SceneTree;
        int index1;
        int index2;

        public Action_MergeLayer(Nodes_Layers Layer1, Nodes_Layers Layer2, SceneTreeViewModel scene, int index1, int index2)
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
            SceneTree.Layers.Insert(index1, Layer1);
            SceneTree.Layers.Insert(index2, Layer2);
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

		public RelayCommand<object> UndoCommand => new RelayCommand<object>(execute => Undo());
		public RelayCommand<object> RedoCommand => new RelayCommand<object>(execute => Redo());
		public UndoManager()
        {
            UndoActions = new Stack<IAction>();
            RedoActions = new Stack<IAction>();
        }

        public static UndoManager GetInstance()
        {
            if (_instance == null)
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
            if (UndoActions.Count <= 0)
                return;
            IAction action = UndoActions.Pop();
            action.Undo();
            RedoActions.Push(action);
        }

        public void Redo()
        {
            if (RedoActions.Count <= 0)
                return;
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
