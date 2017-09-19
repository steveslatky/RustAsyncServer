using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles setting the background music
/// </summary>
public class BackgroundMusic : MonoBehaviour {
    /// <summary>
    /// This toggles background music on or off.
    /// </summary>
    public void setSound() {
        var audioSources = 
            FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        GameObject soundButton = GameObject.Find("SoundButton");

        foreach (var audioSource in audioSources) {
            if (audioSource.volume == 1) {
                soundButton.GetComponentInChildren<Text>().text = "music on";
                audioSource.volume = 0;
            } 
            else {
                soundButton.GetComponentInChildren<Text>().text = "music off";
                audioSource.volume = 1;
            }
        }
    }
}
