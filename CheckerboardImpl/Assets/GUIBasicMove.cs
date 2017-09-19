using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines the behavior of a basic move square.
/// </summary>
public class GUIBasicMove : MonoBehaviour {
    /// <summary>
    /// Gets or sets the move.
    /// </summary>
    /// <value>The move attached to this square</value>
    public Move move { get; set; }

    /// <summary>
    /// Subscribe to EventManager events
    /// </summary>
    public void OnEnable() {
        var manager = EventManager.instance;
        manager.subscribe("deselect_piece", self_destruct);
        manager.subscribe("basic_move", self_destruct);
        manager.subscribe("capture_move", self_destruct);
    }

    /// <summary>
    /// Unsubscribe from EventManager events
    /// </summary>
    public void OnDisable() {
        var manager = EventManager.instance;
        manager.unsubscribe("deselect_piece", self_destruct);
        manager.unsubscribe("basic_move", self_destruct);
        manager.unsubscribe("capture_move", self_destruct);
    }

    /// <summary>
    /// When the square is clicked, publish a basic move event.
    /// </summary>
    public void OnMouseUp() {
        var move_str = Move.serialize(move);
        EventManager.instance.publish("basic_move", move_str);
    }

    /// <summary>
    /// In response to any EventManager event, this square destroys itself.
    /// </summary>
    /// <param name="any_data">Any data, it will be ignored</param>
    private void self_destruct(string any_data) {
        Destroy(gameObject);
    }
}
