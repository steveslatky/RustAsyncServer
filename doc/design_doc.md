---
title: Sunset Checkers Design Document
author:
- Jennifer Bondarchuk
- Stephen Slatky
- Khanh Nguyen
- Peter Gagliardi
geometry: margin=1in
numbersections: true
documentclass: article
---
<!--
    Note: the above is just metadata to control how Pandoc renders
    the output.
-->

<!-- TODO: Steve volunteered to do Intro and Overview -->


# Introduction

## Purpose

The purpose of this document is to explain in detail the implementation of Sunset Checkers to fill the requirements set in the Sunset Checkers Requirement document. Sunset Checkers is designed to be a game of checkers between two players that follows the rules set by usacheckers.com.

## Scope

This document describes the implementation details of the Sunset Checkers software. The software will consist of two systems that are split between namespaces. The two major namespaces are client and AsyncServer. Code in the client namespace is intended to be run on client machines through the Unity game engine. Code in the AsyncServer namespace is intended to be run on the server machine (computer). This document will not specify the testing of the software.


## Definitions

UID - Unique Identification Number.

Mono - Cross platform C# compiler.

Ply - Since "turn" can be a confusing term when talking about board games, we
    opt for the game theory term of ply. A ply is a turn taken by a single
    player (user or opponent).

# System Overview

<!-- TODO: overview diagram should probably go here -->

## Description

Sunset Checkers is designed to be a game of checkers with two players. Each player will try and capture all the opponents pieces. Players will be able to control their pieces through click events through the client.

## Software

Sunset Checkers will use a computer as input devices. Windows 10 and macOS will be supported by the client. The client will communicate with the server over  the Internet. More than one user is needed to play a game of checkers. The development environment is Unity 3D game Engine 5.6.2 to create the client and Mono with a text editor for the server. 

# System Architecture

## Server Architectural Design Components

### Rooms

When 2 players connect to the server they get assigned to a room. The rooms group opponents together so the server is able to send moves from one client to their opponent to keep both player's checkerboards up to date.

### Pools

There are 2 lists of users connected to the server. 1 pool keeps track of each individual user connected and the other keeps track of all the rooms that have been created. This keeps the connections to the server organized and accessible to other methods.

## Client

| System Architecture |
| ----------------- |
|![System Architecture](images/design/systemarchitecture.png)|

### Client Program

The client takes the form of a Unity Engine game.

### Menu Subsystem

The Menu Subsystem is a very simple setup of multiple Unity scenes that will use Unity's
`ScreenManager` class in order to transition between screens. A `GameStatus` class will
control the date flow between the menu system and Game Screen subsystems.

### Game Status

The `GameStatus` is a class that will utilize `EventManager` to communicate with the server
in order to set up and store game session data such as opponent setup and game results.

### Game Screen Subsystem

The Game Screen is the most complicated menu in the client; It was designed
as a subsystem of its own. The Game Screen consists of an on-screen checkerboard
and pieces (`GUICheckerboard` and `GUIPiece`, respectively). The user interacts
with the screen using the mouse. Everything else is event driven using
`EventManager`.

### Server Relay

The `ServerRelay` is a class that runs asynchronously, outside thee menu
subsystem. Its sole purpose is to allow communications between the
server and the `EventManager`. The `EventManager` can then propagate
messages to the rest of the program.

### Event Manager

The `EventManager` is another singleton class that runs asynchronously. It
allows other classes to subscribe to events with callbacks. It also lets
any class publish events with data to whatever classes are subscribed to the
event.

`EventManager` is used extensively in the Game Screen. it is used for
communication between the `GUICheckerboard` and all the `GUIPieces`, and it
is also used to communicate with the server

Some of the menus also use the `EventManager` to communicate with the server

# Component Design


## Server

### Backend Design

||
| ----------------- |
|![Server UML ](images/serverUml.png)|

### Server Design
||
| ----------------- |
|![Server UML ](images/ServerDetailUML.png){ width=250px }|

### Server Methods
||
| ----------------- |
|![Server Methods ](images/serverUMLDescriptionTables.png)|
|![Server Methods Cont. ](images/serverUMLDescriptionTables2.png)|

A Server contains all the methods necessary to receive and send data between 2 clients.

### StateObject Design
|  |
| ----------------- |
|![StateObject UML ](images/StateObjectUML.png){ width=200px }|

A StateObject contains attributes for each client such as the socket the client is using, the size of the buffer for receiving data from the client, the receive buffer itself, and the string to store received data.

### Room Design
|  |
| ----------------- |
|![Room UML ](images/roomUML.png){ width=400px }|

### Room Methods
| |
| ----------------- |
|![Room Methods ](images/roomUMLDescription.png)|

A room contains 2 clients that are playing each other in a game of checkers.

### UniqueSocket Design
|  |
| ----------------- |
|![UniqueSocket UML ](images/UniqueSocketUML.png){ width=300px }|

### UniqueSocket Methods
| |
| ----------------- |
|![UniqueSocket UML Description ](images/UniqueSocketUMLDescription.png)|

A UniqueSocket stores a client's socket object with their UID.

### RoomPool Design
|  |
| ----------------- |
|![RoomPool UML ](images/RoomPool.png){ width=200px }|

### RoomPool Methods
| |
| ----------------- |
|![RoomPool UML Description](images/RoomPoolUMLDescription.png)|

A RoomPool is a list of the rooms that are currently in use. Rooms are removed once 2 players finish a game of checkers.

### UserPool Design
|  |
| ----------------- |
|![UserPool UML Description](images/UserPoolUML.png){ width=300px }|

### UserPool Methods
|  |
| ----------------- |
|![UserPool Methods](images/UserPoolMethods.png)|

A UserPool is a list of users that are currently connected to the server.

### Move Design
|  |
| ----------------- |
|![Move UML](images/Move.png){ width=200 }|

A Move is used to store necessary data from clients. It contains the client who sent the Move's UID, the client's room UID (r_UID) as well and the source (src) and destination (dest) of the checker piece the client moved.

## Client


| Frontend Design |
| ----------------- |
|![Frontend Design](images/design/client_uml2.png)|

Above: This is an overview of the classes used in the client.

### GameStatus

| Game Status |
| ----------------- |
|![GameStatus](images/design/UML/boxes/gamestatus.png){ width=300px }|

The `GameStatus` class is used as a controller outside of the game screen for
setting up/ending games and transitioning from screen to screen. Uses Unity's
`ScreenManager` class.

| Game Status Design Descriptions|
| ----------------- |
|![GameStatus](images/design/UML/gamestatus_table.png)|

### Checkerboard

| Checkerboard |
| ----------------- |
|![Checkerboard](images/design/UML/boxes/checkerboard.png){ width=300px }|

The Checkerboard is the in-memory representation of the gameboard (not the
checkerboard displayed on the screen, for that see `GUICheckerboard`). This
class is used for calculating valid moves for each player.

| Checkerboard Design Descriptions|
| ----------------- |
|![Checkerboard](images/design/UML/Checkerboard_table_1.png)|
| ----------------- |
|![Checkerboard](images/design/UML/Checkerboard_table_2.png)|
| ----------------- |
|![Checkerboard](images/design/UML/Checkerboard_table_3.png)|

### CheckerCoords

| CheckerCoords |
| ----------------- |
|![CheckCoords](images/design/UML/boxes/checkercoords.png){ width=300px }|

`CheckerCoords` is a 2-component vector in `(row, col)` format where each
component is an integer. It is used to calculate locations on the checkerboard.

| CheckerCoords Design Descriptions|
| ----------------- |
|![CheckerCoords Design Descriptions](images/design/UML/CheckerCoords_table_1.png)|
| ----------------- |
|![CheckerCoords Design Descriptions](images/design/UML/CheckerCoords_table_2.png)|
| ----------------- |
|![CheckerCoords Design Descriptions](images/design/UML/CheckerCoords_table_3.png)|

### Move

| Move |
| ----------------- |
|![Move](images/design/UML/boxes/move.png){ width=300px }|

a `Move` is a pair of CheckerCoords `(src, dst)` that represents a move on
the checkerboard. No move type is stored, since it can be easily calculated
from how far apart the two locations are on the checkerboard.

| Move Design Descriptions|
| ----------------- |
|![Move Design Descriptions](images/design/UML/Move_table_1.png)|
| ----------------- |
|![Move Design Descriptions](images/design/UML/Move_table_2.png)|
| ----------------- |
|![Move Design Descriptions](images/design/UML/Move_table_3.png)|

### Piece

| Piece |
| ----------------- |
|![Piece](images/design/UML/boxes/piece.png){ width=300px }|

A `Piece` is a simple data structure that represents a playing piece. It keeps
track of which player owns the piece and if the piece is a king or not.

| Piece Design Descriptions|
| ----------------- |
|![Piece](images/design/UML/Piece_table.png)|

### Event Manager

| EventManager |
| ----------------- |
|![EventManager](images/design/UML/boxes/eventmanager.png){ width=300px }|

The `EventManager` is a singleton instance that handles sending custom
messages between GUI elements. Events are described in detail later
in the document.

| EventManager Design Descriptions|
| ----------------- |
|![EventManager](images/design/UML/EventManager_table.png)|

### GUI Checkerboard

| GUI Checkerboard |
| ----------------- |
|![GUI Checkerboard](images/design/gui_checkerboard.png){ width=300px }|

The `GUICheckerboard` is the checkerboard on the screen (minus the pieces
and move square). It keeps track of game state through a `Checkerboard`
instance and is responsible for creating the `GUIPiece`s and move squares.
Everything else is handled in an event-driven fashion. 

| GUI Checkerboard Design Descriptions|
| ----------------- |
|![GUI Checkerboard Design](images/design/gui_checkerboard_table.png)|

**NOTE:** This does not include the `EventManager` event callbacks for this
class. These events are detailed in a later section of the document.

### GUI Piece

| GUI Piece |
| ----------------- |
|![GUI Piece](images/design/gui_piece.png){ width=300px }|

A `GUIPiece` is a class that represents one checkers piece on the screen.
It has a sprite that changes depending on whether the piece is selected or is
a king. It responds to click events, but all other functionality happens in
`EventManager` event callbacks.
 
| GUI Piece Design Descriptions|
| ----------------- |
|![GUI Piece Design](images/design/gui_piece_table.png)|

**NOTE:** This does not include the `EventManager` event callbacks for this
class. These events are detailed in a later section of the document.

### GUI Basic Move Square

| GUI Basic Move Square |
| ----------------- |
|![GUI Checkerboard](images/design/gui_basic_move_square.png){ width=300px }|

A basic move square is a square on the screen that lets the user select
a valid basic move. When clicked, it publishes a `basic_move` event that
leads to the move being performed in memory and on the screen.

| GUI Basic Move Square Descriptions|
| ----------------- |
|![GUI Piece Design](images/design/gui_basic_move_table.png)|
**NOTE:** This does not include the `EventManager` event callbacks for this
class. These events are detailed in a later section of the document.

### GUI Capture Move Square

| GUI Capture Move Square |
| ----------------- |
|![GUI Capture Move Square](images/design/gui_capture_move_square.png){ width=300px }|

A capture move square is a square on the screen that lets the user select
a valid capture move. When clicked, it publishes a `capture_move` event that
leads to the move being performed in memory and on thee screeen.

| GUI Capture Move Square Descriptions|
| ----------------- |
|![GUI Piece Design](images/design/gui_capture_move_table.png)|

**NOTE:** This does not include the `EventManager` event callbacks for this
class. These events are detailed in a later section of the document.

### Server Relay

| Server Relay |
| ----------------- |
|![ServerRelay](images/design/UML/boxes/serverrelay.png){ width=300px }|

The `ServerRelay` is a component that handles communication between the client
and server in a asynchronous fashion. It has methods to communicate with the
server. It also interacts with the `EventManager` to propagate events
to/from the rest of the client.

`ServerRelay` is described in more depth later in the document.

| Server Relay Design Descriptions |
| ----------------- |
|![Server Relay Table](images/design/UML/serverrelay_table.png)|


**NOTE:** This does not include the `EventManager` event callbacks for this
class. These events are detailed in a later section of the document.

### Enumeration Types

Here are the enumeration types used throughout the client:

* `MoveType`
    * `Basic` - a basic move
    * `Capture` - a capture move
* `PlayerType`
    * `North` - the player on the north side of the board (Player 1)
    * `South` - the player on the south side of the board (Player 2)
* `PieceState`
    * `NonSelectable` - This `GUIPiece` can not be selected by the user
    * `Selectable` - The user can select this `GUIPiece`.
    * `Selected` - The user selected this `GUIPiece` to move it

# Event Manager Subsystem

The game screen's GUI has many moving parts (checkerboard, game
pieces, clickable move squares). We decided to use the Mediator design pattern
to facilitate communication between objects. The `EventManager` serves as a
central messaging service that components use to send information to each other.

The messaging system works as follows:

1. When each `GameObject` is created, it calls
    `EventManager.instance.subscribe(event_name, callback)` to listen for
    events. The event name is a unique string that labels an event type. the
    callback is a `void` function that takes a single string as an argument.
    (this is explained below)
2. Whenever an object needs to send a message, it calls
    `EventManager.instance.publish(event_name, data)`. `event_name` is a string
    that labels the event (same as in `subscribe()`). The data can be anything
    that is serialized to a string.
3. Callbacks are immediately called. They accept a single string as data. The
   callback is responsible for parsing the message. Several data types shall
   have static methods to assist in serializing/deserializing the message.
4. When an object is destroyed, it must call
    `EventManager.instance.subscribe(event_name, callback)` to unsubscribe
    from the event.

## Event Overview

### Checkerboard State Component

| Checkerboard State Component |
| ----------------- |
|![Checkerboard State Component](images/design/checkerboard_component.png)|

### Piece State Component

| Piece State Component |
| ----------------- |
|![Piece State Component](images/design/piece_component.png)|

### Move Square State Component

| Move Square State Component |
| ----------------- |
|![Move Square State Component](images/design/move_square_component.png)|

## Event Descriptions

### Connect Event

**Event Name:** `connect`  
**Triggered By:** `PlayButton` when clicked.  
**Data Format:** None.  
**Actions:**

* `ServerRelay` - send a connect message to the server

<!--TODO: Who handles transitioning to the loading screen? -->

### Start Game Event

**Event Name:** `start_game`  
**Triggered By:** `ServerRelay` when a start game message is sent from the
    server.  
**Data Format:** `<user_player>,<first_player>`  

* `user_player` - either `N` or `S`. This is the side of the board the user
  is playing as
* `first_player` - either `N` or `S`. This player goes first.

**Actions:**  

* `GUICheckerboard`
    * Create `Checkerboard` to keep track of game state
    * If the player is on the `North` side of the checkerboard, rotate
      the board so the player's pieces are on the bottom of the screen
      (closer to the user)
    * publish the `start_ply` event.

### Start Ply Event

**Event Name:** `start_ply`  
**Triggered By:** `GUICheckerboard` after `start_game` is finished.  
**Data Format:** None.  
**Actions:**

* `GUICheckerboard`
    * Get a list of valid moves for the current player.
    * If this list is empty, publish a `game_over` event, specifying the
      winner.
    * If it is the user's turn, publish a `enable_pieces` event with a list of
      all source locations from the moves
    * Otherwise, do nothing. Wait for an `opponent_basic_move` or
      `opponent_capture_move` event.

### Enable Pieces Event

**Event Name:** `enable_pieces`  
**Triggered By:** `GUICheckerboard` when starting a user ply.  
**Data Format:** `<loc>,<loc>,<loc>` - locations of the pieces that need
    to be selected.  
**Actions:**

* `GUIPiece`
  * If the piece's location is in the list of locations, set the state to
    `Selectable`

### Select Piece Event

**Event Name:** `select_piece`  
**Triggered By:** `GUIPiece` click event when state is `Selectable`  
**Data Format:** `<location>` - String with a square ID (number from 1-32).
    this represents the location of the selected piece.  
**Actions:**

* `GUICheckerboard`
    * Get a list of valid moves for the selected piece
    * Create a `GUIBasicMoveSquare`/`GUICaptureMoveSquare` for each move
* `GUIPiece`
    * If the piece is the selected piece, set the state to `Selected`
    * otherwise, set the state to `NonSelectable`

### Deselect Piece Event

**Event Name:** `deselect_piece`  
**Triggered By:**

* `GUIPiece` - click event when state is not `Selectable`
* `GUICheckerboard` - click event

**Data Format:** `<location>` - String with a square ID (number from 1-32).
    this represents the location of the selected piece.  
**Actions:**

* `GUICheckerboard`
    * If it is currently the user's turn, make pieces that have valid moves
      selectable again.
* `GUIBasicMoveSquare`
    * Destroy this square
* `GUICaptureMoveSquare`
    * Destroy this square
* `GUIPiece`
    * If this piece's state is `Selected`, change the state to `Selectable`

### Basic Move Event

**Event Name:** `basic_move`  
**Triggered By:** `GUIBasicMoveSquare` when clicked  
**Data Format:** `<move>` - basic move. Moves are of the form `<src>-<dst>`  
**Actions:**

* `GUICheckerboard`
    * Update the in-memory checkerboard
    * publish a `end_ply` event with the move
* `GUIBasicMoveSquare`
    * Destroy this square
* `GUICaptureMoveSquare`
    * Destroy this square
* `GUIPiece`
    * If this piece's location matches the move's source location:
        * Update the location to the move destination
        * Update the piece's position on the screen
        * Change the piece's state to `NonSelectable`

### Capture Move Event

**Event Name:** `capture_move`  
**Triggered By:** `GUICaptureMoveSquare` when clicked.  
**Data Format:** `<move>[,<move>[,<move>[...]]]` - sequence of moves. Oldest
    on the left. Moves are of the form `<src>-<dst>` where both locations are
    numbers from 1-32. Each time this event is called before an `end_ply`,
    a new move is added to the list.  
**Actions:**

* `GUICheckerboard`
    * Update the in-memory checkerboard
    * Get a list of capture moves from the destination square.
        * If this list is empty, publish a `end_ply` event with the
          entire move sequence as data
        * Otherwise, Create `GUICaptureMoveSquares` for all the valid
          capture moves. Attach the old move sequence + the new move to
          each capture move.
* `GUIPiece`
    * decode only the last move (rightmost) from the move sequence.
    * If this piece's location matches the last move's source location:
        * Update the location to the move destination
        * Update the piece's position on the screen
    * If this piece is on the square in between the last move's source and
      destination square (i.e. the enemy that was captured), destroy the piece
* `GUIBasicMoveSquare`
    * Destroy this square
* `GUICaptureMoveSquare`
    * Destroy this square

### Opponent Basic Move Event

**Event Name:** `opponent_basic_move`  
**Triggered By:** `ServerRelay` when it receives a basic move from the
    server.  
**Data Format:** `<move>` - single move. Moves are of the form `<src>-<dst>`
    where both locations are numbers from 1-32  
**Actions:**

* `GUICheckerboard`
    * Update the in-memory `Checkerboard`
    * publish a `end_ply` event with the move
* `GUIBasicMoveSquare`
    * Destroy this square
* `GUICaptureMoveSquare`
    * Destroy this square
* `GUIPiece`
    * If this piece's location matches the move's source location:
        * Update the location to the move destination
        * Update the piece's position on the screen

### Opponent Capture Move

**Event Name:** `opponent_capture_move`  
**Triggered By:** `ServerRelay` when it receives a capture move from the
    server.  
**Data Format:** `<move>[,<move>[,<move>[...]]]` - sequence of moves. Oldest
    on the left. Moves are of the form `<src>-<dst>` where both locations are
    numbers from 1-32. Each time this event is called, the oldest move event
    is removed.  
**Actions:**

* `GUICheckerboard`
    * Update the in-memory `Checkerboard` with the first move in the move
      sequence.
    * If the move sequence is of length one, publish an `end_ply` event
    * Otherwise, publish a `opponent_capture_move` event with
      the sequence `move2, move3, ... moveN` (i.e. all moves except the first).
* `GUIPiece`
    * decode only the first move (leftmost) from the move sequence.
    * If this piece's location matches the first move's source location:
        * Update the location to the move's destination
        * Update the piece's position on the screen
    * If this piece is on the square in between the first move's source and
      destination square (i.e. the enemy that was captured), destroy the piece

### End Ply Event

**Event Name:** `end_ply`  
**Triggered By:** `GUICheckerboard` after finishing a basic/capture move
    entered by the user.  
**Data Format:** `<move>[,<move>[,<move>[...]]]` - sequence of moves. Oldest
    on the left. Moves are of the form `<src>-<dst>` where both locations are
    numbers from 1-32  
**Actions:**

* `GUICheckerboard`
    * If the current player is the user, publish a `send_moves` event
    * Switch the current player
    * publish a `start_ply` event

### Send Moves Event

**Event Name:** `send_moves`  
**Triggered By:** `GUICheckerboard` in the end ply event.  
**Data Format:** `<move>[,<move>[,<move>[...]]]` - sequence of moves. Oldest
    on the left. Moves are of the form `<src>-<dst>` where both locations are
    numbers from 1-32  
**Actions:**

* `ServerRelay`
    * send the move list to the server.

### Send Forfeit Event

**Event Name:** `send_forfeit`  
**Triggered By:** `ForfeitButton` when clicked.  
**Data Format:** None  
**Actions:**

* `ServerRelay`
    * send a forfeit message to the server

### Game Over Event

**Event Name:** `game_over`  
**Triggered By:**
    * `GUICheckerboard` - when a player starts a turn with no available moves.
    * `ForfeitButton` - when clicked (this means the user lost)
    * `ServerRelay` The server can send a forfeit or timeout message.
**Data Format:** `<lose | win>,<game_over | timeout | forfeit>` - whether the
    user won or lost the game and the reason for the game over.  
**Actions:**

* `GUICheckerboard` - Transition to the Game Over Screen and display a message
    on whether the user won or lost.

# Server Relay Subsystem 

## Overview

The client program uses the `ServerRelay` in tandem with the `EventManager` to
communicate with the server. Below is a description of the 
server/`ServerRelay`/`EventManager` interactions used in the client.

| Server and `ServerRelay` Sequence Diagram |
|-------------------------------------------|
|![Flow Diagram](images/design/UML/flow_diagram.png)|

The `ServerRelay` is responsible for communicating with the server. When it
receives data from the server, it publishes `EventManager` events to the rest
of the game objects. It also subscribes to certain events; on these events it
reformats the data and sends it to the server.

## Receive Events

Below is a list of messages from the server and the corresponding `EventManager`
events that `ServerRelay` shall publish in response. The words in square
brackets are data attached to the message/event

| Message From Server                    | `EventManager` Event Published         |
|----------------------------------------|----------------------------------------|
| `start_game[user_player,first_player]` | `start_game[user_player,first_player]` |
| `timeout[north|south]`                 | `game_over[win|lose,timeout]`          |
| `moves[move_seq]`                      | `opponent_basic_move[move]` or `opponent_capture_move[move_seq]` |
| `forfeit`                              | `game_over[win,forfeit]`               |

## Send Events

Below is a list of `EventManger` events that require sending data to the server.
The words in square brackets are data attached  to the message/event

| `EventManger` Event    | Message To Send   |
|------------------------|-------------------|
| `connect`              | `connect`         |
| `send_moves[move_seq]` | `moves[move_seq]` |
| `send_forfeit`         | `forfeit`         |

# UI Design

<!-- TODO: Khanh -->

## Overview of User Interface
Additional screenflow and screenshots are specified in the the Requirements Specification. Each screen will be implemented in a separate Unity scene with transitions in between. 

## Screen Objects and Actions
The intended user input method for the game will be a mouse.

## Client Menu Flow

| Client Menu Flow |
| ----------------- |
|![Client Menu Flow](images/diagram.png)|

