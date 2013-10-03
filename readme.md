[INTRODUCTION]

This game is my take at the old 'Tanks' game trend.

The game starts with 2-4 players who each have a tank on a square board, and they can move the tank around using the keyboard keys.
The square board is 'spherical' for tanks: any tank who goes from one side comes out the other.
Each tank starts out on a different corner of the board and a customizable color.
SIDENOTE: There are 8 colors to choose from: Red, Green, Blue, Aqua, Purple, Pink, Yellow and Orange.

The game's point is to stay the last one on the board by eliminating other tanks using bullets.

Bullets, in-code, are more commonly called 'Projectiles', as certain projectiles are not actually bullets, but rather missiles and such.
Projectiles, normally, last 10 seconds (can be changed) and each tank can have up to 3 bullets at a time on the board (also changeable - see Settings section).

The tanks also have a defence mechanism - Fences!
A tank can both shoot projectiles and place fences.
Fences look like thick lines with the same color as their placer's tank.
Fences are made to deflect projectiles, so when a tank places a fence, it creates a projectile-proof wall.
(Of course, for some projectiles, it's not enough.)
It is also tank-proof, so tanks cannot go through it (although I have seen some cases where a tank glitches into a fence).
Like projectiles, fences also last for 10 seconds by default, and each tank has a limit of 3 simultaneous fences on the board.
In the future, fences will have different effects, but right now, just good ol' fences.

Next in line, we have power-ups.
Power-ups divide into three obvious groups, all of which inherit the main PowerUp class.

First, The Instants:
The instant power-ups (classes that inherit the InstantPowerUp class) are picked up by tanks and when they get picked up, they do nothing.
When the player wants to use them, he will push the 'place fence' button and then their effect will happen.
Effects include swpping places with other tanks and reflecting all bullets (turning them 180 degrees).
Instant power-ups are usable any time after taking them. (EVEN if you are dead!)

Next, we have the Timed:
The Timed power-ups (classes that inherit the TimedPowerUp class) are the most familiar type of power-ups.
Like Invincibilty for instance, timed power-ups are taken by the player, and give its tank a special ability for a finite amount of time.
I don't need to list any examples, as you probably know, but the invincibility I mentioned earlier if implemented in 2 ways.

And finally, the Appearing power-ups!
These power-ups which inherit the AppearingPowerUp class are unique and don't appear in many games.
These power-ups cannot be taken, but rather 'recycled'.
Think of it like a button, which when you step over it, something happens, but it stays there.
Appearing power-ups come in all shapes and sizes, so examples aren't gonna explain much, but one example is the badly-written Portal.
You go through one, and come out the other.
The players each have 6 keys which can be customized:
Forward - moves forward
Back - moves backward
Left - rotates left
Right - rotates right
Place - either places fence or activates power-up.
Shoot - shoots the tank's pending bullet.

And the only other thing in the game that is a very important part is the Settings file.
The settings file contains many