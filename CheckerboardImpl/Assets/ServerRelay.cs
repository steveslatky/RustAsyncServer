using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// The ServerRelay acts as a bridge between EventManager and the server.
/// </summary>
public class ServerRelay : MonoBehaviour {
    /// <summary>
    /// The port number of the server
    /// </summary>
    public int port = 1234;

    /// <summary>
    /// The user ID. This must be sent to the server with every message
    /// </summary>
    private static int UID_;

    /// <summary>
    /// The socket for communicating with the server
    /// </summary>
    private static Socket socket; 

    /// <summary>
    /// Keep track of which player we are
    /// </summary>
    private static bool IsNorth;

    /// <summary>
    /// The buffer for storing messages
    /// </summary>
    private static byte[] buffer = new byte[1080];

    /// <summary>
    /// Subscribe to EventManager events
    /// </summary>
    public void OnEnable() {
        var manager = EventManager.instance;
        manager.subscribe("connect", on_connect);
        manager.subscribe("send_moves", on_send_moves);
        manager.subscribe("game_over", on_game_over);
    }

    /// <summary>
    /// Unsubscribe from EventManager events
    /// </summary>
    public void OnDisable() {
        var manager = EventManager.instance;
        manager.unsubscribe("connect", on_connect);
        manager.unsubscribe("send_moves", on_send_moves);
        manager.unsubscribe("game_over", on_game_over);
    }
        
    /// <summary>
    /// Make this object persistent
    /// </summary>
    public void Start() {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    // Every update call, poll the server for a single message. If it gets
    // one, publish an event to the EventManager.

    /// <summary>
    /// Every update call, poll the server for a single message. If it gets
    /// one, publish an event to the EventManager.
    /// </summary>
    public void Update() {
        // poll the server once (non-blocking)
        byte[] data = new byte[256];
        if (socket != null && socket.Poll(0, SelectMode.SelectRead)) {
            socket.Receive(data);
            string dataString = Encoding.UTF8.GetString(data);

            if (string.Compare(dataString.Substring(0, 10), "start_game") == 0) {
                on_start_game();
            } 
            else if (string.Compare(dataString.Substring(0, 1), "{") == 0) {
                ServerMove movesReceived = JsonUtility.FromJson<ServerMove>(dataString);

                // check if comma in string for capture moves
                var move = Move.deserialize_seq_first(movesReceived.movesString); 
                if (move.move_type == MoveType.Capture) {
                    EventManager.instance.publish("opponent_capture_move", movesReceived.movesString);
                } 
                else {
                    EventManager.instance.publish("opponent_basic_move", movesReceived.movesString);
                }
            } 
            else if (string.Compare(dataString.Substring(0, 7), "forfeit") == 0) {
                EventManager.instance.publish("game_over", "forfeit");
            } 
            else if (string.Compare(dataString.Substring(0, 3), "win") == 0) {
                EventManager.instance.publish("game_over", "win");
            } 
            else if (string.Compare(dataString.Substring(0, 4), "lose") == 0) {
                EventManager.instance.publish("game_over", "lose");
            }
        }
    }

    /// <summary>
    /// At the start of the game, publish the user side side and which player
    /// goes first.
    /// </summary>
    private void on_start_game() {
        if (IsNorth) {
            EventManager.instance.publish("start_game", "N,N");
        } 
        else {
            EventManager.instance.publish("start_game", "S,N");
        }
    }

    /// <summary>
    /// When ServerRelay gets a connect event, connect to the server.
    /// </summary>
    /// <param name="no_data">No data.</param>
    private void on_connect(string no_data) {
        // make the connection
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ip = System.IO.File.ReadAllText("server_ip.txt").Trim();
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        try {
            socket.Connect(localEndPoint);
            socket.Receive(buffer);
            UID_ = Convert.ToInt32(Encoding.UTF8.GetString(buffer));
            if (UID_ % 2 == 1) {
                IsNorth = true;
            }
            else {
                IsNorth = false;
            }
        }
        catch {
            Debug.Log("Unable to connect to remote endpoint\r\n");
        }
    }

    /// <summary>
    /// When the client has moves to send to the server, send the data
    /// over the socket in JSON format.
    /// </summary>
    /// <param name="moves">the moves to send</param>
    private void on_send_moves(string moves) {
        ServerMove movesToSend = new ServerMove();
        movesToSend.movesString = moves;
        movesToSend.UID = UID_;

        string json = JsonUtility.ToJson(movesToSend);

        int charLocation = json.IndexOf("}", StringComparison.Ordinal);
        json = json.Substring(0, charLocation + 1);
        json = json + "\0"; // server won't terminate read until it gets "\0"

        byte[] buffer = Encoding.ASCII.GetBytes(json);

        socket.Send(buffer);
    }

    /// <summary>
    /// When a game is over, alert the other player
    /// </summary>
    /// <param name="message">
    /// the game over message to send to the server
    /// </param>
    private void on_game_over(string message) {
        message = message + "\0";
        byte[] buffer = Encoding.ASCII.GetBytes(message);

        socket.Send(buffer);
    }

    /// <summary>
    /// Representation of a move from the eyes of the server.
    /// </summary>
    public class ServerMove {
        /// <summary>
        /// A string containing moves
        /// </summary>
        [SerializeField]
        public string movesString;

        /// <summary>
        /// The user ID
        /// </summary>
        [SerializeField]
        public int UID;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMove"/> class.
        /// </summary>
        public ServerMove() {
        }
    }
}
