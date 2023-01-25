using onTrack.Components;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
            if (CurrentObjective != null)
            {
                CurrentObjective.tb.IsEnabled = false;
                CurrentObjective.HideTools();
            }
        }

        ObjectiveControl currentObjective = null;
        ObjectiveControl CurrentObjective { get { return currentObjective; } set { currentObjective = value; } }

        public void SetCurrentObjective(ObjectiveControl objective)
        {
            CurrentObjective = objective;
        }

        IInputElement? focusedElement = null;

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            focusedElement = FocusManager.GetFocusedElement(this);
            if (focusedElement is ObjectiveControl && !focusedElement.Equals(CurrentObjective))
            {
                if (CurrentObjective != null)
                {
                    CurrentObjective.HideTools();
                }
                CurrentObjective = (ObjectiveControl) focusedElement;
                CurrentObjective.ShowTools();
            }
        }

        private void ObjectiveControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((ObjectiveControl)sender).SetScreen(this);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                CurrentObjective.AddChild();
            }
        }
    }
}