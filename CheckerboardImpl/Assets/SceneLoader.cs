using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles loading Unity scenes.
/// </summary>
public class SceneLoader : MonoBehaviour {
    /// <summary>
    /// Load a scene by name.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    public void loadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Exit the program from the loading screen.
    /// </summary>
    public void loadExit() {
        Application.Quit();
    }
}
