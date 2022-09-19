# P06Cutscenes
Tools to create cutscenes in custom stages for Sonic the Hedgehog P-06

# Features
Play custom animations whenever you want
Play custom sounds at any point during the animations
Change the stage background music
Change the current playable character quickly
Load/Unload objects
Teleport the current character
Chain cutscenes together

# Installing 
To install the mod, place the P06Cutscenes.dll from the "In Game" release in "(StH P06 Root Folder)/Mods"

# Creating a cutscene
First, add the P06Cutscenes.dll from the "Creator" release to your custom stage Unity project.

## Setting up animations
All animations for a cutscene start playing at the same time, so if an animation plays at the end of a cutscene, the animation needs to be at the end of the animation clip with a keyframe at the beginning to ensure accurate timing.

Once your animations are played, they need to be added to an animator. Each object needs its own animator. The animator needs a parameter called "start". Make the default animator state an empty new state. Add a transition to the animation. Make sure the transition is set to not use exit time, has a transition duration of 0, and a condition for start being true.

Make a transition from the animation to a different empty state. For this transition, turn on exit, have no conditions, a transition duration of 0, and the transition occurs at the end of the animation.
You can add a second animation coming from the second empty state in the same way as the first animation. Only do this in a controlled stage to make sure the animations will always play in the correct order.

## Setup the cutscene object
Add a cutscene component to a new empty Game Object. Each unique cutscene will need it's own cutscene component, and it's own Game Object. If the cutscene will play when the player enters an area, add a trigger to the Game Object. If the cutscene will play after another cutscene, place it into the first cutscene's "Next Cutscene" variable.

### Animations
Add all relevant animators to the animators section of the cutscene object. Add the clips those animators will be playing. They do not need to be in any order.

### Sounds
Add all the sounds that you want during the cutscene. Each sound in the list will only play once, so if you want a sound to play multiple times, you'll need it in the list multiple times.

Sound start times are the delay between the cutscene starting, and the sound actually playing. The order they're in matters. This is the delay for the sound in the same position, so the first sound start time is the delay for the first sound in the sound list, and the fifth sound start time is the delay for the fifth sound in the sound list.

### Camera
The Camera variable must be filled. This is the object the cutscene camera will attach. Animate this object for camera movement during cutscenes.

### Audio Control
Audio Simmer Mode determines what happens to the stage background music during the cutscene.
Audio Simmer Mode 0 = Immediately stop the background music and restart it after the cutscene.
Audio Simmer Mode 1 = Immediately stop the background music, but don't restart it.
Audio Simmer Mode 2 = Fade the music out over the course of 1 second.
Audio Simmer Mode 3 = Keep playing the music during the cutscene.

Change BGM allows you to play a new song as the stage background music after the cutscene.
Play At Start makes Change BGM play at the start of the cutscene instead of the end.

### Post Cutscene Setup
Everything in this section will happen after the cutscene ends.

Tp Point allows you to teleport the player to a Game Object.
To Disable will disable all objects so the can't be seen or interacted with.
To Enable does the opposite of To Disable.

Next Cutscene will start a cutscene as soon as this one ends if it's filled. If your cutscene switches to an entirely different area, it's recommended to have half of the cutscene on one object, unload the area of the current cutscene, load the cutscene's next area, then run a new cutscene in the new area.

Change Character lets you change the playable character after the cutscene. The default option, "Keep," won't change the character. 

# Auto Disable
This isn't required to use, but helps when using large or multiple areas. Will check if the character is inside a trigger. As soon as the character enters the trigger, the area will load. When the character exits the trigger, the area will unload. You can play a different stage song for respawning in an area by placing an audio clip in Respawn BGM. Target is the object that will load/unload.

If you're changing areas after a cutscene. Don't rely on Auto Disable and use the Cutscene components To Disable/To Enable functions.
