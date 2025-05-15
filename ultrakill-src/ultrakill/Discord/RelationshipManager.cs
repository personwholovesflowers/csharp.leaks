using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020006C2 RID: 1730
	public class RelationshipManager
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x0010FF38 File Offset: 0x0010E138
		private RelationshipManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(RelationshipManager.FFIMethods));
				}
				return (RelationshipManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06002622 RID: 9762 RVA: 0x0010FF68 File Offset: 0x0010E168
		// (remove) Token: 0x06002623 RID: 9763 RVA: 0x0010FFA0 File Offset: 0x0010E1A0
		public event RelationshipManager.RefreshHandler OnRefresh;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06002624 RID: 9764 RVA: 0x0010FFD8 File Offset: 0x0010E1D8
		// (remove) Token: 0x06002625 RID: 9765 RVA: 0x00110010 File Offset: 0x0010E210
		public event RelationshipManager.RelationshipUpdateHandler OnRelationshipUpdate;

		// Token: 0x06002626 RID: 9766 RVA: 0x00110048 File Offset: 0x0010E248
		internal RelationshipManager(IntPtr ptr, IntPtr eventsPtr, ref RelationshipManager.FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
			this.InitEvents(eventsPtr, ref events);
			this.MethodsPtr = ptr;
			if (this.MethodsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x00110097 File Offset: 0x0010E297
		private void InitEvents(IntPtr eventsPtr, ref RelationshipManager.FFIEvents events)
		{
			events.OnRefresh = new RelationshipManager.FFIEvents.RefreshHandler(RelationshipManager.OnRefreshImpl);
			events.OnRelationshipUpdate = new RelationshipManager.FFIEvents.RelationshipUpdateHandler(RelationshipManager.OnRelationshipUpdateImpl);
			Marshal.StructureToPtr<RelationshipManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x001100CC File Offset: 0x0010E2CC
		[MonoPInvokeCallback]
		private static bool FilterCallbackImpl(IntPtr ptr, ref Relationship relationship)
		{
			return ((RelationshipManager.FilterHandler)GCHandle.FromIntPtr(ptr).Target)(ref relationship);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x001100F4 File Offset: 0x0010E2F4
		public void Filter(RelationshipManager.FilterHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.Filter(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new RelationshipManager.FFIMethods.FilterCallback(RelationshipManager.FilterCallbackImpl));
			gchandle.Free();
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x00110138 File Offset: 0x0010E338
		public int Count()
		{
			int num = 0;
			Result result = this.Methods.Count(this.MethodsPtr, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x0011016C File Offset: 0x0010E36C
		public Relationship Get(long userId)
		{
			Relationship relationship = default(Relationship);
			Result result = this.Methods.Get(this.MethodsPtr, userId, ref relationship);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return relationship;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x001101A8 File Offset: 0x0010E3A8
		public Relationship GetAt(uint index)
		{
			Relationship relationship = default(Relationship);
			Result result = this.Methods.GetAt(this.MethodsPtr, index, ref relationship);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return relationship;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x001101E4 File Offset: 0x0010E3E4
		[MonoPInvokeCallback]
		private static void OnRefreshImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.RelationshipManagerInstance.OnRefresh != null)
			{
				discord.RelationshipManagerInstance.OnRefresh();
			}
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00110224 File Offset: 0x0010E424
		[MonoPInvokeCallback]
		private static void OnRelationshipUpdateImpl(IntPtr ptr, ref Relationship relationship)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.RelationshipManagerInstance.OnRelationshipUpdate != null)
			{
				discord.RelationshipManagerInstance.OnRelationshipUpdate(ref relationship);
			}
		}

		// Token: 0x04002FFF RID: 12287
		private IntPtr MethodsPtr;

		// Token: 0x04003000 RID: 12288
		private object MethodsStructure;

		// Token: 0x020006C3 RID: 1731
		internal struct FFIEvents
		{
			// Token: 0x04003003 RID: 12291
			internal RelationshipManager.FFIEvents.RefreshHandler OnRefresh;

			// Token: 0x04003004 RID: 12292
			internal RelationshipManager.FFIEvents.RelationshipUpdateHandler OnRelationshipUpdate;

			// Token: 0x020006C4 RID: 1732
			// (Invoke) Token: 0x06002630 RID: 9776
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RefreshHandler(IntPtr ptr);

			// Token: 0x020006C5 RID: 1733
			// (Invoke) Token: 0x06002634 RID: 9780
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RelationshipUpdateHandler(IntPtr ptr, ref Relationship relationship);
		}

		// Token: 0x020006C6 RID: 1734
		internal struct FFIMethods
		{
			// Token: 0x04003005 RID: 12293
			internal RelationshipManager.FFIMethods.FilterMethod Filter;

			// Token: 0x04003006 RID: 12294
			internal RelationshipManager.FFIMethods.CountMethod Count;

			// Token: 0x04003007 RID: 12295
			internal RelationshipManager.FFIMethods.GetMethod Get;

			// Token: 0x04003008 RID: 12296
			internal RelationshipManager.FFIMethods.GetAtMethod GetAt;

			// Token: 0x020006C7 RID: 1735
			// (Invoke) Token: 0x06002638 RID: 9784
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate bool FilterCallback(IntPtr ptr, ref Relationship relationship);

			// Token: 0x020006C8 RID: 1736
			// (Invoke) Token: 0x0600263C RID: 9788
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FilterMethod(IntPtr methodsPtr, IntPtr callbackData, RelationshipManager.FFIMethods.FilterCallback callback);

			// Token: 0x020006C9 RID: 1737
			// (Invoke) Token: 0x06002640 RID: 9792
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x020006CA RID: 1738
			// (Invoke) Token: 0x06002644 RID: 9796
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMethod(IntPtr methodsPtr, long userId, ref Relationship relationship);

			// Token: 0x020006CB RID: 1739
			// (Invoke) Token: 0x06002648 RID: 9800
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetAtMethod(IntPtr methodsPtr, uint index, ref Relationship relationship);
		}

		// Token: 0x020006CC RID: 1740
		// (Invoke) Token: 0x0600264C RID: 9804
		public delegate bool FilterHandler(ref Relationship relationship);

		// Token: 0x020006CD RID: 1741
		// (Invoke) Token: 0x06002650 RID: 9808
		public delegate void RefreshHandler();

		// Token: 0x020006CE RID: 1742
		// (Invoke) Token: 0x06002654 RID: 9812
		public delegate void RelationshipUpdateHandler(ref Relationship relationship);
	}
}
