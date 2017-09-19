using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Convenience methods for PlayerType
/// </summary>
public class PlayerTypeUtils {
    /// <summary>
    /// Serialize the specified player type
    /// </summary>
    /// <param name="type">the player type</param>
    /// <returns>N or S depending on the player type</returns>
    public static string serialize(PlayerType type) {
        return type == PlayerType.North ? "N" : "S";
    }

    /// <summary>
    /// Deserialize the player type from a string
    /// </summary>
    /// <param name="type">"N" or "S"</param>
    /// <returns>a PlayerType enum value</returns>
    public static PlayerType deserialize(string type) {
        return type == "N" ? PlayerType.North : PlayerType.South;
    }

    /// <summary>
    /// Given a PlayerType, return the opposite player.
    /// </summary>
    /// <returns>The other player.</returns>
    /// <param name="player">the player</param>
    public static PlayerType other_player(PlayerType player) {
        return player == PlayerType.North ? PlayerType.South : PlayerType.North;
    }
}