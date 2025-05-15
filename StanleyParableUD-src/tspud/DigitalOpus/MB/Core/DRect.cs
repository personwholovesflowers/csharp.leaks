using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000283 RID: 643
	public struct DRect
	{
		// Token: 0x0600101E RID: 4126 RVA: 0x0005488D File Offset: 0x00052A8D
		public DRect(Rect r)
		{
			this.x = (double)r.x;
			this.y = (double)r.y;
			this.width = (double)r.width;
			this.height = (double)r.height;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000548C7 File Offset: 0x00052AC7
		public DRect(Vector2 o, Vector2 s)
		{
			this.x = (double)o.x;
			this.y = (double)o.y;
			this.width = (double)s.x;
			this.height = (double)s.y;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x000548FD File Offset: 0x00052AFD
		public DRect(float xx, float yy, float w, float h)
		{
			this.x = (double)xx;
			this.y = (double)yy;
			this.width = (double)w;
			this.height = (double)h;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00054920 File Offset: 0x00052B20
		public DRect(double xx, double yy, double w, double h)
		{
			this.x = xx;
			this.y = yy;
			this.width = w;
			this.height = h;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005493F File Offset: 0x00052B3F
		public Rect GetRect()
		{
			return new Rect((float)this.x, (float)this.y, (float)this.width, (float)this.height);
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x00054962 File Offset: 0x00052B62
		public Vector2 min
		{
			get
			{
				return new Vector2((float)this.x, (float)this.y);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x00054977 File Offset: 0x00052B77
		public Vector2 max
		{
			get
			{
				return new Vector2((float)(this.x + this.width), (float)(this.y + this.width));
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0005499A File Offset: 0x00052B9A
		public Vector2 size
		{
			get
			{
				return new Vector2((float)this.width, (float)this.width);
			}
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x000549B0 File Offset: 0x00052BB0
		public override bool Equals(object obj)
		{
			DRect drect = (DRect)obj;
			return drect.x == this.x && drect.y == this.y && drect.width == this.width && drect.height == this.height;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x000549FF File Offset: 0x00052BFF
		public static bool operator ==(DRect a, DRect b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00054A14 File Offset: 0x00052C14
		public static bool operator !=(DRect a, DRect b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00054A2C File Offset: 0x00052C2C
		public override string ToString()
		{
			return string.Format("(x={0},y={1},w={2},h={3})", new object[]
			{
				this.x.ToString("F5"),
				this.y.ToString("F5"),
				this.width.ToString("F5"),
				this.height.ToString("F5")
			});
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00054A98 File Offset: 0x00052C98
		public bool Encloses(DRect smallToTestIfFits)
		{
			double num = smallToTestIfFits.x;
			double num2 = smallToTestIfFits.y;
			double num3 = smallToTestIfFits.x + smallToTestIfFits.width;
			double num4 = smallToTestIfFits.y + smallToTestIfFits.height;
			double num5 = this.x;
			double num6 = this.y;
			double num7 = this.x + this.width;
			double num8 = this.y + this.height;
			return num5 <= num && num <= num7 && num5 <= num3 && num3 <= num7 && num6 <= num2 && num2 <= num8 && num6 <= num4 && num4 <= num8;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00054B2A File Offset: 0x00052D2A
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.width.GetHashCode() ^ this.height.GetHashCode();
		}

		// Token: 0x04000DC1 RID: 3521
		public double x;

		// Token: 0x04000DC2 RID: 3522
		public double y;

		// Token: 0x04000DC3 RID: 3523
		public double width;

		// Token: 0x04000DC4 RID: 3524
		public double height;
	}
}
