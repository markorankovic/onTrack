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
            Timer.AddFinishCallback(ResetSequence);
            if (Timer.Playing)
            {
                CurrentTime = 251 * ((Timer.TimeEllapsed / 1000) / Timer.Duration);
                RunSequence();
            }
        }

        public static DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(double), typeof(Clock), new PropertyMetadata(0.0));

        public double CurrentTime
        {
            get { return (double)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        Storyboard timeSequence = new Storyboard();
        DoubleAnimation doubleAnimation = new DoubleAnimation();

        private void ResetSequence()
        {
            Dispatcher.Invoke(() =>
            {
                if (Timer.Playing)
                {
                    RunSequence();
                }
                else
                {
                    StopSequence();
                }
            });
        }

        public void RunSequence()
        {
            progressBar.Visibility = Visibility.Visible;
            double max = 251.0;
            doubleAnimation.From = CurrentTime;
            doubleAnimation.To = max;
            double duration = Timer.Duration - (Timer.TimeEllapsed / 1000);
            doubleAnimation.Duration = TimeSpan.FromSeconds(duration >= 0 ? duration : 0);

            timeSequence.Children.Add(doubleAnimation);

            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(CurrentTimeProperty));

            timeSequence.Begin(this, true);
        }

        public void StopSequence()
        {
            progressBar.Visibility = Visibility.Hidden;
            timeSequence.Stop(this);
            CurrentTime = 0.0;
        }

        private void progressBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Trace.WriteLine(CurrentTime);
            Trace.WriteLine(CurrentTimeProperty);
        }
    }
}