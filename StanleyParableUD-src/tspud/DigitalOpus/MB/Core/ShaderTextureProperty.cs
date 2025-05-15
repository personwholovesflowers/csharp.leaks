using System;
using System.Collections.Generic;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000281 RID: 641
	[Serializable]
	public class ShaderTextureProperty
	{
		// Token: 0x06000FDE RID: 4062 RVA: 0x000516A4 File Offset: 0x0004F8A4
		public ShaderTextureProperty(string n, bool norm)
		{
			this.name = n;
			this.isNormalMap = norm;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x000516BC File Offset: 0x0004F8BC
		public static string[] GetNames(List<ShaderTextureProperty> props)
		{
			string[] array = new string[props.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = props[i].name;
			}
			return array;
		}

		// Token: 0x04000DAA RID: 3498
		public string name;

		// Token: 0x04000DAB RID: 3499
		public bool isNormalMap;
	}
}
