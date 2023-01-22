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

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ObjectiveControl), new PropertyMetadata("Your Objective"));

        public ObjectiveControl()
        {
            InitializeComponent();
        }

        private void root_LostFocus(object sender, RoutedEventArgs e)
        {
            HideTools();
        }

        private void root_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void root_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(Screen, this);
        }
    }
}