using System;

// Token: 0x020004F0 RID: 1264
public class ChessPieceData
{
	// Token: 0x06001D08 RID: 7432 RVA: 0x000F3743 File Offset: 0x000F1943
	public ChessPieceData(ChessPieceData.PieceType type, bool isWhite, bool queenSide)
	{
		this.type = type;
		this.isWhite = isWhite;
		this.queenSide = queenSide;
	}

	// Token: 0x04002929 RID: 10537
	public bool isWhite = true;

	// Token: 0x0400292A RID: 10538
	public int timesMoved;

	// Token: 0x0400292B RID: 10539
	public bool queenSide;

	// Token: 0x0400292C RID: 10540
	public bool autoControl;

	// Token: 0x0400292D RID: 10541
	public ChessPieceData.PieceType type;

	// Token: 0x020004F1 RID: 1265
	public enum PieceType
	{
		// Token: 0x0400292F RID: 10543
		Pawn,
		// Token: 0x04002930 RID: 10544
		Rook,
		// Token: 0x04002931 RID: 10545
		Knight,
		// Token: 0x04002932 RID: 10546
		Bishop,
		// Token: 0x04002933 RID: 10547
		Queen,
		// Token: 0x04002934 RID: 10548
		King
	}
}
