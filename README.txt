Hi, here's a quick overview of the project.

PebbleShips Framework
=====================

This encapsulates all the core logic around the battleships game. The main entry point here is the Game class which is used as a point on contact for all player interactions.

PebbleShips
===========

This is the test console application which has a very basic UI for making guesses and getting basic feedback. The AI is quite simply guessing randomly just to provide some kind of second player to interact with. There is also a /test switch which runs an initial test script with two computer players.

PebbleShips Unit Tests
======================

These are the small number of tests I used to write some of the fiddly functions.

Other notes
==========

The game framework does support some basic eventing, but the console app does not use this. Just left it in as it was already started.

The code is only very basically tested and might still be prone to some interesting behaviour!
