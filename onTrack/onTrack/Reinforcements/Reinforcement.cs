using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onTrack.Reinforcements
{
    public interface Reinforcement
    {
        public ToastContentBuilder CreateToast(string goal) { return null; }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs) { return false; }
    }

    public class TypeOutTheGoalReinforcement : Reinforcement
    {
        string Goal = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            this.Goal = goal;
            return new ToastContentBuilder()
                .AddText("Are you focusing?")
                .AddText("Objective: " + goal)
                .AddInputTextBox("tbReply", "Type out the goal here")
                .AddButton(new ToastButton()
                    .SetContent("Submit")
                    .AddArgument("action", "wakeup")
                    .SetBackgroundActivation()
                );
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            string typedOutGoal = toastArgs.UserInput["tbReply"].ToString();
            if (typedOutGoal == Goal)
            {
                return true;
            }
            return false;
        }
    }

    public class StandardReinforcement : Reinforcement
    {
        string Goal = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            this.Goal = goal;
            return new ToastContentBuilder()
                .AddText("Are you focusing?")
                .AddText("Objective: " + goal)
                .AddButton(new ToastButton()
                    .SetContent("Yes")
                    .AddArgument("action", "wakeup")
                    .SetBackgroundActivation()
                );
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            return true;
        }
    }

    public class NoneReinforcement : Reinforcement
    {
        string Goal = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            this.Goal = goal;
            return new ToastContentBuilder()
                .AddText("Objective: " + goal);
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            return true;
        }
    }

    public class PressTheRightYesReinforcement : Reinforcement
    {
        string Goal = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            this.Goal = goal;
            var buttonsFirst = (new Random()).NextDouble() >= 0.5;
            var content = new ToastContentBuilder()
                            .AddText("Are you focusing?")
                            .AddText("Objective: " + goal);
            if (buttonsFirst)
            {
                content
                    .AddButton(new ToastButton()
                        .SetContent("Yes")
                        .AddArgument("focused", "yes")
                        .SetBackgroundActivation()
                        )
                    .AddButton(new ToastButton()
                        .SetContent("No")
                );
            }
            else
            {
                content
                    .AddButton(new ToastButton()
                        .SetContent("No")
                        )
                    .AddButton(new ToastButton()
                        .SetContent("Yes")
                        .AddArgument("focused", "yes")
                        .SetBackgroundActivation()
                );
            }
            return content;
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            if (toastArgs.Argument == "focused=yes")
            {
                return true;
            }
            return false;
        }
    }

    public class WhatYouGonnaDoNowReinforcement : Reinforcement
    {
        string Goal = null;
        string previousResponse = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            this.Goal = goal;
            if (previousResponse != null)
            {
                return new ToastContentBuilder()
                    .AddText("Objective: " + goal)
                    .AddText("What smaller thing are you gonna do right now?")
                    .AddText("Previous response: " + previousResponse)
                    .AddInputTextBox("tbReply", "")
                    .AddButton(new ToastButton()
                        .SetContent("Submit")
                        .AddArgument("action", "wakeup")
                        .SetBackgroundActivation()
                    );
            }
            else
            {
                return new ToastContentBuilder()
                    .AddText("Objective: " + goal)
                    .AddText("What smaller thing are you gonna do right now?")
                    .AddInputTextBox("tbReply", "")
                    .AddButton(new ToastButton()
                        .SetContent("Submit")
                        .AddArgument("action", "wakeup")
                        .SetBackgroundActivation()
                    );
            }
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            if (toastArgs.UserInput["tbReply"] != null)
            {
                previousResponse = toastArgs.UserInput["tbReply"].ToString();
                return true;
            }
            return false;
        }
    }
}
