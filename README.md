# amWRit

# Ed-dy Run

As part of learning through __Harvard's CS50G "Introduction to Game Development"__ course, I created this __Ed-dy Run__ game using Unity and Blender.

__Unity 2020.2.0b7__
__Blender 2.90.1__

# Background
I am from a CS background but I currently work in Education and teach programming to middle grade students in public schools of Nepal. Being from an Ed-Tech sector, the project idea hanging in my mind from the initial days (when I started the course) was that I wanted to make a game for small kids that teaches them some subject ideas or concepts. 

I didn't want it to very complex such that it takes a lot of time to code an develop the assets. I wanted it to be simple but complete enough to learn some subject matter from the game.

My initial was a Math runner kind of game. Taking the idea of a runner game (like Subway Surfer), I wanted the game to be similar where the player runs through an environment and picks up numbers. There would be different arithmetic operations (Add, subtract, multiplication and division). A target would be given. The player has to pick numbers and the operation symbols so as to reach that target. 

Later as I was close to finishing the Math runner game, I thought of adding English too. So, I changed the name of the game to __Ed-dy Run__. Here _Ed_ in Ed-dy stands for __Education__. So it's an education game for kids. I also intentionally used the female character so as to attract more girls into gaming. 

In the English option, the gameplay is similar. The player runs and picks up alphabets. A target is given, which is a simple word. The player has to pick exact alphabets and build up that word like in a scrabble. 

I think it is distinct from the projects that we had done in the course in the sense that it is a scrolling game but from a First person perspective. Also the fact that it is a game directed towards educationg kids making it unique. 

The complexity of the game lies in the level generation and spawn management where the numbers/operator symbols/alphabets are spawned based on the level. 

# How to Play
Player can use __LEFT/RIGHT arrow keys__ to move left and right to avoid the obstacle or pick up numbers/operator symbols/alphabets.

A target is given. And the player has to reach that target by picking up required numbers/operator symbols/alphabets. 
The player scores when the target is met. Also the speed increases after reaching a certain score limit.
The player dies if it hits with an obstacle and the game is over.

After a certain score, the level increases and the game play becomes difficult by the random spawning of required numbers/operator symbols/alphabets.

__Saver cheat keys__
Player can hit C key to clear the current result.
Player can hit SPACE key to jump in extreme conditions.

# Game Play
## __Start State__
	When the game starts, two game options are shown to the player: MATHS and ENGLISH.

## __Play State__
	There are two options of the play state.
	- ### __MATHS__
		A target number is given. For example: 12 
		The numbers are spawned randomly. The player has to use the arithmetic operator and numbers to reach that target.

		There are 4 levels.
		_Level 1_
		Only ADD operator is spawned. So the player has to use numbers in such a way that they add up to the given target.

		_Level 2_
		ADD and SUBTRACT operators are spawned. The player has to think and pick numbers and operators such that the result is equal to the given target.

		_Level 3_
		ADD, SUBTRACT and MULTIPLY operators are spawned. The player has to think and pick numbers and operators such that the result is equal to the given target.

		_Level 4_
		ADD, SUBTRACT, MULTIPLY and DIVIDE operators are spawned. The player has to think and pick numbers and operators such that the result is equal to the given target.


	- ### __ENGLISH__
		A target word is given. Currently there are 26 words, one each from A through Z. Let's say the target word is DOG. The goal for the player is to pick up alphabets such that they become the target word.

		There are 3 levels.
		_Level 1_
		This is a very guided level. In this level, only the alphabets that are in the target word are spawned. So for DOG, first only D is spawned. When the player picks D, then O is spawned. And then G is spawned. 

		_Level 2_
		This is a semi guided level. In this level, similar to level 1, only the alphabets that are in the target word are spawned. But they are randomly spawned. So only D, O and G are spawned but randomly. 

		_Level 3_
		This is a free level. The alphabets are randomly spawned.


	- ### SCORING
		Score increases when the Maths number result or English word result is equal to the target given. 
		After certain score limit, the speed of the game increases. Also the level increases.

	- ### CLEARING RESULT
		The player can press C key to clear the current result and start fresh. 

## __Game Over State__
	- The game is over when the player hits with an obstacle on the road and dies. 
	- The player can use the _restart button_ to restart the game and play again.

# Assets Used
The environment assets (road, skydome, mountainbox) and character were downloaded from Unity's official __Create with Code__ tutorials, which were provided for free to learn.

The numbers, operations and alphabet models were modeled using [TinkerCAD](https://www.tinkercad.com "TinkerCAD").

# Cool Effects Used
## Animation
	- running animation
	- jump animation when space key pressed
	- death animation when player collides with an obstacle

## Particle effect
	- dirt particle effect below the leg when the player is running
	- explosion effect when player collides with an obstacle

## Sound effect
	- background music continuosly player
	- sound effects when player picks up numbers/alphabets/operators or collides with obstacle

# Future Features
- Player health
- More subject options like Science
- Different levels for Maths and English like Grammar, storymaking etc.
- Choice of Character
- Different environment based on level
