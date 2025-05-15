using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000766 RID: 1894
	public class VoiceManager
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600290B RID: 10507 RVA: 0x001123D4 File Offset: 0x001105D4
		private VoiceManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(VoiceManager.FFIMethods));
				}
				return (VoiceManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600290C RID: 10508 RVA: 0x00112404 File Offset: 0x00110604
		// (remove) Token: 0x0600290D RID: 10509 RVA: 0x0011243C File Offset: 0x0011063C
		public event VoiceManager.SettingsUpdateHandler OnSettingsUpdate;

		// Token: 0x0600290E RID: 10510 RVA: 0x00112474 File Offset: 0x00110674
		internal VoiceManager(IntPtr ptr, IntPtr eventsPtr, ref VoiceManager.FFIEvents events)
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

		// Token: 0x0600290F RID: 10511 RVA: 0x001124C3 File Offset: 0x001106C3
		private void InitEvents(IntPtr eventsPtr, ref VoiceManager.FFIEvents events)
		{
			events.OnSettingsUpdate = new VoiceManager.FFIEvents.SettingsUpdateHandler(VoiceManager.OnSettingsUpdateImpl);
			Marshal.StructureToPtr<VoiceManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x001124E4 File Offset: 0x001106E4
		public InputMode GetInputMode()
		{
			InputMode inputMode = default(InputMode);
			Result result = this.Methods.GetInputMode(this.MethodsPtr, ref inputMode);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return inputMode;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x00112520 File Offset: 0x00110720
		[MonoPInvokeCallback]
		private static void SetInputModeCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			VoiceManager.SetInputModeHandler setInputModeHandler = (VoiceManager.SetInputModeHandler)gchandle.Target;
			gchandle.Free();
			setInputModeHandler(result);
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x00112550 File Offset: 0x00110750
		public void SetInputMode(InputMode inputMode, VoiceManager.SetInputModeHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SetInputMode(this.MethodsPtr, inputMode, GCHandle.ToIntPtr(gchandle), new VoiceManager.FFIMethods.SetInputModeCallback(VoiceManager.SetInputModeCallbackImpl));
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x00112590 File Offset: 0x00110790
		public bool IsSelfMute()
		{
			bool flag = false;
			Result result = this.Methods.IsSelfMute(this.MethodsPtr, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x001125C4 File Offset: 0x001107C4
		public void SetSelfMute(bool mute)
		{
			Result result = this.Methods.SetSelfMute(this.MethodsPtr, mute);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x001125F4 File Offset: 0x001107F4
		public bool IsSelfDeaf()
		{
			bool flag = false;
			Result result = this.Methods.IsSelfDeaf(this.MethodsPtr, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x00112628 File Offset: 0x00110828
		public void SetSelfDeaf(bool deaf)
		{
			Result result = this.Methods.SetSelfDeaf(this.MethodsPtr, deaf);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x00112658 File Offset: 0x00110858
		public bool IsLocalMute(long userId)
		{
			bool flag = false;
			Result result = this.Methods.IsLocalMute(this.MethodsPtr, userId, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x0011268C File Offset: 0x0011088C
		public void SetLocalMute(long userId, bool mute)
		{
			Result result = this.Methods.SetLocalMute(this.MethodsPtr, userId, mute);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x001126BC File Offset: 0x001108BC
		public byte GetLocalVolume(long userId)
		{
			byte b = 0;
			Result result = this.Methods.GetLocalVolume(this.MethodsPtr, userId, ref b);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return b;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x001126F0 File Offset: 0x001108F0
		public void SetLocalVolume(long userId, byte volume)
		{
			Result result = this.Methods.SetLocalVolume(this.MethodsPtr, userId, volume);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x00112720 File Offset: 0x00110920
		[MonoPInvokeCallback]
		private static void OnSettingsUpdateImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.VoiceManagerInstance.OnSettingsUpdate != null)
			{
				discord.VoiceManagerInstance.OnSettingsUpdate();
			}
		}

		// Token: 0x04003071 RID: 12401
		private IntPtr MethodsPtr;

		// Token: 0x04003072 RID: 12402
		private object MethodsStructure;

		// Token: 0x02000767 RID: 1895
		internal struct FFIEvents
		{
			// Token: 0x04003074 RID: 12404
			internal VoiceManager.FFIEvents.SettingsUpdateHandler OnSettingsUpdate;

			// Token: 0x02000768 RID: 1896
			// (Invoke) Token: 0x0600291D RID: 10525
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SettingsUpdateHandler(IntPtr ptr);
		}

		// Token: 0x02000769 RID: 1897
		internal struct FFIMethods
		{
			// Token: 0x04003075 RID: 12405
			internal VoiceManager.FFIMethods.GetInputModeMethod GetInputMode;

			// Token: 0x04003076 RID: 12406
			internal VoiceManager.FFIMethods.SetInputModeMethod SetInputMode;

			// Token: 0x04003077 RID: 12407
			internal VoiceManager.FFIMethods.IsSelfMuteMethod IsSelfMute;

			// Token: 0x04003078 RID: 12408
			internal VoiceManager.FFIMethods.SetSelfMuteMethod SetSelfMute;

			// Token: 0x04003079 RID: 12409
			internal VoiceManager.FFIMethods.IsSelfDeafMethod IsSelfDeaf;

			// Token: 0x0400307A RID: 12410
			internal VoiceManager.FFIMethods.SetSelfDeafMethod SetSelfDeaf;

			// Token: 0x0400307B RID: 12411
			internal VoiceManager.FFIMethods.IsLocalMuteMethod IsLocalMute;

			// Token: 0x0400307C RID: 12412
			internal VoiceManager.FFIMethods.SetLocalMuteMethod SetLocalMute;

			// Token: 0x0400307D RID: 12413
			internal VoiceManager.FFIMethods.GetLocalVolumeMethod GetLocalVolume;

			// Token: 0x0400307E RID: 12414
			internal VoiceManager.FFIMethods.SetLocalVolumeMethod SetLocalVolume;

			// Token: 0x0200076A RID: 1898
			// (Invoke) Token: 0x06002921 RID: 10529
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetInputModeMethod(IntPtr methodsPtr, ref InputMode inputMode);

			// Token: 0x0200076B RID: 1899
			// (Invoke) Token: 0x06002925 RID: 10533
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeCallback(IntPtr ptr, Result result);

			// Token: 0x0200076C RID: 1900
			// (Invoke) Token: 0x06002929 RID: 10537
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetInputModeMethod(IntPtr methodsPtr, InputMode inputMode, IntPtr callbackData, VoiceManager.FFIMethods.SetInputModeCallback callback);

			// Token: 0x0200076D RID: 1901
			// (Invoke) Token: 0x0600292D RID: 10541
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfMuteMethod(IntPtr methodsPtr, ref bool mute);

			// Token: 0x0200076E RID: 1902
			// (Invoke) Token: 0x06002931 RID: 10545
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfMuteMethod(IntPtr methodsPtr, bool mute);

			// Token: 0x0200076F RID: 1903
			// (Invoke) Token: 0x06002935 RID: 10549
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsSelfDeafMethod(IntPtr methodsPtr, ref bool deaf);

			// Token: 0x02000770 RID: 1904
			// (Invoke) Token: 0x06002939 RID: 10553
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetSelfDeafMethod(IntPtr methodsPtr, bool deaf);

			// Token: 0x02000771 RID: 1905
			// (Invoke) Token: 0x0600293D RID: 10557
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result IsLocalMuteMethod(IntPtr methodsPtr, long userId, ref bool mute);

			// Token: 0x02000772 RID: 1906
			// (Invoke) Token: 0x06002941 RID: 10561
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalMuteMethod(IntPtr methodsPtr, long userId, bool mute);

			// Token: 0x02000773 RID: 1907
			// (Invoke) Token: 0x06002945 RID: 10565
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetLocalVolumeMethod(IntPtr methodsPtr, long userId, ref byte volume);

			// Token: 0x02000774 RID: 1908
			// (Invoke) Token: 0x06002949 RID: 10569
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result SetLocalVolumeMethod(IntPtr methodsPtr, long userId, byte volume);
		}

		// Token: 0x02000775 RID: 1909
		// (Invoke) Token: 0x0600294D RID: 10573
		public delegate void SetInputModeHandler(Result result);

		// Token: 0x02000776 RID: 1910
		// (Invoke) Token: 0x06002951 RID: 10577
		public delegate void SettingsUpdateHandler();
	}
}
