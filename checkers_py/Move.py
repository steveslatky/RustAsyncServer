from GridVector import GridVector

class Move:
    MOVE_BASIC = 0
    MOVE_CAPTURE = 1
    def __init__(self, from_loc, to_loc, move_type):
        self.src = from_loc
        self.dst = to_loc
        self.move_type = move_type

    def __repr__(self):
        if self.move_type == self.MOVE_BASIC:
            return "{} -> {}".format(self.src, self.dst)
        else:
            return "Capture {} -> {}".format(self.src, self.dst)

    def to_notation(self):
        src_sqr = GridVector.to_square_id(self.src)
        dst_sqr = GridVector.to_square_id(self.dst)
        if self.move_type == self.MOVE_BASIC:
            return "{}-{}".format(src_sqr, dst_sqr)
        else:
            return "{}x{}".format(src_sqr, dst_sqr)
