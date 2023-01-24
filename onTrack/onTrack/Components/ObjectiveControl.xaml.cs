﻿using onTrack.Views;
using System;
using System.Globalization;
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
            return (bool) value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility) value == Visibility.Visible ? true : false;
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
            var currentTask = ((TaskTree)Application.Current.Resources["taskList"]).CurrentTask;
            if (currentTask?.Equals((TaskItem)this.DataContext) ?? false)
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
            ((TaskTree)Application.Current.Resources["taskList"]).CurrentTask = (TaskItem)this.DataContext;
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            var taskTree = ((TaskTree?)Application.Current.Resources["taskList"]);
            var currentTask = taskTree.CurrentTask;
            var TaskItem = (TaskItem)this.DataContext;
            if (currentTask != null)
                currentTask.IsCurrentTask = currentTask?.Equals(TaskItem) ?? false;
        }
    }
}