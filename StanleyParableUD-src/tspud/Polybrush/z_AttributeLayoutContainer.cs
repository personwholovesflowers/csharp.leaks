using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x02000220 RID: 544
	[Serializable]
	public class z_AttributeLayoutContainer : ScriptableObject, IEquatable<z_AttributeLayoutContainer>
	{
		// Token: 0x06000C32 RID: 3122 RVA: 0x00036664 File Offset: 0x00034864
		public static z_AttributeLayoutContainer Create(Shader shader, z_AttributeLayout[] attributes)
		{
			z_AttributeLayoutContainer z_AttributeLayoutContainer = ScriptableObject.CreateInstance<z_AttributeLayoutContainer>();
			z_AttributeLayoutContainer.shader = shader;
			z_AttributeLayoutContainer.attributes = attributes;
			return z_AttributeLayoutContainer;
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0003667C File Offset: 0x0003487C
		public bool Equals(z_AttributeLayoutContainer other)
		{
			if (this.shader != other.shader)
			{
				return false;
			}
			int num = ((this.attributes == null) ? 0 : this.attributes.Length);
			int num2 = ((other.attributes == null) ? 0 : other.attributes.Length);
			if (num != num2)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (!this.attributes[i].Equals(other.attributes[num2]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000BC0 RID: 3008
		public Shader shader;

		// Token: 0x04000BC1 RID: 3009
		public z_AttributeLayout[] attributes;
	}
}
