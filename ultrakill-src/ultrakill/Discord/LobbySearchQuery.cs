using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000684 RID: 1668
	public struct LobbySearchQuery
	{
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x0010EF3D File Offset: 0x0010D13D
		private LobbySearchQuery.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbySearchQuery.FFIMethods));
				}
				return (LobbySearchQuery.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x0010EF70 File Offset: 0x0010D170
		public void Filter(string key, LobbySearchComparison comparison, LobbySearchCast cast, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Filter(this.MethodsPtr, key, comparison, cast, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x0010EFB8 File Offset: 0x0010D1B8
		public void Sort(string key, LobbySearchCast cast, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Sort(this.MethodsPtr, key, cast, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x0010EFFC File Offset: 0x0010D1FC
		public void Limit(uint limit)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Limit(this.MethodsPtr, limit);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x0010F040 File Offset: 0x0010D240
		public void Distance(LobbySearchDistance distance)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.Distance(this.MethodsPtr, distance);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x04002F8F RID: 12175
		internal IntPtr MethodsPtr;

		// Token: 0x04002F90 RID: 12176
		internal object MethodsStructure;

		// Token: 0x02000685 RID: 1669
		internal struct FFIMethods
		{
			// Token: 0x04002F91 RID: 12177
			internal LobbySearchQuery.FFIMethods.FilterMethod Filter;

			// Token: 0x04002F92 RID: 12178
			internal LobbySearchQuery.FFIMethods.SortMethod Sort;

			// Token: 0x04002F93 RID: 12179
			internal LobbySearchQuery.FFIMethods.LimitMethod Limit;

			// Token: 0x04002F94 RID: 12180
			internal LobbySearchQuery.FFIMethods.DistanceMethod Distance;

			// Token: 0x02000686 RID: 1670
			// (Invoke) Token: 0x06002538 RID: 9528
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FilterMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, LobbySearchComparison comparison, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x02000687 RID: 1671
			// (Invoke) Token: 0x0600253C RID: 9532
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SortMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, LobbySearchCast cast, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x02000688 RID: 1672
			// (Invoke) Token: 0x06002540 RID: 9536
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LimitMethod(IntPtr methodsPtr, uint limit);

			// Token: 0x02000689 RID: 1673
			// (Invoke) Token: 0x06002544 RID: 9540
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DistanceMethod(IntPtr methodsPtr, LobbySearchDistance distance);
		}
	}
}
