using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles EventManager events on the Loading screen.
/// It publishes a connect event to the ServerRelay, and when it gets
/// a response from the server, it transitions to the game screen.
/// </summary>
public class LoadGame : MonoBehaviour {
    /// <summary>
    /// When this object is created, alert ServerRelay that we are ready
    /// to connect to the server.
    /// </summary>
    public void Start() {
        EventManager.instance.publish("connect", EventManager.NO_DATA);
    }

    /// <summary>
    /// Subscribe to EventManager callbacks
    /// </summary>
    public void OnEnable() {
        var manager = EventManager.instance;
        manager.subscribe("start_game", on_start_game);
    }

    /// <summary>
    /// Unsubscribe from EventManager callbacks
    /// </summary>
    public void OnDisable() {
        var manager = EventManager.instance;
        manager.unsubscribe("start_game", on_start_game);
    }

    /// <summary>
    /// When the server responds to the client's connect message, transition
    /// to the game screen.
    /// </summary>
    /// <param name="message_from_server">Message from server.</param>
    private void on_start_game(string message_from_server) {
        SceneManager.LoadScene("MainScene");
        EventManager.instance.publish("start_game", message_from_server);
    }
}
