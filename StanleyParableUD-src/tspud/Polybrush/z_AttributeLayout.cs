using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x02000221 RID: 545
	[Serializable]
	public class z_AttributeLayout : IEquatable<z_AttributeLayout>
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x000366F2 File Offset: 0x000348F2
		// (set) Token: 0x06000C36 RID: 3126 RVA: 0x000366FF File Offset: 0x000348FF
		public float min
		{
			get
			{
				return this.range.x;
			}
			set
			{
				this.range.x = value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0003670D File Offset: 0x0003490D
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x0003671A File Offset: 0x0003491A
		public float max
		{
			get
			{
				return this.range.y;
			}
			set
			{
				this.range.y = value;
			}
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00036728 File Offset: 0x00034928
		public z_AttributeLayout(z_MeshChannel channel, z_ComponentIndex index)
			: this(channel, index, Vector2.up, 0)
		{
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00036738 File Offset: 0x00034938
		public z_AttributeLayout(z_MeshChannel channel, z_ComponentIndex index, Vector2 range, int mask)
		{
			this.channel = channel;
			this.index = index;
			this.range = range;
			this.mask = mask;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00036772 File Offset: 0x00034972
		public z_AttributeLayout(z_MeshChannel channel, z_ComponentIndex index, Vector2 range, int mask, string targetProperty, Texture2D texture = null)
			: this(channel, index, range, mask)
		{
			this.propertyTarget = targetProperty;
			this.previewTexture = texture;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00036790 File Offset: 0x00034990
		public bool Equals(z_AttributeLayout other)
		{
			return this.channel == other.channel && this.propertyTarget.Equals(other.propertyTarget) && this.index == other.index && this.range == other.range && this.mask == other.mask;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x000367F0 File Offset: 0x000349F0
		public override string ToString()
		{
			return string.Format("{0} {1}.{2} ({3:f2}, {4:f2})  {5}", new object[]
			{
				this.propertyTarget,
				this.channel.ToString(),
				this.index.GetString(z_ComponentIndexType.Vector),
				this.min,
				this.max,
				this.mask
			});
		}

		// Token: 0x04000BC2 RID: 3010
		public const int NoMask = -1;

		// Token: 0x04000BC3 RID: 3011
		public const int DefaultMask = 0;

		// Token: 0x04000BC4 RID: 3012
		public static readonly int[] DefaultMaskValues = new int[]
		{
			-1, 0, 1, 2, 3, 4, 5, 6, 7, 8,
			9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
			19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
			29, 30, 31
		};

		// Token: 0x04000BC5 RID: 3013
		public static readonly GUIContent[] DefaultMaskDescriptions = new GUIContent[]
		{
			new GUIContent("No Mask"),
			new GUIContent("0"),
			new GUIContent("1"),
			new GUIContent("2"),
			new GUIContent("3"),
			new GUIContent("4"),
			new GUIContent("5"),
			new GUIContent("6"),
			new GUIContent("7"),
			new GUIContent("8"),
			new GUIContent("9"),
			new GUIContent("10"),
			new GUIContent("11"),
			new GUIContent("12"),
			new GUIContent("13"),
			new GUIContent("14"),
			new GUIContent("15"),
			new GUIContent("16"),
			new GUIContent("17"),
			new GUIContent("18"),
			new GUIContent("19"),
			new GUIContent("20"),
			new GUIContent("21"),
			new GUIContent("22"),
			new GUIContent("23"),
			new GUIContent("24"),
			new GUIContent("25"),
			new GUIContent("26"),
			new GUIContent("27"),
			new GUIContent("28"),
			new GUIContent("29"),
			new GUIContent("30"),
			new GUIContent("31")
		};

		// Token: 0x04000BC6 RID: 3014
		public static readonly Vector2 NormalizedRange = new Vector2(0f, 1f);

		// Token: 0x04000BC7 RID: 3015
		public z_MeshChannel channel;

		// Token: 0x04000BC8 RID: 3016
		public z_ComponentIndex index;

		// Token: 0x04000BC9 RID: 3017
		public Vector2 range = new Vector2(0f, 1f);

		// Token: 0x04000BCA RID: 3018
		public string propertyTarget;

		// Token: 0x04000BCB RID: 3019
		public int mask;

		// Token: 0x04000BCC RID: 3020
		[NonSerialized]
		public Texture2D previewTexture;
	}
}
