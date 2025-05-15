using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000680 RID: 1664
	public struct LobbyMemberTransaction
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x0010EE85 File Offset: 0x0010D085
		private LobbyMemberTransaction.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyMemberTransaction.FFIMethods));
				}
				return (LobbyMemberTransaction.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x0010EEB8 File Offset: 0x0010D0B8
		public void SetMetadata(string key, string value)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetMetadata(this.MethodsPtr, key, value);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x0010EEFC File Offset: 0x0010D0FC
		public void DeleteMetadata(string key)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.DeleteMetadata(this.MethodsPtr, key);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x04002F8B RID: 12171
		internal IntPtr MethodsPtr;

		// Token: 0x04002F8C RID: 12172
		internal object MethodsStructure;

		// Token: 0x02000681 RID: 1665
		internal struct FFIMethods
		{
			// Token: 0x04002F8D RID: 12173
			internal LobbyMemberTransaction.FFIMethods.SetMetadataMethod SetMetadata;

			// Token: 0x04002F8E RID: 12174
			internal LobbyMemberTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;

			// Token: 0x02000682 RID: 1666
			// (Invoke) Token: 0x0600252B RID: 9515
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x02000683 RID: 1667
			// (Invoke) Token: 0x0600252F RID: 9519
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);
		}
	}
}
