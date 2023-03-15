using onTrack.Components;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            var objectiveControl = (ObjectiveControl) sender;
            objectiveControl.SetScreen(this);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                CurrentObjective.AddChild();
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        Point previousMousePosition = new Point(0.0, 0.0);
        Point mousePosition = new Point(0.0, 0.0);

        private void DragEnter(object sender, MouseEventArgs e)
        {
            Point delta = new Point(mousePosition.X - previousMousePosition.X, mousePosition.Y - previousMousePosition.Y);
            scroll_viewer.ScrollToHorizontalOffset(scroll_viewer.HorizontalOffset - delta.X);
            scroll_viewer.ScrollToVerticalOffset(scroll_viewer.VerticalOffset - delta.Y);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            previousMousePosition = mousePosition;
            mousePosition = e.GetPosition(this);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                DragEnter(sender, e);
            }
        }
    }
}