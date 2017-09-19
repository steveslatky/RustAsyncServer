using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines the behavior of capture move squares.
/// </summary>
public class GUICaptureMove : MonoBehaviour {
    /// <summary>
    /// Move sequence up to this point, serialized as a string
    /// for message publishing.
    /// </summary>
    /// <value>The move sequence</value>
    public string move_seq { get; set; }

    /// <summary>
    /// Subscribe to EventManager events.
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
    /// When clicked, publish a capture move event
    /// </summary>
    public void OnMouseUp() {
        EventManager.instance.publish("capture_move", move_seq);
    }

    /// <summary>
    /// In response to any EventManager event, this square destroys itself.
    /// </summary>
    /// <param name="any_data">Any data, it will be ignored</param>
    private void self_destruct(string any_data) {
        Destroy(gameObject);
    }
}
