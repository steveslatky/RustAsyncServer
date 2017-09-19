using System;

/// <summary>
/// Moves can either be basic moves or capture moves.
/// </summary>
public enum MoveType {
    /// <summary>
    /// Basic moves move a piece to a diagonally adjacent square.
    /// </summary>
    Basic,

    /// <summary>
    /// Capture moves make the piece jump over an opponent's piece to
    /// remove the opponent's piece from the board.
    /// </summary>
    Capture
}