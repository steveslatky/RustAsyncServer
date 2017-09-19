using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Event manager allows objects to listen to events in a centralized
/// systems.
/// This acts as a Singleton.
/// </summary>
public class EventManager {
    /// <summary>
    /// Explicitly pass no data.
    /// </summary>
    public static readonly string NO_DATA = string.Empty;

    /// <summary>
    /// Singleton instance
    /// </summary>
    private static EventManager _instance;

    /// <summary>
    /// The queue of events and corresponding data
    /// </summary>
    private Queue<Event> event_queue = new Queue<Event>();

    /// <summary>
    /// Map of event name -> message handler delegate
    /// </summary>
    private Dictionary<string, MessageHandler> listeners = 
        new Dictionary<string, MessageHandler>();

    /// <summary>
    /// Initializes a new instance of the <see cref="EventManager"/> class.
    /// This is private since EventManager is a Singleton
    /// </summary>
    private EventManager() {
    }

    /// <summary>
    /// Message handlers must be void functions that accept a single
    /// string as a parameter. The string is a message of any data
    /// that is serializable to a string.
    /// </summary>
    /// <param name="message">The message to send</param>
    public delegate void MessageHandler(string message);

    /// <summary>
    /// Gets the singleton instance
    /// </summary>
    /// <returns>The instance.</returns>
    public static EventManager instance {
        get {
            if (_instance == null)
                _instance = new EventManager();
            return _instance;
        }
    }

    /// <summary>
    /// Add an event handler for a given event name.
    /// </summary>
    /// <param name="event_name">Event name.</param>
    /// <param name="handler">The event handler</param>
    public void subscribe(string event_name, MessageHandler handler) {
        if (listeners.ContainsKey(event_name))
            listeners[event_name] += handler;
        else
            listeners.Add(event_name, handler);
    }

    /// <summary>
    /// Unsubscribe the specified event. The handler is removed from the
    /// dictionary. If the event does not exist, this does nothing.
    /// </summary>
    /// <param name="event_name">Event name.</param>
    /// <param name="handler">The event handler</param>
    public void unsubscribe(string event_name, MessageHandler handler) {
        if (listeners.ContainsKey(event_name))
            listeners[event_name] -= handler;
    }

    /// <summary>
    /// Publish the specified event with a message to send to all objects
    /// that are subscribed.
    /// </summary>
    /// <param name="event_name">Event name.</param>
    /// <param name="message">The message to send</param>
    public void publish(string event_name, string message) {
        var ev = new Event(event_name, message);
        event_queue.Enqueue(ev);
    }

    /// <summary>
    /// Process the event at the front of the event queue. This is meant
    /// to be called from Unity's Update function.
    /// </summary>
    public void next_event() {
        if (event_queue.Count > 0) {
            var ev = event_queue.Dequeue();
            if (listeners.ContainsKey(ev.event_name))
                listeners[ev.event_name].Invoke(ev.event_data);
        }
    }

    /// <summary>
    /// Simple data structure for storing events.
    /// </summary>
    private struct Event {
        /// <summary>
        /// The name of the event
        /// </summary>
        public string event_name;

        /// <summary>
        /// The data to be sent to each event callback
        /// </summary>
        public string event_data;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventManager+Event"/> 
        /// struct.
        /// </summary>
        /// <param name="event_name">Event name.</param>
        /// <param name="event_data">Event data.</param>
        public Event(string event_name, string event_data) {
            this.event_name = event_name;
            this.event_data = event_data;
        }
    }
}