using Microsoft.Toolkit.Uwp.Notifications;
using onTrack.Reinforcements;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Timers;
using System.Windows.Threading;
using System.IO;
using System;
using Windows.UI.Notifications;
using WindowsInput;
using WindowsInput.Native;
using System.Windows.Input;
using System.Windows;

namespace onTrack
{
    public class Main
    {
        static Reinforcement CurrentReinforcement = new StandardReinforcement();

        static string AlarmName = "Wake Up";

        static Timer Timer;
        static SoundPlayer SoundPlayer = new (Properties.Resources.Wake_Up);

        public static double Duration = 30;
        public static int Counted = 0;
        public static int Remaining { get { return (int)(Duration - Counted); }  }

        public static string CurrentObjective = "Your Objective";

        public static bool SoundPlaying = false;

        public static bool Playing = false;

        public static DateTime? TimeInitiated = null;

        public static int TimeEllapsed { get { return (int) DateTime.UtcNow.Subtract(TimeInitiated != null ? TimeInitiated.Value : DateTime.UtcNow).TotalMilliseconds; } }

        public static int? TimeToRespond = null;

        public static Timer? ResponseTimer = null;

        static List<Reinforcement> PreviousReinforcements = new();

        public static bool AFKMode = false;

        public static bool AutoPausePlay = false;

        public static bool AutoFocus = false;

        public static Key? AutoPauseKey;

        public static Location AutoFocusClickLocation = null;

        public static Location AutoPlayClickLocation = null;

        static Main()
        {
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                    if (CurrentReinforcement.IsValidResponse(toastArgs) || toastArgs.Argument.Equals("action=done_task") || toastArgs.Argument.Equals("action=new_goal"))
                    {
                        if (toastArgs.Argument.Equals("action=done_task"))
                        {
                            var taskTree = ((TaskTree?)Application.Current.Resources["taskList"]);
                            if (toastArgs.UserInput.Count == 0)
                            {
                                taskTree?.CurrentTask?.FinishedTask();
                            } else
                            {
                                var newTask = (string)toastArgs.UserInput["tbReply"];
                                taskTree?.CurrentTask?.FinishedTask(newTask);
                            }
                        }
                        else if (toastArgs.Argument.Equals("action=new_goal"))
                        {
                            var taskTree = ((TaskTree?)Application.Current.Resources["taskList"]);
                            var newGoal = new TaskItem();
                            newGoal.Task = (string)toastArgs.UserInput["tbReply"];
                            taskTree?.CurrentTask?.AddChild(newGoal);
                            taskTree?.SetCurrentTask(newGoal);
                        }
                        if (AutoPlayClickLocation != null && AutoPausePlay)
                        {
                            AutoPlay();
                        }
                        Reset();
                    }
                    else
                    {
                        WakeUser();
                    }
                    }));
                });
            };
        }

        static Action Callback;
        static Action FinishCallback;

        public static void AddCallback(Action callback)
        {
            Callback = callback;
        }

        public static void AddFinishCallback(Action callback)
        {
            FinishCallback = callback;
        }

        public static void StartTimerToRespond()
        {
            ResponseTimer = new Timer((double)(TimeToRespond! * 1000));
            ResponseTimer.Elapsed += (Object source, ElapsedEventArgs e) =>
            {
                ResponseTimer.Close();
                WakeUser();
            };
            ResponseTimer.Enabled = true;
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
            SoundPlayer = new(stream);
        }

        public static void SetDuration(double duration)
        {
            Duration = duration;
        }

        public static void SetObjective(string objective)
        {
            CurrentObjective = objective;
        }

        public static Reinforcement GetReinforcement()
        {
            return CurrentReinforcement;
        }

        public static Reinforcement GetReinforcementInstance(Reinforcement reinforcement)
        {
            foreach (Reinforcement previousReinforcement in PreviousReinforcements)
            {
                if (reinforcement.GetType().Equals(previousReinforcement.GetType())) { return previousReinforcement; }
            }
            PreviousReinforcements.Add(reinforcement);
            return reinforcement;
        }

        public static void SetReinforcement(Reinforcement reinforcement)
        {
            foreach(Reinforcement previousReinforcement in PreviousReinforcements)
            {
                if (reinforcement.GetType().Equals(previousReinforcement.GetType())) { CurrentReinforcement = previousReinforcement; return; }
            }
            CurrentReinforcement = reinforcement;
            PreviousReinforcements.Add(reinforcement);
        }

        public static void SetReinforcement(string Type)
        {
            switch (Type)
            {
                case "TypeOutTheGoalReinforcement": Main.SetReinforcement(new TypeOutTheGoalReinforcement()); return;
                case "StandardReinforcement": Main.SetReinforcement(new StandardReinforcement()); return;
                case "NoneReinforcement": Main.SetReinforcement(new NoneReinforcement()); return;
                case "PressTheRightGoalReinforcement": Main.SetReinforcement(new PressTheRightGoalReinforcement()); return;
                case "WhatYouGonnaDoNowReinforcement": Main.SetReinforcement(new WhatYouGonnaDoNowReinforcement()); return;
                case "RandomReinforcement": Main.SetReinforcement(new RandomReinforcement()); return;
            }
        }

        private static void ExecuteCallbacks()
        {
            Callback();
        }

        private static void ExecuteFinishCallbacks()
        {
            FinishCallback();
        }

        public static void Stop()
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                Counted = 0;
                Playing = false;
                ExecuteCallbacks();
                ExecuteFinishCallbacks();
                SoundPlayer.Stop();
                Timer?.Stop();
            });
        }

        private static void WakeUser()
        {
            SoundPlayer.PlayLooping();
        }

        public static void PlayAlarm()
        {
            SoundPlayer.PlayLooping();
            SoundPlaying = true;
        }

        public static void StopAlarm()
        {
            SoundPlayer.Stop();
            SoundPlaying = false;
        }

        private static void ResetTimer()
        {
            TimeInitiated = DateTime.UtcNow;
            Playing = true;
            ExecuteCallbacks();
            ExecuteFinishCallbacks();
            ResponseTimer?.Close();
            Counted = 0;
            Trace.WriteLine("Duration: " + Duration);
            Timer?.Stop();
            Timer = new(Duration * 1000);
            Timer.Elapsed += OnTimedEvent;
            Timer.Interval = 1000;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public static void Reset()
        {
            Dispatcher.CurrentDispatcher.Invoke(() => {
                Stop();
                ResetTimer();
            });
        }
         
        private static void AlertUser()
        {
            CurrentReinforcement.CreateToast(CurrentObjective)
                .Show(toast =>
                {
                    toast.Dismissed += OnToastPassed;
                });
        }

        public static void SimulateNotification()
        {
            new ToastContentBuilder()
                .AddText("Click where the text box is")
                .AddText("When the notification disappears")
                .AddInputTextBox("", "")
                .AddButton(
                    new ToastButton()
                        .SetContent("Click up there ⬆️")
                )
                .Show();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                Counted += 1;
                if (Counted <= Duration)
                {
                    ExecuteCallbacks();
                    if (AFKMode && !AFKTracker.EvaluateIsAFK((int)Duration))
                    {
                        Reset();
                    }
                    return;
                }
                Timer.AutoReset = false;
                Timer.Enabled = false;

                AlertUser();

                if (TimeToRespond != null && !(CurrentReinforcement is NoneReinforcement))
                {
                    StartTimerToRespond();
                }
                if (AutoPauseKey != null && AutoPausePlay)
                {
                    SendAutoPauseKey();
                }
                if (CurrentReinforcement is WhatYouGonnaDoNowReinforcement && AutoFocus)
                {
                    FocusOnTheTextBox();
                }
            });
        }

        private static void AutoPlay()
        {
            InputSimulator inputSimulator = new InputSimulator();
            var X = AutoPlayClickLocation.x * 65535 / SystemParameters.WorkArea.Width;
            var Y = AutoPlayClickLocation.y * 65535 / SystemParameters.WorkArea.Height;
            inputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(X, Y);
            System.Threading.Thread.Sleep(500);
            inputSimulator.Mouse.LeftButtonClick();
        }

        private static void FocusOnTheTextBox()
        {
            InputSimulator inputSimulator = new InputSimulator();
            var X = AutoFocusClickLocation.x * 65535 / SystemParameters.WorkArea.Width;
            var Y = AutoFocusClickLocation.y * 65535 / SystemParameters.WorkArea.Height;
            inputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(X, Y);
            System.Threading.Thread.Sleep(500);
            inputSimulator.Mouse.LeftButtonClick();
        }

        private static void SendAutoPauseKey()
        {
            if (AutoPauseKey == null) return;
            InputSimulator inputSimulator = new InputSimulator();
            VirtualKeyCode keyCode = (VirtualKeyCode) KeyInterop.VirtualKeyFromKey(AutoPauseKey.Value);
            inputSimulator.Keyboard.KeyDown(keyCode);
        }

        private static void OnToastPassed(object sender, ToastDismissedEventArgs e)
        {
            if (!(CurrentReinforcement is NoneReinforcement) && Remaining == -1)
            {
                WakeUser();
            } else
            {
                ResetTimer();
            }
            ToastNotificationManagerCompat.History.Clear();
        }

        public static void RecordClick(int x, int y, bool autoFocus)
        {
            if (autoFocus)
                AutoFocusClickLocation = new Location(x, y);
            else
                AutoPlayClickLocation = new Location(x, y);
        }
    }
}