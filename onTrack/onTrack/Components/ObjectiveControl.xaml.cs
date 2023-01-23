using onTrack.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace onTrack.Components
{
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
            tools.Visibility = Visibility.Visible;
        }

        public void HideTools()
        {
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
    }
}