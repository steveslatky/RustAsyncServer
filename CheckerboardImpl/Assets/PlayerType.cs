using System;

/// <summary>
/// Enum to label the two players by side of the in-memory board.
/// </summary>
public enum PlayerType {
    /// <summary>
    /// The north player is on the top half of the board (low row indices)
    /// </summary>
    North,

    /// <summary>
    /// The south player is on the bottom half of the board (high row indices
    /// </summary>
    South
}