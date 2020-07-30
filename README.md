# Google-Glass-Spaceship-Shooter

This is a very simple app made for the Google Glass Enterprise 2. 
I used the Unity Engine to develop it

The basic objective of the game is to shoot as many spaceships as possible. At this point there is no time limit or risk factor but I do plan on having the aliens shoot back and having power-ups for the player to collect
The player can shoot by lining up the reticle with the spaceships and then tapping the side of the Glasses to shoot. If the shot hits then the spaceships will explode and the player wil be awarded points.

The game can be paused by swiping down. The game can be exited by pausing the game and then swiping until they reach the "exit button"

This project also contains a very useful GestureDetector which I converted from Java because it was originally made for Android Studio. This GestureDetector is what is used to handle any gestures made on the Google Glass's touch pad. In order to accomodate for multiple tappable/interactive gameObjects, this script has an "isFocus" bool which controls which object will handle any gestures that are detected.

![Screenshot 1](https://user-images.githubusercontent.com/54575751/88876829-7b47f800-d267-11ea-9b02-afbe5c174f1f.PNG)

![Screenshot 2](https://user-images.githubusercontent.com/54575751/88876835-7c792500-d267-11ea-8a4b-7011fe1dbb68.PNG)
