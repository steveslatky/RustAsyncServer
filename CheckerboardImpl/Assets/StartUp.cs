using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles setting up the program.
/// </summary>
public class StartUp : MonoBehaviour {
    /// <summary>
    /// When the program is launched, make it full-screen
    /// </summary>
    public void Start() {
        Screen.fullScreen = false;
    }
}
