using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A GUIPiece is a playing piece on the screen.
/// </summary>
public class GUIPiece : MonoBehaviour {
    /// <summary>
    /// Sprite for the normal checker piece
    /// </summary>
    public Sprite spr_normal;

    /// <summary>
    /// Sprite for when the checker is selected.
    /// </summary>
    public Sprite spr_selected;

    /// <summary>
    /// Sprite for a king piece
    /// </summary>
    public Sprite spr_king;

    /// <summary>
    /// Sprite for a selected king piece
    /// </summary>
    public Sprite spr_king_selected;

    /// <summary>
    /// Current state of the sprite.
    /// </summary>
    /// <value>The state.</value>
    public PieceState state { get; set; }

    /// <summary>
    /// Piece that this GUI element represents
    /// </summary>
    /// <value>The piece.</value>
    public Piece piece { get; set; }

    /// <summary>
    /// Location in the checkerboard. This will make it easy
    /// to interact with the checkerboard in event handlers.
    /// </summary>
    /// <value>The location.</value>
    public CheckerCoords location { get; set; }

    /// <summary>
    /// Get the parent GUICheckerboard so we can
    /// update the board over time.
    /// </summary>
    /// <value>The parent.</value>
    public GUICheckerboard parent {
        get {
            return transform.parent.GetComponent<GUICheckerboard>();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="GUIPiece"/> is 
    /// king.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this piece is a king; 
    ///     otherwise, <c>false</c>.
    /// </value>
    public bool is_king {
        get {
            return piece.is_king;
        }

        set {
            piece.is_king = value;
        }
    }

    /// <summary>
    /// Subscribe to EventManager events.
    /// </summary>
    public void OnEnable() {
        var manager = EventManager.instance;
        manager.subscribe("enable_piece", on_enable_piece);
        manager.subscribe("select_piece", on_select_piece);
        manager.subscribe("deselect_piece", on_deselect_piece);
        manager.subscribe("basic_move", on_basic_move);
        manager.subscribe("opponent_basic_move", on_opponent_basic_move);
        manager.subscribe("capture_move", on_capture_move);
        manager.subscribe("opponent_capture_move", on_opponent_capture_move);
    }

    /// <summary>
    /// Unsubscribe from EventManager events.
    /// </summary>
    public void OnDisable() {
        var manager = EventManager.instance;
        manager.unsubscribe("enable_piece", on_enable_piece);
        manager.unsubscribe("select_piece", on_select_piece);
        manager.unsubscribe("deselect_piece", on_deselect_piece);
        manager.unsubscribe("basic_move", on_basic_move);
        manager.unsubscribe("opponent_basic_move", on_opponent_basic_move);
        manager.unsubscribe("capture_move", on_capture_move);
        manager.unsubscribe("opponent_capture_move", on_opponent_capture_move);
    }

    /// <summary>
    /// Make this piece non-selectable when first created.
    /// </summary>
    public void Awake() {
        state = PieceState.NonSelectable;
    }

    /// <summary>
    /// Every frame, make sure we are using the correct sprite.
    /// </summary>
    public void Update() {
        update_sprite();
    }

    /// <summary>
    /// Switch between Selectable and selected when clicked.
    /// If not selectable, do nothing.
    /// </summary>
    public void OnMouseUp() {
        var manager = EventManager.instance;
        var loc_str = CheckerCoords.serialize(location);

        if (state == PieceState.Selectable) {
            // select this piece
            manager.publish("select_piece", loc_str);
        } 
        else if (!GUICheckerboard.in_capture_chain) {
            // Deselect this piece. This is disabled if we already
            // started a capture chain
            manager.publish("deselect_piece", EventManager.NO_DATA);
        }
    }

    /// <summary>
    /// Enable this piece if the current location matches the location
    /// in the message from GUICheckerboard.
    /// </summary>
    /// <param name="loc_str">Location of the piece to enable</param>
    private void on_enable_piece(string loc_str) {
        var loc = CheckerCoords.deserialize(loc_str);

        if (loc.Equals(location)) {
            state = PieceState.Selectable;
        }
    }

    /// <summary>
    /// Event handler for when a piece is selected by the user.
    /// Data: serialized CheckerCoords representing the location of
    ///       the selected piece
    /// If this piece's location matches that of the event, set the state
    /// to selected.
    /// Otherwise, make this piece non-selectable.
    /// </summary>
    /// <param name="loc_str">Location string.</param>
    private void on_select_piece(string loc_str) {
        var loc = CheckerCoords.deserialize(loc_str);
        if (loc.Equals(location))
            state = PieceState.Selected;
        else
            state = PieceState.NonSelectable;
    }

    /// <summary>
    /// Perform a capture move. If this piece is located at the source
    /// square of the move, it is the capturing piece and must move to
    /// the destination square. If this piece is at the midpoint of the
    /// move, it is the captured piece and must be destroyed
    /// </summary>
    /// <param name="move_seq_str">Move sequence as a string.</param>
    private void on_capture_move(string move_seq_str) {
        var move = Move.deserialize_seq_last(move_seq_str);
        if (location.Equals(move.src)) {
            location = move.dst;
            state = PieceState.NonSelectable;
            GUICheckerboard.place_object(
                gameObject, move.dst, GUICheckerboard.CHECKER_DEPTH);
        } 
        else if (location.Equals(move.enemy_location)) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Event handler for when the user clicks away from the selected piece.
    /// </summary>
    /// <param name="no_data">
    ///     An empty string is given here and is not used.This string value
    ///     is only needed to match the delegate signature.
    /// </param>
    private void on_deselect_piece(string no_data) {
        // If this is the selected piece, deselect it.
        if (state == PieceState.Selected)
            state = PieceState.Selectable;
    }

    /// <summary>
    /// On a basic move, if this is the piece that moved, move it to the
    /// destination square and make the piece non-selectable
    /// </summary>
    /// <param name="move_str">Move string.</param>
    private void on_basic_move(string move_str) {
        // move the selected piece on the screen
        var move = Move.deserialize(move_str);
        if (move.src.Equals(location)) {
            location = move.dst;
            state = PieceState.NonSelectable;
            GUICheckerboard.place_object(
                gameObject, move.dst, GUICheckerboard.CHECKER_DEPTH);
        }
    }

    /// <summary>
    /// The opponent's basic move event works similarly to a user
    /// basic move event except it doesn't need to update the state,
    /// opponent pieces are always non-selectable.
    /// </summary>
    /// <param name="move_str">Move string.</param>
    private void on_opponent_basic_move(string move_str) {
        var move = Move.deserialize(move_str);
        if (move.src.Equals(location)) {
            location = move.dst;
            GUICheckerboard.place_object(
                gameObject, move.dst, GUICheckerboard.CHECKER_DEPTH);
        }
    }

    /// <summary>
    /// opponent capture moves are similar to basic capture moves.
    /// The capturing piece moves to the destination square. The
    /// captured piece is destroyed.
    /// </summary>
    /// <param name="move_seq_str">Move sequence string.</param>
    private void on_opponent_capture_move(string move_seq_str) {
        var first_move = Move.deserialize_seq_first(move_seq_str);

        if (location.Equals(first_move.src)) {
            location = first_move.dst;
            GUICheckerboard.place_object(
                gameObject, first_move.dst, GUICheckerboard.CHECKER_DEPTH);
        } 
        else if (location.Equals(first_move.enemy_location)) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Updates the sprite based on the current state.
    /// </summary>
    private void update_sprite() {
        var spr_renderer = GetComponent<SpriteRenderer>();
        if (state == PieceState.Selected && is_king)
            spr_renderer.sprite = spr_king_selected;
        else if (state == PieceState.Selected)
            spr_renderer.sprite = spr_selected;
        else if (is_king)
            spr_renderer.sprite = spr_king;
        else
            spr_renderer.sprite = spr_normal;

        // TODO: Debugging code, remove this later
        /*
        if (state == PieceState.NonSelectable)
            spr_renderer.color = Color.green;
        else
            spr_renderer.color = Color.white;
        */
    }
}
