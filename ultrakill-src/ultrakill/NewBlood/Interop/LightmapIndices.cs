using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NewBlood.Interop
{
	// Token: 0x020005ED RID: 1517
	[StructLayout(LayoutKind.Explicit)]
	public struct LightmapIndices
	{
		// Token: 0x04002DBD RID: 11709
		[FieldOffset(0)]
		public uint combined;

		// Token: 0x04002DBE RID: 11710
		[FixedBuffer(typeof(ushort), 2)]
		[FieldOffset(0)]
		public LightmapIndices.<indices>e__FixedBuffer indices;

		// Token: 0x020005EE RID: 1518
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 4)]
		public struct <indices>e__FixedBuffer
		{
			// Token: 0x04002DBF RID: 11711
			public ushort FixedElementField;
		}
	}
}
