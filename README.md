Space Invaders Clon
===================


This is an almost perfect Space Invaders Clon made using Unity3D.

You can play it here: http://www.jocyf.com/Preview/SpaceInvadersWebGL/index.html


![Screenshot](spaceinvaders.jpg)


Space Invaders project notes:
------------------------

Font used: "arcade machine font": https://www.fontspace.com/codeman38/press-start-2p  (codeman38_press-start-2p)
This project only uses sprites (Almost all sprites are contained in one file) and 2d physics.


Project code notes:
-------------------

This project uses only one scene to show the Main menu and to play, activating/deactivating things properly.

Managers.

SpaceInvadersManager -> StartGame, PauseGame, Detect GameOver or LevelFinished. Manage players live and score and update this values on screen UI.

EnemiesManager -> Generates/Destroys the enemies and always know the enemies number left in game (so it can tell us when there isn't any at all) to level up

UFOManager -> Creates/Destroy the red UFO after waiting a period of time.

BarrierManager -> Creates/Destroy the barriers.


AudioManager -> A very basic audio manager to play some sounds. It's not a good manager because it creates/destroy the AudioSource to play a sound but
				it's is enought in this little project.

Al the oher code enemies, playership, movement, fire , etc it's almost self explanatory.

There are several videos explaining all the process there are long videos (all in Spanish)

These videos are Youtuve_Life content, so I'm not only talking about this project, but there is an step by step part en each one that you can follow.

Part 1: https://www.youtube.com/watch?v=ubBeW9tYXuk

Part 2: https://www.youtube.com/watch?v=QWUglDFwkdA

Part 3: https://www.youtube.com/watch?v=XNkFsoVP48g

Part 4: https://www.youtube.com/watch?v=EFiATOWOlyQ

Part 5: https://www.youtube.com/watch?v=6R5QcixrWM8

Part 6 (Barriers): https://www.youtube.com/watch?v=eu2K2baqNO4

Part 7 (Sounds): https://www.youtube.com/watch?v=bYo0yUZbdss


License
-------
Unity project and source code license:
[MIT License](https://opensource.org/licenses/MIT).

Note: This project is coying the sprites and sounds from the original "Space Invaders" game licensed to Taito Corporation Inc.
in order to create an Space Invaders clon game for learning purposes only.




