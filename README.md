![graph_1](https://github.com/user-attachments/assets/2f90c2e9-2772-4e60-a86e-f05b14e20dac)

This graph depicts a macro which initializes the "projector boundaries" of the arena.  The player's drone assistant flies to the center of the screen, then launches 3 miniature drones which then activate the hologram projections that create the game boundaries.
The boundaries are flagged as actors, as are the miniature drones which sit off-screen initially, which allows them to be controlled by the graph processor at runtime.  On the very bottom are some examples of the "custom script" node which runs an advanced C# script to handle the activity.  Some properties are exposed to allow precision control of the event.  The parallelization node posts the directives connected to their output ports in tandem, so all of these activities happen in unison with no performance hit.

![graph_2](https://github.com/user-attachments/assets/87c7f49d-879d-4329-b7f4-d2c46174a48e)

This depicts the early part of a graph that initializes the main gameplay loop.  Whenever an enemy wave is encountered, this cinematic graph is used to operate the actions of what goes on on the player's screen.  All of the nodes depicted are written for
this project specifically, and required at least ~40 hours of cumulative development to get them all working and tested.  A prior iteration of the cinematics system used a much more primitive array of action models, so much time was saved by being able to migrate over most of that original code, which took another week or so of development on it's own.
