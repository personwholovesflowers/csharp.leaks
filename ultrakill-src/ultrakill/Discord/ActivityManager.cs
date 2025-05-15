using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000636 RID: 1590
	public class ActivityManager
	{
		// Token: 0x06002486 RID: 9350 RVA: 0x0010E645 File Offset: 0x0010C845
		public void RegisterCommand()
		{
			this.RegisterCommand(null);
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x0010E64E File Offset: 0x0010C84E
		private ActivityManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ActivityManager.FFIMethods));
				}
				return (ActivityManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06002488 RID: 9352 RVA: 0x0010E680 File Offset: 0x0010C880
		// (remove) Token: 0x06002489 RID: 9353 RVA: 0x0010E6B8 File Offset: 0x0010C8B8
		public event ActivityManager.ActivityJoinHandler OnActivityJoin;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600248A RID: 9354 RVA: 0x0010E6F0 File Offset: 0x0010C8F0
		// (remove) Token: 0x0600248B RID: 9355 RVA: 0x0010E728 File Offset: 0x0010C928
		public event ActivityManager.ActivitySpectateHandler OnActivitySpectate;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600248C RID: 9356 RVA: 0x0010E760 File Offset: 0x0010C960
		// (remove) Token: 0x0600248D RID: 9357 RVA: 0x0010E798 File Offset: 0x0010C998
		public event ActivityManager.ActivityJoinRequestHandler OnActivityJoinRequest;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600248E RID: 9358 RVA: 0x0010E7D0 File Offset: 0x0010C9D0
		// (remove) Token: 0x0600248F RID: 9359 RVA: 0x0010E808 File Offset: 0x0010CA08
		public event ActivityManager.ActivityInviteHandler OnActivityInvite;

		// Token: 0x06002490 RID: 9360 RVA: 0x0010E840 File Offset: 0x0010CA40
		internal ActivityManager(IntPtr ptr, IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
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

		// Token: 0x06002491 RID: 9361 RVA: 0x0010E890 File Offset: 0x0010CA90
		private void InitEvents(IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
		{
			events.OnActivityJoin = new ActivityManager.FFIEvents.ActivityJoinHandler(ActivityManager.OnActivityJoinImpl);
			events.OnActivitySpectate = new ActivityManager.FFIEvents.ActivitySpectateHandler(ActivityManager.OnActivitySpectateImpl);
			events.OnActivityJoinRequest = new ActivityManager.FFIEvents.ActivityJoinRequestHandler(ActivityManager.OnActivityJoinRequestImpl);
			events.OnActivityInvite = new ActivityManager.FFIEvents.ActivityInviteHandler(ActivityManager.OnActivityInviteImpl);
			Marshal.StructureToPtr<ActivityManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x0010E8F4 File Offset: 0x0010CAF4
		public void RegisterCommand(string command)
		{
			Result result = this.Methods.RegisterCommand(this.MethodsPtr, command);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x0010E924 File Offset: 0x0010CB24
		public void RegisterSteam(uint steamId)
		{
			Result result = this.Methods.RegisterSteam(this.MethodsPtr, steamId);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0010E954 File Offset: 0x0010CB54
		[MonoPInvokeCallback]
		private static void UpdateActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.UpdateActivityHandler updateActivityHandler = (ActivityManager.UpdateActivityHandler)gchandle.Target;
			gchandle.Free();
			updateActivityHandler(result);
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x0010E984 File Offset: 0x0010CB84
		public void UpdateActivity(Activity activity, ActivityManager.UpdateActivityHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.UpdateActivity(this.MethodsPtr, ref activity, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.UpdateActivityCallback(ActivityManager.UpdateActivityCallbackImpl));
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x0010E9C4 File Offset: 0x0010CBC4
		[MonoPInvokeCallback]
		private static void ClearActivityCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.ClearActivityHandler clearActivityHandler = (ActivityManager.ClearActivityHandler)gchandle.Target;
			gchandle.Free();
			clearActivityHandler(result);
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0010E9F4 File Offset: 0x0010CBF4
		public void ClearActivity(ActivityManager.ClearActivityHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ClearActivity(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.ClearActivityCallback(ActivityManager.ClearActivityCallbackImpl));
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0010EA30 File Offset: 0x0010CC30
		[MonoPInvokeCallback]
		private static void SendRequestReplyCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.SendRequestReplyHandler sendRequestReplyHandler = (ActivityManager.SendRequestReplyHandler)gchandle.Target;
			gchandle.Free();
			sendRequestReplyHandler(result);
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0010EA60 File Offset: 0x0010CC60
		public void SendRequestReply(long userId, ActivityJoinRequestReply reply, ActivityManager.SendRequestReplyHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SendRequestReply(this.MethodsPtr, userId, reply, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.SendRequestReplyCallback(ActivityManager.SendRequestReplyCallbackImpl));
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0010EAA0 File Offset: 0x0010CCA0
		[MonoPInvokeCallback]
		private static void SendInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.SendInviteHandler sendInviteHandler = (ActivityManager.SendInviteHandler)gchandle.Target;
			gchandle.Free();
			sendInviteHandler(result);
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x0010EAD0 File Offset: 0x0010CCD0
		public void SendInvite(long userId, ActivityActionType type, string content, ActivityManager.SendInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SendInvite(this.MethodsPtr, userId, type, content, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.SendInviteCallback(ActivityManager.SendInviteCallbackImpl));
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x0010EB10 File Offset: 0x0010CD10
		[MonoPInvokeCallback]
		private static void AcceptInviteCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ActivityManager.AcceptInviteHandler acceptInviteHandler = (ActivityManager.AcceptInviteHandler)gchandle.Target;
			gchandle.Free();
			acceptInviteHandler(result);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x0010EB40 File Offset: 0x0010CD40
		public void AcceptInvite(long userId, ActivityManager.AcceptInviteHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.AcceptInvite(this.MethodsPtr, userId, GCHandle.ToIntPtr(gchandle), new ActivityManager.FFIMethods.AcceptInviteCallback(ActivityManager.AcceptInviteCallbackImpl));
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x0010EB80 File Offset: 0x0010CD80
		[MonoPInvokeCallback]
		private static void OnActivityJoinImpl(IntPtr ptr, string secret)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityJoin != null)
			{
				discord.ActivityManagerInstance.OnActivityJoin(secret);
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x0010EBC0 File Offset: 0x0010CDC0
		[MonoPInvokeCallback]
		private static void OnActivitySpectateImpl(IntPtr ptr, string secret)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivitySpectate != null)
			{
				discord.ActivityManagerInstance.OnActivitySpectate(secret);
			}
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x0010EC00 File Offset: 0x0010CE00
		[MonoPInvokeCallback]
		private static void OnActivityJoinRequestImpl(IntPtr ptr, ref User user)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityJoinRequest != null)
			{
				discord.ActivityManagerInstance.OnActivityJoinRequest(ref user);
			}
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x0010EC40 File Offset: 0x0010CE40
		[MonoPInvokeCallback]
		private static void OnActivityInviteImpl(IntPtr ptr, ActivityActionType type, ref User user, ref Activity activity)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.ActivityManagerInstance.OnActivityInvite != null)
			{
				discord.ActivityManagerInstance.OnActivityInvite(type, ref user, ref activity);
			}
		}

		// Token: 0x04002EB5 RID: 11957
		private IntPtr MethodsPtr;

		// Token: 0x04002EB6 RID: 11958
		private object MethodsStructure;

		// Token: 0x02000637 RID: 1591
		internal struct FFIEvents
		{
			// Token: 0x04002EBB RID: 11963
			internal ActivityManager.FFIEvents.ActivityJoinHandler OnActivityJoin;

			// Token: 0x04002EBC RID: 11964
			internal ActivityManager.FFIEvents.ActivitySpectateHandler OnActivitySpectate;

			// Token: 0x04002EBD RID: 11965
			internal ActivityManager.FFIEvents.ActivityJoinRequestHandler OnActivityJoinRequest;

			// Token: 0x04002EBE RID: 11966
			internal ActivityManager.FFIEvents.ActivityInviteHandler OnActivityInvite;

			// Token: 0x02000638 RID: 1592
			// (Invoke) Token: 0x060024A3 RID: 9379
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			// Token: 0x02000639 RID: 1593
			// (Invoke) Token: 0x060024A7 RID: 9383
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivitySpectateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

			// Token: 0x0200063A RID: 1594
			// (Invoke) Token: 0x060024AB RID: 9387
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityJoinRequestHandler(IntPtr ptr, ref User user);

			// Token: 0x0200063B RID: 1595
			// (Invoke) Token: 0x060024AF RID: 9391
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ActivityInviteHandler(IntPtr ptr, ActivityActionType type, ref User user, ref Activity activity);
		}

		// Token: 0x0200063C RID: 1596
		internal struct FFIMethods
		{
			// Token: 0x04002EBF RID: 11967
			internal ActivityManager.FFIMethods.RegisterCommandMethod RegisterCommand;

			// Token: 0x04002EC0 RID: 11968
			internal ActivityManager.FFIMethods.RegisterSteamMethod RegisterSteam;

			// Token: 0x04002EC1 RID: 11969
			internal ActivityManager.FFIMethods.UpdateActivityMethod UpdateActivity;

			// Token: 0x04002EC2 RID: 11970
			internal ActivityManager.FFIMethods.ClearActivityMethod ClearActivity;

			// Token: 0x04002EC3 RID: 11971
			internal ActivityManager.FFIMethods.SendRequestReplyMethod SendRequestReply;

			// Token: 0x04002EC4 RID: 11972
			internal ActivityManager.FFIMethods.SendInviteMethod SendInvite;

			// Token: 0x04002EC5 RID: 11973
			internal ActivityManager.FFIMethods.AcceptInviteMethod AcceptInvite;

			// Token: 0x0200063D RID: 1597
			// (Invoke) Token: 0x060024B3 RID: 9395
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterCommandMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string command);

			// Token: 0x0200063E RID: 1598
			// (Invoke) Token: 0x060024B7 RID: 9399
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RegisterSteamMethod(IntPtr methodsPtr, uint steamId);

			// Token: 0x0200063F RID: 1599
			// (Invoke) Token: 0x060024BB RID: 9403
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityCallback(IntPtr ptr, Result result);

			// Token: 0x02000640 RID: 1600
			// (Invoke) Token: 0x060024BF RID: 9407
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UpdateActivityMethod(IntPtr methodsPtr, ref Activity activity, IntPtr callbackData, ActivityManager.FFIMethods.UpdateActivityCallback callback);

			// Token: 0x02000641 RID: 1601
			// (Invoke) Token: 0x060024C3 RID: 9411
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityCallback(IntPtr ptr, Result result);

			// Token: 0x02000642 RID: 1602
			// (Invoke) Token: 0x060024C7 RID: 9415
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ClearActivityMethod(IntPtr methodsPtr, IntPtr callbackData, ActivityManager.FFIMethods.ClearActivityCallback callback);

			// Token: 0x02000643 RID: 1603
			// (Invoke) Token: 0x060024CB RID: 9419
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyCallback(IntPtr ptr, Result result);

			// Token: 0x02000644 RID: 1604
			// (Invoke) Token: 0x060024CF RID: 9423
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendRequestReplyMethod(IntPtr methodsPtr, long userId, ActivityJoinRequestReply reply, IntPtr callbackData, ActivityManager.FFIMethods.SendRequestReplyCallback callback);

			// Token: 0x02000645 RID: 1605
			// (Invoke) Token: 0x060024D3 RID: 9427
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000646 RID: 1606
			// (Invoke) Token: 0x060024D7 RID: 9431
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SendInviteMethod(IntPtr methodsPtr, long userId, ActivityActionType type, [MarshalAs(UnmanagedType.LPStr)] string content, IntPtr callbackData, ActivityManager.FFIMethods.SendInviteCallback callback);

			// Token: 0x02000647 RID: 1607
			// (Invoke) Token: 0x060024DB RID: 9435
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteCallback(IntPtr ptr, Result result);

			// Token: 0x02000648 RID: 1608
			// (Invoke) Token: 0x060024DF RID: 9439
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void AcceptInviteMethod(IntPtr methodsPtr, long userId, IntPtr callbackData, ActivityManager.FFIMethods.AcceptInviteCallback callback);
		}

		// Token: 0x02000649 RID: 1609
		// (Invoke) Token: 0x060024E3 RID: 9443
		public delegate void UpdateActivityHandler(Result result);

		// Token: 0x0200064A RID: 1610
		// (Invoke) Token: 0x060024E7 RID: 9447
		public delegate void ClearActivityHandler(Result result);

		// Token: 0x0200064B RID: 1611
		// (Invoke) Token: 0x060024EB RID: 9451
		public delegate void SendRequestReplyHandler(Result result);

		// Token: 0x0200064C RID: 1612
		// (Invoke) Token: 0x060024EF RID: 9455
		public delegate void SendInviteHandler(Result result);

		// Token: 0x0200064D RID: 1613
		// (Invoke) Token: 0x060024F3 RID: 9459
		public delegate void AcceptInviteHandler(Result result);

		// Token: 0x0200064E RID: 1614
		// (Invoke) Token: 0x060024F7 RID: 9463
		public delegate void ActivityJoinHandler(string secret);

		// Token: 0x0200064F RID: 1615
		// (Invoke) Token: 0x060024FB RID: 9467
		public delegate void ActivitySpectateHandler(string secret);

		// Token: 0x02000650 RID: 1616
		// (Invoke) Token: 0x060024FF RID: 9471
		public delegate void ActivityJoinRequestHandler(ref User user);

		// Token: 0x02000651 RID: 1617
		// (Invoke) Token: 0x06002503 RID: 9475
		public delegate void ActivityInviteHandler(ActivityActionType type, ref User user, ref Activity activity);
	}
}
