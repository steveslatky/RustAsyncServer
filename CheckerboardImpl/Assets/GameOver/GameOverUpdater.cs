using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles displaying the game over text.
/// </summary>
public class GameOverUpdater : MonoBehaviour {
    /// <summary>
    /// When this object is created, display the game result 
    /// (e.g. "You Win", "You Lose!")
    /// </summary>
    public void Start() {
        GetComponent<Text>().text = 
            "You " 
            + GameStatus.getInstance().getGameResult() 
            + "!";
    }
}
