using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class represents the game screen in the menu subsystem.
/// </summary>
public class GameScreen : MonoBehaviour {
    /// <summary>
    /// Subscribe to EventManager events
    /// </summary>
    public void OnEnable() {
        var manager = EventManager.instance;
        manager.subscribe("game_over", on_game_over);
    }

    /// <summary>
    /// Unsubscribe from EventManager Events
    /// </summary>
    public void OnDisable() {
        var manager = EventManager.instance;
        manager.unsubscribe("game_over", on_game_over);
    }

    /// <summary>
    /// When the forfeit button is pressed, this counts as an automatic
    /// game over.
    /// </summary>
    public void forfeitGame() {
        EventManager.instance.publish("game_over", "Forfeit");
    }

    /// <summary>
    /// On game over, update GameStatus and transition to the game over
    /// screen.
    /// </summary>
    /// <param name="message_from_server">Message from server.</param>
    private void on_game_over(string message_from_server) {
        GameStatus gs = GameStatus.getInstance();
        gs.setGameResult(message_from_server);
        SceneManager.LoadScene("GameOver");
    }
}
