﻿using Microsoft.Toolkit.Uwp.Notifications;
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
using System.Threading.Tasks;

namespace onTrack
{
    public class Location
    {
        public int x;
        public int y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Timer
    {
        static Reinforcement CurrentReinforcement = new StandardReinforcement();

        static string AlarmName = "Wake Up";

        static System.Timers.Timer timer;
        static SoundPlayer soundPlayer = new (Properties.Resources.Wake_Up);

        public static double Duration = 10;
        public static int Counted = 0;
        public static int Remaining { get { return (int)(Duration - Counted); }  }

        public static string Objective = "Your Objective";

        public static bool SoundPlaying = false;

        public static bool Playing = false;

        public static DateTime? TimeInitiated = null;

        public static int TimeEllapsed { get { return (int) DateTime.UtcNow.Subtract(TimeInitiated != null ? TimeInitiated.Value : DateTime.UtcNow).TotalMilliseconds; } }

        static List<Reinforcement> previousReinforcements = new();

        public static bool autoPausePlay = false;

        public static bool autoFocus = false;

        public static Key? autoPauseKey;

        public static Location autoFocusClickLocation = null;

        public static Location autoPlayClickLocation = null;

        static Timer()
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
                            taskTree?.SetCurrentTask(newGoal);
                        }
                        if (autoPlayClickLocation != null && autoPausePlay)
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
                soundPlayer.Stop();
                timer?.Stop();
            });
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
            TimeInitiated = DateTime.UtcNow;
            Playing = true;
            ExecuteCallbacks();
            ExecuteFinishCallbacks();
            Counted = 0;
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
                Stop();
                ResetTimer();
            });
        }
         
        private static void AlertUser()
        {
            CurrentReinforcement.CreateToast(Objective)
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

        private static void OnTimedEvent(System.Object source, ElapsedEventArgs e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                Counted += 1;
                if (Counted <= Duration)
                {
                    ExecuteCallbacks();
                    return;
                }
                timer.AutoReset = false;
                timer.Enabled = false;

                AlertUser();

                if (autoPauseKey != null && autoPausePlay)
                {
                    SendAutoPauseKey();
                }
                if (CurrentReinforcement is WhatYouGonnaDoNowReinforcement && autoFocus)
                {
                    FocusOnTheTextBox();
                }
            });
        }

        private static void AutoPlay()
        {
            InputSimulator inputSimulator = new InputSimulator();
            var X = autoPlayClickLocation.x * 65535 / System.Windows.SystemParameters.WorkArea.Width;
            var Y = autoPlayClickLocation.y * 65535 / System.Windows.SystemParameters.WorkArea.Height;
            inputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(X, Y);
            System.Threading.Thread.Sleep(500);
            inputSimulator.Mouse.LeftButtonClick();
        }

        private static void FocusOnTheTextBox()
        {
            InputSimulator inputSimulator = new InputSimulator();
            var X = autoFocusClickLocation.x * 65535 / System.Windows.SystemParameters.WorkArea.Width;
            var Y = autoFocusClickLocation.y * 65535 / System.Windows.SystemParameters.WorkArea.Height;
            inputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(X, Y);
            System.Threading.Thread.Sleep(500);
            inputSimulator.Mouse.LeftButtonClick();
        }

        private static void ClickTheCentreOfTheScreen()
        {
            InputSimulator inputSimulator = new InputSimulator();
            var X = (3840 / 2) * 65535 / 3840;
            var Y = (2160 / 2) * 65535 / 2160;
            inputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(X, Y);
            inputSimulator.Mouse.LeftButtonClick();
        }

        private static void SendAutoPauseKey()
        {
            if (autoPauseKey == null) return;
            InputSimulator inputSimulator = new InputSimulator();
            VirtualKeyCode keyCode = (VirtualKeyCode) KeyInterop.VirtualKeyFromKey(autoPauseKey.Value);
            inputSimulator.Keyboard.KeyDown(keyCode);
        }

        private static void OnToastPassed(object sender, ToastDismissedEventArgs e)
        {
            if (!(CurrentReinforcement is NoneReinforcement) && Remaining == -1)
            {
                WakeUser();
            }
            ToastNotificationManagerCompat.History.Clear();
        }

        public static void RecordClick(int x, int y, bool autoFocus)
        {
            if (autoFocus)
                autoFocusClickLocation = new Location(x, y);
            else
                autoPlayClickLocation = new Location(x, y);
        }
    }
}