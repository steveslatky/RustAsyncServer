using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A spinning sun sprite as a waiting indicator on the Loading screen.
/// </summary>
public class SunSpinner : MonoBehaviour {
    /// <summary>
    /// Every frame, rotate the sprite a little bit more around its center
    /// </summary>
    public void Update() {
        transform.Rotate(Vector3.back, 1.25f);
    }
}
