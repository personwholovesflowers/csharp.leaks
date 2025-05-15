using System;

namespace I2.Loc
{
	// Token: 0x0200029F RID: 671
	[Serializable]
	public class LanguageData
	{
		// Token: 0x060010CD RID: 4301 RVA: 0x0005BE5C File Offset: 0x0005A05C
		public bool IsEnabled()
		{
			return (this.Flags & 1) == 0;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0005BE69 File Offset: 0x0005A069
		public void SetEnabled(bool bEnabled)
		{
			if (bEnabled)
			{
				this.Flags = (byte)((int)this.Flags & -2);
				return;
			}
			this.Flags |= 1;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0005BE8E File Offset: 0x0005A08E
		public bool IsLoaded()
		{
			return (this.Flags & 4) == 0;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0005BE9B File Offset: 0x0005A09B
		public bool CanBeUnloaded()
		{
			return (this.Flags & 2) == 0;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0005BEA8 File Offset: 0x0005A0A8
		public void SetLoaded(bool loaded)
		{
			if (loaded)
			{
				this.Flags = (byte)((int)this.Flags & -5);
				return;
			}
			this.Flags |= 4;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0005BECD File Offset: 0x0005A0CD
		public void SetCanBeUnLoaded(bool allowUnloading)
		{
			if (allowUnloading)
			{
				this.Flags = (byte)((int)this.Flags & -3);
				return;
			}
			this.Flags |= 2;
		}

		// Token: 0x04000DFA RID: 3578
		public string Name;

		// Token: 0x04000DFB RID: 3579
		public string Code;

		// Token: 0x04000DFC RID: 3580
		public byte Flags;

		// Token: 0x04000DFD RID: 3581
		[NonSerialized]
		public bool Compressed;
	}
}
