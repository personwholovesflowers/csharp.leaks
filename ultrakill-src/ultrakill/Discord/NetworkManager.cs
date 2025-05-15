using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000719 RID: 1817
	public class NetworkManager
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x001113AA File Offset: 0x0010F5AA
		private NetworkManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(NetworkManager.FFIMethods));
				}
				return (NetworkManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060027BD RID: 10173 RVA: 0x001113DC File Offset: 0x0010F5DC
		// (remove) Token: 0x060027BE RID: 10174 RVA: 0x00111414 File Offset: 0x0010F614
		public event NetworkManager.MessageHandler OnMessage;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060027BF RID: 10175 RVA: 0x0011144C File Offset: 0x0010F64C
		// (remove) Token: 0x060027C0 RID: 10176 RVA: 0x00111484 File Offset: 0x0010F684
		public event NetworkManager.RouteUpdateHandler OnRouteUpdate;

		// Token: 0x060027C1 RID: 10177 RVA: 0x001114BC File Offset: 0x0010F6BC
		internal NetworkManager(IntPtr ptr, IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
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

		// Token: 0x060027C2 RID: 10178 RVA: 0x0011150B File Offset: 0x0010F70B
		private void InitEvents(IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
		{
			events.OnMessage = new NetworkManager.FFIEvents.MessageHandler(NetworkManager.OnMessageImpl);
			events.OnRouteUpdate = new NetworkManager.FFIEvents.RouteUpdateHandler(NetworkManager.OnRouteUpdateImpl);
			Marshal.StructureToPtr<NetworkManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x00111540 File Offset: 0x0010F740
		public ulong GetPeerId()
		{
			ulong num = 0UL;
			this.Methods.GetPeerId(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x0011156C File Offset: 0x0010F76C
		public void Flush()
		{
			Result result = this.Methods.Flush(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x0011159C File Offset: 0x0010F79C
		public void OpenPeer(ulong peerId, string routeData)
		{
			Result result = this.Methods.OpenPeer(this.MethodsPtr, peerId, routeData);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x001115CC File Offset: 0x0010F7CC
		public void UpdatePeer(ulong peerId, string routeData)
		{
			Result result = this.Methods.UpdatePeer(this.MethodsPtr, peerId, routeData);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x001115FC File Offset: 0x0010F7FC
		public void ClosePeer(ulong peerId)
		{
			Result result = this.Methods.ClosePeer(this.MethodsPtr, peerId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x0011162C File Offset: 0x0010F82C
		public void OpenChannel(ulong peerId, byte channelId, bool reliable)
		{
			Result result = this.Methods.OpenChannel(this.MethodsPtr, peerId, channelId, reliable);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00111660 File Offset: 0x0010F860
		public void CloseChannel(ulong peerId, byte channelId)
		{
			Result result = this.Methods.CloseChannel(this.MethodsPtr, peerId, channelId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00111690 File Offset: 0x0010F890
		public void SendMessage(ulong peerId, byte channelId, byte[] data)
		{
			Result result = this.Methods.SendMessage(this.MethodsPtr, peerId, channelId, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x001116C4 File Offset: 0x0010F8C4
		[MonoPInvokeCallback]
		private static void OnMessageImpl(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.NetworkManagerInstance.OnMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.NetworkManagerInstance.OnMessage(peerId, channelId, array);
			}
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00111718 File Offset: 0x0010F918
		[MonoPInvokeCallback]
		private static void OnRouteUpdateImpl(IntPtr ptr, string routeData)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.NetworkManagerInstance.OnRouteUpdate != null)
			{
				discord.NetworkManagerInstance.OnRouteUpdate(routeData);
			}
		}

		// Token: 0x0400303C RID: 12348
		private IntPtr MethodsPtr;

		// Token: 0x0400303D RID: 12349
		private object MethodsStructure;

		// Token: 0x0200071A RID: 1818
		internal struct FFIEvents
		{
			// Token: 0x04003040 RID: 12352
			internal NetworkManager.FFIEvents.MessageHandler OnMessage;

			// Token: 0x04003041 RID: 12353
			internal NetworkManager.FFIEvents.RouteUpdateHandler OnRouteUpdate;

			// Token: 0x0200071B RID: 1819
			// (Invoke) Token: 0x060027CE RID: 10190
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MessageHandler(IntPtr ptr, ulong peerId, byte channelId, IntPtr dataPtr, int dataLen);

			// Token: 0x0200071C RID: 1820
			// (Invoke) Token: 0x060027D2 RID: 10194
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void RouteUpdateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string routeData);
		}

		// Token: 0x0200071D RID: 1821
		internal struct FFIMethods
		{
			// Token: 0x04003042 RID: 12354
			internal NetworkManager.FFIMethods.GetPeerIdMethod GetPeerId;

			// Token: 0x04003043 RID: 12355
			internal NetworkManager.FFIMethods.FlushMethod Flush;

			// Token: 0x04003044 RID: 12356
			internal NetworkManager.FFIMethods.OpenPeerMethod OpenPeer;

			// Token: 0x04003045 RID: 12357
			internal NetworkManager.FFIMethods.UpdatePeerMethod UpdatePeer;

			// Token: 0x04003046 RID: 12358
			internal NetworkManager.FFIMethods.ClosePeerMethod ClosePeer;

			// Token: 0x04003047 RID: 12359
			internal NetworkManager.FFIMethods.OpenChannelMethod OpenChannel;

			// Token: 0x04003048 RID: 12360
			internal NetworkManager.FFIMethods.CloseChannelMethod CloseChannel;

			// Token: 0x04003049 RID: 12361
			internal NetworkManager.FFIMethods.SendMessageMethod SendMessage;

			// Token: 0x0200071E RID: 1822
			// (Invoke) Token: 0x060027D6 RID: 10198
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetPeerIdMethod(IntPtr methodsPtr, ref ulong peerId);

			// Token: 0x0200071F RID: 1823
			// (Invoke) Token: 0x060027DA RID: 10202
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushMethod(IntPtr methodsPtr);

			// Token: 0x02000720 RID: 1824
			// (Invoke) Token: 0x060027DE RID: 10206
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenPeerMethod(IntPtr methodsPtr, ulong peerId, [MarshalAs(UnmanagedType.LPStr)] string routeData);

			// Token: 0x02000721 RID: 1825
			// (Invoke) Token: 0x060027E2 RID: 10210
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result UpdatePeerMethod(IntPtr methodsPtr, ulong peerId, [MarshalAs(UnmanagedType.LPStr)] string routeData);

			// Token: 0x02000722 RID: 1826
			// (Invoke) Token: 0x060027E6 RID: 10214
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ClosePeerMethod(IntPtr methodsPtr, ulong peerId);

			// Token: 0x02000723 RID: 1827
			// (Invoke) Token: 0x060027EA RID: 10218
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId, bool reliable);

			// Token: 0x02000724 RID: 1828
			// (Invoke) Token: 0x060027EE RID: 10222
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CloseChannelMethod(IntPtr methodsPtr, ulong peerId, byte channelId);

			// Token: 0x02000725 RID: 1829
			// (Invoke) Token: 0x060027F2 RID: 10226
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendMessageMethod(IntPtr methodsPtr, ulong peerId, byte channelId, byte[] data, int dataLen);
		}

		// Token: 0x02000726 RID: 1830
		// (Invoke) Token: 0x060027F6 RID: 10230
		public delegate void MessageHandler(ulong peerId, byte channelId, byte[] data);

		// Token: 0x02000727 RID: 1831
		// (Invoke) Token: 0x060027FA RID: 10234
		public delegate void RouteUpdateHandler(string routeData);
	}
}
