# onTrack 1.2.0
- Introducing the task tree
- App data is persisted in JSON format

# Introduction
The aim of onTrack is to help people who struggle to focus on a given task and sometimes also lose track of things they were supposed to do. onTrack monitors the user's focus through routinely communications between the app and the user with minimal disruptions to the user's activity. This takes place by specifying the task the user is focused on and the interval at which the app which fire a notification to check if the user is still focused on the task. However, simply displaying notifications to user can eventually serve little to no purpose if the user gets used to seeing the same notification. To overcome this issue the app can use Reinforcements for the notifications designed to obtain responses from the user in such a way that the user would have to be conscious about the task in order to deliver a valid response. Failure to do so will result in the alarm setting off to which the user would have to go back on the app and reset/stop the timer.

# Usage
1. Open the app and you should see the following screen.
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/0.png)
2. Where it says "Your Objective", replace that with the title of your task (if you haven't already set your tasks).
3. The timer below is for you to adjust how often you want the notification to fire (By default it is 30s).
4. Before you start the timer by pressing the play button, check the settings by pressing the ⚙️ button.
5. Create a task tree (see Task Tree) by navigating to the screen by pressing the 📃 button.
6. Adjust the settings (see Settings) as needed
7. You're ready to get onTrack!

# Task Tree
The task tree allows users to break up their tasks into smaller, but specific sub-tasks which in turn can be repeated on sub-tasks.

![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/task_tree_1.png)

Let's say you wanna be an astronaut, that's quite ambitious ain't it? 
So let's break the dream down into realistic steps! 
Let's first rename "Your Objective" by clicking on it and then the pencil icon that shows up. 
Let's instead put "Apply for NASA".

![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/task_tree_2.png)

It now should look as follows ⬇️

![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/task_tree_3.png)

Let's now break it down by adding a sub-task which you can do by pressing the ESC button on "Apply for NASA".

![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/task_tree_4.png)

Let's now rename "New Goal" to "Study Physics".

![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/task_tree_5.png)

Now to set the sub-task as the current task, press the arrow button to the right.

![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/task_tree_6.png)

Now your task is to study Physics if you're gonna dream about becoming an astronaut! 🧑‍🚀

# Settings
## Notifications
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/notifications_settings.png)
### Reinforcements
Reinforcements offer a variety of UIs on the notifications aimed to reinforce the user's awareness of the task by demanding certain responses that require the user's attention.
#### Standard
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/3.png)

The Standard reinforcement features a notification which asks the user if they're focusing with their task displayed and the user must respond by clicking on the Yes button.
#### Type out the task
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/4.png)

This reinforcement requires that the user types out the task they're supposed to do and then press the submit button.
#### Press the right goal
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/6.png)

The user has to select the corresponding objective they have specified. The name of the objective is copied multiple times and only one is the original one with the rest modified to look subtly different. This way the user has to carefully look at what the goal is to be reminded of what to do in the event that the user procrastinates. 
#### What you gonna do now
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/8.png)

What you gonna now, the moment the notification is gone? Not the ultimate task you have set, but the exact action you will take right at the moment?
The idea here is to make the user think about smaller milestones for reaching the objective they've set. The next time this notification fires, it will display the previous response made by the user. Pressing the done button similarly to the Standard reinforcement will move the current task to the next one, which is a sibling task if there is either a pre-existing one or the parent task, otherwise the response is a new sibling task. Pressing the New Goal button will set the response as a sub-task of the current task.
#### Random
Randomly selects a reinforcement after every interval. This way users don't get too habitual with their responses.
## Automations
Automations (Or macros) are used so certain actions such as pausing/playing a video is done automatically when the time is up and users don't have to always move the mouse to focus on the textbox if they need to make a textual response to reinforcements such as What You Gonna Do Now or Type Out The Goal.
### Recording a button press
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/automations_settings.png)
Click on "Record" for the pause button and when it says "Stop" then press the key you want to use to pause the video.
### Recording a click
For playing a video or focusing on a textbox of a reinforcement, press record and then click on the part of the screen like the play button or the textbox of the simulated notification. NOTE: Don't actually press the textbox, instead place the cursor close to it (as shown by the [red cross](https://www.icrc.org/en/donate) in the screenshot below) and wait for the notification to disappear and then move the cursor to where the textbox was and then click. 
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/recording_focus_click.png)
I'm sorry for the inconvenience, I know it takes blood & sweat!
## Sound
### Alarm Sounds
![alt text](https://github.com/markorankovic/onTrack/blob/release/1.2.0/Screenshots/audio_settings.png)
Here you can choose which alarm sound will play on failure to respond to the notification within 10 seconds. The default sound is Wake Up which is a wake-up alarm sound effect, the other two is Evacuation featuring a nuclear warning alarm sound effect and the Police siren. Below is the Test button allowing you to test out the selected alarm sound.
