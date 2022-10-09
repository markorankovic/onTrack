using Microsoft.Toolkit.Uwp.Notifications;
using onTrack.Reinforcements;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Timers;
using System.Windows.Threading;
using System.IO;
using System;

namespace onTrack
{
    public class Timer
    {
        static Reinforcement CurrentReinforcement = new StandardReinforcement();

        static string AlarmName = "Wake Up";

        static System.Timers.Timer timer;
        static SoundPlayer soundPlayer = new (Properties.Resources.Wake_Up);

        public static double Duration = 30;
        public static int Counted = 0;
        public static int Remaining { get { return (int)(Duration - Counted); }  }

        public static string Objective = "Your Objective";

        public static bool SoundPlaying = false;

        public static bool Playing = false;

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

        static Action Callback;

        public static void AddCallback(Action callback)
        {
            Timer.Callback = callback;
        }

        public static string GetAlarmName()
        {
            return AlarmName;
        }

        public static void SetAlarmName(string alarmName)
        {
            AlarmName = alarmName;
            Stream stream;
            switch (alarmName)
            {
                case "Police": stream = Properties.Resources.Police; break;
                case "Evacuation": stream = Properties.Resources.Evacuation; break;
                default: stream = Properties.Resources.Wake_Up; break;
            }
            soundPlayer = new(stream);
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

        public static Reinforcement GetReinforcementInstance(Reinforcement reinforcement)
        {
            foreach (Reinforcement previousReinforcement in previousReinforcements)
            {
                if (reinforcement.GetType().Equals(previousReinforcement.GetType())) { return previousReinforcement; }
            }
            previousReinforcements.Add(reinforcement);
            return reinforcement;
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
            Counted = 0;
            Callback();
            Playing = false;
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
            Playing = true;
            Counted = 0;
            Callback();
            Trace.WriteLine("Duration: " + Duration);
            timer?.Stop();
            timer = new(Duration * 1000);
            timer.Elapsed += OnTimedEvent;
            timer.Interval = 1000;
            timer.AutoReset = true;
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
                Counted += 1;
                if (Counted <= Duration)
                {
                    Callback();
                    return;
                }
                timer.AutoReset = false;
                timer.Enabled = false;
                AlertUser();
                if (!(CurrentReinforcement is NoneReinforcement))
                {
                    timer = new(15 * 1000);
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
