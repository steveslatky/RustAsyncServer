using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
	[Category("CheckerCoords Tests")]
	internal class CheckerCoordsTests
	{
		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsDiagonal()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual(3, cc.diagonal);
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsToString()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual("(2, 1)", cc.ToString());
		}

		//GetHashCode

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsEqualsObjF()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			CheckerCoords cc2 = new CheckerCoords(2, 1);
			object move = new Move(cc, cc2);
			Assert.AreEqual(false, cc.Equals(move));
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsEqualsObjT()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			object cc2 = new CheckerCoords(2, 1);
			Assert.AreEqual(true, cc.Equals(cc2));
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsEqualsRowCol()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			CheckerCoords cc2 = new CheckerCoords(2, 1);
			Assert.AreEqual(true, cc.Equals(cc2));
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsAdd()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			CheckerCoords cc2 = new CheckerCoords(3, 2);
			Assert.AreEqual(new CheckerCoords(5, 3), cc + cc2);
		}
			
		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsSubtract()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			CheckerCoords cc2 = new CheckerCoords(3, 2);
			Assert.AreEqual(new CheckerCoords(1, 1), cc2 - cc);
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsMultiply()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual(new CheckerCoords(4, 2), 2 * cc);
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsMultiply2()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual(new CheckerCoords(4, 2), cc * 2);
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsDivide()
		{
			CheckerCoords cc = new CheckerCoords(4, 2);
			Assert.AreEqual(new CheckerCoords(2, 1), cc / 2);
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsMidpoint()
		{
			CheckerCoords cc = new CheckerCoords(4, 2);
			CheckerCoords cc2 = new CheckerCoords(1, 2);
			Assert.AreEqual(new CheckerCoords(2, 2), CheckerCoords.midpoint(cc, cc2));
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsToCoords()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual(cc, CheckerCoords.to_coords(9));
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsToSquareId()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual(9, CheckerCoords.to_square_id(cc));
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsSerialize()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual("9", CheckerCoords.serialize(cc));
		}

		[Test]
		[Category("CheckerCoords Tests")]
		public void TestCheckerCoordsDeserialize()
		{
			CheckerCoords cc = new CheckerCoords(2, 1);
			Assert.AreEqual(cc, CheckerCoords.deserialize("9"));
		}
    }

	[TestFixture]
	[Category("Piece Tests")]
	internal class PieceTests
	{
		//piece constructor?

		[Test]
		[Category("Piece Tests")]
		public void TestPieceDescriptor()
		{
			Piece p = new Piece(PlayerType.North, true);
			Assert.AreEqual("N", p.descriptor);
		}

		[Test]
		[Category("Piece Tests")]
		public void TestPieceToString()
		{
			Piece p = new Piece(PlayerType.South, false);
			Assert.AreEqual("s", p.ToString());
		}
	}

	[TestFixture]
	[Category("Move Tests")]
	internal class MoveTests
	{
		[Test]
		[Category("Move Tests")]
		public void WhichPlayer(){
			Move pieceMove = new Move(CheckerCoords.to_coords(23), CheckerCoords.to_coords(19));

			Assert.AreEqual(pieceMove.player, PlayerType.South);

			Move otherpieceMove = new Move(CheckerCoords.to_coords(10), CheckerCoords.to_coords(15));

			Assert.AreEqual(otherpieceMove.player, PlayerType.North);
		}

		[Test]
		[Category("Move Tests")]
		public void IsCrowning(){
			Move pieceMove = new Move(CheckerCoords.to_coords(6), CheckerCoords.to_coords(1));

			Assert.IsTrue(pieceMove.is_crowning);

			Move otherpieceMove = new Move(CheckerCoords.to_coords(25), CheckerCoords.to_coords(29));

			Assert.IsTrue(otherpieceMove.is_crowning);
		}

		[Test]
		[Category("Move Tests")]
		public void EnemyLocation(){
			CheckerCoords pieceLoc = CheckerCoords.to_coords(19);
			CheckerCoords pieceDstLoc = CheckerCoords.to_coords(12);
			Move pieceMove = new Move(pieceLoc, pieceDstLoc);

			Assert.AreEqual(pieceMove.enemy_location, CheckerCoords.to_coords(16));
		}


		[Test]
		[Category("Move Tests")]
		public void MoveSerialize()
		{
			CheckerCoords cc = new CheckerCoords(2, 0);
			CheckerCoords cc2 = new CheckerCoords(3, 2);
			Move m = new Move(cc, cc2);
			Assert.AreEqual("9-14", m.ToString());
		}

		[Test]
		[Category("Move Tests")]
		public void MoveDeserialize(){
			string moveStr = "2-6";
			var deserialized_move_object = Move.deserialize(moveStr);
			var src = CheckerCoords.deserialize("2");
			var dst = CheckerCoords.deserialize("6");
			Move moveObject = new Move(src, dst);
			Assert.AreEqual(moveObject.ToString(), deserialized_move_object.ToString());
		}

		[Test]
		[Category("Move Tests")]
		public void MoveDeserializeSeqFirst(){
			string movesString = "2-6,9-14";
			Assert.AreEqual(Move.deserialize_seq_first(movesString).ToString(), "2-6");

		}

		[Test]
		[Category("Move Tests")]
		public void MoveDeserializeSeqLast(){
			string movesString = "2-6,9-14";
			Assert.AreEqual(Move.deserialize_seq_last(movesString).ToString(), "9-14");

		}


		[Test]
		[Category("Move Tests")]
		public void DeserializeSequence(){
			string movesString = "2-6,9-14";
			Move firstMove = new Move(CheckerCoords.to_coords(2), CheckerCoords.to_coords(6));
			Move secondMove = new Move(CheckerCoords.to_coords(9), CheckerCoords.to_coords(14));

			var listMoves = new List<Move>();
			listMoves.Add(firstMove);
			listMoves.Add(secondMove);

			var allMoves = Move.deserialize_seq(movesString);

			for(int i = 0; i < listMoves.Count; i++){
				Assert.AreEqual(allMoves[i].ToString(), listMoves[i].ToString());
			}

		}

		[Test]
		[Category("Move Tests")]
		public void SerializeSequence(){
			string movesString = "2-6,9-14";
			Move firstMove = new Move(CheckerCoords.to_coords(2), CheckerCoords.to_coords(6));
			Move secondMove = new Move(CheckerCoords.to_coords(9), CheckerCoords.to_coords(14));

			var listMoves = new List<Move>();
			listMoves.Add(firstMove);
			listMoves.Add(secondMove);

			var allMovesStr = Move.serialize_seq(listMoves);

			Assert.AreEqual(allMovesStr, movesString);

		}
	}

	[TestFixture]
	[Category("Checkerboard Tests")]
	internal class CheckerboardTests
	{
		[Test]
		[Category("Checkerboard Tests")]
		public void InitializeBoard(){
			Checkerboard board = new Checkerboard();

			// check north
			for(int i = 1; i <= 12; i++){
				var loc = CheckerCoords.to_coords(i);
				Assert.AreEqual(board[loc].ToString(), "n");
			}

			// check south
			for(int i = 21; i <= 32; i++){
				var loc = CheckerCoords.to_coords(i);
				Assert.AreEqual(board[loc].ToString(), "s");
			}
		}

		[Test]
		[Category("Checkerboard Tests")]
		public void ValidBoardLocation(){
			Checkerboard board = new Checkerboard();

			// Valid Location
			CheckerCoords ValidCoord = CheckerCoords.to_coords(2);

			Assert.IsTrue(board.is_valid(ValidCoord));

			CheckerCoords invalidCoord = CheckerCoords.to_coords(35);

			Assert.AreEqual(board.is_valid(invalidCoord), false);

		}

		[Test]
		[Category("Checkerboard Tests")]
		public void BoardLocationEmpty(){
			Checkerboard board = new Checkerboard();

			Assert.IsTrue(board.is_empty(CheckerCoords.to_coords(18)));

			Assert.AreEqual(board.is_empty(CheckerCoords.to_coords(26)), false);
		}

		[Test]
		[Category("Checkerboard Tests")]
		public void RemoveBoardPiece(){
			Checkerboard board = new Checkerboard();

			// remove north piece
			Piece removedNPiece = board.remove(CheckerCoords.to_coords(8));

			Assert.AreEqual(removedNPiece.ToString(), "n");

			// remove south piece

			Piece removedSPiece = board.remove(CheckerCoords.to_coords(26));

			Assert.AreEqual(removedSPiece.ToString(), "s");
		}

		[Test]
		[Category("Checkerboard Tests")]
		public void FindingAllPieceLocations(){
			Checkerboard board = new Checkerboard();

			var nLocs = new List<CheckerCoords>();

			for(int i = 1; i <= 12; i++) {
				nLocs.Add(CheckerCoords.to_coords(i));
			}

			Assert.AreEqual(board.find_piece_locations(PlayerTypeUtils.deserialize("N")), nLocs);

			var sLocs = new List<CheckerCoords>();

			for(int i = 21; i <= 32; i++) {
				sLocs.Add(CheckerCoords.to_coords(i));
			}

			Assert.AreEqual(board.find_piece_locations(PlayerTypeUtils.deserialize("S")), sLocs);
		}

		[Test]
		[Category("Checkerboard Tests")]
		public void PerformMove(){
			Checkerboard board = new Checkerboard();

			// Move south piece from 22 -> 18

			Assert.AreEqual(board[CheckerCoords.to_coords(22)].ToString(), "s");

			var srcLoc = CheckerCoords.to_coords(22);
			var dstLoc = CheckerCoords.to_coords(18);
			Move moveObject = new Move(srcLoc, dstLoc);
			board.perform_move(moveObject);

			Assert.IsNull(board[srcLoc]);

			Assert.AreEqual(board[dstLoc].ToString(), "s");

			// Move enemy piece to setup capture

			Assert.AreEqual(board[CheckerCoords.to_coords(9)].ToString(), "n");


			var enemySrcLoc = CheckerCoords.to_coords(9);
			var enemyDstLoc = CheckerCoords.to_coords(14);
			Move enemyMove = new Move(enemySrcLoc, enemyDstLoc);
			board.perform_move(enemyMove);

			Assert.IsNull(board[enemySrcLoc]);
			Assert.AreEqual(board[enemyDstLoc].ToString(), "n");

			// Capture move
			Move captureMove = new Move(dstLoc, enemySrcLoc);
			board.perform_move(captureMove);

			Assert.IsNull(board[enemyDstLoc]);
			Assert.AreEqual(board[enemySrcLoc].ToString(), "s");

		}

		[Test]
		[Category("Checkerboard Tests")]
		public void FindLocBasicMoves(){
			Checkerboard board = new Checkerboard();
			CheckerCoords pieceLoc = CheckerCoords.to_coords(11);

			var possibleMoves = new List<Move>();
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(16)));
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(15)));

			var allMoves = board.find_basic_moves(pieceLoc);

			for(int i = 0; i < possibleMoves.Count; i++){
				Assert.AreEqual(allMoves[i].ToString(), possibleMoves[i].ToString());

			}

		}

		[Test]
		[Category("Checkerboard Tests")]
		public void FindLocCaptureMoves(){
			Checkerboard board = new Checkerboard();
			CheckerCoords pieceLoc = CheckerCoords.to_coords(23);
			Move pieceMove = new Move(pieceLoc, CheckerCoords.to_coords(19));

			board.perform_move(pieceMove);
			pieceLoc = CheckerCoords.to_coords(19);

			var possibleMoves = new List<Move>();
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(12)));
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(10)));

			var enemySrcLoc1 = CheckerCoords.to_coords(10);
			var enemyDstLoc1 = CheckerCoords.to_coords(15);
			Move enemyMove1 = new Move(enemySrcLoc1, enemyDstLoc1);

			var enemySrcLoc2 = CheckerCoords.to_coords(12);
			var enemyDstLoc2 = CheckerCoords.to_coords(16);
			Move enemyMove2 = new Move(enemySrcLoc2, enemyDstLoc2);

			board.perform_move(enemyMove1);
			board.perform_move(enemyMove2);

			var allMoves = board.find_capture_moves(pieceLoc);

			for(int i = 0; i < possibleMoves.Count; i++){
				Assert.AreEqual(allMoves[i].ToString(), possibleMoves[i].ToString());

			}

		}

		[Test]
		[Category("Checkerboard Tests")]
		public void FindPlayerBasicMoves(){
			Checkerboard board = new Checkerboard();

			Assert.AreEqual(board.find_all_basic_moves(PlayerTypeUtils.deserialize("N")).Count, 7);

			Assert.AreEqual(board.find_all_basic_moves(PlayerTypeUtils.deserialize("S")).Count, 7);

			CheckerCoords pieceLoc = CheckerCoords.to_coords(9);
			Move pieceMove = new Move(pieceLoc, CheckerCoords.to_coords(14));

			board.perform_move(pieceMove);

			Assert.AreEqual(board.find_all_basic_moves(PlayerTypeUtils.deserialize("N")).Count, 8);


		}

		[Test]
		[Category("Checkerboard Tests")]
		public void FindPlayerCaptureMoves(){
			Checkerboard board = new Checkerboard();
			CheckerCoords pieceLoc = CheckerCoords.to_coords(23);
			Move pieceMove = new Move(pieceLoc, CheckerCoords.to_coords(19));

			board.perform_move(pieceMove);
			pieceLoc = CheckerCoords.to_coords(19);

			var possibleMoves = new List<Move>();
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(12)));
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(10)));

			var enemySrcLoc1 = CheckerCoords.to_coords(10);
			var enemyDstLoc1 = CheckerCoords.to_coords(15);
			Move enemyMove1 = new Move(enemySrcLoc1, enemyDstLoc1);

			var enemySrcLoc2 = CheckerCoords.to_coords(12);
			var enemyDstLoc2 = CheckerCoords.to_coords(16);
			Move enemyMove2 = new Move(enemySrcLoc2, enemyDstLoc2);

			board.perform_move(enemyMove1);
			board.perform_move(enemyMove2);

			var allSouthCaptureMoves = board.find_all_capture_moves(PlayerTypeUtils.deserialize("S"));

			Assert.AreEqual(allSouthCaptureMoves.Count, 2);

			var allNorthCaptureMoves = board.find_all_capture_moves(PlayerTypeUtils.deserialize("N"));

			Assert.AreEqual(allNorthCaptureMoves.Count, 1);


		}

		[Test]
		[Category("Checkerboard Tests")]
		public void FindPlayerAllMoves(){
			Checkerboard board = new Checkerboard();
			CheckerCoords pieceLoc = CheckerCoords.to_coords(23);
			Move pieceMove = new Move(pieceLoc, CheckerCoords.to_coords(19));

			board.perform_move(pieceMove);
			pieceLoc = CheckerCoords.to_coords(19);

			var possibleMoves = new List<Move>();
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(12)));
			possibleMoves.Add(new Move(pieceLoc, CheckerCoords.to_coords(10)));

			var enemySrcLoc1 = CheckerCoords.to_coords(10);
			var enemyDstLoc1 = CheckerCoords.to_coords(15);
			Move enemyMove1 = new Move(enemySrcLoc1, enemyDstLoc1);

			var enemySrcLoc2 = CheckerCoords.to_coords(12);
			var enemyDstLoc2 = CheckerCoords.to_coords(16);
			Move enemyMove2 = new Move(enemySrcLoc2, enemyDstLoc2);

			board.perform_move(enemyMove1);
			board.perform_move(enemyMove2);

			var allSouthMoves = board.find_all_moves(PlayerTypeUtils.deserialize("S"));
			Assert.AreEqual(allSouthMoves.Count, 8);


		}
	}

	[TestFixture]
	[Category("PlayerTypeUtils Tests")]
	internal class PlayerTypeUtilsTests
	{
		[Test]
		[Category("PlayerTypeUtils Tests")]
		public void SerializePlayerType(){
			Assert.AreEqual(PlayerTypeUtils.serialize(PlayerType.South), "S");
			Assert.AreEqual(PlayerTypeUtils.serialize(PlayerType.North), "N");
		}

		[Test]
		[Category("PlayerTypeUtils Tests")]
		public void DeserializePlayerType(){
			Assert.AreEqual(PlayerTypeUtils.deserialize("S"), PlayerType.South);
			Assert.AreEqual(PlayerTypeUtils.deserialize("N"), PlayerType.North);
		}

		[Test]
		[Category("PlayerTypeUtils Tests")]
		public void OtherPlayerType(){
			Assert.AreEqual(PlayerTypeUtils.other_player(PlayerType.South), PlayerType.North);
			Assert.AreEqual(PlayerTypeUtils.other_player(PlayerType.North), PlayerType.South);
		}
	}
	
}
