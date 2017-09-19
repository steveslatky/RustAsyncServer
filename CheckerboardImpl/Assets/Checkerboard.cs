using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the in-memory representation of the checkerboard. It provides
/// methods for searching for valid moves.
/// </summary>
public class Checkerboard {
    /// <summary>
    /// The checkerboard is 8x8
    /// </summary>
    public const int BOARD_SIZE = 8;

    /// <summary>
    /// These offsets mark the directions of the four diagonally adjacent
    /// squares. They can be scaled up as needed.
    /// </summary>
    private readonly CheckerCoords[] OFFSETS = new CheckerCoords[] {
        new CheckerCoords(1, 1),
        new CheckerCoords(1, -1),
        new CheckerCoords(-1, 1),
        new CheckerCoords(-1, -1)
    };

    /// <summary>
    /// Internal representation of a grid.
    /// </summary>
    private Piece[,] grid = new Piece[BOARD_SIZE, BOARD_SIZE];

    /// <summary>
    /// Creates a new Checkerboard with the 24 checkers pieces in their
    /// starting locations.
    /// </summary>
    public Checkerboard() {
        // Populate north side of board
        const int FIRST_NORTH_SQUARE = 1;
        const int LAST_NORTH_SQUARE = 12;
        for (int i = FIRST_NORTH_SQUARE; i <= LAST_NORTH_SQUARE; i++) {
            var loc = CheckerCoords.to_coords(i);
            this[loc] = new Piece(PlayerType.North, false);
        }

        // Populate the south side of the board
        const int FIRST_SOUTH_SQUARE = 21;
        const int LAST_SOUTH_SQUARE = 32;
        for (int i = FIRST_SOUTH_SQUARE; i <= LAST_SOUTH_SQUARE; i++) {
            var loc = CheckerCoords.to_coords(i);
            this[loc] = new Piece(PlayerType.South, false);
        }
    }

    /// <summary>
    /// Overload the subscript operator so we can look up playing pieces given
    /// a checkerboard square.
    /// </summary>
    /// <param name="loc">The square to check</param>
    /// <returns>
    /// The Piece stored at the given square or null if the square is
    /// empty
    /// </returns>
    public Piece this[CheckerCoords loc] {
        get {
            return grid[loc.row, loc.col]; 
        }

        set {
            grid[loc.row, loc.col] = value;
        }
    }

    /// <summary>
    /// Check if a location is valid in the grid. This means:
    /// 1. The location is within the bounds of the grid
    /// 2. the location's diagonal number is odd.
    /// </summary>
    /// <returns>
    ///     <c>true</c>, if the location is valid.
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <param name="loc">location to check</param>
    public bool is_valid(CheckerCoords loc) {
        // bounds check
        var in_bounds =
            0 <= loc.row && loc.row < BOARD_SIZE
            && 0 <= loc.col && loc.col < BOARD_SIZE;

        // Adding the row and column numbers of a location gives
        // the diagonal number. valid checkers locations happen to all
        // fall on odd-numbered diagonals.
        var on_valid_diagonal = loc.diagonal % 2 == 1;

        return in_bounds && on_valid_diagonal;
    }

    /// <summary>
    /// Checks if a checkerboard square is empty
    /// </summary>
    /// <returns>
    ///     <c>true</c>, if the square is empty
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <param name="loc">The square to check</param>
    public bool is_empty(CheckerCoords loc) {
        return this[loc] == null;
    }

    /// <summary>
    /// Remove the piece at the given location.
    /// </summary>
    /// <param name="loc">The square to check</param>
    /// <returns>the removed piece</returns>
    public Piece remove(CheckerCoords loc) {
        var removed = this[loc];
        this[loc] = null;
        return removed;
    }

    /// <summary>
    /// Find the locations of playing pieces for one of the players
    /// </summary>
    /// <returns>the locations of the player's pieces</returns>
    /// <param name="player">The player to get pieces for</param>
    public List<CheckerCoords> find_piece_locations(PlayerType player) {
        var locs = new List<CheckerCoords>();
        for (int i = 0; i < BOARD_SIZE; i++) {
            for (int j = 0; j < BOARD_SIZE; j++) {
                var loc = new CheckerCoords(i, j);
                if (!is_empty(loc) && this[loc].player == player)
                    locs.Add(loc);
            }
        }

        return locs;
    }

    /// <summary>
    /// Find the locations of all playing pieces.
    /// </summary>
    /// <returns>The all piece locations.</returns>
    public List<CheckerCoords> find_all_piece_locations() {
        var locs = find_piece_locations(PlayerType.North);
        locs.AddRange(find_piece_locations(PlayerType.South));
        return locs;
    }

    /// <summary>
    /// Perform a move, updating the in-memory checkerboard. 
    /// This handles both basic and capture moves.
    /// </summary>
    /// <param name="move">The move to perform</param>
    public void perform_move(Move move) {
        // Always move the piece from src -> dst
        var piece = remove(move.src);
        this[move.dst] = piece;

        // For capture moves, remove the enemy piece
        if (move.move_type == MoveType.Capture) {
            var enemy_loc = CheckerCoords.midpoint(move.src, move.dst);
            remove(enemy_loc);
        }

        // Check if a piece needs to be crowned.
        if (move.is_crowning)
            piece.is_king = true;
    }

    /// <summary>
    /// Find all basic moves for a given location.
    /// </summary>
    /// <returns>A list of basic moves from the location</returns>
    /// <param name="from_loc">The location to check</param>
    public List<Move> find_basic_moves(CheckerCoords from_loc) {
        var piece = this[from_loc];
        var moves = new List<Move>();
        foreach (var offset in OFFSETS) {
            var to_loc = from_loc + offset;
            var move = new Move(from_loc, to_loc);
            var direction_valid = move.player == piece.player || piece.is_king;
            var destination_valid = is_valid(to_loc) && is_empty(to_loc);
            if (direction_valid && destination_valid)
                moves.Add(move);
        }

        return moves;
    }

    /// <summary>
    /// Find all capture moves from a given square
    /// </summary>
    /// <returns>a list of valid capture moves</returns>
    /// <param name="from_loc">The square to check</param>
    public List<Move> find_capture_moves(CheckerCoords from_loc) {
        var piece = this[from_loc];
        var moves = new List<Move>();
        foreach (var offset in OFFSETS) {
            var to_loc = from_loc + (2 * offset);
            var move = new Move(from_loc, to_loc);
            var direction_valid = move.player == piece.player || piece.is_king;
            var destination_valid = is_valid(to_loc) && is_empty(to_loc);

            // We can only check for a valid enemy if the destination is
            // valid.
            var enemy_valid = false;
            if (destination_valid) {
                var enemy_loc = CheckerCoords.midpoint(from_loc, to_loc);
                var enemy = this[enemy_loc];
                enemy_valid = enemy != null && enemy.player != piece.player;
            }

            // Make sure all three conditions are met before adding the move
            if (direction_valid && destination_valid && enemy_valid)
                moves.Add(move);
        }

        return moves;
    }

    /// <summary>
    /// Get all basic (non-capturing) moves for a player
    /// </summary>
    /// <returns>A list of basic moves.</returns>
    /// <param name="player">the player to check for</param>
    public List<Move> find_all_basic_moves(PlayerType player) {
        var moves = new List<Move>();
        foreach (var loc in find_piece_locations(player))
            moves.AddRange(find_basic_moves(loc));
        return moves;
    }

    /// <summary>
    /// Finds all capture moves for a given player
    /// </summary>
    /// <returns>A list of all capture moves for the player</returns>
    /// <param name="player">The player to get moves for</param>
    public List<Move> find_all_capture_moves(PlayerType player) {
        var moves = new List<Move>();
        foreach (var loc in find_piece_locations(player))
            moves.AddRange(find_capture_moves(loc));
        return moves;
    }

    /// <summary>
    /// Finds all moves for a given player
    /// </summary>
    /// <returns>All moves. for the player</returns>
    /// <param name="player">The player to get moves for.</param>
    public List<Move> find_all_moves(PlayerType player) {
        var move_list = find_all_basic_moves(player);
        move_list.AddRange(find_all_capture_moves(player));
        return move_list;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current 
    /// <see cref="Checkerboard"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current 
    /// <see cref="Checkerboard"/>.</returns>
    public override string ToString() {
        var lines = new List<string>();
        lines.Add("  0 1 2 3 4 5 6 7");
        for (int i = 0; i < BOARD_SIZE; i++) {
            var row_str = string.Empty;
            for (int j = 0; j < BOARD_SIZE; j++) {
                var piece = grid[i, j];
                if (piece == null)
                    row_str += ". ";
                else
                    row_str += string.Format("{0} ", piece);
            }

            var line = string.Format("{0} {1}", i, row_str);
            lines.Add(line);
        }

        return string.Join("\n", lines.ToArray());
    }
}
