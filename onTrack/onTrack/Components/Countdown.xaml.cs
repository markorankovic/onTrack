using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
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

        public Countdown()
        {
            InitializeComponent();
            UpdateTime();
            Timer.AddCallback(UpdateTime);
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
            return (time[0] * 60 * 10) + (time[1] * 60) + (time[2] * 10) + time[3];
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Timer.Playing)
            {
                textbox.IsEnabled = true;
            }
        }

        private void textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            textbox.IsEnabled = false;
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
            var res = time[0] + "" + time[1] + "m" + " " + time[2] + "" + time[3] + "s";
            previousText = res;
            textbox.Text = res;
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Timer.Playing) return;
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
    }
}