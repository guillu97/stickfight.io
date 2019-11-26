# stickfight.io
a stickfighter multiplayer io game


The project and our analyse:
Our main idea is to product a game on Unity with a NodeJs server. It will be a web application. We will save the best scores on a MongoDB database.
	The main point is first: to share your information with the server, to get the information from the server, to update the information locally according to the information you got from the server and the last point is to communicate with the database.
	This is a 2D fighting game where one single attack kills you. The attacks are a sword and a shield. The player use the sword to attack and kill the other player, and the shield is here to block the other player’s attacks. If you stand still for at least one second you will be rewarded by death. A party is composed by 10 sessions in which one only have one life and will die when touched by an enemy or when you fall out of the scene.
	This game is also a platform one : to avoid too much repetition between the different sessions we will create multiple parts with different mechanisms.
	When a game ends, all the players are send to a page where the scores of the party are displayed. From there they are able to return to the main menu and to keep on playing. When the last player leaves, the room is deleted.
	About the score : many information may be stored : the maximum amount of consecutive sessions victories, the number of kills in a single game or in multiple games.
	Last but not least the main menu: it will allow you to join or create a session, to access the best scores and to choose the shortcut keys’ configuration to control the player’s character.

	Vocabulary:
session : in a session, each player only has 1 life and try to kill the opponents. Once only character remains alive, the session stops and we move on to the next session.
game : composed of 10 sessions.
room : virtual place where players can play the games. They have limited amount of player and ends after 1 game.

	The main problems will be :
to learn how to code on Unity => little training needed.
How to move a character? How to animate a character (the player)? How to make an attack, with a collision check between the swords and the shields, and an animation?
what information will we need to share ?
how will we handle this information ?
how will we handle the sessions/games ?
how will we create the animations?


Conception:
	The main features are:
create a scene with all needed items.
be able to interact with you own character.
share character states / game state with all the players through the network
save the best scores in the database
retrieve the information from the database.
create the different attacks.
create a player register system => register login/ password
create a player sign in system
keep track of number of games played => stats
keep track of his wins over losses => stats



	Unity provides a lot of tutorial for beginners, they should be enough. 
	For the animations we will take as much as possible the ones from the Unity Store.
	We will need to share the positions (vertical movement and horizontal ones)


Roadmap:
1st phase - initial prototype:
front end:
animate the character => movement and attacks
do the attack collisions
the player should be able to win or lose a game

back end:
share the player’s characters positions and actions
the player should be able to win or lose a game (without bothering with sessions of games for now)
2nd phase:
front end:
add a menu to create / join rooms of player, and to access to the stats of the player (the stats menu at this point will be a fake)

back end:
multiple rooms to divide the players (i.e. 4 players play together in room 1, and we put the next connecting players connecting to a new room)
register and login
3rd phase:
front end:
there should be multiple sessions in a game, the winner of the game is the player that wins the most of the sessions
display the score
back end:
there should be multiple sessions in a game, the winner of the game is the player that wins the most of the sessions
save the stats of the games (save the score)
4th phase:
front end:
get the best scores displayed in the stats
back end:
record and save the best scores
5th phase:
front end:
add to the menu a way to change the player shortcut keys
roll through the different maps (i.e. for each 2 sessions a new map is displayed)
back end:
roll through the different maps(i.e. for each 2 sessions a new map is displayed)
6th phase :
front end:
add a recap of the score at the end of the game


Initial prototype:
Objectifs:
Be able to connect multiple players to the server.
Be able to register to the game using login/password
Be able to connect to the game by login/password
Be able to retrieve the best score from the database.
Be able to update the best score.
Be able to see the other players on the screen.
Be able to see the other players actions.
Be able to transmit your actions to the others players.
Be able to change the shortcut keys to control the player’s character
Have multiple maps ready.
Be able to have multiple rooms of players at the same time and organize the repartition of players.

	Classic scenario :
	The user logs in the web browser then join a room. When 4 players joined, the game starts. At the end of the 10 sessions, the 4 players 


Targeted result:
Be able to connect multiple players to the server.
Be able to register to the game using login/password
Be able to connect to the game by login/password
Be able to retrieve the best score from the database.
Be able to update the best score.
Be able to see the other players on the screen.
Be able to see the other players actions.
Be able to transmit your actions to the others players.
Be able to change the shortcut keys to control the player’s character
Have multiple maps ready.
Be able to have multiple rooms of players at the same time and organize the repartition of players.
