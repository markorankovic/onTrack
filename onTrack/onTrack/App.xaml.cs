using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace onTrack
{
    public class TaskTree : INotifyPropertyChanged
    {
        TaskItem _Root;
        public TaskItem Root { get { return _Root; } set { _Root = value; NotifyPropertyChanged("Root"); } }

        public ObservableCollection<TaskItem> Items { get; set; }

        TaskItem? _CurrentTask;
        public TaskItem? CurrentTask { 
            get { return _CurrentTask; } 
            set { 
                _CurrentTask = value; 
                NotifyPropertyChanged("CurrentTask");
            } 
        }

        public TaskTree(TaskItem root)
        {
            _Root = root;
            _CurrentTask = _Root;
            Items = new ObservableCollection<TaskItem>();
            Items.Add(root);
        }

        public void SetCurrentTask(TaskItem task)
        {
            if (CurrentTask != null)
            {
                Trace.WriteLine("CurrentTask.AddChild");
                CurrentTask.AddChild(task);
                Trace.WriteLine("Added child to current task");
            }
            CurrentTask = task;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            Trace.WriteLine("Property Changed");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    public class TaskItem : INotifyPropertyChanged
    {
        TaskItem? Parent;
        public ObservableCollection<TaskItem> Children { get; set; }
        string _Task = "";
        public string Task { get { return _Task; } set { _Task = value; NotifyPropertyChanged("Task"); } }
        public bool IsCurrentTask {
            get {
                var taskTree = ((TaskTree?)Application.Current.Resources["taskList"]);
                return taskTree?.CurrentTask?.Equals(this) ?? false;
            }
        }
        public bool IsDone = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskItem() 
        {
            Children = new ObservableCollection<TaskItem>();
        }

        TaskItem? GetUndoneChildTask()
        {
            foreach (var taskItem in Children)
            {
                if (!taskItem.IsDone)
                {
                    return taskItem;
                }
            }
            return null;
        }

        TaskItem BottomChild(TaskItem taskItem)
        {
            foreach (TaskItem task in taskItem.Children)
            {
                return BottomChild(task);
            }
            return taskItem;
        }

        public void FinishedTask(String? newTask = null)
        {
            var taskTree = ((TaskTree?)Application.Current.Resources["taskList"]);
            if ((!(Parent?.Equals(taskTree.CurrentTask)) ?? false) && newTask != null && !(newTask?.Equals("") ?? false))
            {
                var task = new TaskItem();
                task.Task = newTask;
                Parent?.AddChild(task);
                taskTree.CurrentTask.IsDone = true;
                taskTree.CurrentTask = task;
                return;
            }
            Parent?.ChildFinishedTask(this);
        }

        public void ChildFinishedTask(TaskItem Child)
        {
            Child.IsDone = true;
            var taskTree = ((TaskTree?)Application.Current.Resources["taskList"]);
            var undoneTask = GetUndoneChildTask();
            if (undoneTask != null)
            {
                var nextTask = BottomChild(undoneTask);
                taskTree.CurrentTask = nextTask;
                return;
            }
            taskTree.CurrentTask = this;
        }

        public void AddChild(TaskItem task)
        {
            Children.Add(task);
            task.Parent = this;
        }

        public void RemoveChild(TaskItem task)
        {
            if (task.IsCurrentTask)
            {
                ChildFinishedTask(task);
            }
            Children.Remove(task);
        }

        public void RemoveFromParent()
        {
            Parent?.RemoveChild(this);
        }

        public void SetTask(string task)
        {
            Task = task;
        }

        public void SetParent(TaskItem Parent)
        {
            this.Parent = Parent;
        }

        public string GetTask()
        {
            return Task;
        }
    }

    public partial class App : Application {
        public App()
        {
            InitializeComponent();

            TaskItem parent = new TaskItem();
            parent.SetTask("Your Objective");

            TaskTree taskTree = new TaskTree(parent);

            Resources.Add("taskList", taskTree);
        }
    }
}