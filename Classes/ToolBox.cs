using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace VectorDrawing.Classes
{
	internal class ToolBox
	{
		public event EventHandler<Tools> ToolSelected;
		Panel panel;
		private Dictionary<ToggleButton, Tools> ButtonToolMap;
		List<ToggleButton> Buttons;

		public ToolBox(StackPanel panel) {
			this.panel = panel;
			Buttons = new List<ToggleButton>();
			ButtonToolMap = new Dictionary<ToggleButton, Tools>();
			Init_ToggleButtons();
		}

		private void Associate_Tools_Button(ToggleButton Button)
		{
			if(Button.Name == "Tool_Line")
				ButtonToolMap.Add(Button, new Tools_Line());
			else if(Button.Name == "Tool_Eraser")
				ButtonToolMap.Add(Button, new Tools_Line());
			else if (Button.Name == "Tool_Selecteur")
				ButtonToolMap.Add(Button, new Tools_Line());
		}

		private void Init_ToggleButtons () {
			foreach (WrapPanel wrapPanel in panel.Children)
			{
				foreach(ToggleButton Button in wrapPanel.Children)
				{
					Button.Checked += ToggleButton_Checked;
					Associate_Tools_Button (Button);
					Buttons.Add(Button);
				}
			}
		}
		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			ToggleButton toggleButton = (ToggleButton)sender;
			if (toggleButton.IsChecked == true)
			{
				foreach (ToggleButton Button in Buttons)
				{
					if (Button.IsChecked == true && Button != toggleButton )
					{
						Button.IsChecked = false;
						OnToolSelected(ButtonToolMap[Button]);
					}
				}
			}	
		}
		private void OnToolSelected(Tools tool)
		{
			ToolSelected?.Invoke(this, tool);
		}
		
	}
}
