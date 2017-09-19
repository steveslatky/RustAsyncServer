using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GUI checkerboard is the on-screen checkerboard. This class handles
/// most of the game logic.
/// </summary>
public class GUICheckerboard : MonoBehaviour {
    /// <summary>
    /// Z-coordinate of checkers in the scene
    /// </summary>
    public static float CHECKER_DEPTH = -1.0f;

    /// <summary>
    /// vector that represents the middle of the game board
    /// </summary>
    public static Vector3 CENTER = new Vector3(3.5f, -3.5f, 0f);

    /// <summary>
    /// flag to check if we are in a capture chain (and therefore
    /// deselect events should not be published).
    /// </summary>
    public static bool in_capture_chain = false;

    /// <summary>
    /// Prefab for a piece on the north side of the board.
    /// </summary>
    public GameObject north_prefab;

    /// <summary>
    /// Prefab for a piece on the south side of the board.
    /// </summary>
    public GameObject south_prefab;

    /// <summary>
    /// Prefab for the basic move squares.
    /// </summary>
    public GameObject basic_move_prefab;

    /// <summary>
    /// Prefab for capture move squares.
    /// </summary>
    public GameObject capture_move_prefab;

    /// <summary>
    /// Z-coordinate of clickable squares in the scene.
    /// </summary>
    private static float CLICK_SQUARE_DEPTH = -2.0f;

    /// <summary>
    /// The board.
    /// </summary>
    private Checkerboard board = new Checkerboard();

    /// <summary>
    /// This is the player we are playing as. This determines
    /// which side of the board is on the bottom.
    /// </summary>
    private PlayerType user_side;

    /// <summary>
    /// Which player's turn it is
    /// </summary>
    private PlayerType current_player;

    /// <summary>
    /// Place a GUI object onto the screen
    /// </summary>
    /// <param name="obj">The object to place</param>
    /// <param name="loc">Location to move to</param>
    /// <param name="layer">Z-depth of the object</param>
    public static void place_object(
        GameObject obj, CheckerCoords loc, float layer) {
        var pos = new Vector3(loc.col, -loc.row, layer);
        obj.transform.localPosition = pos - CENTER;
    }

    /// <summary>
    /// Subscribe to EventManager events
    /// </summary>
    public void OnEnable() {
        var manager = EventManager.instance;
        manager.subscribe("start_game", on_start_game);
        manager.subscribe("start_ply", on_start_ply);
        manager.subscribe("end_ply", on_end_ply);
        manager.subscribe("select_piece", on_select_piece);
        manager.subscribe("deselect_piece", on_deselect_piece);
        manager.subscribe("basic_move", on_basic_move);
        manager.subscribe("opponent_basic_move", on_basic_move);
        manager.subscribe("capture_move", on_capture_move);
        manager.subscribe("opponent_capture_move", on_opponent_capture_move);
    }

    /// <summary>
    /// Unsubscribe from EventManager events
    /// </summary>
    public void OnDisable() {
        var manager = EventManager.instance;
        manager.unsubscribe("start_game", on_start_game);
        manager.unsubscribe("start_ply", on_start_ply);
        manager.unsubscribe("end_ply", on_end_ply);
        manager.unsubscribe("select_piece", on_select_piece);
        manager.unsubscribe("deselect_piece", on_deselect_piece);
        manager.unsubscribe("basic_move", on_basic_move);
        manager.unsubscribe("opponent_basic_move", on_basic_move);
        manager.unsubscribe("capture_move", on_capture_move);
        manager.unsubscribe("opponent_capture_move", on_opponent_capture_move);
    }

    /// <summary>
    /// When the checkerboard is clicked and the user isn't in the middle
    /// of a capture chain, deselect the piece.
    /// </summary>
    public void OnMouseUp() {
        // Deselect pieces unless the user started a capture chain.
        if (!in_capture_chain)
            EventManager.instance.publish(
                "deselect_piece", EventManager.NO_DATA);
    }

    /// <summary>
    /// At the start of the game, set up the checkerboard and publish the start
    /// ply event for whichever player goes first
    /// </summary>
    /// <param name="setup_str">
    /// Setup string. (user side and current player separated by a comma)
    /// </param>
    private void on_start_game(string setup_str) {
        var parts = setup_str.Split(',');

        user_side = PlayerTypeUtils.deserialize(parts[0]);
        current_player = PlayerTypeUtils.deserialize(parts[1]);

        generate_board();
        rotate_board();

        // Forward the message for the turn indicator
        EventManager.instance.publish("start_ply", setup_str);
    }

    /// <summary>
    /// When a piece is selected, create basic/capture move squares
    /// so the user can select valid moves.
    /// </summary>
    /// <param name="src_loc_str">Source location as a string.</param>
    private void on_select_piece(string src_loc_str) {
        var src_loc = CheckerCoords.deserialize(src_loc_str);

        // Create move squares for the given piece.
        var moves = board.find_basic_moves(src_loc);
        foreach (var move in moves)
            make_basic_move_square(move);
        
        var capture_moves = board.find_capture_moves(src_loc);
        foreach (var move in capture_moves) {
            var move_seq_str = Move.serialize(move);
            make_capture_move_square(move_seq_str);
        }
    }

    /// <summary>
    /// When a piece is deselected, re-enable the user's pieces with
    /// valid moves so they can be clicked.
    /// </summary>
    /// <param name="no_data">No data.</param>
    private void on_deselect_piece(string no_data) {
        if (current_player == user_side)
            enable_pieces();
    }

    /// <summary>
    /// On a basic move, update the in-memory Checkerboard and end the
    /// player's turn.
    /// </summary>
    /// <param name="move_str">Move string.</param>
    private void on_basic_move(string move_str) {
        var move = Move.deserialize(move_str);
        board.perform_move(move);
        EventManager.instance.publish("end_ply", move_str);
    }

    /// <summary>
    /// On a capture move, update the board and check if it is possible to
    /// chain capture moves. If so, create new capture moves. If not,
    /// end the turn which sends the final move sequence to the server.
    /// </summary>
    /// <param name="move_seq_str">Move sequence string.</param>
    private void on_capture_move(string move_seq_str) {
        // Get the latest move
        var last_move = Move.deserialize_seq_last(move_seq_str);

        // Update the in-memory board
        board.perform_move(last_move);

        // We are in a capture move chain now. Deselect events will not work
        in_capture_chain = true;

        // check if the user can chain capture moves.
        var valid_moves = board.find_capture_moves(last_move.dst);

        // Stop if we do not have any further moves or the user
        // moved a piece to the king row.
        if (last_move.is_crowning || valid_moves.Count == 0)
            EventManager.instance.publish("end_ply", move_seq_str);
        else {
            foreach (Move next_move in valid_moves) {
                var new_seq = move_seq_str + "," + Move.serialize(next_move);
                make_capture_move_square(new_seq);
            }
        }
    }

    /// <summary>
    /// On an opponent capture move, process a single move and
    /// re-publish the event "recursively". at the end of the sequence,
    /// end the turn.
    /// </summary>
    /// <param name="move_seq_str">Move sequence string.</param>
    private void on_opponent_capture_move(string move_seq_str) {
        // Get all the moves
        var moves = Move.deserialize_seq(move_seq_str);

        var first_move = moves[0];
        board.perform_move(first_move);

        if (moves.Count == 1)
            EventManager.instance.publish("end_ply", move_seq_str);
        else {
            // "recursively" publish the remaining moves
            moves.RemoveAt(0);
            var remaining_moves = Move.serialize_seq(moves);
            EventManager.instance.publish(
                "opponent_capture_move", remaining_moves);
        }
    }

    /// <summary>
    /// On the start of the term, check if the player is out of moves
    /// (the game over condition), otherwise, get the board ready for
    /// one of the players to make a move.
    /// </summary>
    /// <param name="turn_status">Turn status.</param>
    private void on_start_ply(string turn_status) {
        // Game over check.
        var moves = board.find_all_moves(current_player);
        if (moves.Count == 0) {
            var win_or_loss = (current_player == user_side) ? "Lose" : "Win";
            EventManager.instance.publish("game_over", win_or_loss);
            return;
        }
        
        if (current_player == user_side)
            enable_pieces();

        // Otherwise, wait for opponent move event.
    }

    /// <summary>
    /// At the end of the ply, send the move sequence to the server and
    /// switch to the other player's turn.
    /// </summary>
    /// <param name="move_seq">Move sequence</param>
    private void on_end_ply(string move_seq) {
        var manager = EventManager.instance;

        // If this was the user's term, we need to send the moves to the
        // server
        if (current_player == user_side)
            manager.publish("send_moves", move_seq);

        // Switch to the other player
        current_player = PlayerTypeUtils.other_player(current_player);

        // We are no longer in a capture move chain.
        in_capture_chain = false;

        // Start the next ply
        var user_side_str = PlayerTypeUtils.serialize(user_side);
        var current_player_str = PlayerTypeUtils.serialize(current_player);
        var message = user_side_str + "," + current_player_str;
        manager.publish("start_ply", message);
    }

    /// <summary>
    /// Generate GUIPieces to match the internal board.
    /// </summary>
    private void generate_board() {
        foreach (var loc in board.find_all_piece_locations()) {
            var piece = board[loc];
            make_gui_piece(loc, piece);
        }
    }

    /// <summary>
    /// Make a checkerboard piece on the screen.
    /// </summary>
    /// <param name="loc">the square to place the board at.</param>
    /// <param name="piece">The piece data to attach to the GUI piece</param>
    private void make_gui_piece(CheckerCoords loc, Piece piece) {
        // Load the appropriate prefab checker piece.
        GameObject obj;
        if (piece.player == PlayerType.North)
            obj = (GameObject)Instantiate(north_prefab);
        else
            obj = (GameObject)Instantiate(south_prefab);

        // Attach data to the piece.
        var piece_gui = obj.GetComponent<GUIPiece>();
        piece_gui.piece = piece;
        piece_gui.location = loc;

        // Make sure all the sprites are children of the board.
        // This allows us to rotate the whole board at once.
        obj.transform.parent = transform;

        // Use the location to position the sprite relative to
        // the center of the board.
        place_object(obj, loc, CHECKER_DEPTH);
    }

    /// <summary>
    /// Enable all pieces that have valid moves so the user can select one
    /// of them.
    /// </summary>
    private void enable_pieces() {
        var moves = board.find_all_moves(user_side);

        foreach (var move in moves) {
            var loc_str = CheckerCoords.serialize(move.src);
            EventManager.instance.publish("enable_piece", loc_str);
        }
    }

    /// <summary>
    /// Rotates the board so the player's side is closer
    /// to the player.
    /// </summary>
    private void rotate_board() {
        // If the player is north, the board needs to be rotated 180°
        // around the z-axis so north is now at the bottom of the screen.
        var angle = user_side == PlayerType.North ? 180.0f : 0.0f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Create a clickable square for selecting a basic move.
    /// </summary>
    /// <param name="move">Move that was selected</param>
    private void make_basic_move_square(Move move) {
        // Load the prefab
        var obj = (GameObject)Instantiate(basic_move_prefab);

        var square = obj.GetComponent<GUIBasicMove>();
        square.move = move;

        // Make sure all the sprites are children of the board.
        // This allows us to rotate the whole board at once.
        obj.transform.parent = transform;

        // Use the location to position the sprite relative to
        // the center of the board
        place_object(obj, move.dst, CLICK_SQUARE_DEPTH);
    }

    /// <summary>
    /// Create a clickable square for selecting a capture move.
    /// </summary>
    /// <param name="move_seq_str">Move sequence string.</param>
    private void make_capture_move_square(string move_seq_str) {
        // Load the prefab
        var obj = (GameObject)Instantiate(capture_move_prefab);

        var square = obj.GetComponent<GUICaptureMove>();
        square.move_seq = move_seq_str;

        // Make sure all the sprites are children of the board.
        // This allows us to rotate the whole board at once.
        obj.transform.parent = transform;

        var last_move = Move.deserialize_seq_last(move_seq_str);
        place_object(obj, last_move.dst, CLICK_SQUARE_DEPTH);
    }
}
