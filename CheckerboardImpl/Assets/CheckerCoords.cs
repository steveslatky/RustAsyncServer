using System;
using UnityEngine;

/// <summary>
/// CheckerCoords is a lot like Vector2, but it uses integer coordinates
/// and a couple other checkers-specific properties like diagonal number.
/// </summary>
public class CheckerCoords : IEquatable<CheckerCoords> {
    /// <summary>
    /// How many pieces in a row of the checkerboard.
    /// </summary>
    private const int PIECES_PER_ROW = 4;

    /// <summary>
    /// Spacing between valid columns horizontally.
    /// </summary>
    private const int COLUMN_SPACING = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckerCoords"/> class.
    /// </summary>
    /// <param name="row">The row index</param>
    /// <param name="col">The column index</param>
    public CheckerCoords(int row, int col) {
        this.row = row;
        this.col = col;
    }

    /// <summary>
    /// Gets or sets the row index
    /// </summary>
    /// <value>The row index</value>
    public int row { get; set; }

    /// <summary>
    /// Gets or sets the column index
    /// </summary>
    /// <value>The column index</value>
    public int col { get; set; }

    /// <summary>
    /// Gets the diagonal number of the square.
    /// Each diagonal on a 2D grid can be identified by the sum of
    /// row and column. This is useful for determining which squares
    /// on the checkerboard are valid.
    /// </summary>
    /// <value>The diagonal.</value>
    public int diagonal { 
        get {
            return row + col;
        }
    }

    /// <summary>
    /// perform vector addition
    /// left + right = (left.row + right.row, left.col + right.col)
    /// </summary>
    /// <param name="left">first location</param>
    /// <param name="right">second location </param>
    /// <returns>A new CheckerCoords instance with the sum</returns>
    public static CheckerCoords operator +(
            CheckerCoords left, 
            CheckerCoords right) {
        var row = left.row + right.row;
        var col = left.col + right.col;
        return new CheckerCoords(row, col);
    }

    /// <summary>
    /// perform vector subtraction
    /// left - right = (left.row - right.row, left.col - right.col)
    /// </summary>
    /// <param name="left">first location</param>
    /// <param name="right">second location</param>
    /// <returns>
    /// A new coordinate vector containing the vector difference
    /// </returns>
    public static CheckerCoords operator -(
            CheckerCoords left, 
            CheckerCoords right) {
        var row = left.row - right.row;
        var col = left.col - right.col;
        return new CheckerCoords(row, col);
    }

    /// <summary>
    /// perform scalar multiplication of the coordinate vector
    /// scalar * coords = (scalar * coords.row, scalar * coords.col)
    /// </summary>
    /// <param name="scalar">A scalar multiple</param>
    /// <param name="coords">coordinate vector</param>
    /// <returns>A new scaled coordinate vector</returns>
    public static CheckerCoords operator *(int scalar, CheckerCoords coords) {
        var row = scalar * coords.row;
        var col = scalar * coords.col;
        return new CheckerCoords(row, col);
    }

    /// <summary>
    /// Make scalar multiplication commutative:
    /// coords * scalar = scalar * coords
    /// </summary>
    /// <param name="coords">coordinate vector</param>
    /// <param name="scalar">a scalar multiple</param>
    /// <returns>A new scaled coordinate vector</returns>
    public static CheckerCoords operator *(CheckerCoords coords, int scalar) {
        return scalar * coords;
    }  

    /// <summary>
    /// for convenience, define scalar division:
    /// coords / divisor = (coords.row / divisor, coords.col / divisor)
    /// Note that this performs integer division!
    /// This is mainly used in midpoint()
    /// </summary>
    /// <param name="coords">The vector to divide</param>
    /// <param name="divisor">The divisor as an integer</param>
    /// <returns>A new scaled coordinate vector</returns>
    public static CheckerCoords operator /(CheckerCoords coords, int divisor) {
        var row = coords.row / divisor;
        var col = coords.col / divisor;
        return new CheckerCoords(row, col);
    }

    /// <summary>
    /// Find the midpoint between locations a and b.
    /// This uses integer division, so use with care!
    /// The main use is determining where the enemy piece is
    /// when finding capture moves.
    /// </summary>
    /// <param name="a">the start location</param>
    /// <param name="b">the end location</param>
    /// <returns>a new vector containing the midpoint</returns>
    public static CheckerCoords midpoint(CheckerCoords a, CheckerCoords b) {
        return (a + b) / 2;
    }

    /// <summary>
    /// Convert from a square ID (from 1-32)
    /// to (row, col) coordinates. See the checkerboard specifications
    /// for more information about why this algorithm works.
    /// </summary>
    /// <returns>The coordinates</returns>
    /// <param name="square_id">Square identifier (1-32)</param>
    public static CheckerCoords to_coords(int square_id) {
        // Convert from 1-indexed to 0-indexed
        var index = square_id - 1;

        // Calculate the row number. This is the typical calculation
        // when converting from 1D coords -> 2D coords
        var row = index / PIECES_PER_ROW;

        // Calculate the column number if all the valid squares
        // were lined up vertically. We'll shift every other row
        // in a moment.
        var col = COLUMN_SPACING * (index % PIECES_PER_ROW);

        // Even-numbered rows need to have their columns shifted 1
        // to the right
        if (row % 2 == 0)
            col++;

        return new CheckerCoords(row, col);
    }

    /// <summary>
    /// Convert from (row, col) coordinates to a square ID (1-32)
    /// See the checkerboard specifications for more information about
    /// why this algorithm works.
    /// </summary>
    /// <returns>The square identifier.</returns>
    /// <param name="coords">Coordinates in the grid.</param>
    public static int to_square_id(CheckerCoords coords) {
        var row = coords.row;
        var col = coords.col;

        // make the columns line up in a rectangle
        if (row % 2 == 0)
            col--;

        // Ignore the spacing between squares horizontally
        col /= COLUMN_SPACING;

        // convert from 2D coordinates -> 1D coordinates (row-major)
        var index = (row * PIECES_PER_ROW) + col;
    
        // Convert from 0-indexing -> 1-indexing
        return index + 1;
    }

    /// <summary>
    /// Serialize the coordinates to a short string. The square ID (an number
    /// from 1-32) is calculated, then cast to a string
    /// </summary>
    /// <param name="coords">The checker coordinates to serialize</param>
    /// <returns>A string containing a number 1-32</returns>
    public static string serialize(CheckerCoords coords) {
        return to_square_id(coords).ToString();
    }

    /// <summary>
    /// Deserialize the specified location from a string.
    /// The string must contain an integer in the range [1, 32]
    /// </summary>
    /// <param name="location">The serialized string</param>
    /// <returns>The decoded location as an object</returns>
    public static CheckerCoords deserialize(string location) {
        var square_id = int.Parse(location);
        return to_coords(square_id);
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current 
    /// <see cref="CheckerCoords"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current 
    /// <see cref="CheckerCoords"/>.</returns>
    public override string ToString() {
        return "(" + row + ", " + col + ")";
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="CheckerCoords"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in 
    /// hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode() {
        var index = (row * Checkerboard.BOARD_SIZE) + col;
        return index.GetHashCode();
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to
    /// the current <see cref="CheckerCoords"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the 
    /// current <see cref="CheckerCoords"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is 
    /// equal to the current <see cref="CheckerCoords"/>;
    /// otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj) {
        return obj is CheckerCoords && Equals((CheckerCoords)obj);
    }

    /// <summary>
    /// Determines whether the specified <see cref="CheckerCoords"/> is equal to
    /// the current <see cref="CheckerCoords"/>.
    /// </summary>
    /// <param name="other">The <see cref="CheckerCoords"/> to compare with the 
    /// current <see cref="CheckerCoords"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="CheckerCoords"/> is 
    /// equal to the current <see cref="CheckerCoords"/>;
    /// otherwise, <c>false</c>.</returns>
    public bool Equals(CheckerCoords other) {
        return row == other.row && col == other.col;
    }
}