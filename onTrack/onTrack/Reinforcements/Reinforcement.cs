using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (toastArgs.Argument == "") return false;
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
                .AddText(goal);
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            return true;
        }
    }

    public class PressTheRightGoalReinforcement : Reinforcement
    {
        public class CharacterMutater
        {
            Random rand = new Random();
            private List<int> generatedIndices = new List<int>();

            public void AddGeneratedIndex(int index, string objective)
            {
                generatedIndices.Add(index);
                if (generatedIndices.Count == objective.Length + 2)
                    generatedIndices.Clear();
            }

            public enum Mutation
            {
                add,
                sub
            }

            public Mutation RandomMutation()
            {
                int randomInt = rand.Next(0, 2);
                return randomInt == 0 ? Mutation.add : Mutation.sub;
            }

            public List<int> NonGeneratedIndices(string objective, bool outsideRange)
            {
                List<int> indices = new List<int>();

                for (int i = 0; i < objective.Length; i++)
                    if (!generatedIndices.Contains(i))
                        indices.Add(i);

                if (outsideRange)
                {
                    if (!generatedIndices.Contains(-1))
                    {
                        indices.Add(-1);
                        AddGeneratedIndex(-1, objective);
                    }
                    if (!generatedIndices.Contains(objective.Length))
                    {
                        indices.Add(objective.Length);
                        AddGeneratedIndex(objective.Length, objective);
                    }
                }

                return indices;
            }

            public int RandomIndex(string objective, bool outsideRange = true)
            {
                List<int> nonGeneratedIndices = NonGeneratedIndices(objective, outsideRange);
                int randomInt = rand.Next(0, nonGeneratedIndices.Count);
                int index = nonGeneratedIndices[randomInt];
                AddGeneratedIndex(index, objective);
                return index;
            }

            public string AddChar(string objective)
            {
                int randomIndex = RandomIndex(objective);
                if (randomIndex < 0) return "." + objective;
                else if (randomIndex > objective.Length) return objective + ".";
                return objective.Insert(randomIndex, ".");
            }

            public string SubChar(string objective)
            {
                int randomIndex = RandomIndex(objective, false);
                return objective.Remove(randomIndex, 1);
            }

            public string GenerateCharacterMutation(string objective)
            {
                return RandomMutation() == Mutation.add ? AddChar(objective) : SubChar(objective);
            }

            public string[] GenerateCharacterMutations(string objective, int count)
            {
                string[] results = new string[count];
                for (var i = 0; i < count; i++)
                    results[i] = GenerateCharacterMutation(objective);
                generatedIndices.Clear();
                return results;
            }

            public string[] GenerateMutationsOfObjective(string objective, int count)
            {
                int randPlace = rand.Next(count);
                string[] mutations = GenerateCharacterMutations(objective, count - 1);
                List<string> results = mutations.ToList();
                results.Insert(randPlace, objective);
                return results.ToArray();
            }
        }

        string Goal = null;
        public ToastContentBuilder CreateToast(string goal)
        {
            CharacterMutater characterMutater = new CharacterMutater();
            this.Goal = goal;
            var content = new ToastContentBuilder()
                            .AddText("Are you focusing?")
                            .AddText("Objective: " + goal);
            string[] mutations = characterMutater.GenerateMutationsOfObjective(goal, 4);
            foreach (string mutation in mutations)
            {
                content.AddButton(
                    new ToastButton()
                        .SetContent(mutation)
                        .AddArgument("focused", mutation == goal ? "yes" : "yes")
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
                var response = toastArgs.UserInput["tbReply"].ToString();
                if (response == null || response.Length == 0) return false;
                previousResponse = response;
                return true;
            }
            return false;
        }
    }

    public class RandomReinforcement : Reinforcement
    {
        string Goal = null;
        Reinforcement chosenReinforcement = null;
        Reinforcement[] reinforcements = { new StandardReinforcement(), new PressTheRightGoalReinforcement(), new TypeOutTheGoalReinforcement(), new WhatYouGonnaDoNowReinforcement() };
        Random random = new Random();
        Reinforcement GetReinforcement()
        {
            var randIndex = random.Next(reinforcements.Length);
            var reinforcement = reinforcements[randIndex];
            return Timer.GetReinforcementInstance(reinforcement);
        }
        public ToastContentBuilder CreateToast(string goal)
        {
            chosenReinforcement = GetReinforcement();
            return chosenReinforcement.CreateToast(goal);
        }
        public bool IsValidResponse(ToastNotificationActivatedEventArgsCompat toastArgs)
        {
            return chosenReinforcement.IsValidResponse(toastArgs);
        }
    }
}