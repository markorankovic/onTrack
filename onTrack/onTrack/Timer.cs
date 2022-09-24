using Microsoft.Toolkit.Uwp.Notifications;
using onTrack.Reinforcements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using System.IO;

namespace onTrack
{
    public class Timer
    {
        static Reinforcement CurrentReinforcement = new StandardReinforcement();

        static string AlarmName = "Wake Up";

        static string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string sFile = Path.Combine(sCurrentDirectory, @"..\..\..\Alarm Sounds\" + AlarmName + ".wav");
        static string sFilePath = Path.GetFullPath(sFile);
        static string alertSoundPath = sFilePath;

        static System.Timers.Timer timer;
        static SoundPlayer soundPlayer = new (alertSoundPath);

        public static double Duration = 0.5;
        public static string Objective = "Your Objective";

        public static bool SoundPlaying = false;

        static List<Reinforcement> previousReinforcements = new();

        static Timer()
        {
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    if (CurrentReinforcement.IsValidResponse(toastArgs))
                    {
                        Reset();
                    }
                });
            };
        }

        public static string GetAlarmName()
        {
            return AlarmName;
        }

        public static void SetAlarmName(string alarmName)
        {
            AlarmName = alarmName;
            sFile = Path.Combine(sCurrentDirectory, @"..\..\..\Alarm Sounds\" + AlarmName + ".wav");
            sFilePath = Path.GetFullPath(sFile);
            alertSoundPath = sFilePath;
            Trace.WriteLine(alertSoundPath);
            soundPlayer = new(alertSoundPath);
        }

        public static void SetDuration(double duration)
        {
            Duration = duration;
        }

        public static void SetObjective(string objective)
        {
            Objective = objective;
        }

        public static Reinforcement GetReinforcement()
        {
            return CurrentReinforcement;
        }

        public static void SetReinforcement(Reinforcement reinforcement)
        {
            foreach(Reinforcement previousReinforcement in previousReinforcements)
            {
                if (reinforcement.GetType().Equals(previousReinforcement.GetType())) { CurrentReinforcement = previousReinforcement; return; }
            }
            CurrentReinforcement = reinforcement;
            previousReinforcements.Add(reinforcement);
        }

        public static void Stop()
        {
            soundPlayer.Stop();
            timer?.Stop();
        }

        private static void WakeUser()
        {
            soundPlayer.PlayLooping();
        }

        public static void PlayAlarm()
        {
            soundPlayer.PlayLooping();
            SoundPlaying = true;
        }

        public static void StopAlarm()
        {
            soundPlayer.Stop();
            SoundPlaying = false;
        }

        private static void ResetTimer()
        {
            Trace.WriteLine("Duration: " + Duration);
            timer?.Stop();
            timer = new(Duration * 60 * 1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        public static void Reset()
        {
            Dispatcher.CurrentDispatcher.Invoke(() => {
                soundPlayer.Stop();
                ResetTimer();
            });
        }

        private static void AlertUser()
        {
            CurrentReinforcement.CreateToast(Objective).Show();
        }

        private static void OnTimedEvent(System.Object source, ElapsedEventArgs e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                AlertUser();
                if (!(CurrentReinforcement is NoneReinforcement))
                {
                    timer = new(10 * 1000);
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    timer.Elapsed += OnToastPassed;
                }
                else { ResetTimer(); }
            });
        }

        private static void OnToastPassed(System.Object source, ElapsedEventArgs e)
        {
            WakeUser();
        }
    }
}
