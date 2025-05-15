using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000678 RID: 1656
	public struct LobbyTransaction
	{
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x0010ECBE File Offset: 0x0010CEBE
		private LobbyTransaction.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyTransaction.FFIMethods));
				}
				return (LobbyTransaction.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x0010ECF0 File Offset: 0x0010CEF0
		public void SetType(LobbyType type)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetType(this.MethodsPtr, type);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x0010ED34 File Offset: 0x0010CF34
		public void SetOwner(long ownerId)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetOwner(this.MethodsPtr, ownerId);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x0010ED78 File Offset: 0x0010CF78
		public void SetCapacity(uint capacity)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetCapacity(this.MethodsPtr, capacity);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x0010EDBC File Offset: 0x0010CFBC
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

		// Token: 0x0600250D RID: 9485 RVA: 0x0010EE00 File Offset: 0x0010D000
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

		// Token: 0x0600250E RID: 9486 RVA: 0x0010EE44 File Offset: 0x0010D044
		public void SetLocked(bool locked)
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				Result result = this.Methods.SetLocked(this.MethodsPtr, locked);
				if (result != Result.Ok)
				{
					throw new ResultException(result);
				}
			}
		}

		// Token: 0x04002F83 RID: 12163
		internal IntPtr MethodsPtr;

		// Token: 0x04002F84 RID: 12164
		internal object MethodsStructure;

		// Token: 0x02000679 RID: 1657
		internal struct FFIMethods
		{
			// Token: 0x04002F85 RID: 12165
			internal LobbyTransaction.FFIMethods.SetTypeMethod SetType;

			// Token: 0x04002F86 RID: 12166
			internal LobbyTransaction.FFIMethods.SetOwnerMethod SetOwner;

			// Token: 0x04002F87 RID: 12167
			internal LobbyTransaction.FFIMethods.SetCapacityMethod SetCapacity;

			// Token: 0x04002F88 RID: 12168
			internal LobbyTransaction.FFIMethods.SetMetadataMethod SetMetadata;

			// Token: 0x04002F89 RID: 12169
			internal LobbyTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;

			// Token: 0x04002F8A RID: 12170
			internal LobbyTransaction.FFIMethods.SetLockedMethod SetLocked;

			// Token: 0x0200067A RID: 1658
			// (Invoke) Token: 0x06002510 RID: 9488
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetTypeMethod(IntPtr methodsPtr, LobbyType type);

			// Token: 0x0200067B RID: 1659
			// (Invoke) Token: 0x06002514 RID: 9492
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetOwnerMethod(IntPtr methodsPtr, long ownerId);

			// Token: 0x0200067C RID: 1660
			// (Invoke) Token: 0x06002518 RID: 9496
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetCapacityMethod(IntPtr methodsPtr, uint capacity);

			// Token: 0x0200067D RID: 1661
			// (Invoke) Token: 0x0600251C RID: 9500
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

			// Token: 0x0200067E RID: 1662
			// (Invoke) Token: 0x06002520 RID: 9504
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);

			// Token: 0x0200067F RID: 1663
			// (Invoke) Token: 0x06002524 RID: 9508
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLockedMethod(IntPtr methodsPtr, bool locked);
		}
	}
}
