using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x020006AF RID: 1711
	public class UserManager
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060025D8 RID: 9688 RVA: 0x0010FAE8 File Offset: 0x0010DCE8
		private UserManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(UserManager.FFIMethods));
				}
				return (UserManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060025D9 RID: 9689 RVA: 0x0010FB18 File Offset: 0x0010DD18
		// (remove) Token: 0x060025DA RID: 9690 RVA: 0x0010FB50 File Offset: 0x0010DD50
		public event UserManager.CurrentUserUpdateHandler OnCurrentUserUpdate;

		// Token: 0x060025DB RID: 9691 RVA: 0x0010FB88 File Offset: 0x0010DD88
		internal UserManager(IntPtr ptr, IntPtr eventsPtr, ref UserManager.FFIEvents events)
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

		// Token: 0x060025DC RID: 9692 RVA: 0x0010FBD7 File Offset: 0x0010DDD7
		private void InitEvents(IntPtr eventsPtr, ref UserManager.FFIEvents events)
		{
			events.OnCurrentUserUpdate = new UserManager.FFIEvents.CurrentUserUpdateHandler(UserManager.OnCurrentUserUpdateImpl);
			Marshal.StructureToPtr<UserManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x0010FBF8 File Offset: 0x0010DDF8
		public User GetCurrentUser()
		{
			User user = default(User);
			Result result = this.Methods.GetCurrentUser(this.MethodsPtr, ref user);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return user;
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x0010FC34 File Offset: 0x0010DE34
		[MonoPInvokeCallback]
		private static void GetUserCallbackImpl(IntPtr ptr, Result result, ref User user)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			UserManager.GetUserHandler getUserHandler = (UserManager.GetUserHandler)gchandle.Target;
			gchandle.Free();
			getUserHandler(result, ref user);
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x0010FC64 File Offset: 0x0010DE64
		public void GetUser(long userId, UserManager.GetUserHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.GetUser(this.MethodsPtr, userId, GCHandle.ToIntPtr(gchandle), new UserManager.FFIMethods.GetUserCallback(UserManager.GetUserCallbackImpl));
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x0010FCA4 File Offset: 0x0010DEA4
		public PremiumType GetCurrentUserPremiumType()
		{
			PremiumType premiumType = PremiumType.None;
			Result result = this.Methods.GetCurrentUserPremiumType(this.MethodsPtr, ref premiumType);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return premiumType;
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x0010FCD8 File Offset: 0x0010DED8
		public bool CurrentUserHasFlag(UserFlag flag)
		{
			bool flag2 = false;
			Result result = this.Methods.CurrentUserHasFlag(this.MethodsPtr, flag, ref flag2);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag2;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x0010FD0C File Offset: 0x0010DF0C
		[MonoPInvokeCallback]
		private static void OnCurrentUserUpdateImpl(IntPtr ptr)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.UserManagerInstance.OnCurrentUserUpdate != null)
			{
				discord.UserManagerInstance.OnCurrentUserUpdate();
			}
		}

		// Token: 0x04002FF2 RID: 12274
		private IntPtr MethodsPtr;

		// Token: 0x04002FF3 RID: 12275
		private object MethodsStructure;

		// Token: 0x020006B0 RID: 1712
		internal struct FFIEvents
		{
			// Token: 0x04002FF5 RID: 12277
			internal UserManager.FFIEvents.CurrentUserUpdateHandler OnCurrentUserUpdate;

			// Token: 0x020006B1 RID: 1713
			// (Invoke) Token: 0x060025E4 RID: 9700
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CurrentUserUpdateHandler(IntPtr ptr);
		}

		// Token: 0x020006B2 RID: 1714
		internal struct FFIMethods
		{
			// Token: 0x04002FF6 RID: 12278
			internal UserManager.FFIMethods.GetCurrentUserMethod GetCurrentUser;

			// Token: 0x04002FF7 RID: 12279
			internal UserManager.FFIMethods.GetUserMethod GetUser;

			// Token: 0x04002FF8 RID: 12280
			internal UserManager.FFIMethods.GetCurrentUserPremiumTypeMethod GetCurrentUserPremiumType;

			// Token: 0x04002FF9 RID: 12281
			internal UserManager.FFIMethods.CurrentUserHasFlagMethod CurrentUserHasFlag;

			// Token: 0x020006B3 RID: 1715
			// (Invoke) Token: 0x060025E8 RID: 9704
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserMethod(IntPtr methodsPtr, ref User currentUser);

			// Token: 0x020006B4 RID: 1716
			// (Invoke) Token: 0x060025EC RID: 9708
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserCallback(IntPtr ptr, Result result, ref User user);

			// Token: 0x020006B5 RID: 1717
			// (Invoke) Token: 0x060025F0 RID: 9712
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetUserMethod(IntPtr methodsPtr, long userId, IntPtr callbackData, UserManager.FFIMethods.GetUserCallback callback);

			// Token: 0x020006B6 RID: 1718
			// (Invoke) Token: 0x060025F4 RID: 9716
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetCurrentUserPremiumTypeMethod(IntPtr methodsPtr, ref PremiumType premiumType);

			// Token: 0x020006B7 RID: 1719
			// (Invoke) Token: 0x060025F8 RID: 9720
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result CurrentUserHasFlagMethod(IntPtr methodsPtr, UserFlag flag, ref bool hasFlag);
		}

		// Token: 0x020006B8 RID: 1720
		// (Invoke) Token: 0x060025FC RID: 9724
		public delegate void GetUserHandler(Result result, ref User user);

		// Token: 0x020006B9 RID: 1721
		// (Invoke) Token: 0x06002600 RID: 9728
		public delegate void CurrentUserUpdateHandler();
	}
}
