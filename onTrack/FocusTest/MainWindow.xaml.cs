using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FocusTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            var focusedElement = FocusManager.GetFocusedElement(this);
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, button);
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, grid);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            var focusedElement = FocusManager.GetFocusedElement(this);
        }
    }
}
