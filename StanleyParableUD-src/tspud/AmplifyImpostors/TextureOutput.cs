using System;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x0200030F RID: 783
	[Serializable]
	public class TextureOutput
	{
		// Token: 0x060013FE RID: 5118 RVA: 0x0006A821 File Offset: 0x00068A21
		public TextureOutput()
		{
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0006A858 File Offset: 0x00068A58
		public TextureOutput(bool a, string n, TextureScale s, bool sr, TextureChannels c, TextureCompression nc, ImageFormat i)
		{
			this.Active = a;
			this.Name = n;
			this.Scale = s;
			this.SRGB = sr;
			this.Channels = c;
			this.Compression = nc;
			this.ImageFormat = i;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0006A8CE File Offset: 0x00068ACE
		public TextureOutput Clone()
		{
			return (TextureOutput)base.MemberwiseClone();
		}

		// Token: 0x04001007 RID: 4103
		[SerializeField]
		public int Index = -1;

		// Token: 0x04001008 RID: 4104
		[SerializeField]
		public OverrideMask OverrideMask;

		// Token: 0x04001009 RID: 4105
		public bool Active = true;

		// Token: 0x0400100A RID: 4106
		public string Name = string.Empty;

		// Token: 0x0400100B RID: 4107
		public TextureScale Scale = TextureScale.Full;

		// Token: 0x0400100C RID: 4108
		public bool SRGB;

		// Token: 0x0400100D RID: 4109
		public TextureChannels Channels;

		// Token: 0x0400100E RID: 4110
		public TextureCompression Compression = TextureCompression.Normal;

		// Token: 0x0400100F RID: 4111
		public ImageFormat ImageFormat = ImageFormat.TGA;
	}
}
