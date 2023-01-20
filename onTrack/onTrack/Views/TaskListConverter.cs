using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace onTrack.Views
{
    public class TaskListConverter : IValueConverter
    {
        void TaskItemToTreeViewItem(TreeViewItem parent, TaskItem task)
        {
            if (task == null) return;
            parent.Header = task.Task;
            foreach (TaskItem item in task.Children)
            {
                TreeViewItem child = new TreeViewItem();
                parent.Items.Add(child);
                TaskItemToTreeViewItem(child, item);
            }
        }

        TreeView ConvertTaskToTree(TaskItem task)
        {
            TreeView treeView = new TreeView();

            TreeViewItem parent = new TreeViewItem();

            TaskItemToTreeViewItem(parent, task);

            treeView.Items.Add(parent);
            return treeView;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TaskItem task = (TaskItem) value;
            var result = ConvertTaskToTree(task);
            return result.Items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
