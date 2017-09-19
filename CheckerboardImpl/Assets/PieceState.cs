using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The piece state labels the states of a GUIPiece.
/// </summary>
public enum PieceState {
    /// <summary>
    /// The piece cannot be clicked
    /// </summary>
    NonSelectable,

    /// <summary>
    /// The user can click this piece to select it
    /// </summary>
    Selectable,

    /// <summary>
    /// The user has selected this piece.
    /// </summary>
    Selected
}