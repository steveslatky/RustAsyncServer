using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages the text on the screen that indicates whose turn
/// it is.
/// </summary>
public class TurnIndicator : MonoBehaviour {
    /// <summary>
    /// Subscribe to EventManager events
    /// </summary>
    public void OnEnable() {
        var manager = EventManager.instance;
        manager.subscribe("start_ply", on_start_ply);
    }

    /// <summary>
    /// Unsubscribe to EventManager events
    /// </summary>
    public void OnDisable() {
        var manager = EventManager.instance;
        manager.unsubscribe("start_ply", on_start_ply);
    }

    /// <summary>
    /// At the start of either player's turn, update the text on the screen.
    /// </summary>
    /// <param name="msg">
    /// comma delimited string representing the user's side and the current 
    /// player.
    /// </param>
    private void on_start_ply(string msg) {
        var parts = msg.Split(',');
        var user_side = parts[0];
        var current_player = parts[1];

        if (user_side == current_player) {
            GetComponent<Text>().text = "It's your turn!";
        } 
        else {
            GetComponent<Text>().text = "Waiting for\nopponent move.";
        }
    }
}
