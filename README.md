![graph_1](https://github.com/user-attachments/assets/2f90c2e9-2772-4e60-a86e-f05b14e20dac)

This graph depicts a macro which initializes the "projector boundaries" of the arena.  The player's drone assistant flies to the center of the screen, then launches 3 miniature drones which then activate the hologram projections that create the game boundaries.
The boundaries are flagged as actors, as are the miniature drones which sit off-screen initially, which allows them to be controlled by the graph processor at runtime.

![graph_2](https://github.com/user-attachments/assets/87c7f49d-879d-4329-b7f4-d2c46174a48e)

This depicts the early part of a graph that initializes the main gameplay loop.  Whenever an enemy wave is encountered, this cinematic graph is used to operate the actions of what goes on on the player's screen.  All of the nodes depicted are written for
this project specifically, and required at least ~40 hours of cumulative development to get them all working and tested.
