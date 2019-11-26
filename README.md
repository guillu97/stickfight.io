# stickfight.io
a stickfighter multiplayer io game

## The project and our analysis:
Our main idea is to product a game on Unity with a NodeJs server. It will be a web application. We will save the best scores on a MongoDB database.<br/>
The main point is first: to share your information with the server, to get the information from the server, to update the information locally according to the information you got from the server and the last point is to communicate with the database.<br/>
This is a 2D fighting game where one single attack kills you. The attacks are a sword and a shield. The player use the sword to attack and kill the other player, and the shield is here to block the other player’s attacks. If you stand still for at least one second you will be rewarded by death. A game is composed of 10 sessions in which one only have one life and will die when touched by an enemy or when you fall out of the scene.<br/>
	This game is also a platforming one : to avoid too much repetition between the different sessions we will create multiple parts with different mechanisms.<br/>
	When a game ends, all the players are sent to a page where the scores of the sessions are displayed. From there they are able to return to the main menu and to keep on playing. When the last player leaves, the room is deleted.
	**About the score** : many informations may be stored : the maximum amount of consecutive sessions victories, the number of kills in a single game or in multiple games.<br/>
	Last but not least the **main menu**: it will allow you to join or create a session, to access the best scores and to choose the shortcut keys’ configuration to control the player’s character.<br/>
<br/>
##	Vocabulary:<br/> 
**Session** : in a session, each player only has 1 life and try to kill the opponents. Once only character remains alive, the session stops and we move on to the next session.<br/>
**Game** : composed of 10 sessions.<br/>
**Room** : virtual place where players can play the games. They have a limited amount of player and ends after 1 game.<br/>
<br/>
## The main problems will be :<br/>
* to learn how to code on Unity => little training needed.<br/>
* How to move a character? How to animate a character (the player)? How to make an attack, with a collision check between the swords and the shields, and an animation?<br/>
* what information will we need to share ?<br/>
* how will we handle this information ?<br/>
* how will we handle the sessions/games ?<br/>
* how will we create the animations?<br/>
  <br/>
## Conception:
The main features are:<br/>
- create a scene with all needed items.<br/>
- be able to interact with you own character.<br/>
- share character states / game state with all the players through the network<br/>
- save the best scores in the database<br/>
- retrieve the information from the database.<br/>
- create the different attacks.<br/>
- create a player register system => register login/ password<br/>
- create a player sign in system<br/>
- keep track of number of games played => stats<br/>
- keep track of his wins over losses => stats<br/>
  <br/>
Unity provides a lot of tutorial for beginners, they should be enough. <br/>
For the animations we will take as much as possible the ones from the Unity Store.<br/>
We will need to share the positions (vertical movement and horizontal ones)<br/>
  <br/>
## Roadmap:
### 1st phase - initial prototype:
- **front end**:<br/>
  animate the character => movement and attacks<br/>
  do the attack collisions<br/>
  the player should be able to win or lose a game<br/>
- **back end**:<br/>
  share the player’s characters positions and actions<br/>
  the player should be able to win or lose a game (without bothering with sessions of games for now)<br/>
### 2nd phase:
- **front end**:<br/>
  add a menu to create / join rooms of player, and to access to the stats of the player (the stats menu at this point will be a fake)<br/>
- **back end**:<br/>
  multiple rooms to divide the players (i.e. 4 players play together in room 1, and we put the next connecting players connecting to a new room)<br/>
  register and login<br/>
### 3rd phase:
- **front end**:<br/>
  there should be multiple sessions in a game, the winner of the game is the player that wins the most of the sessions<br/>
  display the score<br/>
- **back end**:<br/>
  there should be multiple sessions in a game, the winner of the game is the player that wins the most of the sessions<br/>
  save the stats of the games (save the score)<br/>
### 4th phase:<br/>
- **front end**:<br/>
  get the best scores displayed in the stats<br/>
- **back end**:<br/>
  record and save the best scores<br/>
### 5th phase:<br/>
- **front end**:<br/>
  add to the menu a way to change the player shortcut keys<br/>
  roll through the different maps (i.e. for each 2 sessions a new map is displayed)<br/>
- **back end**:<br/>
  roll through the different maps(i.e. for each 2 sessions a new map is displayed)<br/>
### 6th phase :
- **front end**:<br/>
  add a recap of the score at the end of the game<br/>
# Initial prototype:
## Goals:
- [ ] Be able to connect multiple players to the server.<br/>
- [ ] Be able to register to the game using login/password<br/>
- [ ] Be able to connect to the game by login/password<br/>
- [ ] Be able to retrieve the best score from the database.<br/>
- [ ] Be able to update the best score.<br/>
- [ ] Be able to see the other players on the screen.<br/>
- [ ] Be able to see the other players actions.<br/>
- [ ] Be able to transmit your actions to the others players.<br/>
- [ ] Be able to change the shortcut keys to control the player’s character<br/>
- [ ] Have multiple maps ready.<br/>
- [ ] Be able to have multiple rooms of players at the same time and organize the repartition of players.<br/>
## Classic scenario :
The user logs in the web browser then join a room. When 4 players joined, the game starts. At the end of the 10 sessions, the 4 players <br/>
## Targeted result:<br/>
* Be able to connect multiple players to the server.<br/>
* Be able to register to the game using login/password<br/>
* Be able to connect to the game by login/password<br/>
* Be able to retrieve the best score from the database.<br/>
* Be able to update the best score.<br/>
* Be able to see the other players on the screen.<br/>
* Be able to see the other players actions.<br/>
* Be able to transmit your actions to the others players.<br/>
* Be able to change the shortcut keys to control the player’s character<br/>
* Have multiple maps ready.<br/>
* Be able to have multiple rooms of players at the same time and organize the repartition of players.<br/>
