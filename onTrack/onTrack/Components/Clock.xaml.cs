using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace onTrack.Components
{
    public partial class Clock : UserControl
    {
        public Clock()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(double), typeof(Clock), new PropertyMetadata(0.0));

        public double CurrentTime
        {
            get { return (double)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public void RunSequence()
        {
            double max = 251.0;
            Storyboard timeSequence = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = max;
            doubleAnimation.Duration = TimeSpan.FromSeconds(Timer.Duration);

            timeSequence.Children.Add(doubleAnimation);

            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(CurrentTimeProperty));

            timeSequence.Begin(this);
        }

        private void progressBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Trace.WriteLine(CurrentTime);
            Trace.WriteLine(CurrentTimeProperty);
        }
    }
}