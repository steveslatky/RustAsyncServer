using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages button callbacks in  menus
/// </summary>
public class ButtonManager : MonoBehaviour {
    /// <summary>
    /// The Play button transitions to the loading screen.
    /// </summary>
    public void PlayGameBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// The About button transitions to the About screen.
    /// </summary>
    public void AboutBtn()
    {
        SceneManager.LoadScene("About");
    }

    /// <summary>
    /// This back button transitions from About -> Main Menu screens.
    /// </summary>
    public void AboutBackBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// This button exits the game.
    /// </summary>
    /// <param name="Game">Game description</param>
    public void ExitGameBtn(string Game)
    {
        Application.Quit();
    }
}
