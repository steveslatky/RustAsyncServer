using System;

/// <summary>
/// A piece is a simple data type that represents a single playing piece
/// in memory. It keeps track of the player who owns the piece and whether
/// the piece is a king.
/// </summary>
public class Piece {
    /// <summary>
    /// Initializes a new instance of the <see cref="Piece"/> class.
    /// </summary>
    /// <param name="player">Player (North/South)</param>
    /// <param name="is_king">True for king, false for basic piece</param>
    public Piece(PlayerType player, bool is_king) {
        this.player = player;
        this.is_king = is_king;
    }

    /// <summary>
    /// Constructor from a string piece descriptor.
    /// </summary>
    /// <param name="descriptor">
    /// Piece Descriptor. One of the following:
    /// n - basic piece for player North
    /// N - king piece for player North
    /// s - basic piece for player South
    /// S - king piece for player South
    /// </param>
    public Piece(string descriptor) {
        // Descriptors must be length 1
        // DISCUSS: String of length 1 or char?
        if (descriptor.Length != 1)
            throw new ArgumentException("Descriptor must be length 1!");

        // If the descriptor is uppercase, it denotes a king.
        is_king = char.IsUpper(descriptor, 0);

        // Get the piece type
        if (descriptor.ToLower() == "n")
            player = PlayerType.North;
        else if (descriptor.ToLower() == "s")
            player = PlayerType.South;
        else
            throw new ArgumentException(
                "Descriptor must be one of {n, N, s, S}");
    }

    /// <summary>
    /// The player type (North or South)
    /// </summary>
    /// <value>The player.</value>
    public PlayerType player { get; set; }

    /// <summary>
    /// True if this is a king piece. King pieces can move backwards
    /// as well as forwards.
    /// </summary>
    /// <value>
    ///     <c>true</c> if is king; 
    ///     <c>false</c> marks a basic piece.
    /// </value>
    public bool is_king { get; set; }

    /// <summary>
    /// Return a descriptor string. See the constructor that
    /// takes a descriptor for more information
    /// </summary>
    /// <value>The descriptor, one of {n, N, s, S}</value>
    public string descriptor {
        get {
            // Get the letter of the player's side
            string d;
            if (player == PlayerType.North)
                d = "n";
            else
                d = "s";

            // Kings are uppercase
            if (is_king)
                return d.ToUpper();
            else
                return d;
        }
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current 
    /// <see cref="Piece"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current 
    /// <see cref="Piece"/>.</returns>
    public override string ToString() {
        return descriptor;
    }
}