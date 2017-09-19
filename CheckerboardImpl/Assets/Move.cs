using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A move represents a single user's action on the board. Moves are specified
/// as a pair of (source, destination) squares on the checkerboard.
/// Capture move chains are represented as a list of capture Moves performed in
/// succession.
/// </summary>
public class Move {
    /// <summary>
    /// Basic moves have an offset of 1 in both row and column
    /// directions (ignoring sign)
    /// </summary>
    private const int OFFSET_BASIC = 1;

    /// <summary>
    /// Capture moves have an offset of 2 in both row and column
    /// directions (ignoring sign)
    /// </summary>
    private const int OFFSET_CAPTURE = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="Move"/> class.
    /// </summary>
    /// <param name="src">Source square</param>
    /// <param name="dst">Destination square</param>
    public Move(CheckerCoords src, CheckerCoords dst) {
        this.src = src;
        this.dst = dst;
    }

    /// <summary>
    /// Gets or sets the source location
    /// </summary>
    /// <value>The source location.</value>
    public CheckerCoords src { get; set; }

    /// <summary>
    /// Gets or sets the destination location
    /// </summary>
    /// <value>The destination location</value>
    public CheckerCoords dst { get; set; }

    /// <summary>
    /// Calculate the type of move (Basic/Capture) based on the
    /// source and destination locations. Basic moves always move 1
    /// in both direction, capture moves always move 2 spaces in both
    /// directions
    /// </summary>
    /// <value>The type of the move.</value>
    public MoveType move_type {
        get {
            var offset = dst - src;
            var rows = Math.Abs(offset.row);
            var cols = Math.Abs(offset.col);
            if (rows == OFFSET_BASIC && cols == OFFSET_BASIC)
                return MoveType.Basic;
            else if (rows == OFFSET_CAPTURE && cols == OFFSET_CAPTURE)
                return MoveType.Capture;
            else
                throw new Exception("Invalid move!");
        }
    }

    /// <summary>
    /// If this was a move taken by a non-king piece, determine from the
    /// vertical change of the move which player must have made the move.
    /// e.g. a move from (1, 0) to (0, 1) would only be valid for the
    /// South player for a normal piece since the piece moves towards
    /// the north side of the board.
    /// This is useful for detecting valid moves.
    /// </summary>
    /// <value>
    /// The player for which this move is valid (for a non-king piece)
    /// </value>
    public PlayerType player {
        get {
            var offset = dst - src;
            if (offset.row > 0)
                return PlayerType.North;
            else if (offset.row < 0)
                return PlayerType.South;
            else
                throw new Exception("Invalid move direction!");
        }
    }

    /// <summary>
    /// Return true if the move ends on either the eighth row on either
    /// side of the checkerboard.
    /// </summary>
    /// <value>
    ///     <c>true</c> if move ends on row 0 or 7; 
    ///     otherwise, <c>false</c>.
    /// </value>
    public bool is_crowning {
        get {
            return dst.row == 0 || dst.row == Checkerboard.BOARD_SIZE - 1;
        }
    }

    /// <summary>
    /// Gets the enemy location. This is the midpoint of the move.
    /// This operation only makes sense for capture moves.
    /// </summary>
    /// <value>The enemy location.</value>
    public CheckerCoords enemy_location {
        get {
            if (move_type != MoveType.Capture)
                throw new Exception("Only capture moves have enemy locations");
            else
                return CheckerCoords.midpoint(src, dst);
        }
    }

    /// <summary>
    /// Serialize the specified move.
    /// </summary>
    /// <param name="move">the move to serialize</param>
    /// <returns>
    /// A string {source}-{destination} where each is a serialized
    /// CheckerCoords
    /// </returns>
    public static string serialize(Move move) {
        var src_str = CheckerCoords.serialize(move.src);
        var dst_str = CheckerCoords.serialize(move.dst);
        return src_str + "-" + dst_str;
    }

    /// <summary>
    /// Deserialize the specified move string.
    /// </summary>
    /// <param name="move_str">
    /// Move string {source}-{destination} where each square is
    /// a serialized CheckerCoords</param>
    /// <returns>A Move object</returns>
    public static Move deserialize(string move_str) {
        var parts = move_str.Split('-');
        var src = CheckerCoords.deserialize(parts[0]);
        var dst = CheckerCoords.deserialize(parts[1]);
        return new Move(src, dst);
    }

    /// <summary>
    /// Deserializes the first move in a sequence
    /// </summary>
    /// <returns>The first element of the sequence</returns>
    /// <param name="move_seq">Move sequence {m1},{m2},...,{mN}</param>
    public static Move deserialize_seq_first(string move_seq) {
        var move_strs = move_seq.Split(',');
        var last_move_str = move_strs[0];
        return deserialize(last_move_str);
    }

    /// <summary>
    /// Deserializes the last move in a sequence
    /// </summary>
    /// <returns>The last element of a sequence</returns>
    /// <param name="move_seq">a move sequence {m1},{m2},...,{mN}</param>
    public static Move deserialize_seq_last(string move_seq) {
        var move_strs = move_seq.Split(',');
        var last_move_str = move_strs[move_strs.Length - 1];
        return deserialize(last_move_str);
    }

    /// <summary>
    /// Deserializes an entire move sequence
    /// </summary>
    /// <returns>The move sequence as a List</returns>
    /// <param name="move_seq">Move sequence {m1},{m2},...,{mN}</param>
    public static List<Move> deserialize_seq(string move_seq) {
        var move_strs = move_seq.Split(',');
        var moves = new List<Move>();
        foreach (var move_str in move_strs) {
            var move = deserialize(move_str);
            moves.Add(move);
        }

        return moves;
    }

    /// <summary>
    /// Serializes a sequence to a string
    /// </summary>
    /// <returns>The sequence as a string {m1},{m2},...,{mN}</returns>
    /// <param name="moves">a list of moves</param>
    public static string serialize_seq(List<Move> moves) {
        var seq_str = string.Empty;
        for (int i = 0; i < moves.Count; i++) {
            var move_str = serialize(moves[i]);
            if (i != 0)
                seq_str += ",";
            seq_str += move_str;
        }

        return seq_str;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current 
    /// <see cref="Move"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current 
    /// <see cref="Move"/>.</returns>
    public override string ToString() {
        return Move.serialize(this);
    }
}