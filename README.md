Owen Brandon: (100969833)

Computer Graphics Final Project Progression.

Part 1: Scene Setup.
As a simple "Grey boxed" scene used for a base project from my GDW group, the project has a veriety of objects that are interactable as well as some that are dynamic including the NPC's and a door that has to be moved in order to move forward.
There were also new animations that I made to spice up the environment a bit, however, Github lost some data and in the process lost those animations so I didn't have time to re-add them.

There are two main characters that can be swapped between (as long as there is line of sight and < 10m distance between them) in order to traverse different areas of the map (Press F to switch)
The human: Taller but slower, is capable of vaulting small ledges in order to reach new heights
The Fox: Smaller but more agile, is capable of fitting into smaller spaces that the human can't in order to progress.

There is a Win/Lose condition, however, since this is a singleplayer storybased game, the win condition is to reach the end of the level (as we haven't come up with enemy ideas yet, and only plan on using environmental threats in the mansion setting for the tutorial)
To reach this Win condition, make your way to the end of the map with one or more characters and jump onto the final platform to restart the scene.

Our visual style isn't fully defined yet since we're waiting on Art, so for now, we've gone with primitive characters that have been spiced up to add some character and emotion to them.

For the quality of the scene, I added more vaultable objects and some basic puzzles to quickly teach the mechanics as a greybox "Tutorial"

Part 2: Illumination.

Diffuse: This was implemented as a simple shader to showcase the light direction in the scene. It will change as the player moves around, however, it will always be on the same side as the main light doesn't move.
<img width="1084" height="610" alt="Screenshot 2025-10-23 224544" src="https://github.com/user-attachments/assets/920b8dea-b6d1-4115-822d-f79bb75fb1e3" />

Ambient: This was implemented similarly to the diffuse lighting, it is a simple shader that shows the direction of the main light source, however, this one also takes into consideration what color the ambient sky lighting is to make a more natural shading.
<img width="1093" height="614" alt="Screenshot 2025-10-23 224522" src="https://github.com/user-attachments/assets/11b928d4-cdd7-418d-8c5c-a198b78dae5d" />

Specular: This was the most effective in my opinion as it allowed me to become more creative with other tested shader ideas that didn't make it into this project, but may make it into the final project. and I also really liked the color scheme of blue on black.
<img width="1092" height="613" alt="Screenshot 2025-10-24 050608" src="https://github.com/user-attachments/assets/1c4ca3af-9899-4372-8b19-13a00cd5a973" />

I used a script that had a current index for the amount of materials within a list so that the script could see that there were 3 materials, clamp the list to a total of 3 before cycling back around and restarting at the top of the list. This way when you press space,
you will always change material types without fail.
<img width="1825" height="873" alt="Screenshot 2025-10-24 040950" src="https://github.com/user-attachments/assets/42bee135-0997-4356-b733-1dbd610a339f" />

Part 3: Color Correction.

This one gave me the most trouble as it wouldn't show up on my camera for the longest time, and eventually when it did show up, it was all distorted and pixelated. I managed to solve the pixelation problem, however, the resolution stretching is an issue that I'm not
quite sure how to fix just yet. So while the corrections do look good in my opinion, I need to find out how to work out some intricacies before they can be used.

Dark Warm: <img width="1088" height="610" alt="Screenshot 2025-10-23 224202" src="https://github.com/user-attachments/assets/b520ddd6-f22e-4c69-b6bc-b56953cc452a" />
LUT: <img width="1024" height="32" alt="Warm Dark" src="https://github.com/user-attachments/assets/a7131512-7da8-4d44-a89c-aba6955311e3" />

Dark Cool: <img width="1094" height="613" alt="Screenshot 2025-10-23 224254" src="https://github.com/user-attachments/assets/d9156c8d-cafd-44ed-a405-49eaf1d924de" />
LUT: <img width="1024" height="32" alt="Cool Dark" src="https://github.com/user-attachments/assets/d3550ad1-5f9d-426b-9510-14f3a5ace9d0" />

Light Cool: <img width="1088" height="611" alt="Screenshot 2025-10-23 224341" src="https://github.com/user-attachments/assets/e6a4334e-2249-442e-abd9-4fa890816dec" />
LUT:<img width="1024" height="32" alt="Cool bright" src="https://github.com/user-attachments/assets/78da231c-62b6-4f0d-8bd3-9a496d22335b" />

The script and shader that were used in this project were both taken from Canvas as I don't yet understand how color correction shaders work completely.
https://learn.ontariotechu.ca/courses/34225/pages/color-correction
[Lecture 05 Visual effects (1).pdf](https://github.com/user-attachments/files/23119958/Lecture.05.Visual.effects.1.pdf)

Part 3: Shaders and Effects.

I played with some previously used shader effects and tried experimenting or combining them to get different effects, one worked well, the other not so much in my opinion.

Rim + Inner Glow: <img width="166" height="216" alt="Screenshot 2025-10-23 224622" src="https://github.com/user-attachments/assets/13934ce7-f08d-4574-b87f-4fe497bb46ab" />
Bump + Specular + Lambert + Ambient: <img width="1091" height="609" alt="Screenshot 2025-10-23 224643" src="https://github.com/user-attachments/assets/2c5645f4-09b1-44cc-a74c-ddf1a63b84a3" />

One I had in mind specifically for characters to showcase a ghostly appearence with them being translucent but still prevelent in the game, and the other was just testing in order to prepare for a final shader that I will be implementing into the final game.










