using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class keeps track of game status information that needs to be
/// persisted from scene to scene.
/// </summary>
public class GameStatus : MonoBehaviour {
    /// <summary>
    /// The result of the last game.
    /// </summary>
    protected string result = "You won!";

    /// <summary>
    /// The singleton instance.
    /// </summary>
    private static GameStatus instance;

    /// <summary>
    /// Get the singleton instance
    /// </summary>
    /// <returns>The singleton instance.</returns>
    public static GameStatus getInstance() {
        return instance;
    }

    /// <summary>
    /// Make sure this object is a singleton. Also make this object persistent.
    /// </summary>
    public void Start() {
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Set the result of the game.
    /// </summary>
    /// <param name="gameResult">Game result.</param>
    public void setGameResult(string gameResult) {
        result = gameResult;
    }

    /// <summary>
    /// Gets the game result.
    /// </summary>
    /// <returns>The game result.</returns>
    public string getGameResult() {
        return result;    
    }
}
