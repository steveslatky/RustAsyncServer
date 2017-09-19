class GridVector:
    PIECES_PER_ROW = 4
    def __init__(self, i, j):
        self.row = i
        self.col = j

    def __iter__(self):
        yield self.row
        yield self.col

    def __add__(self, other):
        row = self.row + other.row
        col = self.col + other.col
        return GridVector(row, col)

    def __sub__(self, other):
        row = self.row - other.row
        col = self.col - other.col
        return GridVector(row, col)

    def __rmul__(self, scalar):
        row = scalar * self.row
        col = scalar * self.col
        return GridVector(row, col)

    def __floordiv__(self, divisor):
        row = self.row // divisor
        col = self.col // divisor
        return GridVector(row, col)

    @property
    def diagonal(self):
        return self.row + self.col
    
    def __repr__(self):
        return "V({}, {})".format(self.row, self.col)

    @classmethod
    def midpoint(cls, x, y):
        return (x + y) // 2

    @classmethod
    def to_coords(cls, square_id):
        index = square_id - 1
        row = index // cls.PIECES_PER_ROW
        col_unshifted = 2 * (index % cls.PIECES_PER_ROW)
        col = col_unshifted + 1 if row % 2 == 0 else col_unshifted
        return cls(row, col)

    @classmethod
    def to_square_id(cls, loc):
        row = loc.row
        col = loc.col
        col_rect = col - 1 if loc.row % 2 == 0 else col
        scaled_col = col_rect // 2
        index = row * cls.PIECES_PER_ROW + scaled_col
        square_id = index + 1
        return square_id
