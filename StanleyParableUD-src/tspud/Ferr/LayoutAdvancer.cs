using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002EF RID: 751
	[Serializable]
	public class LayoutAdvancer
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x00067FE4 File Offset: 0x000661E4
		public float X
		{
			get
			{
				return this.mPos.x;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00067FF1 File Offset: 0x000661F1
		public float Y
		{
			get
			{
				return this.mPos.y;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x00067FFE File Offset: 0x000661FE
		public LayoutAdvancer.Direction Dir
		{
			get
			{
				return this.mDirection;
			}
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00068006 File Offset: 0x00066206
		public LayoutAdvancer(Vector2 aStartLocation, LayoutAdvancer.Direction aDirection)
		{
			this.mPos = aStartLocation;
			this.mDirection = aDirection;
			this.mPrevPos = aStartLocation;
			this.mPrevious = Vector2.zero;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0006802E File Offset: 0x0006622E
		public void Step(Vector2 aSize)
		{
			this.Step(aSize.x, aSize.y);
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00068044 File Offset: 0x00066244
		public void Step(float aX, float aY)
		{
			this.mPrevPos.x = this.mPos.x;
			this.mPrevPos.y = this.mPos.y;
			if (this.mDirection == LayoutAdvancer.Direction.Horizontal)
			{
				this.mPos.x = this.mPos.x + aX;
			}
			if (this.mDirection == LayoutAdvancer.Direction.Vertical)
			{
				this.mPos.y = this.mPos.y + aY;
			}
			this.mPrevious.x = aX;
			this.mPrevious.y = aY;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x000680C6 File Offset: 0x000662C6
		public Rect GetRect()
		{
			return new Rect(this.mPrevPos.x, this.mPrevPos.y, this.mPrevious.x, this.mPrevious.y);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x000680FC File Offset: 0x000662FC
		public Rect GetRect(float aOverrideDir)
		{
			if (this.mDirection == LayoutAdvancer.Direction.Vertical)
			{
				return new Rect(this.mPrevPos.x, this.mPrevPos.y, this.mPrevious.x, aOverrideDir);
			}
			return new Rect(this.mPrevPos.x, this.mPrevPos.y, aOverrideDir, this.mPrevious.y);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00068160 File Offset: 0x00066360
		public Rect GetRect(float aOverrideWidth, float aOverrideHeight)
		{
			return new Rect(this.mPos.x, this.mPos.y, aOverrideWidth, aOverrideHeight);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00068180 File Offset: 0x00066380
		public Rect GetRectPad(float aPaddingX, float aPaddingY)
		{
			return new Rect(this.mPrevPos.x + aPaddingX, this.mPrevPos.y + aPaddingY, this.mPrevious.x - aPaddingX * 2f, this.mPrevious.y - aPaddingY * 2f);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x000681D2 File Offset: 0x000663D2
		public Rect GetRectPad(float aPadding)
		{
			return this.GetRectPad(aPadding, aPadding);
		}

		// Token: 0x04000F42 RID: 3906
		[SerializeField]
		private Vector2 mPos;

		// Token: 0x04000F43 RID: 3907
		[SerializeField]
		private LayoutAdvancer.Direction mDirection;

		// Token: 0x04000F44 RID: 3908
		[SerializeField]
		private Vector2 mPrevious;

		// Token: 0x04000F45 RID: 3909
		[SerializeField]
		private Vector2 mPrevPos;

		// Token: 0x020004A3 RID: 1187
		public enum Direction
		{
			// Token: 0x04001788 RID: 6024
			Vertical,
			// Token: 0x04001789 RID: 6025
			Horizontal
		}
	}
}
