using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace onTrack.Components
{
    public partial class Countdown : UserControl
    {
        string previousText = "";
        int[] time = {0, 0, 0, 0};
        string _timeStr = "";
        string timeStr { 
            get { return _timeStr; } 
            set { 
                _timeStr = value.Length > 4 ? value.Substring(1) : value;
                var timeStrArr = _timeStr.ToCharArray();
                for (int i = 0; i < 4; i++)
                {
                    var timeIndex = time.Length - 1 - i;
                    var timeStrArrIndex = timeStrArr.Length - 1 - i;
                    time[timeIndex] = (i + 1) > timeStrArr.Length ? 0 : int.Parse(timeStrArr[timeStrArrIndex].ToString());
                }
                ApplyTimeFormat();
            }
        }

        public int Duration = 0; 

        bool _enabled = false;
        bool Enabled { 
            get { return _enabled; } 
            set { 
                _enabled = value; 
                textbox.Opacity = _enabled ? 0.5 : 1; 
                second.Opacity = _enabled ? 0.5 : 1; 
                caret.Visibility = _enabled ? Visibility.Visible : Visibility.Hidden; 
                textbox.Cursor = _enabled ? Cursors.Arrow : Cursors.Hand; 
                second.Cursor = _enabled ? Cursors.Arrow : Cursors.Hand; 
            } 
        }

        public Countdown()
        {
            InitializeComponent();
            UpdateTime();
            Timer.AddCallback(UpdateTime);
            Enabled = false;
        }

        private void UpdateTime()
        {
            Dispatcher.Invoke(() =>
            {
                var minutes = Timer.Remaining / 60;
                var seconds = Timer.Remaining - (minutes * 60);
                timeStr = (minutes < 10 ? 0 + "" + minutes : "" + minutes) + "" + (seconds < 10 ? 0 + "" + seconds : "" + seconds);
            });
        }

        int StringToSeconds()
        {
            var seconds = (time[0] * 60 * 10) + (time[1] * 60) + (time[2] * 10) + time[3];
            if (seconds > 3600)
            {
                return 3600;
            }
            else if (seconds < 30)
            {
                return 30;
            }
            return seconds;
        }



        private void textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            Enabled = false;
        }

        private string GetDifference(string before, string after)
        {
            var afterArr = after.ToCharArray();
            var beforeArr = before.ToCharArray();
            for (int index = 0; index < afterArr.Length; index++)
            {
                if (beforeArr.Length == 0) break;
                if (afterArr[index] != beforeArr[index]) return afterArr[index].ToString();
            }
            return "";
        }

        private void ApplyTimeFormat()
        {
            var res = time[0] + "" + time[1] + "m" + " " + time[2] + "" + time[3];
            previousText = res;
            textbox.Text = res;
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Timer.Playing || !Enabled)
            {
                textbox.Text = previousText; 
                return;
            }
            string diff = GetDifference(previousText, textbox.Text);
            bool parsed = int.TryParse(diff, out _);
            if (parsed)
            {
                timeStr += diff;
                Timer.SetDuration(StringToSeconds());
            } else
            {
                textbox.Text = previousText;
            }
        }

        private void textbox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Timer.Playing)
            {
                Enabled = true;
            }
        }

        bool Second_Clicked = false;

        public void MoveFocus()
        {
            if (Second_Clicked)
            {
                Second_Clicked = false; 
            } else
            {
                textbox.MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Back) && !Timer.Playing)
            {
                if (timeStr.Length < 1) return;
                timeStr = timeStr.Remove(timeStr.Length - 1);
                Timer.SetDuration(StringToSeconds());
            }
        }

        private void StackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            textbox_PreviewMouseDown(sender, e);
            textbox.Focus();
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!Timer.Playing)
            {
                Enabled = true;
            }
        }

        private void second_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Second_Clicked = true;
        }
    }
}