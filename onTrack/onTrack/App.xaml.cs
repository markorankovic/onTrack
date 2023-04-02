using onTrack.Reinforcements;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace onTrack
{
    public class AppStore
    {
        [JsonInclude]
        public TaskTree taskTree;
        [JsonInclude]
        public Settings settings;
    }
    public class Point2D
    {
        [JsonInclude]
        public int x;
        [JsonInclude]
        public int y;
    }
    public class Settings
    {
        // Timer
        [JsonInclude]
        public double Duration;
        // Reinforcement
        [JsonInclude]
        public string reinforcement;
        // Audio
        [JsonInclude]
        public string alarmName;
        // Automations
        [JsonInclude]
        public bool autoPausePlayEnabled;
        [JsonInclude]
        public bool autoFocusEnabled;
        [JsonInclude]
        public Key? autoPauseKey;
        [JsonInclude]
        public Point2D? autoFocusClickLocation;
        [JsonInclude]
        public Point2D? autoPlayClickLocation;
    }

    public class TaskTree : INotifyPropertyChanged
    {
        TaskItem _Root;
        [JsonIgnore]
        public TaskItem Root { get { return _Root; } set { _Root = value; NotifyPropertyChanged("Root"); } }

        public ObservableCollection<TaskItem> Items { get; set; }
        [JsonInclude]
        public TaskItem? _CurrentTask;
        [JsonIgnore]
        public TaskItem? CurrentTask { 
            get { return _CurrentTask; } 
            set {
                var oldTask = CurrentTask;
                _CurrentTask = value;
                oldTask?.NotifyPropertyChanged("IsCurrentTask");
                CurrentTask?.NotifyPropertyChanged("IsCurrentTask");
                NotifyPropertyChanged();
            }
        }

        public TaskTree(TaskItem root)
        {
            _Root = root;
            Items = new ObservableCollection<TaskItem>();
            Items.Add(root);
        }

        public void SetParentOnChildTasks(TaskItem taskItem)
        {
            if (taskItem.Id.Equals(_CurrentTask.Id)) { 
                CurrentTask = taskItem; 
            }
            foreach (TaskItem t in taskItem.Children)
            {
                t.SetParent(taskItem);
                SetParentOnChildTasks(t);
            }
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    public class TaskItem : INotifyPropertyChanged
    {
        [JsonInclude]
        public Guid Id = Guid.NewGuid();
        TaskItem? Parent;
        public ObservableCollection<TaskItem> Children { get; set; }
        string _Task = "";
        public string Task { 
            get { return _Task; } 
            set { _Task = value; NotifyPropertyChanged("Task"); } 
        }

        public bool IsCurrentTask {
            get {
                var taskTree = ((TaskTree?)Application.Current.Resources["taskList"]);
                return taskTree?.CurrentTask?.Equals(this) ?? false;
            }
        }

        [JsonInclude]
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
        }

        void Save()
        {
            TaskTree taskTree = (TaskTree) Current.Resources["taskList"];
            Settings settings = new Settings();
            settings.Duration = Timer.Duration;
            settings.reinforcement = Timer.GetReinforcement().GetType().Name;
            settings.alarmName = Timer.GetAlarmName();
            settings.autoPausePlayEnabled = Timer.autoPausePlay;
            settings.autoFocusEnabled = Timer.autoFocus;
            settings.autoFocusClickLocation = new Point2D() { x = Timer.autoFocusClickLocation.x, y = Timer.autoFocusClickLocation.y };
            settings.autoPlayClickLocation = new Point2D() { x = Timer.autoPlayClickLocation.x, y = Timer.autoPlayClickLocation.y };
            settings.autoPauseKey = Timer.autoPauseKey;
            AppStore store = new AppStore();
            store.taskTree = taskTree;
            store.settings = settings;

            string jsonString = JsonSerializer.Serialize(store, new JsonSerializerOptions { WriteIndented = true });
            string currentPath = Environment.CurrentDirectory;
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(currentPath, "appStore.json")))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        void Open()
        {
            try
            {
                string currentPath = Environment.CurrentDirectory;
                string jsonString;
                using (StreamReader sr = new StreamReader(Path.Combine(currentPath, "appStore.json")))
                {
                    jsonString = sr.ReadToEnd();
                }
                AppStore appStore = JsonSerializer.Deserialize<AppStore>(jsonString)!;

                TaskItem root = appStore.taskTree.Items[0];
                appStore.taskTree.SetParentOnChildTasks(root);
                Current.Resources.Add("taskList", appStore.taskTree);

                Timer.Duration = appStore.settings.Duration;
                Timer.autoPausePlay = appStore.settings.autoPausePlayEnabled;
                Timer.autoFocus = appStore.settings.autoFocusEnabled;
                Timer.autoPauseKey = appStore.settings.autoPauseKey;
                Timer.SetReinforcement(appStore.settings.reinforcement);
                Timer.SetAlarmName(appStore.settings.alarmName);
                try
                {
                    Timer.autoFocusClickLocation = new Location(x: appStore.settings.autoFocusClickLocation!.x, y: appStore.settings.autoFocusClickLocation.y);
                    Timer.autoPlayClickLocation = new Location(x: appStore.settings.autoPlayClickLocation!.x, y: appStore.settings.autoPlayClickLocation.y);
                } catch { 
                }
            }
            catch
            {
                TaskItem parent = new TaskItem();
                parent.SetTask("Your Objective");
                TaskTree taskTree = new TaskTree(parent);
                taskTree.SetCurrentTask(parent);
                Current.Resources.Add("taskList", taskTree);
            }
        }

        void Application_Exit(object sender, ExitEventArgs e)
        {
            this.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Open();
        }
    }
}