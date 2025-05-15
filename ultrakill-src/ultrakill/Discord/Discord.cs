using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200068B RID: 1675
	public class Discord : IDisposable
	{
		// Token: 0x06002548 RID: 9544
		[DllImport("discord_game_sdk", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
		private static extern Result DiscordCreate(uint version, ref Discord.FFICreateParams createParams, out IntPtr manager);

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06002549 RID: 9545 RVA: 0x0010F096 File Offset: 0x0010D296
		private Discord.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(Discord.FFIMethods));
				}
				return (Discord.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x0010F0C8 File Offset: 0x0010D2C8
		public Discord(long clientId, ulong flags)
		{
			Discord.FFICreateParams fficreateParams;
			fficreateParams.ClientId = clientId;
			fficreateParams.Flags = flags;
			this.Events = default(Discord.FFIEvents);
			this.EventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Discord.FFIEvents>(this.Events));
			fficreateParams.Events = this.EventsPtr;
			this.SelfHandle = GCHandle.Alloc(this);
			fficreateParams.EventData = GCHandle.ToIntPtr(this.SelfHandle);
			this.ApplicationEvents = default(ApplicationManager.FFIEvents);
			this.ApplicationEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ApplicationManager.FFIEvents>(this.ApplicationEvents));
			fficreateParams.ApplicationEvents = this.ApplicationEventsPtr;
			fficreateParams.ApplicationVersion = 1U;
			this.UserEvents = default(UserManager.FFIEvents);
			this.UserEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<UserManager.FFIEvents>(this.UserEvents));
			fficreateParams.UserEvents = this.UserEventsPtr;
			fficreateParams.UserVersion = 1U;
			this.ImageEvents = default(ImageManager.FFIEvents);
			this.ImageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ImageManager.FFIEvents>(this.ImageEvents));
			fficreateParams.ImageEvents = this.ImageEventsPtr;
			fficreateParams.ImageVersion = 1U;
			this.ActivityEvents = default(ActivityManager.FFIEvents);
			this.ActivityEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ActivityManager.FFIEvents>(this.ActivityEvents));
			fficreateParams.ActivityEvents = this.ActivityEventsPtr;
			fficreateParams.ActivityVersion = 1U;
			this.RelationshipEvents = default(RelationshipManager.FFIEvents);
			this.RelationshipEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<RelationshipManager.FFIEvents>(this.RelationshipEvents));
			fficreateParams.RelationshipEvents = this.RelationshipEventsPtr;
			fficreateParams.RelationshipVersion = 1U;
			this.LobbyEvents = default(LobbyManager.FFIEvents);
			this.LobbyEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LobbyManager.FFIEvents>(this.LobbyEvents));
			fficreateParams.LobbyEvents = this.LobbyEventsPtr;
			fficreateParams.LobbyVersion = 1U;
			this.NetworkEvents = default(NetworkManager.FFIEvents);
			this.NetworkEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NetworkManager.FFIEvents>(this.NetworkEvents));
			fficreateParams.NetworkEvents = this.NetworkEventsPtr;
			fficreateParams.NetworkVersion = 1U;
			this.OverlayEvents = default(OverlayManager.FFIEvents);
			this.OverlayEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<OverlayManager.FFIEvents>(this.OverlayEvents));
			fficreateParams.OverlayEvents = this.OverlayEventsPtr;
			fficreateParams.OverlayVersion = 1U;
			this.StorageEvents = default(StorageManager.FFIEvents);
			this.StorageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StorageManager.FFIEvents>(this.StorageEvents));
			fficreateParams.StorageEvents = this.StorageEventsPtr;
			fficreateParams.StorageVersion = 1U;
			this.StoreEvents = default(StoreManager.FFIEvents);
			this.StoreEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StoreManager.FFIEvents>(this.StoreEvents));
			fficreateParams.StoreEvents = this.StoreEventsPtr;
			fficreateParams.StoreVersion = 1U;
			this.VoiceEvents = default(VoiceManager.FFIEvents);
			this.VoiceEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<VoiceManager.FFIEvents>(this.VoiceEvents));
			fficreateParams.VoiceEvents = this.VoiceEventsPtr;
			fficreateParams.VoiceVersion = 1U;
			this.AchievementEvents = default(AchievementManager.FFIEvents);
			this.AchievementEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<AchievementManager.FFIEvents>(this.AchievementEvents));
			fficreateParams.AchievementEvents = this.AchievementEventsPtr;
			fficreateParams.AchievementVersion = 1U;
			this.InitEvents(this.EventsPtr, ref this.Events);
			Result result = Discord.DiscordCreate(2U, ref fficreateParams, out this.MethodsPtr);
			if (result != Result.Ok)
			{
				this.Dispose();
				throw new ResultException(result);
			}
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x0010F3FD File Offset: 0x0010D5FD
		private void InitEvents(IntPtr eventsPtr, ref Discord.FFIEvents events)
		{
			Marshal.StructureToPtr<Discord.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x0010F40C File Offset: 0x0010D60C
		public void Dispose()
		{
			if (this.MethodsPtr != IntPtr.Zero)
			{
				this.Methods.Destroy(this.MethodsPtr);
			}
			this.SelfHandle.Free();
			Marshal.FreeHGlobal(this.EventsPtr);
			Marshal.FreeHGlobal(this.ApplicationEventsPtr);
			Marshal.FreeHGlobal(this.UserEventsPtr);
			Marshal.FreeHGlobal(this.ImageEventsPtr);
			Marshal.FreeHGlobal(this.ActivityEventsPtr);
			Marshal.FreeHGlobal(this.RelationshipEventsPtr);
			Marshal.FreeHGlobal(this.LobbyEventsPtr);
			Marshal.FreeHGlobal(this.NetworkEventsPtr);
			Marshal.FreeHGlobal(this.OverlayEventsPtr);
			Marshal.FreeHGlobal(this.StorageEventsPtr);
			Marshal.FreeHGlobal(this.StoreEventsPtr);
			Marshal.FreeHGlobal(this.VoiceEventsPtr);
			Marshal.FreeHGlobal(this.AchievementEventsPtr);
			if (this.setLogHook != null)
			{
				this.setLogHook.Value.Free();
			}
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x0010F4FC File Offset: 0x0010D6FC
		public void RunCallbacks()
		{
			Result result = this.Methods.RunCallbacks(this.MethodsPtr);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x0010F52C File Offset: 0x0010D72C
		[MonoPInvokeCallback]
		private static void SetLogHookCallbackImpl(IntPtr ptr, LogLevel level, string message)
		{
			((Discord.SetLogHookHandler)GCHandle.FromIntPtr(ptr).Target)(level, message);
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x0010F554 File Offset: 0x0010D754
		public void SetLogHook(LogLevel minLevel, Discord.SetLogHookHandler callback)
		{
			if (this.setLogHook != null)
			{
				this.setLogHook.Value.Free();
			}
			this.setLogHook = new GCHandle?(GCHandle.Alloc(callback));
			this.Methods.SetLogHook(this.MethodsPtr, minLevel, GCHandle.ToIntPtr(this.setLogHook.Value), new Discord.FFIMethods.SetLogHookCallback(Discord.SetLogHookCallbackImpl));
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x0010F5C5 File Offset: 0x0010D7C5
		public ApplicationManager GetApplicationManager()
		{
			if (this.ApplicationManagerInstance == null)
			{
				this.ApplicationManagerInstance = new ApplicationManager(this.Methods.GetApplicationManager(this.MethodsPtr), this.ApplicationEventsPtr, ref this.ApplicationEvents);
			}
			return this.ApplicationManagerInstance;
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x0010F602 File Offset: 0x0010D802
		public UserManager GetUserManager()
		{
			if (this.UserManagerInstance == null)
			{
				this.UserManagerInstance = new UserManager(this.Methods.GetUserManager(this.MethodsPtr), this.UserEventsPtr, ref this.UserEvents);
			}
			return this.UserManagerInstance;
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x0010F63F File Offset: 0x0010D83F
		public ImageManager GetImageManager()
		{
			if (this.ImageManagerInstance == null)
			{
				this.ImageManagerInstance = new ImageManager(this.Methods.GetImageManager(this.MethodsPtr), this.ImageEventsPtr, ref this.ImageEvents);
			}
			return this.ImageManagerInstance;
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x0010F67C File Offset: 0x0010D87C
		public ActivityManager GetActivityManager()
		{
			if (this.ActivityManagerInstance == null)
			{
				this.ActivityManagerInstance = new ActivityManager(this.Methods.GetActivityManager(this.MethodsPtr), this.ActivityEventsPtr, ref this.ActivityEvents);
			}
			return this.ActivityManagerInstance;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x0010F6B9 File Offset: 0x0010D8B9
		public RelationshipManager GetRelationshipManager()
		{
			if (this.RelationshipManagerInstance == null)
			{
				this.RelationshipManagerInstance = new RelationshipManager(this.Methods.GetRelationshipManager(this.MethodsPtr), this.RelationshipEventsPtr, ref this.RelationshipEvents);
			}
			return this.RelationshipManagerInstance;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x0010F6F6 File Offset: 0x0010D8F6
		public LobbyManager GetLobbyManager()
		{
			if (this.LobbyManagerInstance == null)
			{
				this.LobbyManagerInstance = new LobbyManager(this.Methods.GetLobbyManager(this.MethodsPtr), this.LobbyEventsPtr, ref this.LobbyEvents);
			}
			return this.LobbyManagerInstance;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x0010F733 File Offset: 0x0010D933
		public NetworkManager GetNetworkManager()
		{
			if (this.NetworkManagerInstance == null)
			{
				this.NetworkManagerInstance = new NetworkManager(this.Methods.GetNetworkManager(this.MethodsPtr), this.NetworkEventsPtr, ref this.NetworkEvents);
			}
			return this.NetworkManagerInstance;
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x0010F770 File Offset: 0x0010D970
		public OverlayManager GetOverlayManager()
		{
			if (this.OverlayManagerInstance == null)
			{
				this.OverlayManagerInstance = new OverlayManager(this.Methods.GetOverlayManager(this.MethodsPtr), this.OverlayEventsPtr, ref this.OverlayEvents);
			}
			return this.OverlayManagerInstance;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x0010F7AD File Offset: 0x0010D9AD
		public StorageManager GetStorageManager()
		{
			if (this.StorageManagerInstance == null)
			{
				this.StorageManagerInstance = new StorageManager(this.Methods.GetStorageManager(this.MethodsPtr), this.StorageEventsPtr, ref this.StorageEvents);
			}
			return this.StorageManagerInstance;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x0010F7EA File Offset: 0x0010D9EA
		public StoreManager GetStoreManager()
		{
			if (this.StoreManagerInstance == null)
			{
				this.StoreManagerInstance = new StoreManager(this.Methods.GetStoreManager(this.MethodsPtr), this.StoreEventsPtr, ref this.StoreEvents);
			}
			return this.StoreManagerInstance;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x0010F827 File Offset: 0x0010DA27
		public VoiceManager GetVoiceManager()
		{
			if (this.VoiceManagerInstance == null)
			{
				this.VoiceManagerInstance = new VoiceManager(this.Methods.GetVoiceManager(this.MethodsPtr), this.VoiceEventsPtr, ref this.VoiceEvents);
			}
			return this.VoiceManagerInstance;
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x0010F864 File Offset: 0x0010DA64
		public AchievementManager GetAchievementManager()
		{
			if (this.AchievementManagerInstance == null)
			{
				this.AchievementManagerInstance = new AchievementManager(this.Methods.GetAchievementManager(this.MethodsPtr), this.AchievementEventsPtr, ref this.AchievementEvents);
			}
			return this.AchievementManagerInstance;
		}

		// Token: 0x04002F96 RID: 12182
		private GCHandle SelfHandle;

		// Token: 0x04002F97 RID: 12183
		private IntPtr EventsPtr;

		// Token: 0x04002F98 RID: 12184
		private Discord.FFIEvents Events;

		// Token: 0x04002F99 RID: 12185
		private IntPtr ApplicationEventsPtr;

		// Token: 0x04002F9A RID: 12186
		private ApplicationManager.FFIEvents ApplicationEvents;

		// Token: 0x04002F9B RID: 12187
		internal ApplicationManager ApplicationManagerInstance;

		// Token: 0x04002F9C RID: 12188
		private IntPtr UserEventsPtr;

		// Token: 0x04002F9D RID: 12189
		private UserManager.FFIEvents UserEvents;

		// Token: 0x04002F9E RID: 12190
		internal UserManager UserManagerInstance;

		// Token: 0x04002F9F RID: 12191
		private IntPtr ImageEventsPtr;

		// Token: 0x04002FA0 RID: 12192
		private ImageManager.FFIEvents ImageEvents;

		// Token: 0x04002FA1 RID: 12193
		internal ImageManager ImageManagerInstance;

		// Token: 0x04002FA2 RID: 12194
		private IntPtr ActivityEventsPtr;

		// Token: 0x04002FA3 RID: 12195
		private ActivityManager.FFIEvents ActivityEvents;

		// Token: 0x04002FA4 RID: 12196
		internal ActivityManager ActivityManagerInstance;

		// Token: 0x04002FA5 RID: 12197
		private IntPtr RelationshipEventsPtr;

		// Token: 0x04002FA6 RID: 12198
		private RelationshipManager.FFIEvents RelationshipEvents;

		// Token: 0x04002FA7 RID: 12199
		internal RelationshipManager RelationshipManagerInstance;

		// Token: 0x04002FA8 RID: 12200
		private IntPtr LobbyEventsPtr;

		// Token: 0x04002FA9 RID: 12201
		private LobbyManager.FFIEvents LobbyEvents;

		// Token: 0x04002FAA RID: 12202
		internal LobbyManager LobbyManagerInstance;

		// Token: 0x04002FAB RID: 12203
		private IntPtr NetworkEventsPtr;

		// Token: 0x04002FAC RID: 12204
		private NetworkManager.FFIEvents NetworkEvents;

		// Token: 0x04002FAD RID: 12205
		internal NetworkManager NetworkManagerInstance;

		// Token: 0x04002FAE RID: 12206
		private IntPtr OverlayEventsPtr;

		// Token: 0x04002FAF RID: 12207
		private OverlayManager.FFIEvents OverlayEvents;

		// Token: 0x04002FB0 RID: 12208
		internal OverlayManager OverlayManagerInstance;

		// Token: 0x04002FB1 RID: 12209
		private IntPtr StorageEventsPtr;

		// Token: 0x04002FB2 RID: 12210
		private StorageManager.FFIEvents StorageEvents;

		// Token: 0x04002FB3 RID: 12211
		internal StorageManager StorageManagerInstance;

		// Token: 0x04002FB4 RID: 12212
		private IntPtr StoreEventsPtr;

		// Token: 0x04002FB5 RID: 12213
		private StoreManager.FFIEvents StoreEvents;

		// Token: 0x04002FB6 RID: 12214
		internal StoreManager StoreManagerInstance;

		// Token: 0x04002FB7 RID: 12215
		private IntPtr VoiceEventsPtr;

		// Token: 0x04002FB8 RID: 12216
		private VoiceManager.FFIEvents VoiceEvents;

		// Token: 0x04002FB9 RID: 12217
		internal VoiceManager VoiceManagerInstance;

		// Token: 0x04002FBA RID: 12218
		private IntPtr AchievementEventsPtr;

		// Token: 0x04002FBB RID: 12219
		private AchievementManager.FFIEvents AchievementEvents;

		// Token: 0x04002FBC RID: 12220
		internal AchievementManager AchievementManagerInstance;

		// Token: 0x04002FBD RID: 12221
		private IntPtr MethodsPtr;

		// Token: 0x04002FBE RID: 12222
		private object MethodsStructure;

		// Token: 0x04002FBF RID: 12223
		private GCHandle? setLogHook;

		// Token: 0x0200068C RID: 1676
		internal struct FFIEvents
		{
		}

		// Token: 0x0200068D RID: 1677
		internal struct FFIMethods
		{
			// Token: 0x04002FC0 RID: 12224
			internal Discord.FFIMethods.DestroyHandler Destroy;

			// Token: 0x04002FC1 RID: 12225
			internal Discord.FFIMethods.RunCallbacksMethod RunCallbacks;

			// Token: 0x04002FC2 RID: 12226
			internal Discord.FFIMethods.SetLogHookMethod SetLogHook;

			// Token: 0x04002FC3 RID: 12227
			internal Discord.FFIMethods.GetApplicationManagerMethod GetApplicationManager;

			// Token: 0x04002FC4 RID: 12228
			internal Discord.FFIMethods.GetUserManagerMethod GetUserManager;

			// Token: 0x04002FC5 RID: 12229
			internal Discord.FFIMethods.GetImageManagerMethod GetImageManager;

			// Token: 0x04002FC6 RID: 12230
			internal Discord.FFIMethods.GetActivityManagerMethod GetActivityManager;

			// Token: 0x04002FC7 RID: 12231
			internal Discord.FFIMethods.GetRelationshipManagerMethod GetRelationshipManager;

			// Token: 0x04002FC8 RID: 12232
			internal Discord.FFIMethods.GetLobbyManagerMethod GetLobbyManager;

			// Token: 0x04002FC9 RID: 12233
			internal Discord.FFIMethods.GetNetworkManagerMethod GetNetworkManager;

			// Token: 0x04002FCA RID: 12234
			internal Discord.FFIMethods.GetOverlayManagerMethod GetOverlayManager;

			// Token: 0x04002FCB RID: 12235
			internal Discord.FFIMethods.GetStorageManagerMethod GetStorageManager;

			// Token: 0x04002FCC RID: 12236
			internal Discord.FFIMethods.GetStoreManagerMethod GetStoreManager;

			// Token: 0x04002FCD RID: 12237
			internal Discord.FFIMethods.GetVoiceManagerMethod GetVoiceManager;

			// Token: 0x04002FCE RID: 12238
			internal Discord.FFIMethods.GetAchievementManagerMethod GetAchievementManager;

			// Token: 0x0200068E RID: 1678
			// (Invoke) Token: 0x0600255D RID: 9565
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DestroyHandler(IntPtr MethodsPtr);

			// Token: 0x0200068F RID: 1679
			// (Invoke) Token: 0x06002561 RID: 9569
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RunCallbacksMethod(IntPtr methodsPtr);

			// Token: 0x02000690 RID: 1680
			// (Invoke) Token: 0x06002565 RID: 9573
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookCallback(IntPtr ptr, LogLevel level, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x02000691 RID: 1681
			// (Invoke) Token: 0x06002569 RID: 9577
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookMethod(IntPtr methodsPtr, LogLevel minLevel, IntPtr callbackData, Discord.FFIMethods.SetLogHookCallback callback);

			// Token: 0x02000692 RID: 1682
			// (Invoke) Token: 0x0600256D RID: 9581
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetApplicationManagerMethod(IntPtr discordPtr);

			// Token: 0x02000693 RID: 1683
			// (Invoke) Token: 0x06002571 RID: 9585
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetUserManagerMethod(IntPtr discordPtr);

			// Token: 0x02000694 RID: 1684
			// (Invoke) Token: 0x06002575 RID: 9589
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetImageManagerMethod(IntPtr discordPtr);

			// Token: 0x02000695 RID: 1685
			// (Invoke) Token: 0x06002579 RID: 9593
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetActivityManagerMethod(IntPtr discordPtr);

			// Token: 0x02000696 RID: 1686
			// (Invoke) Token: 0x0600257D RID: 9597
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetRelationshipManagerMethod(IntPtr discordPtr);

			// Token: 0x02000697 RID: 1687
			// (Invoke) Token: 0x06002581 RID: 9601
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetLobbyManagerMethod(IntPtr discordPtr);

			// Token: 0x02000698 RID: 1688
			// (Invoke) Token: 0x06002585 RID: 9605
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetNetworkManagerMethod(IntPtr discordPtr);

			// Token: 0x02000699 RID: 1689
			// (Invoke) Token: 0x06002589 RID: 9609
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetOverlayManagerMethod(IntPtr discordPtr);

			// Token: 0x0200069A RID: 1690
			// (Invoke) Token: 0x0600258D RID: 9613
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStorageManagerMethod(IntPtr discordPtr);

			// Token: 0x0200069B RID: 1691
			// (Invoke) Token: 0x06002591 RID: 9617
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStoreManagerMethod(IntPtr discordPtr);

			// Token: 0x0200069C RID: 1692
			// (Invoke) Token: 0x06002595 RID: 9621
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetVoiceManagerMethod(IntPtr discordPtr);

			// Token: 0x0200069D RID: 1693
			// (Invoke) Token: 0x06002599 RID: 9625
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetAchievementManagerMethod(IntPtr discordPtr);
		}

		// Token: 0x0200069E RID: 1694
		internal struct FFICreateParams
		{
			// Token: 0x04002FCF RID: 12239
			internal long ClientId;

			// Token: 0x04002FD0 RID: 12240
			internal ulong Flags;

			// Token: 0x04002FD1 RID: 12241
			internal IntPtr Events;

			// Token: 0x04002FD2 RID: 12242
			internal IntPtr EventData;

			// Token: 0x04002FD3 RID: 12243
			internal IntPtr ApplicationEvents;

			// Token: 0x04002FD4 RID: 12244
			internal uint ApplicationVersion;

			// Token: 0x04002FD5 RID: 12245
			internal IntPtr UserEvents;

			// Token: 0x04002FD6 RID: 12246
			internal uint UserVersion;

			// Token: 0x04002FD7 RID: 12247
			internal IntPtr ImageEvents;

			// Token: 0x04002FD8 RID: 12248
			internal uint ImageVersion;

			// Token: 0x04002FD9 RID: 12249
			internal IntPtr ActivityEvents;

			// Token: 0x04002FDA RID: 12250
			internal uint ActivityVersion;

			// Token: 0x04002FDB RID: 12251
			internal IntPtr RelationshipEvents;

			// Token: 0x04002FDC RID: 12252
			internal uint RelationshipVersion;

			// Token: 0x04002FDD RID: 12253
			internal IntPtr LobbyEvents;

			// Token: 0x04002FDE RID: 12254
			internal uint LobbyVersion;

			// Token: 0x04002FDF RID: 12255
			internal IntPtr NetworkEvents;

			// Token: 0x04002FE0 RID: 12256
			internal uint NetworkVersion;

			// Token: 0x04002FE1 RID: 12257
			internal IntPtr OverlayEvents;

			// Token: 0x04002FE2 RID: 12258
			internal uint OverlayVersion;

			// Token: 0x04002FE3 RID: 12259
			internal IntPtr StorageEvents;

			// Token: 0x04002FE4 RID: 12260
			internal uint StorageVersion;

			// Token: 0x04002FE5 RID: 12261
			internal IntPtr StoreEvents;

			// Token: 0x04002FE6 RID: 12262
			internal uint StoreVersion;

			// Token: 0x04002FE7 RID: 12263
			internal IntPtr VoiceEvents;

			// Token: 0x04002FE8 RID: 12264
			internal uint VoiceVersion;

			// Token: 0x04002FE9 RID: 12265
			internal IntPtr AchievementEvents;

			// Token: 0x04002FEA RID: 12266
			internal uint AchievementVersion;
		}

		// Token: 0x0200069F RID: 1695
		// (Invoke) Token: 0x0600259D RID: 9629
		public delegate void SetLogHookHandler(LogLevel level, string message);
	}
}
