class Piece:
    COLOR_RED = 0
    COLOR_WHITE = 1

    def __init__(self, color, is_king=False):
        self.color = color
        self.is_king = is_king

    def __repr__(self):
        letter = 'r' if self.color == self.COLOR_RED else 'w'
        if self.is_king:
            return letter.upper()
        else:
            return letter

    @classmethod
    def from_descriptor(cls, desc):
        is_king = desc.isupper()
        color = cls.COLOR_RED if desc.lower() == 'r' else cls.COLOR_WHITE
        return cls(color, is_king)
