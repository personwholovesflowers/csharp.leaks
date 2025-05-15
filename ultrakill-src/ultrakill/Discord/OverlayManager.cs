using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000728 RID: 1832
	public class OverlayManager
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x00111757 File Offset: 0x0010F957
		private OverlayManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(OverlayManager.FFIMethods));
				}
				return (OverlayManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060027FE RID: 10238 RVA: 0x00111788 File Offset: 0x0010F988
		// (remove) Token: 0x060027FF RID: 10239 RVA: 0x001117C0 File Offset: 0x0010F9C0
		public event OverlayManager.ToggleHandler OnToggle;

		// Token: 0x06002800 RID: 10240 RVA: 0x001117F8 File Offset: 0x0010F9F8
		internal OverlayManager(IntPtr ptr, IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
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

		// Token: 0x06002801 RID: 10241 RVA: 0x00111847 File Offset: 0x0010FA47
		private void InitEvents(IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
		{
			events.OnToggle = new OverlayManager.FFIEvents.ToggleHandler(OverlayManager.OnToggleImpl);
			Marshal.StructureToPtr<OverlayManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x00111868 File Offset: 0x0010FA68
		public bool IsEnabled()
		{
			bool flag = false;
			this.Methods.IsEnabled(this.MethodsPtr, ref flag);
			return flag;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x00111890 File Offset: 0x0010FA90
		public bool IsLocked()
		{
			bool flag = false;
			this.Methods.IsLocked(this.MethodsPtr, ref flag);
			return flag;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x001118B8 File Offset: 0x0010FAB8
		[MonoPInvokeCallback]
		private static void SetLockedCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.SetLockedHandler setLockedHandler = (OverlayManager.SetLockedHandler)gchandle.Target;
			gchandle.Free();
			setLockedHandler(result);
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x001118E8 File Offset: 0x0010FAE8
		public void SetLocked(bool locked, OverlayManager.SetLockedHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SetLocked(this.MethodsPtr, locked, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.SetLockedCallback(OverlayManager.SetLockedCallbackImpl));
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x00111928 File Offset: 0x0010FB28
		[MonoPInvokeCallback]
		private static void OpenActivityInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenActivityInviteHandler openActivityInviteHandler = (OverlayManager.OpenActivityInviteHandler)gchandle.Target;
			gchandle.Free();
			openActivityInviteHandler(result);
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x00111958 File Offset: 0x0010FB58
		public void OpenActivityInvite(ActivityActionType type, OverlayManager.OpenActivityInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.OpenActivityInvite(this.MethodsPtr, type, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.OpenActivityInviteCallback(OverlayManager.OpenActivityInviteCallbackImpl));
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x00111998 File Offset: 0x0010FB98
		[MonoPInvokeCallback]
		private static void OpenGuildInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenGuildInviteHandler openGuildInviteHandler = (OverlayManager.OpenGuildInviteHandler)gchandle.Target;
			gchandle.Free();
			openGuildInviteHandler(result);
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x001119C8 File Offset: 0x0010FBC8
		public void OpenGuildInvite(string code, OverlayManager.OpenGuildInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.OpenGuildInvite(this.MethodsPtr, code, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.OpenGuildInviteCallback(OverlayManager.OpenGuildInviteCallbackImpl));
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x00111A08 File Offset: 0x0010FC08
		[MonoPInvokeCallback]
		private static void OpenVoiceSettingsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			OverlayManager.OpenVoiceSettingsHandler openVoiceSettingsHandler = (OverlayManager.OpenVoiceSettingsHandler)gchandle.Target;
			gchandle.Free();
			openVoiceSettingsHandler(result);
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x00111A38 File Offset: 0x0010FC38
		public void OpenVoiceSettings(OverlayManager.OpenVoiceSettingsHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.OpenVoiceSettings(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new OverlayManager.FFIMethods.OpenVoiceSettingsCallback(OverlayManager.OpenVoiceSettingsCallbackImpl));
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x00111A74 File Offset: 0x0010FC74
		[MonoPInvokeCallback]
		private static void OnToggleImpl(IntPtr ptr, bool locked)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.OverlayManagerInstance.OnToggle != null)
			{
				discord.OverlayManagerInstance.OnToggle(locked);
			}
		}

		// Token: 0x0400304A RID: 12362
		private IntPtr MethodsPtr;

		// Token: 0x0400304B RID: 12363
		private object MethodsStructure;

		// Token: 0x02000729 RID: 1833
		internal struct FFIEvents
		{
			// Token: 0x0400304D RID: 12365
			internal OverlayManager.FFIEvents.ToggleHandler OnToggle;

			// Token: 0x0200072A RID: 1834
			// (Invoke) Token: 0x0600280E RID: 10254
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ToggleHandler(IntPtr ptr, bool locked);
		}

		// Token: 0x0200072B RID: 1835
		internal struct FFIMethods
		{
			// Token: 0x0400304E RID: 12366
			internal OverlayManager.FFIMethods.IsEnabledMethod IsEnabled;

			// Token: 0x0400304F RID: 12367
			internal OverlayManager.FFIMethods.IsLockedMethod IsLocked;

			// Token: 0x04003050 RID: 12368
			internal OverlayManager.FFIMethods.SetLockedMethod SetLocked;

			// Token: 0x04003051 RID: 12369
			internal OverlayManager.FFIMethods.OpenActivityInviteMethod OpenActivityInvite;

			// Token: 0x04003052 RID: 12370
			internal OverlayManager.FFIMethods.OpenGuildInviteMethod OpenGuildInvite;

			// Token: 0x04003053 RID: 12371
			internal OverlayManager.FFIMethods.OpenVoiceSettingsMethod OpenVoiceSettings;

			// Token: 0x0200072C RID: 1836
			// (Invoke) Token: 0x06002812 RID: 10258
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsEnabledMethod(IntPtr methodsPtr, ref bool enabled);

			// Token: 0x0200072D RID: 1837
			// (Invoke) Token: 0x06002816 RID: 10262
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void IsLockedMethod(IntPtr methodsPtr, ref bool locked);

			// Token: 0x0200072E RID: 1838
			// (Invoke) Token: 0x0600281A RID: 10266
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedCallback(IntPtr ptr, Result result);

			// Token: 0x0200072F RID: 1839
			// (Invoke) Token: 0x0600281E RID: 10270
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLockedMethod(IntPtr methodsPtr, bool locked, IntPtr callbackData, OverlayManager.FFIMethods.SetLockedCallback callback);

			// Token: 0x02000730 RID: 1840
			// (Invoke) Token: 0x06002822 RID: 10274
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000731 RID: 1841
			// (Invoke) Token: 0x06002826 RID: 10278
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenActivityInviteMethod(IntPtr methodsPtr, ActivityActionType type, IntPtr callbackData, OverlayManager.FFIMethods.OpenActivityInviteCallback callback);

			// Token: 0x02000732 RID: 1842
			// (Invoke) Token: 0x0600282A RID: 10282
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000733 RID: 1843
			// (Invoke) Token: 0x0600282E RID: 10286
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenGuildInviteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string code, IntPtr callbackData, OverlayManager.FFIMethods.OpenGuildInviteCallback callback);

			// Token: 0x02000734 RID: 1844
			// (Invoke) Token: 0x06002832 RID: 10290
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsCallback(IntPtr ptr, Result result);

			// Token: 0x02000735 RID: 1845
			// (Invoke) Token: 0x06002836 RID: 10294
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void OpenVoiceSettingsMethod(IntPtr methodsPtr, IntPtr callbackData, OverlayManager.FFIMethods.OpenVoiceSettingsCallback callback);
		}

		// Token: 0x02000736 RID: 1846
		// (Invoke) Token: 0x0600283A RID: 10298
		public delegate void SetLockedHandler(Result result);

		// Token: 0x02000737 RID: 1847
		// (Invoke) Token: 0x0600283E RID: 10302
		public delegate void OpenActivityInviteHandler(Result result);

		// Token: 0x02000738 RID: 1848
		// (Invoke) Token: 0x06002842 RID: 10306
		public delegate void OpenGuildInviteHandler(Result result);

		// Token: 0x02000739 RID: 1849
		// (Invoke) Token: 0x06002846 RID: 10310
		public delegate void OpenVoiceSettingsHandler(Result result);

		// Token: 0x0200073A RID: 1850
		// (Invoke) Token: 0x0600284A RID: 10314
		public delegate void ToggleHandler(bool locked);
	}
}
