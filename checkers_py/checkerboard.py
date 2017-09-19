from GridVector import GridVector
from Piece import Piece
from Move import Move

class Checkerboard:
    BOARD_SIZE = 8
    NW = GridVector(-1, -1)
    NE = GridVector(-1, 1)
    SW = GridVector(1, -1)
    SE = GridVector(1, 1)
    
    def __init__(self, description=None):
        """
        description: list of pairs (square_id, piece_descriptor)
        where piece_descriptor is one of
        r, R, w or W for red, red king, white, white king
        """
        self.grid = [
            [None for j in range(self.BOARD_SIZE)]
            for y in range(self.BOARD_SIZE)]

        if description:
            self.create_from_description(description)
        else:
            self.create_pieces()

    def create_from_description(self, desc):
        for square_id, piece_descriptor in desc:
            loc = GridVector.to_coords(square_id)
            piece = Piece.from_descriptor(piece_descriptor)
            self.put_down(loc, piece)

    def create_pieces(self):
        for i in range(self.BOARD_SIZE):
            for j in range(self.BOARD_SIZE):
                loc = GridVector(i, j)
                # red pieces always go on the first three rows
                if i < 3 and self.is_valid(loc):
                    self.put_down(loc, Piece(Piece.COLOR_RED))
                # white pieces always go on the bottom.
                elif i >= self.BOARD_SIZE - 3 and self.is_valid(loc):
                    self.put_down(loc, Piece(Piece.COLOR_WHITE))

    def is_valid(self, loc):
        return self.in_grid(loc) and loc.diagonal % 2 == 1

    def is_empty(self, loc):
        return self.get(loc) is None

    def in_grid(self, loc):
        return (
            0 <= loc.row < self.BOARD_SIZE 
            and 0 <= loc.col < self.BOARD_SIZE)

    def put_down(self, loc, piece):
        self.grid[loc.row][loc.col] = piece

    def get(self, loc):
        return self.grid[loc.row][loc.col]

    def pick_up(self, loc):
        piece = self.get(loc)
        self.put_down(loc, None)
        return piece

    def find_pieces(self, color):
        locations = []
        for i, row in enumerate(self.grid):
            for j, piece in enumerate(row):
                if piece and piece.color == color:
                    locations.append(GridVector(i, j))
        return locations

    def basic_move(self, from_loc, to_loc):
        piece = self.pick_up(from_loc)
        self.put_down(to_loc, piece) 

    def capture_move(self, from_loc, to_loc):
        enemy_loc = GridVector.midpoint(from_loc, to_loc)
        self.basic_move(from_loc, to_loc)
        self.pick_up(enemy_loc)

    def find_basic_moves(self, from_loc):
        piece = self.get(from_loc)
        north_facing = piece.color == Piece.COLOR_WHITE
        moves = []
        #TODO: Refactor me!
        if north_facing or piece.is_king:
            for offset in [self.NW, self.NE]:
                to_loc = from_loc + offset
                if self.is_valid(to_loc) and self.is_empty(to_loc):
                    moves.append(Move(from_loc, to_loc, Move.MOVE_BASIC))
        if not north_facing or piece.is_king:
            for offset in [self.SW, self.SE]:
                to_loc = from_loc + offset
                if self.is_valid(to_loc) and self.is_empty(to_loc):
                    moves.append(Move(from_loc, to_loc, Move.MOVE_BASIC)) 
        return moves

    def find_capture_moves(self, from_loc):
        piece = self.get(from_loc)
        north_facing = piece.color == Piece.COLOR_WHITE
        moves = []
        #TODO: Refactor me!
        if north_facing or piece.is_king:
            for offset in [self.NW, self.NE]:
                to_loc = from_loc + 2 * offset
                if not self.is_valid(to_loc) and self.is_empty(to_loc):
                    continue
                enemy_loc = GridVector.midpoint(from_loc, to_loc)
                enemy = self.get(enemy_loc)
                if enemy is not None and enemy.color != piece.color:
                    moves.append(Move(from_loc, to_loc, Move.MOVE_CAPTURE))
        if not north_facing or piece.is_king:
            for offset in [self.SW, self.SE]:
                to_loc = from_loc + 2 * offset
                if not self.is_valid(to_loc) and self.is_empty(to_loc):
                    continue
                enemy_loc = GridVector.midpoint(from_loc, to_loc)
                enemy = self.get(enemy_loc)
                if enemy is not None and enemy.color != piece.color:
                    moves.append(Move(from_loc, to_loc, Move.MOVE_CAPTURE))

        return moves

    def find_all_basic_moves(self, color):
        locs = self.find_pieces(color)
        results = []
        for loc in locs:
            results.extend(self.find_basic_moves(loc))
        return results

    def find_all_capture_moves(self, color):
        locs = self.find_pieces(color)
        results = []
        for loc in locs:
            results.extend(self.find_capture_moves(loc))
        return results

    def find_all_moves(self, color):
        return (
            self.find_all_basic_moves(color) 
            + self.find_all_capture_moves(color))

    def __str__(self):
        output = ["  0 1 2 3 4 5 6 7"]
        for i, row in enumerate(self.grid):
            row_str = ""
            for piece in row:
                if piece is None:
                    row_str += ". "
                else:
                    row_str += "{} ".format(piece)
            output.append("{} {}".format(i, row_str))
        return "\n".join(output)

if __name__ == "__main__":
    board = Checkerboard()
    print(board)
    print("--------")
    print("Red Moves:")
    print(board.find_all_basic_moves(Piece.COLOR_RED))
    print("White Moves:")
    print(board.find_all_basic_moves(Piece.COLOR_WHITE))
    
    board2 = Checkerboard([
        (14, 'r'), 
        (18, 'w'),
        (22, 'w'),
        (27, 'w'),
        (7, 'r')])
    print(board2)
    print(board2.find_all_moves(Piece.COLOR_RED))
    print([x.to_notation() for x in board2.find_all_moves(Piece.COLOR_RED)])
