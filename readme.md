[INTRODUCTION]

This game is my take at the old 'Tanks' game trend.

The game starts with 2-4 players who each have a tank on a square board, and they can move the tank around using the keyboard keys.
The square board is 'spherical' for tanks: any tank who goes from one side comes out the other.
Each tank starts out on a different corner of the board and a customizable color.
SIDENOTE: There are 8 colors to choose from: Red, Green, Blue, Aqua, Purple, Pink, Yellow and Orange.

The game's point is to stay the last one on the board by eliminating other tanks using bullets.
After you are the last one on the board, the game waits for 3 seconds to avoid 'photo-finishes', and furthermore, even freezes for 1 second after the game ended,
so you get to see that sweet victory frame.
AND if you die during these 3 seconds, they have served their purpose: no one gets a point.

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

Next up, we have Sudden Deaths!
Sudden Deaths occur after a set amount of time has passed that the players agree is too long for one match.
The goal of sudden deaths is to kill everybody because the match is too long.
You probaly know that, but sudden deaths aim to make everybody play an almost equal amount of time, and not sit in the side as 2 self-proclaimed 'Pros' shoot away for 10 minutes.
Sudden deaths kill in funny, awesome and sometimes weird ways, and are guaranteed to kill everyone within 20 seconds. (If no one dies of conventional ways, they just simply disappear.)
There is a list of sudden deaths below, but I recommend being surprised ;)

The players each have 6 keys which can be customized:
Forward - moves forward
Back - moves backward
Left - rotates left
Right - rotates right
Place - either places fence or activates power-up.
Shoot - shoots the tank's pending bullet.

[SETTINGS]

Another very important part of the game is the Settings file.
The settings file is in the bin/x86/debug folder, along with the exe to play the game.
In the settings file, you will find many settings, so i made up a list for you so you won't get lost.

Players - Straightforward, number of players. Between 2 to 4, I still don't have a good AI :(

FenceLimit - Maximum amount of fences one tank can place simultaneously in milliseconds. Negative=Infinity.

FenceTimeLimit - The amount of time a fence stays on board before despawning in milliseconds. Negative=Infinity.

ShotLimit - Maximum amount of projectiles one tank can shoot simultaneously in milliseconds. Negative=Infinity.

ShotTimeLimit - The amount of time a projectile stays on board before despawning in milliseconds. Negative=Infinity.

EndingDelay - The amount of time to wait after one or zero tanks have remained on the board in milliseconds.

FreezeTime - The amount of time to have the game freeze at the winning frame in milliseconds. I believe too long of a time there can cause 'Not Responding' errors.

ScreenWidth and ScreenHeight - Used to set the screen width and height in pixels. Primary goal is to shrink the window for low resolution screens.

TankScale - Normally 2, it makes the tanks smaller or bigger. The precise size is 32*TankScale in pixels for one tank. Can also be fractions like 2.5.

PickupTime - How often, in milliseconds, does one pickup spawn. for instance, 5000 means one pickup every 5 seconds.

PickupDuration - How long, in milliseconds, does one pickup stay on board. (Does not apply for how long one power-up stays on the tank.)

SuddenDeathAfter - The amount of time, in seconds, until sudden death occurs. Negative=Infinity.

TankSpeed - Simply, the speed of the tank. I cannot really put my finger on how much that is, but you know, more is faster, less is slower. Can also be fractions.

BulletSpeed - The speed of the bullets. Usually make that greater than the tank's, or else they would just die if they shoot while moving forwards. Can also be fractions.

BlastRadius - The radius of explosion booms. Scaled 32 * BlastRadius. Can also be fractions.

SpeedBoostFactor - How faster does a speed boost make the tank. For instance, 2 is 2x faster. Can also be fractions.

Player1Keys,Player2Keys etc. - Keys to set. 6 keys, separated by commas, arranged: Up,Left,Right,Down,Place Fence,Shoot.
You probably want to take a look at the Microsoft XNA Keys Enum to put your non-letter keys at this link : http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx
You can also leave these if you like them.

Player1Color, Player2Color etc. - Sets each player's color. However, these can only be 8 colors: Red, Green, Blue, Aqua, Purple, Pink, Yellow and Orange.


[POWER UPS]

We got there! Here is the list of power-ups.


I'll start from the instants:

Deflector - Looks like a green 'recycle' sign.
When Used, turns all bullets 180 degrees, so if a tank shot them and didn't move, it will come back to their face.

Disabler - Looks like a red X.
When used, it removes everyone's power-ups, instant power-ups, pending projectiles, all pickups on board and all projectiles on board.

Roulette - A half black, half white circle.
This one is very useful, but sometimes dangerous.
When used, it picks a random playing tank, dead or alive, and changes its state in the game.
If it's alive, it dies.
If it's dead, it gets revived.
It can be used even after dying and includes you, so it gives you a chance of coming back to life, or if you are already alive,
a chance of dying.

Switcher - Looks like two black arrows going one up, one down.
This is my favourite.
When used, it causes you to switch places with the living tank closest to you.
However, it looks as if you just switched colors.
You get the exact same position, the same rotation, and even the same power-up as the closest living tank.
So yes, if the closest living tank happens to have a power-up, you steal it.
However if you have a power-up, that tank will steal it too.
AND since it can be used even after being dead, yes, you can switch places with a living tank and kill it in the process, taking its life.
Nice.


Now, we're moving to the timed:

Minimize - Looks like a small square surrounded by four arrows pointing at its corners.
Oh, it is great.
It makes you 3x smaller, so you can avoid bullets easily!
And it also makes your fences tiny!
Lasts for 10 seconds.

Speed Boost - Looks like a fast-forward button, with one arrow being red and the other being yellow.
Speeds you up according to the factor written in the settings, and also makes your turns sharper.
Lasts for 5 seconds.

Tripler - Looks like an arrow splitting into 3 arrows.
Makes you shoot 3 bullets at a time!
And they don't even count towards you maximum!

Force Field - Looks like a light blue circle.
Gives you a light blue aura, and causes you to be invincible.
BUT if a normal bullet hits you, it bounces off of you and back to the sender!
However, if you drive into a bullet, it will get stuck inside you since it will be going forwards-backwards-forwards-bacwards etc.
Lasts for 10 seconds.

Extra Life - Looks like a heart.
Gives you a spinning red halo, that gives you another life!
If a normal bullet hits you, it gets deflected off.
When you are supposed to be killed, even by, say, roulette, you just lose that halo and stay alive!
Lasts indefinitely, until you are killed.
(So that power-up is not even timed!)

Lock - Looks like a freaking lock.
It causes a random tank (that's not you) to not be able to move, shoot bullets or place fences.
Lasts for 3 seconds, and gives that power-up to the random tank - removes its current power-up if it has one.

Ghost - Looks like 3 black strips of bacon (I can't draw).
You can pass through fences and bullets. (They won't kill you.)
Lasts for 10 seconds.

Concealer - Looks like a question mark inside a white square - reminds the question mark box from Super Mario Bros.
This one is a little complicated.
For 10 seconds, you get the keys and color of a random living tank, so you kind of become a clone of that tank.
But the cool part about it, is that it either switches the tanks, or not.
So when one of the two tanks dies, it is impossible to tell whether is was you or the original tank.
During those 10 seconds you lose control of your tank, but with a 50% increased chance of survival.
It's like an insurance.
Lasts for 10 seconds.


Now we get to the appearing power-ups!
The appearing power-ups will seem unique and weird, because they pretty much are.

Portal - Looks like a freaking portal. Even animated.
You come in one, you go out the other.
Bullets too.
However, since I suck, It always teleports you to where you would go if you went forward, so if you go in backwards, you rapidly switch between portals.
Will be fixed when I will care enough.

Accelerator - Looks like the speed boost, except vertical and in a white box.
Will give you a quick boost - 20x your speed for 0.1 seconds. 
Yep, it is stupid.

Black Hole - Looks like a circle with black and white stripes coming out of its center. (You will know it is one when you see one.)
This one's awesome.
It sucks in all projectiles on board then explodes them out.
It only appears when there are projectiles on board.
It overloads at 50 bullets to keep players from constantly shooting bullets into it for hours.


[PROJECTILES]

You can pick up some projectiles, then they will be the next thing you will shoot.

Extreme bullet - Looks like a big black oval.
DESTROYS everything in its path, but despawns when it hits a wall.
I recommend shooting it while going backwards.

Homing bullet - Like the normal bullet but red.
Homes at the closest tank to it (EACH TICK - can change tanks if another tank gets closer).
Starts homing after 2 seconds.

Missile - A freaking missile.
Explodes when should despawn, or a 10% chance that it explodes in your face!
Explosions will be as big as the BlastRadius setting.
All tanks going into the explosion will be killed, with the exception of force fields and ghosts.


[SUDDEN DEATHS]

Do not read this list if you want to be surprised!


Explosion Death - Everyone go 'BOOM!'

Glitchy Death/Awesome Death - 
This one is weird. The tanks get to see their trails (like a HOM),
then random awesome faces appear, and after 10 seconds they rapidly fill the screen.
The tanks just die in the process for no logical reason.

Shrinking Death - Everyone get smaller until disappearing.

Holey Death (PUN TOO AWESOME) - A black hole sucks EVERYTHING into it.

Homing Death - 4 homing bullets come from the side.
This one can be dodged and if two tanks do, again, they just disappear after 20 seconds.

Possession Death - Everyone starts shooting uncontrollably and the board is filled with bullets.

Supernova Death - A black hole appears in the middle, then 4 stacks of bullets com from the corners and get sucked into the black hole, which causes an explosion of 360 bullets that create a circle that kills everyone.
Seriously. No escape, unless you have the force field or ghost power-ups.


FINISHING NOTES:
In the project directory you will find a 'Fences' Folder.
It is pending and will be implemented in a later update.
I am new in GitHub, but i know C# for over 6 years and I really hope you enjoy this game, even though it's kinda complicated.
Thank you for taking the time to read this and hopefully play the game, and any comments will be appreciated.
Nitzan Bueno
