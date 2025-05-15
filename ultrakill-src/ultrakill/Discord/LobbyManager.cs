using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x020006CF RID: 1743
	public class LobbyManager
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x00110263 File Offset: 0x0010E463
		private LobbyManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(LobbyManager.FFIMethods));
				}
				return (LobbyManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06002658 RID: 9816 RVA: 0x00110294 File Offset: 0x0010E494
		// (remove) Token: 0x06002659 RID: 9817 RVA: 0x001102CC File Offset: 0x0010E4CC
		public event LobbyManager.LobbyUpdateHandler OnLobbyUpdate;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600265A RID: 9818 RVA: 0x00110304 File Offset: 0x0010E504
		// (remove) Token: 0x0600265B RID: 9819 RVA: 0x0011033C File Offset: 0x0010E53C
		public event LobbyManager.LobbyDeleteHandler OnLobbyDelete;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600265C RID: 9820 RVA: 0x00110374 File Offset: 0x0010E574
		// (remove) Token: 0x0600265D RID: 9821 RVA: 0x001103AC File Offset: 0x0010E5AC
		public event LobbyManager.MemberConnectHandler OnMemberConnect;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600265E RID: 9822 RVA: 0x001103E4 File Offset: 0x0010E5E4
		// (remove) Token: 0x0600265F RID: 9823 RVA: 0x0011041C File Offset: 0x0010E61C
		public event LobbyManager.MemberUpdateHandler OnMemberUpdate;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06002660 RID: 9824 RVA: 0x00110454 File Offset: 0x0010E654
		// (remove) Token: 0x06002661 RID: 9825 RVA: 0x0011048C File Offset: 0x0010E68C
		public event LobbyManager.MemberDisconnectHandler OnMemberDisconnect;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06002662 RID: 9826 RVA: 0x001104C4 File Offset: 0x0010E6C4
		// (remove) Token: 0x06002663 RID: 9827 RVA: 0x001104FC File Offset: 0x0010E6FC
		public event LobbyManager.LobbyMessageHandler OnLobbyMessage;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06002664 RID: 9828 RVA: 0x00110534 File Offset: 0x0010E734
		// (remove) Token: 0x06002665 RID: 9829 RVA: 0x0011056C File Offset: 0x0010E76C
		public event LobbyManager.SpeakingHandler OnSpeaking;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06002666 RID: 9830 RVA: 0x001105A4 File Offset: 0x0010E7A4
		// (remove) Token: 0x06002667 RID: 9831 RVA: 0x001105DC File Offset: 0x0010E7DC
		public event LobbyManager.NetworkMessageHandler OnNetworkMessage;

		// Token: 0x06002668 RID: 9832 RVA: 0x00110614 File Offset: 0x0010E814
		internal LobbyManager(IntPtr ptr, IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
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

		// Token: 0x06002669 RID: 9833 RVA: 0x00110664 File Offset: 0x0010E864
		private void InitEvents(IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
		{
			events.OnLobbyUpdate = new LobbyManager.FFIEvents.LobbyUpdateHandler(LobbyManager.OnLobbyUpdateImpl);
			events.OnLobbyDelete = new LobbyManager.FFIEvents.LobbyDeleteHandler(LobbyManager.OnLobbyDeleteImpl);
			events.OnMemberConnect = new LobbyManager.FFIEvents.MemberConnectHandler(LobbyManager.OnMemberConnectImpl);
			events.OnMemberUpdate = new LobbyManager.FFIEvents.MemberUpdateHandler(LobbyManager.OnMemberUpdateImpl);
			events.OnMemberDisconnect = new LobbyManager.FFIEvents.MemberDisconnectHandler(LobbyManager.OnMemberDisconnectImpl);
			events.OnLobbyMessage = new LobbyManager.FFIEvents.LobbyMessageHandler(LobbyManager.OnLobbyMessageImpl);
			events.OnSpeaking = new LobbyManager.FFIEvents.SpeakingHandler(LobbyManager.OnSpeakingImpl);
			events.OnNetworkMessage = new LobbyManager.FFIEvents.NetworkMessageHandler(LobbyManager.OnNetworkMessageImpl);
			Marshal.StructureToPtr<LobbyManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x00110710 File Offset: 0x0010E910
		public LobbyTransaction GetLobbyCreateTransaction()
		{
			LobbyTransaction lobbyTransaction = default(LobbyTransaction);
			Result result = this.Methods.GetLobbyCreateTransaction(this.MethodsPtr, ref lobbyTransaction.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbyTransaction;
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x00110750 File Offset: 0x0010E950
		public LobbyTransaction GetLobbyUpdateTransaction(long lobbyId)
		{
			LobbyTransaction lobbyTransaction = default(LobbyTransaction);
			Result result = this.Methods.GetLobbyUpdateTransaction(this.MethodsPtr, lobbyId, ref lobbyTransaction.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbyTransaction;
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x00110790 File Offset: 0x0010E990
		public LobbyMemberTransaction GetMemberUpdateTransaction(long lobbyId, long userId)
		{
			LobbyMemberTransaction lobbyMemberTransaction = default(LobbyMemberTransaction);
			Result result = this.Methods.GetMemberUpdateTransaction(this.MethodsPtr, lobbyId, userId, ref lobbyMemberTransaction.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbyMemberTransaction;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x001107D0 File Offset: 0x0010E9D0
		[MonoPInvokeCallback]
		private static void CreateLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.CreateLobbyHandler createLobbyHandler = (LobbyManager.CreateLobbyHandler)gchandle.Target;
			gchandle.Free();
			createLobbyHandler(result, ref lobby);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x00110800 File Offset: 0x0010EA00
		public void CreateLobby(LobbyTransaction transaction, LobbyManager.CreateLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.CreateLobby(this.MethodsPtr, transaction.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.CreateLobbyCallback(LobbyManager.CreateLobbyCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x00110850 File Offset: 0x0010EA50
		[MonoPInvokeCallback]
		private static void UpdateLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.UpdateLobbyHandler updateLobbyHandler = (LobbyManager.UpdateLobbyHandler)gchandle.Target;
			gchandle.Free();
			updateLobbyHandler(result);
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x00110880 File Offset: 0x0010EA80
		public void UpdateLobby(long lobbyId, LobbyTransaction transaction, LobbyManager.UpdateLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.UpdateLobby(this.MethodsPtr, lobbyId, transaction.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.UpdateLobbyCallback(LobbyManager.UpdateLobbyCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x001108D0 File Offset: 0x0010EAD0
		[MonoPInvokeCallback]
		private static void DeleteLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DeleteLobbyHandler deleteLobbyHandler = (LobbyManager.DeleteLobbyHandler)gchandle.Target;
			gchandle.Free();
			deleteLobbyHandler(result);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x00110900 File Offset: 0x0010EB00
		public void DeleteLobby(long lobbyId, LobbyManager.DeleteLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.DeleteLobby(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.DeleteLobbyCallback(LobbyManager.DeleteLobbyCallbackImpl));
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x00110940 File Offset: 0x0010EB40
		[MonoPInvokeCallback]
		private static void ConnectLobbyCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectLobbyHandler connectLobbyHandler = (LobbyManager.ConnectLobbyHandler)gchandle.Target;
			gchandle.Free();
			connectLobbyHandler(result, ref lobby);
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x00110970 File Offset: 0x0010EB70
		public void ConnectLobby(long lobbyId, string secret, LobbyManager.ConnectLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ConnectLobby(this.MethodsPtr, lobbyId, secret, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.ConnectLobbyCallback(LobbyManager.ConnectLobbyCallbackImpl));
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x001109B0 File Offset: 0x0010EBB0
		[MonoPInvokeCallback]
		private static void ConnectLobbyWithActivitySecretCallbackImpl(IntPtr ptr, Result result, ref Lobby lobby)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectLobbyWithActivitySecretHandler connectLobbyWithActivitySecretHandler = (LobbyManager.ConnectLobbyWithActivitySecretHandler)gchandle.Target;
			gchandle.Free();
			connectLobbyWithActivitySecretHandler(result, ref lobby);
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x001109E0 File Offset: 0x0010EBE0
		public void ConnectLobbyWithActivitySecret(string activitySecret, LobbyManager.ConnectLobbyWithActivitySecretHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ConnectLobbyWithActivitySecret(this.MethodsPtr, activitySecret, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback(LobbyManager.ConnectLobbyWithActivitySecretCallbackImpl));
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x00110A20 File Offset: 0x0010EC20
		[MonoPInvokeCallback]
		private static void DisconnectLobbyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DisconnectLobbyHandler disconnectLobbyHandler = (LobbyManager.DisconnectLobbyHandler)gchandle.Target;
			gchandle.Free();
			disconnectLobbyHandler(result);
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x00110A50 File Offset: 0x0010EC50
		public void DisconnectLobby(long lobbyId, LobbyManager.DisconnectLobbyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.DisconnectLobby(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.DisconnectLobbyCallback(LobbyManager.DisconnectLobbyCallbackImpl));
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x00110A90 File Offset: 0x0010EC90
		public Lobby GetLobby(long lobbyId)
		{
			Lobby lobby = default(Lobby);
			Result result = this.Methods.GetLobby(this.MethodsPtr, lobbyId, ref lobby);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobby;
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x00110ACC File Offset: 0x0010ECCC
		public string GetLobbyActivitySecret(long lobbyId)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			Result result = this.Methods.GetLobbyActivitySecret(this.MethodsPtr, lobbyId, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x00110B10 File Offset: 0x0010ED10
		public string GetLobbyMetadataValue(long lobbyId, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetLobbyMetadataValue(this.MethodsPtr, lobbyId, key, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x00110B54 File Offset: 0x0010ED54
		public string GetLobbyMetadataKey(long lobbyId, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Result result = this.Methods.GetLobbyMetadataKey(this.MethodsPtr, lobbyId, index, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x00110B98 File Offset: 0x0010ED98
		public int LobbyMetadataCount(long lobbyId)
		{
			int num = 0;
			Result result = this.Methods.LobbyMetadataCount(this.MethodsPtr, lobbyId, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x00110BCC File Offset: 0x0010EDCC
		public int MemberCount(long lobbyId)
		{
			int num = 0;
			Result result = this.Methods.MemberCount(this.MethodsPtr, lobbyId, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x00110C00 File Offset: 0x0010EE00
		public long GetMemberUserId(long lobbyId, int index)
		{
			long num = 0L;
			Result result = this.Methods.GetMemberUserId(this.MethodsPtr, lobbyId, index, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x00110C38 File Offset: 0x0010EE38
		public User GetMemberUser(long lobbyId, long userId)
		{
			User user = default(User);
			Result result = this.Methods.GetMemberUser(this.MethodsPtr, lobbyId, userId, ref user);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return user;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x00110C74 File Offset: 0x0010EE74
		public string GetMemberMetadataValue(long lobbyId, long userId, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetMemberMetadataValue(this.MethodsPtr, lobbyId, userId, key, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x00110CB8 File Offset: 0x0010EEB8
		public string GetMemberMetadataKey(long lobbyId, long userId, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Result result = this.Methods.GetMemberMetadataKey(this.MethodsPtr, lobbyId, userId, index, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x00110CFC File Offset: 0x0010EEFC
		public int MemberMetadataCount(long lobbyId, long userId)
		{
			int num = 0;
			Result result = this.Methods.MemberMetadataCount(this.MethodsPtr, lobbyId, userId, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x00110D34 File Offset: 0x0010EF34
		[MonoPInvokeCallback]
		private static void UpdateMemberCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.UpdateMemberHandler updateMemberHandler = (LobbyManager.UpdateMemberHandler)gchandle.Target;
			gchandle.Free();
			updateMemberHandler(result);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x00110D64 File Offset: 0x0010EF64
		public void UpdateMember(long lobbyId, long userId, LobbyMemberTransaction transaction, LobbyManager.UpdateMemberHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.UpdateMember(this.MethodsPtr, lobbyId, userId, transaction.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.UpdateMemberCallback(LobbyManager.UpdateMemberCallbackImpl));
			transaction.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x00110DB8 File Offset: 0x0010EFB8
		[MonoPInvokeCallback]
		private static void SendLobbyMessageCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.SendLobbyMessageHandler sendLobbyMessageHandler = (LobbyManager.SendLobbyMessageHandler)gchandle.Target;
			gchandle.Free();
			sendLobbyMessageHandler(result);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x00110DE8 File Offset: 0x0010EFE8
		public void SendLobbyMessage(long lobbyId, byte[] data, LobbyManager.SendLobbyMessageHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SendLobbyMessage(this.MethodsPtr, lobbyId, data, data.Length, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.SendLobbyMessageCallback(LobbyManager.SendLobbyMessageCallbackImpl));
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x00110E2C File Offset: 0x0010F02C
		public LobbySearchQuery GetSearchQuery()
		{
			LobbySearchQuery lobbySearchQuery = default(LobbySearchQuery);
			Result result = this.Methods.GetSearchQuery(this.MethodsPtr, ref lobbySearchQuery.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return lobbySearchQuery;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x00110E6C File Offset: 0x0010F06C
		[MonoPInvokeCallback]
		private static void SearchCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.SearchHandler searchHandler = (LobbyManager.SearchHandler)gchandle.Target;
			gchandle.Free();
			searchHandler(result);
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x00110E9C File Offset: 0x0010F09C
		public void Search(LobbySearchQuery query, LobbyManager.SearchHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.Search(this.MethodsPtr, query.MethodsPtr, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.SearchCallback(LobbyManager.SearchCallbackImpl));
			query.MethodsPtr = IntPtr.Zero;
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x00110EEC File Offset: 0x0010F0EC
		public int LobbyCount()
		{
			int num = 0;
			this.Methods.LobbyCount(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x00110F14 File Offset: 0x0010F114
		public long GetLobbyId(int index)
		{
			long num = 0L;
			Result result = this.Methods.GetLobbyId(this.MethodsPtr, index, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x00110F4C File Offset: 0x0010F14C
		[MonoPInvokeCallback]
		private static void ConnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.ConnectVoiceHandler connectVoiceHandler = (LobbyManager.ConnectVoiceHandler)gchandle.Target;
			gchandle.Free();
			connectVoiceHandler(result);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x00110F7C File Offset: 0x0010F17C
		public void ConnectVoice(long lobbyId, LobbyManager.ConnectVoiceHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ConnectVoice(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.ConnectVoiceCallback(LobbyManager.ConnectVoiceCallbackImpl));
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x00110FBC File Offset: 0x0010F1BC
		[MonoPInvokeCallback]
		private static void DisconnectVoiceCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			LobbyManager.DisconnectVoiceHandler disconnectVoiceHandler = (LobbyManager.DisconnectVoiceHandler)gchandle.Target;
			gchandle.Free();
			disconnectVoiceHandler(result);
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x00110FEC File Offset: 0x0010F1EC
		public void DisconnectVoice(long lobbyId, LobbyManager.DisconnectVoiceHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.DisconnectVoice(this.MethodsPtr, lobbyId, GCHandle.ToIntPtr(gchandle), new LobbyManager.FFIMethods.DisconnectVoiceCallback(LobbyManager.DisconnectVoiceCallbackImpl));
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0011102C File Offset: 0x0010F22C
		public void ConnectNetwork(long lobbyId)
		{
			Result result = this.Methods.ConnectNetwork(this.MethodsPtr, lobbyId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x0011105C File Offset: 0x0010F25C
		public void DisconnectNetwork(long lobbyId)
		{
			Result result = this.Methods.DisconnectNetwork(this.MethodsPtr, lobbyId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0011108C File Offset: 0x0010F28C
		public void FlushNetwork()
		{
			Result result = this.Methods.FlushNetwork(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x001110BC File Offset: 0x0010F2BC
		public void OpenNetworkChannel(long lobbyId, byte channelId, bool reliable)
		{
			Result result = this.Methods.OpenNetworkChannel(this.MethodsPtr, lobbyId, channelId, reliable);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x001110F0 File Offset: 0x0010F2F0
		public void SendNetworkMessage(long lobbyId, long userId, byte channelId, byte[] data)
		{
			Result result = this.Methods.SendNetworkMessage(this.MethodsPtr, lobbyId, userId, channelId, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x00111128 File Offset: 0x0010F328
		[MonoPInvokeCallback]
		private static void OnLobbyUpdateImpl(IntPtr ptr, long lobbyId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyUpdate != null)
			{
				discord.LobbyManagerInstance.OnLobbyUpdate(lobbyId);
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x00111168 File Offset: 0x0010F368
		[MonoPInvokeCallback]
		private static void OnLobbyDeleteImpl(IntPtr ptr, long lobbyId, uint reason)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyDelete != null)
			{
				discord.LobbyManagerInstance.OnLobbyDelete(lobbyId, reason);
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x001111A8 File Offset: 0x0010F3A8
		[MonoPInvokeCallback]
		private static void OnMemberConnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberConnect != null)
			{
				discord.LobbyManagerInstance.OnMemberConnect(lobbyId, userId);
			}
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x001111E8 File Offset: 0x0010F3E8
		[MonoPInvokeCallback]
		private static void OnMemberUpdateImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberUpdate != null)
			{
				discord.LobbyManagerInstance.OnMemberUpdate(lobbyId, userId);
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x00111228 File Offset: 0x0010F428
		[MonoPInvokeCallback]
		private static void OnMemberDisconnectImpl(IntPtr ptr, long lobbyId, long userId)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnMemberDisconnect != null)
			{
				discord.LobbyManagerInstance.OnMemberDisconnect(lobbyId, userId);
			}
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x00111268 File Offset: 0x0010F468
		[MonoPInvokeCallback]
		private static void OnLobbyMessageImpl(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnLobbyMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.LobbyManagerInstance.OnLobbyMessage(lobbyId, userId, array);
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x001112BC File Offset: 0x0010F4BC
		[MonoPInvokeCallback]
		private static void OnSpeakingImpl(IntPtr ptr, long lobbyId, long userId, bool speaking)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnSpeaking != null)
			{
				discord.LobbyManagerInstance.OnSpeaking(lobbyId, userId, speaking);
			}
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x00111300 File Offset: 0x0010F500
		[MonoPInvokeCallback]
		private static void OnNetworkMessageImpl(IntPtr ptr, long lobbyId, long userId, byte channelId, IntPtr dataPtr, int dataLen)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.LobbyManagerInstance.OnNetworkMessage != null)
			{
				byte[] array = new byte[dataLen];
				Marshal.Copy(dataPtr, array, 0, dataLen);
				discord.LobbyManagerInstance.OnNetworkMessage(lobbyId, userId, channelId, array);
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x00111358 File Offset: 0x0010F558
		public IEnumerable<User> GetMemberUsers(long lobbyID)
		{
			int num = this.MemberCount(lobbyID);
			List<User> list = new List<User>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetMemberUser(lobbyID, this.GetMemberUserId(lobbyID, i)));
			}
			return list;
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x00111395 File Offset: 0x0010F595
		public void SendLobbyMessage(long lobbyID, string data, LobbyManager.SendLobbyMessageHandler handler)
		{
			this.SendLobbyMessage(lobbyID, Encoding.UTF8.GetBytes(data), handler);
		}

		// Token: 0x04003009 RID: 12297
		private IntPtr MethodsPtr;

		// Token: 0x0400300A RID: 12298
		private object MethodsStructure;

		// Token: 0x020006D0 RID: 1744
		internal struct FFIEvents
		{
			// Token: 0x04003013 RID: 12307
			internal LobbyManager.FFIEvents.LobbyUpdateHandler OnLobbyUpdate;

			// Token: 0x04003014 RID: 12308
			internal LobbyManager.FFIEvents.LobbyDeleteHandler OnLobbyDelete;

			// Token: 0x04003015 RID: 12309
			internal LobbyManager.FFIEvents.MemberConnectHandler OnMemberConnect;

			// Token: 0x04003016 RID: 12310
			internal LobbyManager.FFIEvents.MemberUpdateHandler OnMemberUpdate;

			// Token: 0x04003017 RID: 12311
			internal LobbyManager.FFIEvents.MemberDisconnectHandler OnMemberDisconnect;

			// Token: 0x04003018 RID: 12312
			internal LobbyManager.FFIEvents.LobbyMessageHandler OnLobbyMessage;

			// Token: 0x04003019 RID: 12313
			internal LobbyManager.FFIEvents.SpeakingHandler OnSpeaking;

			// Token: 0x0400301A RID: 12314
			internal LobbyManager.FFIEvents.NetworkMessageHandler OnNetworkMessage;

			// Token: 0x020006D1 RID: 1745
			// (Invoke) Token: 0x060026A1 RID: 9889
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyUpdateHandler(IntPtr ptr, long lobbyId);

			// Token: 0x020006D2 RID: 1746
			// (Invoke) Token: 0x060026A5 RID: 9893
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyDeleteHandler(IntPtr ptr, long lobbyId, uint reason);

			// Token: 0x020006D3 RID: 1747
			// (Invoke) Token: 0x060026A9 RID: 9897
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberConnectHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x020006D4 RID: 1748
			// (Invoke) Token: 0x060026AD RID: 9901
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberUpdateHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x020006D5 RID: 1749
			// (Invoke) Token: 0x060026B1 RID: 9905
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void MemberDisconnectHandler(IntPtr ptr, long lobbyId, long userId);

			// Token: 0x020006D6 RID: 1750
			// (Invoke) Token: 0x060026B5 RID: 9909
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyMessageHandler(IntPtr ptr, long lobbyId, long userId, IntPtr dataPtr, int dataLen);

			// Token: 0x020006D7 RID: 1751
			// (Invoke) Token: 0x060026B9 RID: 9913
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SpeakingHandler(IntPtr ptr, long lobbyId, long userId, bool speaking);

			// Token: 0x020006D8 RID: 1752
			// (Invoke) Token: 0x060026BD RID: 9917
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void NetworkMessageHandler(IntPtr ptr, long lobbyId, long userId, byte channelId, IntPtr dataPtr, int dataLen);
		}

		// Token: 0x020006D9 RID: 1753
		internal struct FFIMethods
		{
			// Token: 0x0400301B RID: 12315
			internal LobbyManager.FFIMethods.GetLobbyCreateTransactionMethod GetLobbyCreateTransaction;

			// Token: 0x0400301C RID: 12316
			internal LobbyManager.FFIMethods.GetLobbyUpdateTransactionMethod GetLobbyUpdateTransaction;

			// Token: 0x0400301D RID: 12317
			internal LobbyManager.FFIMethods.GetMemberUpdateTransactionMethod GetMemberUpdateTransaction;

			// Token: 0x0400301E RID: 12318
			internal LobbyManager.FFIMethods.CreateLobbyMethod CreateLobby;

			// Token: 0x0400301F RID: 12319
			internal LobbyManager.FFIMethods.UpdateLobbyMethod UpdateLobby;

			// Token: 0x04003020 RID: 12320
			internal LobbyManager.FFIMethods.DeleteLobbyMethod DeleteLobby;

			// Token: 0x04003021 RID: 12321
			internal LobbyManager.FFIMethods.ConnectLobbyMethod ConnectLobby;

			// Token: 0x04003022 RID: 12322
			internal LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretMethod ConnectLobbyWithActivitySecret;

			// Token: 0x04003023 RID: 12323
			internal LobbyManager.FFIMethods.DisconnectLobbyMethod DisconnectLobby;

			// Token: 0x04003024 RID: 12324
			internal LobbyManager.FFIMethods.GetLobbyMethod GetLobby;

			// Token: 0x04003025 RID: 12325
			internal LobbyManager.FFIMethods.GetLobbyActivitySecretMethod GetLobbyActivitySecret;

			// Token: 0x04003026 RID: 12326
			internal LobbyManager.FFIMethods.GetLobbyMetadataValueMethod GetLobbyMetadataValue;

			// Token: 0x04003027 RID: 12327
			internal LobbyManager.FFIMethods.GetLobbyMetadataKeyMethod GetLobbyMetadataKey;

			// Token: 0x04003028 RID: 12328
			internal LobbyManager.FFIMethods.LobbyMetadataCountMethod LobbyMetadataCount;

			// Token: 0x04003029 RID: 12329
			internal LobbyManager.FFIMethods.MemberCountMethod MemberCount;

			// Token: 0x0400302A RID: 12330
			internal LobbyManager.FFIMethods.GetMemberUserIdMethod GetMemberUserId;

			// Token: 0x0400302B RID: 12331
			internal LobbyManager.FFIMethods.GetMemberUserMethod GetMemberUser;

			// Token: 0x0400302C RID: 12332
			internal LobbyManager.FFIMethods.GetMemberMetadataValueMethod GetMemberMetadataValue;

			// Token: 0x0400302D RID: 12333
			internal LobbyManager.FFIMethods.GetMemberMetadataKeyMethod GetMemberMetadataKey;

			// Token: 0x0400302E RID: 12334
			internal LobbyManager.FFIMethods.MemberMetadataCountMethod MemberMetadataCount;

			// Token: 0x0400302F RID: 12335
			internal LobbyManager.FFIMethods.UpdateMemberMethod UpdateMember;

			// Token: 0x04003030 RID: 12336
			internal LobbyManager.FFIMethods.SendLobbyMessageMethod SendLobbyMessage;

			// Token: 0x04003031 RID: 12337
			internal LobbyManager.FFIMethods.GetSearchQueryMethod GetSearchQuery;

			// Token: 0x04003032 RID: 12338
			internal LobbyManager.FFIMethods.SearchMethod Search;

			// Token: 0x04003033 RID: 12339
			internal LobbyManager.FFIMethods.LobbyCountMethod LobbyCount;

			// Token: 0x04003034 RID: 12340
			internal LobbyManager.FFIMethods.GetLobbyIdMethod GetLobbyId;

			// Token: 0x04003035 RID: 12341
			internal LobbyManager.FFIMethods.ConnectVoiceMethod ConnectVoice;

			// Token: 0x04003036 RID: 12342
			internal LobbyManager.FFIMethods.DisconnectVoiceMethod DisconnectVoice;

			// Token: 0x04003037 RID: 12343
			internal LobbyManager.FFIMethods.ConnectNetworkMethod ConnectNetwork;

			// Token: 0x04003038 RID: 12344
			internal LobbyManager.FFIMethods.DisconnectNetworkMethod DisconnectNetwork;

			// Token: 0x04003039 RID: 12345
			internal LobbyManager.FFIMethods.FlushNetworkMethod FlushNetwork;

			// Token: 0x0400303A RID: 12346
			internal LobbyManager.FFIMethods.OpenNetworkChannelMethod OpenNetworkChannel;

			// Token: 0x0400303B RID: 12347
			internal LobbyManager.FFIMethods.SendNetworkMessageMethod SendNetworkMessage;

			// Token: 0x020006DA RID: 1754
			// (Invoke) Token: 0x060026C1 RID: 9921
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyCreateTransactionMethod(IntPtr methodsPtr, ref IntPtr transaction);

			// Token: 0x020006DB RID: 1755
			// (Invoke) Token: 0x060026C5 RID: 9925
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId, ref IntPtr transaction);

			// Token: 0x020006DC RID: 1756
			// (Invoke) Token: 0x060026C9 RID: 9929
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUpdateTransactionMethod(IntPtr methodsPtr, long lobbyId, long userId, ref IntPtr transaction);

			// Token: 0x020006DD RID: 1757
			// (Invoke) Token: 0x060026CD RID: 9933
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x020006DE RID: 1758
			// (Invoke) Token: 0x060026D1 RID: 9937
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CreateLobbyMethod(IntPtr methodsPtr, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.CreateLobbyCallback callback);

			// Token: 0x020006DF RID: 1759
			// (Invoke) Token: 0x060026D5 RID: 9941
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x020006E0 RID: 1760
			// (Invoke) Token: 0x060026D9 RID: 9945
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.UpdateLobbyCallback callback);

			// Token: 0x020006E1 RID: 1761
			// (Invoke) Token: 0x060026DD RID: 9949
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x020006E2 RID: 1762
			// (Invoke) Token: 0x060026E1 RID: 9953
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DeleteLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DeleteLobbyCallback callback);

			// Token: 0x020006E3 RID: 1763
			// (Invoke) Token: 0x060026E5 RID: 9957
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x020006E4 RID: 1764
			// (Invoke) Token: 0x060026E9 RID: 9961
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyMethod(IntPtr methodsPtr, long lobbyId, [MarshalAs(UnmanagedType.LPStr)] string secret, IntPtr callbackData, LobbyManager.FFIMethods.ConnectLobbyCallback callback);

			// Token: 0x020006E5 RID: 1765
			// (Invoke) Token: 0x060026ED RID: 9965
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretCallback(IntPtr ptr, Result result, ref Lobby lobby);

			// Token: 0x020006E6 RID: 1766
			// (Invoke) Token: 0x060026F1 RID: 9969
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectLobbyWithActivitySecretMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string activitySecret, IntPtr callbackData, LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback callback);

			// Token: 0x020006E7 RID: 1767
			// (Invoke) Token: 0x060026F5 RID: 9973
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyCallback(IntPtr ptr, Result result);

			// Token: 0x020006E8 RID: 1768
			// (Invoke) Token: 0x060026F9 RID: 9977
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectLobbyMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DisconnectLobbyCallback callback);

			// Token: 0x020006E9 RID: 1769
			// (Invoke) Token: 0x060026FD RID: 9981
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMethod(IntPtr methodsPtr, long lobbyId, ref Lobby lobby);

			// Token: 0x020006EA RID: 1770
			// (Invoke) Token: 0x06002701 RID: 9985
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyActivitySecretMethod(IntPtr methodsPtr, long lobbyId, StringBuilder secret);

			// Token: 0x020006EB RID: 1771
			// (Invoke) Token: 0x06002705 RID: 9989
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataValueMethod(IntPtr methodsPtr, long lobbyId, [MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			// Token: 0x020006EC RID: 1772
			// (Invoke) Token: 0x06002709 RID: 9993
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, int index, StringBuilder key);

			// Token: 0x020006ED RID: 1773
			// (Invoke) Token: 0x0600270D RID: 9997
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result LobbyMetadataCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			// Token: 0x020006EE RID: 1774
			// (Invoke) Token: 0x06002711 RID: 10001
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberCountMethod(IntPtr methodsPtr, long lobbyId, ref int count);

			// Token: 0x020006EF RID: 1775
			// (Invoke) Token: 0x06002715 RID: 10005
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserIdMethod(IntPtr methodsPtr, long lobbyId, int index, ref long userId);

			// Token: 0x020006F0 RID: 1776
			// (Invoke) Token: 0x06002719 RID: 10009
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberUserMethod(IntPtr methodsPtr, long lobbyId, long userId, ref User user);

			// Token: 0x020006F1 RID: 1777
			// (Invoke) Token: 0x0600271D RID: 10013
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataValueMethod(IntPtr methodsPtr, long lobbyId, long userId, [MarshalAs(UnmanagedType.LPStr)] string key, StringBuilder value);

			// Token: 0x020006F2 RID: 1778
			// (Invoke) Token: 0x06002721 RID: 10017
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetMemberMetadataKeyMethod(IntPtr methodsPtr, long lobbyId, long userId, int index, StringBuilder key);

			// Token: 0x020006F3 RID: 1779
			// (Invoke) Token: 0x06002725 RID: 10021
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result MemberMetadataCountMethod(IntPtr methodsPtr, long lobbyId, long userId, ref int count);

			// Token: 0x020006F4 RID: 1780
			// (Invoke) Token: 0x06002729 RID: 10025
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberCallback(IntPtr ptr, Result result);

			// Token: 0x020006F5 RID: 1781
			// (Invoke) Token: 0x0600272D RID: 10029
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateMemberMethod(IntPtr methodsPtr, long lobbyId, long userId, IntPtr transaction, IntPtr callbackData, LobbyManager.FFIMethods.UpdateMemberCallback callback);

			// Token: 0x020006F6 RID: 1782
			// (Invoke) Token: 0x06002731 RID: 10033
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageCallback(IntPtr ptr, Result result);

			// Token: 0x020006F7 RID: 1783
			// (Invoke) Token: 0x06002735 RID: 10037
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendLobbyMessageMethod(IntPtr methodsPtr, long lobbyId, byte[] data, int dataLen, IntPtr callbackData, LobbyManager.FFIMethods.SendLobbyMessageCallback callback);

			// Token: 0x020006F8 RID: 1784
			// (Invoke) Token: 0x06002739 RID: 10041
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSearchQueryMethod(IntPtr methodsPtr, ref IntPtr query);

			// Token: 0x020006F9 RID: 1785
			// (Invoke) Token: 0x0600273D RID: 10045
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchCallback(IntPtr ptr, Result result);

			// Token: 0x020006FA RID: 1786
			// (Invoke) Token: 0x06002741 RID: 10049
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SearchMethod(IntPtr methodsPtr, IntPtr query, IntPtr callbackData, LobbyManager.FFIMethods.SearchCallback callback);

			// Token: 0x020006FB RID: 1787
			// (Invoke) Token: 0x06002745 RID: 10053
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void LobbyCountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x020006FC RID: 1788
			// (Invoke) Token: 0x06002749 RID: 10057
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLobbyIdMethod(IntPtr methodsPtr, int index, ref long lobbyId);

			// Token: 0x020006FD RID: 1789
			// (Invoke) Token: 0x0600274D RID: 10061
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceCallback(IntPtr ptr, Result result);

			// Token: 0x020006FE RID: 1790
			// (Invoke) Token: 0x06002751 RID: 10065
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ConnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.ConnectVoiceCallback callback);

			// Token: 0x020006FF RID: 1791
			// (Invoke) Token: 0x06002755 RID: 10069
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceCallback(IntPtr ptr, Result result);

			// Token: 0x02000700 RID: 1792
			// (Invoke) Token: 0x06002759 RID: 10073
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DisconnectVoiceMethod(IntPtr methodsPtr, long lobbyId, IntPtr callbackData, LobbyManager.FFIMethods.DisconnectVoiceCallback callback);

			// Token: 0x02000701 RID: 1793
			// (Invoke) Token: 0x0600275D RID: 10077
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ConnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			// Token: 0x02000702 RID: 1794
			// (Invoke) Token: 0x06002761 RID: 10081
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DisconnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

			// Token: 0x02000703 RID: 1795
			// (Invoke) Token: 0x06002765 RID: 10085
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result FlushNetworkMethod(IntPtr methodsPtr);

			// Token: 0x02000704 RID: 1796
			// (Invoke) Token: 0x06002769 RID: 10089
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result OpenNetworkChannelMethod(IntPtr methodsPtr, long lobbyId, byte channelId, bool reliable);

			// Token: 0x02000705 RID: 1797
			// (Invoke) Token: 0x0600276D RID: 10093
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SendNetworkMessageMethod(IntPtr methodsPtr, long lobbyId, long userId, byte channelId, byte[] data, int dataLen);
		}

		// Token: 0x02000706 RID: 1798
		// (Invoke) Token: 0x06002771 RID: 10097
		public delegate void CreateLobbyHandler(Result result, ref Lobby lobby);

		// Token: 0x02000707 RID: 1799
		// (Invoke) Token: 0x06002775 RID: 10101
		public delegate void UpdateLobbyHandler(Result result);

		// Token: 0x02000708 RID: 1800
		// (Invoke) Token: 0x06002779 RID: 10105
		public delegate void DeleteLobbyHandler(Result result);

		// Token: 0x02000709 RID: 1801
		// (Invoke) Token: 0x0600277D RID: 10109
		public delegate void ConnectLobbyHandler(Result result, ref Lobby lobby);

		// Token: 0x0200070A RID: 1802
		// (Invoke) Token: 0x06002781 RID: 10113
		public delegate void ConnectLobbyWithActivitySecretHandler(Result result, ref Lobby lobby);

		// Token: 0x0200070B RID: 1803
		// (Invoke) Token: 0x06002785 RID: 10117
		public delegate void DisconnectLobbyHandler(Result result);

		// Token: 0x0200070C RID: 1804
		// (Invoke) Token: 0x06002789 RID: 10121
		public delegate void UpdateMemberHandler(Result result);

		// Token: 0x0200070D RID: 1805
		// (Invoke) Token: 0x0600278D RID: 10125
		public delegate void SendLobbyMessageHandler(Result result);

		// Token: 0x0200070E RID: 1806
		// (Invoke) Token: 0x06002791 RID: 10129
		public delegate void SearchHandler(Result result);

		// Token: 0x0200070F RID: 1807
		// (Invoke) Token: 0x06002795 RID: 10133
		public delegate void ConnectVoiceHandler(Result result);

		// Token: 0x02000710 RID: 1808
		// (Invoke) Token: 0x06002799 RID: 10137
		public delegate void DisconnectVoiceHandler(Result result);

		// Token: 0x02000711 RID: 1809
		// (Invoke) Token: 0x0600279D RID: 10141
		public delegate void LobbyUpdateHandler(long lobbyId);

		// Token: 0x02000712 RID: 1810
		// (Invoke) Token: 0x060027A1 RID: 10145
		public delegate void LobbyDeleteHandler(long lobbyId, uint reason);

		// Token: 0x02000713 RID: 1811
		// (Invoke) Token: 0x060027A5 RID: 10149
		public delegate void MemberConnectHandler(long lobbyId, long userId);

		// Token: 0x02000714 RID: 1812
		// (Invoke) Token: 0x060027A9 RID: 10153
		public delegate void MemberUpdateHandler(long lobbyId, long userId);

		// Token: 0x02000715 RID: 1813
		// (Invoke) Token: 0x060027AD RID: 10157
		public delegate void MemberDisconnectHandler(long lobbyId, long userId);

		// Token: 0x02000716 RID: 1814
		// (Invoke) Token: 0x060027B1 RID: 10161
		public delegate void LobbyMessageHandler(long lobbyId, long userId, byte[] data);

		// Token: 0x02000717 RID: 1815
		// (Invoke) Token: 0x060027B5 RID: 10165
		public delegate void SpeakingHandler(long lobbyId, long userId, bool speaking);

		// Token: 0x02000718 RID: 1816
		// (Invoke) Token: 0x060027B9 RID: 10169
		public delegate void NetworkMessageHandler(long lobbyId, long userId, byte channelId, byte[] data);
	}
}
