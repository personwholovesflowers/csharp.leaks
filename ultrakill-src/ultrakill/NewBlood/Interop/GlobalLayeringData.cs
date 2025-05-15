using System;
using System.Runtime.InteropServices;

namespace NewBlood.Interop
{
	// Token: 0x020005EA RID: 1514
	[StructLayout(LayoutKind.Explicit)]
	public struct GlobalLayeringData
	{
		// Token: 0x04002DB8 RID: 11704
		[FieldOffset(0)]
		public uint layerAndOrder;

		// Token: 0x04002DB9 RID: 11705
		[FieldOffset(4)]
		public GlobalLayeringData.SortingGroup sortingGroup;

		// Token: 0x04002DBA RID: 11706
		[FieldOffset(4)]
		public uint sortingGroupAll;

		// Token: 0x020005EB RID: 1515
		public struct SortingGroup
		{
			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x060021D8 RID: 8664 RVA: 0x0010B1E5 File Offset: 0x001093E5
			// (set) Token: 0x060021D9 RID: 8665 RVA: 0x0010B1F3 File Offset: 0x001093F3
			public uint order
			{
				readonly get
				{
					return this._bitfield & 4095U;
				}
				set
				{
					this._bitfield = (this._bitfield & 4294963200U) | (value & 4095U);
				}
			}

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x060021DA RID: 8666 RVA: 0x0010B20F File Offset: 0x0010940F
			// (set) Token: 0x060021DB RID: 8667 RVA: 0x0010B220 File Offset: 0x00109420
			public uint id
			{
				readonly get
				{
					return (this._bitfield >> 12) & 1048575U;
				}
				set
				{
					this._bitfield = (this._bitfield & 4095U) | ((value & 1048575U) << 12);
				}
			}

			// Token: 0x04002DBB RID: 11707
			private uint _bitfield;
		}
	}
}
