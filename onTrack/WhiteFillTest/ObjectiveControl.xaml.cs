using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WhiteFillTest
{
    public partial class ObjectiveControl : UserControl
    {
        public ObjectiveControl()
        {
            InitializeComponent();
         }

        private void root_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Focus();
        }

        private void edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void root_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public void AddChild()
        {
        }

        private void current_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TaskItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        }

        private void root_LostFocus(object sender, RoutedEventArgs e)
        {
        }
    }
}