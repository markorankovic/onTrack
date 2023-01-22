using onTrack.Components;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace onTrack.Views
{
    public partial class TaskListView : UserControl
    {
        public TaskListView()
        {
            InitializeComponent();
            DataContext = ((TaskTree) Application.Current.Resources["taskList"]);
        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        ObjectiveControl currentObjective = null;
        ObjectiveControl CurrentObjective { get { return currentObjective; } set { if (currentObjective != null) { currentObjective.HideTools(); } currentObjective = value; currentObjective.ShowTools(); } }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            var focusedElement = FocusManager.GetFocusedElement(this);
            if (focusedElement is ObjectiveControl)
            {
                CurrentObjective = (ObjectiveControl)focusedElement;
            } else
            {
                if (CurrentObjective != null)
                {
                    CurrentObjective.HideTools();
                }
            }
        }

        private void ObjectiveControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((ObjectiveControl)sender).SetScreen(this);
        }
    }
}