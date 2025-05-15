using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020004F2 RID: 1266
public static class ChessStringHandler
{
	// Token: 0x06001D09 RID: 7433 RVA: 0x000F3768 File Offset: 0x000F1968
	public static void LogMatchHistory(List<string> matchHistory)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string text in matchHistory)
		{
			stringBuilder.AppendLine(text);
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x000F37C8 File Offset: 0x000F19C8
	public static string GenerateFenString(ChessPieceData[] board, bool isWhiteTurn, string castlingAvailability, string enPassantTarget, int halfmoveClock, int fullmoveNumber)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 7; i >= 0; i--)
		{
			int num = 0;
			for (int j = 0; j < 8; j++)
			{
				int num2 = i * 8 + j;
				ChessPieceData chessPieceData = board[num2];
				if (chessPieceData == null)
				{
					num++;
				}
				else
				{
					if (num != 0)
					{
						stringBuilder.Append(num);
						num = 0;
					}
					char fenCharForPiece = ChessStringHandler.GetFenCharForPiece(chessPieceData);
					stringBuilder.Append(fenCharForPiece);
				}
			}
			if (num != 0)
			{
				stringBuilder.Append(num);
			}
			if (i > 0)
			{
				stringBuilder.Append('/');
			}
		}
		stringBuilder.Append(isWhiteTurn ? " w " : " b ");
		stringBuilder.Append(castlingAvailability);
		stringBuilder.Append(" ");
		stringBuilder.Append(string.IsNullOrWhiteSpace(enPassantTarget) ? "-" : enPassantTarget);
		stringBuilder.Append(" ");
		stringBuilder.Append(halfmoveClock);
		stringBuilder.Append(" ");
		stringBuilder.Append(fullmoveNumber);
		return stringBuilder.ToString();
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x000F38B4 File Offset: 0x000F1AB4
	private static char GetFenCharForPiece(ChessPieceData piece)
	{
		char c = '0';
		switch (piece.type)
		{
		case ChessPieceData.PieceType.Pawn:
			c = 'p';
			break;
		case ChessPieceData.PieceType.Rook:
			c = 'r';
			break;
		case ChessPieceData.PieceType.Knight:
			c = 'n';
			break;
		case ChessPieceData.PieceType.Bishop:
			c = 'b';
			break;
		case ChessPieceData.PieceType.Queen:
			c = 'q';
			break;
		case ChessPieceData.PieceType.King:
			c = 'k';
			break;
		default:
			Debug.LogError("Received an invalid piece type from the chess engine");
			break;
		}
		if (!piece.isWhite)
		{
			return c;
		}
		return char.ToUpper(c);
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x000F3924 File Offset: 0x000F1B24
	public static string CalculateCastlingAvailability(ChessPieceData[] board)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		bool flag4 = true;
		bool flag5 = true;
		bool flag6 = true;
		foreach (ChessPieceData chessPieceData in board)
		{
			if (chessPieceData != null)
			{
				if (chessPieceData.type == ChessPieceData.PieceType.King)
				{
					if (chessPieceData.isWhite)
					{
						flag = chessPieceData.timesMoved != 0;
					}
					else
					{
						flag2 = chessPieceData.timesMoved != 0;
					}
				}
				else if (chessPieceData.type == ChessPieceData.PieceType.Rook)
				{
					if (chessPieceData.isWhite)
					{
						if (chessPieceData.queenSide)
						{
							flag4 = chessPieceData.timesMoved == 0;
						}
						else
						{
							flag3 = chessPieceData.timesMoved == 0;
						}
					}
					else if (chessPieceData.queenSide)
					{
						flag6 = chessPieceData.timesMoved == 0;
					}
					else
					{
						flag5 = chessPieceData.timesMoved == 0;
					}
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (!flag)
		{
			if (flag3)
			{
				stringBuilder.Append("K");
			}
			if (flag4)
			{
				stringBuilder.Append("Q");
			}
		}
		if (!flag2)
		{
			if (flag5)
			{
				stringBuilder.Append("k");
			}
			if (flag6)
			{
				stringBuilder.Append("q");
			}
		}
		if (stringBuilder.Length <= 0)
		{
			return "-";
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x000F3A58 File Offset: 0x000F1C58
	public static string ConvertToChessNotation(int2 position)
	{
		char c = (char)(97 + position.x);
		int num = position.y + 1;
		return string.Format("{0}{1}", c, num);
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000F3A90 File Offset: 0x000F1C90
	public static int2 ConvertFromChessNotation(string notation)
	{
		if (notation.Length < 2)
		{
			Debug.LogError("Invalid chess notation");
		}
		int num = (int)notation[0];
		int num2 = int.Parse(notation.Substring(1));
		int num3 = num - 97;
		int num4 = num2 - 1;
		return new int2(num3, num4);
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x000F3AD4 File Offset: 0x000F1CD4
	[return: TupleElementNames(new string[] { "start", "end", "promotionType" })]
	public static ValueTuple<int2, int2, ChessPieceData.PieceType> ProcessFullMove(string move)
	{
		if (string.IsNullOrWhiteSpace(move) || move.Length < 4)
		{
			Debug.LogError("Got invalid move from bot");
		}
		string text = move.Substring(0, 2);
		string text2 = move.Substring(2, 2);
		if (move.Contains("none"))
		{
			Debug.LogError("Bot found move: " + move);
		}
		int2 @int = ChessStringHandler.ConvertFromChessNotation(text);
		int2 int2 = ChessStringHandler.ConvertFromChessNotation(text2);
		ChessPieceData.PieceType pieceType = ChessPieceData.PieceType.Pawn;
		if (move.Length > 4)
		{
			char c = move[4];
			if (c != 'b')
			{
				switch (c)
				{
				case 'n':
					pieceType = ChessPieceData.PieceType.Knight;
					break;
				case 'q':
					pieceType = ChessPieceData.PieceType.Queen;
					break;
				case 'r':
					pieceType = ChessPieceData.PieceType.Rook;
					break;
				}
			}
			else
			{
				pieceType = ChessPieceData.PieceType.Bishop;
			}
		}
		return new ValueTuple<int2, int2, ChessPieceData.PieceType>(@int, int2, pieceType);
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000F3B88 File Offset: 0x000F1D88
	public static string UCIMove(ChessManager.MoveData moveData)
	{
		string text = ChessStringHandler.ConvertToChessNotation(moveData.StartPosition) + ChessStringHandler.ConvertToChessNotation(moveData.EndPosition);
		char c = 'p';
		switch (moveData.PromotionType)
		{
		case ChessPieceData.PieceType.Rook:
			c = 'r';
			break;
		case ChessPieceData.PieceType.Knight:
			c = 'n';
			break;
		case ChessPieceData.PieceType.Bishop:
			c = 'b';
			break;
		case ChessPieceData.PieceType.Queen:
			c = 'q';
			break;
		}
		if (moveData.PromotionType != ChessPieceData.PieceType.Pawn)
		{
			text += c.ToString();
		}
		return text;
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x000F3C00 File Offset: 0x000F1E00
	public static string LogPerft(ChessManager.MoveData moveData, int subsequentMoves = 0)
	{
		string text = ChessStringHandler.ConvertToChessNotation(moveData.StartPosition) + ChessStringHandler.ConvertToChessNotation(moveData.EndPosition);
		char c = 'p';
		switch (moveData.PromotionType)
		{
		case ChessPieceData.PieceType.Rook:
			c = 'r';
			break;
		case ChessPieceData.PieceType.Knight:
			c = 'n';
			break;
		case ChessPieceData.PieceType.Bishop:
			c = 'b';
			break;
		case ChessPieceData.PieceType.Queen:
			c = 'q';
			break;
		}
		if (moveData.PromotionType != ChessPieceData.PieceType.Pawn)
		{
			text += c.ToString();
		}
		return text + string.Format(": {0}", subsequentMoves);
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000F3C90 File Offset: 0x000F1E90
	public static void LogMoveData(ChessManager.MoveData moveData, int subsequentMoves = 0)
	{
		string text = ChessStringHandler.ConvertToChessNotation(moveData.StartPosition) + ChessStringHandler.ConvertToChessNotation(moveData.EndPosition);
		if (subsequentMoves > 0)
		{
			text += string.Format(" {0}", subsequentMoves);
		}
		Debug.Log(string.Concat(new string[]
		{
			text,
			"\nMove Data:\n",
			string.Format("Piece Type: {0}\n", moveData.PieceToMove.type),
			string.Format("Start Position: {0}\n", moveData.StartPosition),
			"Color: ",
			moveData.PieceToMove.isWhite ? "White" : "Black",
			"\n",
			string.Format("End Position: {0}\n", moveData.EndPosition),
			"Capture Piece: ",
			(moveData.CapturePiece != null) ? moveData.CapturePiece.type.ToString() : "None",
			"\n",
			string.Format("Castle State: {0}", moveData.SpecialMove)
		}));
	}
}
