using onTrack.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace onTrack.Components
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Trace.WriteLine(value);
            var CurrentTask = (TaskItem) value;
            var TaskItem = (TaskItem) parameter;
            return CurrentTask.Equals(TaskItem) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public partial class ObjectiveControl : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(ObjectiveControl), new PropertyMetadata("Your Objective"));

        private UserControl Screen = null;

        public void SetScreen(UserControl screen)
        {
            Screen = screen;
        }

        public void ShowTools()
        {
            current.Visibility = Visibility.Visible;
            tools.Visibility = Visibility.Visible;
        }

        public void HideTools()
        {
            if (!(this.DataContext is TaskItem)) return;

            var taskItem = (TaskItem)this.DataContext;

            var currentTask = ((TaskTree)Application.Current.Resources["taskList"]).CurrentTask;
            if (currentTask?.Equals(taskItem) ?? false)
            {
                current.Visibility = Visibility.Visible;
            } else
            {
                current.Visibility = Visibility.Hidden;
            }
            tools.Visibility = Visibility.Hidden;
        }

        public ObjectiveControl()
        {
            InitializeComponent();
        }

        private void root_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(Screen, this);
            ShowTools();
        }

        private void edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tb.IsEnabled = true;
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            HideTools();
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var TaskItem = (TaskItem) this.DataContext;
            TaskItem.RemoveFromParent();
        }

        private void root_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public void AddChild()
        {
            var TaskItem = (TaskItem)this.DataContext;
            var NewChild = new TaskItem();
            NewChild.Task = "New Goal";
            TaskItem.AddChild(NewChild);
        }

        private void current_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((TaskTree)Application.Current.Resources["taskList"]).CurrentTask = (TaskItem) this.DataContext;
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var taskItem = (TaskItem)DataContext;

            taskItem.PropertyChanged += TaskItem_PropertyChanged;
            current.Visibility = taskItem.IsCurrentTask ? Visibility.Visible : Visibility.Hidden;

            bool isRoot = !((TaskItem)DataContext).HasParent();
            dragIcon.Visibility = isRoot ? Visibility.Collapsed : Visibility.Visible;
        }

        private void TaskItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                var taskItem = (TaskItem)DataContext;
                current.Visibility = taskItem.IsCurrentTask ? Visibility.Visible : Visibility.Hidden;
            }
            catch
            {
                Console.WriteLine("TaskItem is now likely disconnected");
            }
        }

        private void root_LostFocus(object sender, RoutedEventArgs e)
        {
            tb.IsEnabled = false;
        }

        private void DragAndDrop_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            TaskItem taskItem = (TaskItem)DataContext;
            bool isRoot = !taskItem.HasParent();
            if (e.LeftButton == MouseButtonState.Pressed && !isRoot)
            {
                DataObject data = new DataObject();
                data.SetData("Task", taskItem);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        private void DragAndDrop_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Hand);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }

        private void DragAndDrop_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent("Task"))
            {
                try
                {
                    object taskObject = e.Data.GetData("Task");
                    TaskItem taskItem = (TaskItem)taskObject;
                    TaskItem newParent = (TaskItem)DataContext;
                    if (taskItem != newParent && !taskItem.IsDescendant(newParent))
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                }
                catch
                {
                }
            }
            e.Handled = true;
        }

        private void DragAndDrop_Drop(object sender, DragEventArgs e)
        {
            try
            {
                object taskObject = e.Data.GetData("Task");
                TaskItem taskItem = (TaskItem)taskObject;
                TaskItem newParent = (TaskItem)DataContext;
                taskItem.SwitchParent(newParent);
            }
            catch
            {
                Console.WriteLine("Drop operation error: Failed to retrieve task data");
            }
        }
    }
}