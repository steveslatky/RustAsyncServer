using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The EventRunner drives the EventManager loop. This uses Unity's Update
/// event instead of a separate thread/async function. 
/// </summary>
public class EventRunner : MonoBehaviour {
    /// <summary>
    /// Once every update, process one event from the event queue.
    /// </summary>
    public void Update() {
        EventManager.instance.next_event();
    }
}
