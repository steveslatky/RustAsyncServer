---  
title: Sunset Checkers Requirements  
author:  
- Khanh Nguyen  
- Stephen Slatky  
- Jennifer Bondarchuk  
- Peter Gagliardi  
geometry: margin=1in  
numbersections: true  
documentclass: article  
---  
<!--  
    Note: the above is just metadata to control how Pandoc renders  
    the output.  
-->  
  
# Introduction  
  
## Purpose  
  
The purpose of this document is to explain in detail the testing to be done on Sunset Checkers in order to verify the implementation of features specified in the Sunset Checkers Design and Requirements documents. Sunset Checkers is designed to be a game of checkers between two players that follows the rules set by usacheckers.com.  
  
## Definitions, Acronyms, and Abbreviations  

Ply - Since "turn" can be a confusing term when talking about board games, we
    opt for the game theory term of ply. A ply is a turn taken by a single
    player (user or opponent).
  
# Testing Environments  
  
## Mac  

**Machine Name:** MacBook Pro (Mid 2015)  
**OS/Version:** macOS Sierra (10.12.6); 16GB RAM; 256GB SSD  
**Tester Name:** Jen Bondarchuk  
**Compiler (Server):** mcs version 5.0.1.1  
**Unity Engine Version (Client):** 5.6.2f1  
**Client Server/Back-End:** MacBook Pro  
**Test Date:** 8/17/17  
**State:**    
  
## Windows  
  
**Machine Name:** Lenovo Z50  
**OS/Version:** Windows 10 Home (10.0.15063); 8GB RAM; 1TB HDD  
**Tester Name:** Peter Gagliardi  
**Compiler (Server):** mcs version 5.0.1.1  
**Unity Engine Version (Client):** 5.6.2f1  
**Client Server/Back-End:** Lenovo Z50  
**Test Date:** 8/17/17  
**State:**  
  
# Setup Information and Prerequisites

Prior to running the program the following prerequisites must be met.

* The program is meant to run with a GUI. All test cases will be run through the GUI.
* An internet connection (broadband recommended) is required for optimum
performance of the program.
* The Server must be running first in order for the program to function.
  
# Test Cases  
  
## Client
  
**ID: C1**    
**Requirement 3.1.1.1:**    
**Description:**  Quit option shall terminate the program  
**Execution Steps:**    

1. Click "quit" option on main menu   

**Expected Result:**   
Game exits  
  
**Actual Result:**   
Game exits
  
**ID: C2**    
**Requirement 3.1.1.2:**    
**Description:**  X button shall terminate the program  
**Execution Steps:**    

1. Click "X" button on window  

**Expected Result:**   
Game exits  
  
**Actual Result:**  
Game exits 
  
**ID: C3**    
**Requirement 3.1.1.3:**    
**Description:**  About button shall transition to the about screen  
**Execution Steps:**    

1. Click "About"  

**Expected Result:**    
Show "about" screen  
  
**Actual Result:**  
Show "about" screen   
  
**ID: C4**    
**Requirement 3.1.1.4:**    
**Description:** Play button shall transition to the loading screen  
**Execution Steps:**    

1. Click "Play"  

**Expected Result:**    
Show loading screen that says "Finding opponent..." with a loading indicator (rotating sun)  
  
**Actual Result:**
Show loading screen that says "Finding opponent..." with a loading indicator (rotating sun)  
  
**ID: C5**    
**Requirement 3.1.2.1:**    
**Description:** About screen shall display the version number of the program  
**Execution Steps:**    

1. Click "about"  on the main menu 

**Expected Result:**    
Text on the screen that indicates version i.e. v. 1.0.0  
  
**Actual Result:**  
Text on the screen that indicates version i.e. v. 1.0.0   
  
**ID: C6**    
**Requirement 3.1.2.2:**    
**Description:**  About screen shall display release notes for current version of program  
**Execution Steps:**    

1. Click "about"  on the main menu   

**Expected Result:**    
Text on the screen that describes the changes in current release version of the game.  
  
**Actual Result:**  
Text on the screen that describes the changes in current release version of the game.   
  
**ID: C7**    
**Requirement 3.1.2.3:**    
**Description:**  Back button shall transition to the main menu  
**Execution Steps:**    

1. Click "about" on the main menu    
2. Click "back"   

**Expected Result:**    
Show main menu screen  
  
**Actual Result:** 
Show main menu screen    
  
**ID: C8**    
**Requirement 3.1.3.1:**    
**Description:**  When finished loading (connecting to opponent) loading screen will transition to game screen  
**Execution Steps:**    

1. Click "play" on the main menu  
2. Wait to successfully find an opponent. 

**Expected Result:**    
Loading screen will transition to game screen  
  
**Actual Result:**   
Loading screen will transition to game screen
  
**ID: C9**    
**Requirement 3.1.3.2:**    
**Description:** When unable to find an opponent (timeout), loading screen will transition back to main menu screen  
**Execution Steps:**    

1. Click "play" on the main menu  
2. Wait 3 minutes without finding an opponent.  

**Expected Result:**    
Loading screen will transition to main menu screen  
  
**Actual Result:**  
Doesn't time out 
  
**ID: C10**    
**Requirement 3.1.4.1:**    
**Description:**  Game over screen shall display game results    
**Execution Steps:**    

1. Click "play" on the main menu    
2. Wait to connect to opponent    
3. Play a full game of checkers    
  
**Expected Result:**    
Game over screen displays "you win" if player won, or "you lose" if the opponent won    
  
**Actual Result:**    
Game over screen displays "you win" if player won, or "you lose" if the opponent won 
  
**ID: C11**    
**Requirement 3.1.4.2:**    
**Description:**  New game option shall transition to loading screen    
**Execution Steps:**    

1. Click "New game"    

**Expected Result:**    
Show loading screen that says "Finding opponent..." with a loading indicator (rotating sun)   
  
**Actual Result:**   
Show loading screen that says "Finding opponent..." with a loading indicator (rotating sun)   
  
**ID: C12**    
**Requirement 3.1.4.3:**    
**Description:** Quit option shall terminate the program  
**Execution Steps:**    

1. Click "quit" option on game over screen  

**Expected Result:**    
Game exits  
  
**Actual Result:**  
Game exits
  
**ID: C13**    
**Requirement 3.1.5.1:**    
**Description:** The user shall use the mouse to control user pieces    
**Execution Steps:**    

1. Click "play" on the main menu    
2. Wait to connect to opponent    
3. On user turn, click a valid move piece    
4. Click a valid move destination   

**Expected Result:**    
The selected piece will move to the selected destination    
  
**Actual Result:**    
The selected piece will move to the selected destination
  
**ID: C14**    
**Requirement 3.1.5.1:**     
**Description:** The user shall not be able to control opponent pieces.   
**Execution Steps:**    

1. Click "play" on the main menu    
2. Wait to connect to opponent.   
3. On user turn, click opponent's move piece   

**Expected Result:**    
Nothing happens    
  
**Actual Result:**   
Nothing happens  
  
**ID: C15**    
**Requirement 3.1.5.2:**    
**Description:**  The program shall automatically move the opponent's pieces to reflect the proper state of the game  
**Execution Steps:**    

1. Click "play" on the main menu   
2. Wait to connect to opponent  
3. Wait for opponent ply to end    

**Expected Result:**    
Opponent's checker piece on user screen should reflect the move on their screen  
  
**Actual Result:**   
Opponent's checker piece on user screen should reflect the move on their screen
  
**ID: C16**    
**Requirement 3.1.5.3:**    
**Description:**  The user shall only be able to select valid moves  
**Execution Steps:**    

1. Click "play" on the main menu   
2. Wait to connect to opponent  
3. On user turn, click a valid move piece  
4. Click an invalid move destination (see Checkers Rules)  

**Expected Result:**    
Nothing happens.  
  
**Actual Result:**  
Nothing happens.

**ID: C17**  
**Requirement #: 3.1.5.4**  
**Description:** Forfeit Option shall transition to the Game Over screen  
**Execution Steps:**  

1. Enter a game with an opponent
2. Press the Forfeit button

**Expected Result:**  
The game should now display the Game Over Screen

**Actual Result:**  
The game should now display the Game Over Screen

**ID: C18**  
**Requirement #: 3.1.5.5**  
**Description:** The Forfeit option shall count as an automatic loss  
**Execution Steps:**  

1. Enter a game with an opponent
2. Press the Forfeit button

**Expected Result:**    
The Game Over screen should display "You Forfeit!"

**Actual Result:**  
The Game Over screen should display "You Forfeit!"

**ID: C19**  
**Requirement #: 3.1.5.6**  
**Description:** At the end of a game, the game shall transition to the Game
Over screen  
**Execution Steps:**  

1. Enter a game with an opponent
2. Play the game until one of the players runs out of valid moves.

**Expected Result:**  
The game shall now display the Game Over screen

**Actual Result:**  
The game shall now display the Game Over screen

**ID: C20**  
**Requirement #: 3.1.6.1**  
**Description:** Game pieces can move to empty, diagonally adjacent squares  
**Execution Steps:**  

1. Enter a game with an opponent
2. On the player's first turn, click a playing piece in the first row
3. Click an adjacent square

**Expected Result:**  
The piece should move to the adjacent square.

**Actual Result:**  
The piece should move to the adjacent square.

**ID: C21**  
**Requirement #: 3.1.6.2**  
**Description:**  Game pieces can jump over opponent pieces to capture them.    
**Execution Steps:**  

1. Enter a game with an opponent.
2. Have one player move a piece adjacent to the opponent's piece in such
   a way that the other player can perform a capture move.
3. Have the other player click the piece
4. Click the destination square that jumps over the first player's piece.

Example:
```
North Turn:
.   N      .   .
  .    -->   N    
S   .      S   .

South Turn:
.   .      .   S
  N    -->   .
S   .      .   .
```

**Expected Result:**  
The first player's piece should be captured (removed from the game) The
second player's piece should have jumped over the captured piece.

**Actual Result:**  
The first player's piece should be captured (removed from the game) The
second player's piece should have jumped over the captured piece.

**ID: C22**  
**Requirement #: 3.1.6.3**  
**Description:** Capture moves can be chained  
**Execution Steps:**  

1. Start a game with an opponent
2. Have one player move pieces into a pattern like the following:

```
Player 1 is North:
.   .
  N
.   .
  N
S   .
```
3. Have the other player make the chain of capture moves:

```
Player 2 is South:
.   .     .   .     S   .
  N         N         .
.   .  -> .   S  -> .   .
  N         .         .
S   .     .   .     .   .
```

**Expected Result:**  
The first player's pieces should be removed from the board. The second player's
piece should have jumped over each piece in succession

**Actual Result:**  
The first player's pieces should be removed from the board. The second player's
piece should have jumped over each piece in succession

**ID: C23**  
**Requirement #: 3.1.6.4**  
**Description:**  When one player runs out of moves, the game ends.  
**Execution Steps:**  

1. Start a game with an opponent
2. Play the game until one person has no moves remaining.

**Expected Result:**  
The game shall end; no further interaction with the board is possible.

**Actual Result:**  
The game shall end; no further interaction with the board is possible.

**ID: C24**  
**Requirement #: 3.1.6.5**  
**Description:** The player who runs out of moves first is the loser  
**Execution Steps:**  

1. Start a game with an opponent
2. Play the game until one player loses

**Expected Result:**  
On the loser's Game Over Screen, the text should read "You Lose!". On the other
player's screen, it should read "You Win!"

**Actual Result:**  
On the loser's Game Over Screen, the text should read "You Lose!". On the other
player's screen, it should read "You Win!"

**ID: C25**  
**Requirement #: 3.1.6.6**  
**Description:**  Basic Moves can only move forwards  
**Execution Steps:**  

1. Start a game with an opponent
2. Move a basic game piece forwards so that there are empty adjacent spaces
   behind the piece
3. On the next term, select that piece.

**Expected Result:**  
It should not be possible to select one of the squares that move backwards

**Actual Result:**  
It should not be possible to select one of the squares that move backwards

**ID: C26**  
**Requirement #: 3.1.6.7**  
**Description:**  Pieces that reach the 8th row become kings  
**Execution Steps:**  

1. Start a game with an opponent
2. Have one player move a piece to the 8th row (the row closest to the
   opponent)

**Expected Result:**  
The piece should now display a king logo

**Actual Result:**  
The piece should now display a king logo

**ID: C27**  
**Requirement #: 3.1.6.8**  
**Description:** Crowning moves end a turn.  
**Execution Steps:**  

1. Start a game with an opponent
2. Move a piece into the following pattern:

```
South to Move:
.   .   .     .   S   .
  N   S    ->   N   a    (a and b must be empty)
.   .   .     b   .   .
```
**Expected Result:**  

All of the following should be true:

* the piece becomes a king
* square `a` is not selectable
* square `b` is not selectable

**Actual Result:**  
All of the following are true:

* the piece becomes a king
* square `a` is not selectable
* square `b` is not selectable

**ID: C28**  
**Requirement #: 3.1.6.9**  
**Description:** Kings can move forwards and backwards  
**Execution Steps:**  

1. Start a game with an opponent
2. Move pieces into the following pattern:

```
South To Move:
.   .   .     a   .   .
  N   .         N   b
.   .   .  -> .   S   .
  N   S         N   c
.   .   .     d   .   .
```

**Expected Result:**  

All of the following should be true:

* Location `a` is selectable
* Location `b` is selectable
* Location `c` is selectable
* Location `d` is selectable

**Actual Result:**

All of the following are true:

* Location `a` is selectable
* Location `b` is selectable
* Location `c` is selectable
* Location `d` is selectable

## Server 

**ID:  S1**  
**Requirement #:3.2.1**  
**Description:** Match players to form a new game of checkers.  
**Execution Steps:**  

1. Have 2 clients open.
2. Start a game by clicking "Play".

**Expected Result:**  
Clients are connected and the users are brought to the game screen.

**Actual Result:**
Clients are connected and the users are brought to the game screen.

**ID: S2**  
**Requirement #:3.2.2**  
**Description:** Act as a communication channel between clients to send moves.  
**Execution Steps:**  

1. Make a move in game.

**Expected Result:**  
Move will show on opponents screen and the board will match on both user's clients.  

**Actual Result:**
Move will show on opponents screen and the board will match on both user's clients.

**ID: S3**  
**Requirement #3.2.3:**  
**Description:**  Keep track of time since the last move was received and declare a timeout if  
  time has surpassed 5 minutes.  
**Execution Steps:**  

1. Do nothing in game for 5 minutes.  

**Expected Result:**  
User disconnect and the opponent is shown as winner.

**Actual Result:**
Server does not track time

