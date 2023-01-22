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
            if (CurrentObjective != null)
            {
                CurrentObjective.tb.IsEnabled = false;
                CurrentObjective.HideTools();
                //CurrentObjective = null;
            }
        }

        ObjectiveControl currentObjective = null;
        ObjectiveControl CurrentObjective { get { return currentObjective; } set { currentObjective = value; } }

        public void SetCurrentObjective(ObjectiveControl objective)
        {
            CurrentObjective = objective;
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            var focusedElement = FocusManager.GetFocusedElement(this);
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
    }
}