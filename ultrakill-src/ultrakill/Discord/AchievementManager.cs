using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000777 RID: 1911
	public class AchievementManager
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06002954 RID: 10580 RVA: 0x0011275E File Offset: 0x0011095E
		private AchievementManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(AchievementManager.FFIMethods));
				}
				return (AchievementManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06002955 RID: 10581 RVA: 0x00112790 File Offset: 0x00110990
		// (remove) Token: 0x06002956 RID: 10582 RVA: 0x001127C8 File Offset: 0x001109C8
		public event AchievementManager.UserAchievementUpdateHandler OnUserAchievementUpdate;

		// Token: 0x06002957 RID: 10583 RVA: 0x00112800 File Offset: 0x00110A00
		internal AchievementManager(IntPtr ptr, IntPtr eventsPtr, ref AchievementManager.FFIEvents events)
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

		// Token: 0x06002958 RID: 10584 RVA: 0x0011284F File Offset: 0x00110A4F
		private void InitEvents(IntPtr eventsPtr, ref AchievementManager.FFIEvents events)
		{
			events.OnUserAchievementUpdate = new AchievementManager.FFIEvents.UserAchievementUpdateHandler(AchievementManager.OnUserAchievementUpdateImpl);
			Marshal.StructureToPtr<AchievementManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00112870 File Offset: 0x00110A70
		[MonoPInvokeCallback]
		private static void SetUserAchievementCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			AchievementManager.SetUserAchievementHandler setUserAchievementHandler = (AchievementManager.SetUserAchievementHandler)gchandle.Target;
			gchandle.Free();
			setUserAchievementHandler(result);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x001128A0 File Offset: 0x00110AA0
		public void SetUserAchievement(long achievementId, byte percentComplete, AchievementManager.SetUserAchievementHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.SetUserAchievement(this.MethodsPtr, achievementId, percentComplete, GCHandle.ToIntPtr(gchandle), new AchievementManager.FFIMethods.SetUserAchievementCallback(AchievementManager.SetUserAchievementCallbackImpl));
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x001128E0 File Offset: 0x00110AE0
		[MonoPInvokeCallback]
		private static void FetchUserAchievementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			AchievementManager.FetchUserAchievementsHandler fetchUserAchievementsHandler = (AchievementManager.FetchUserAchievementsHandler)gchandle.Target;
			gchandle.Free();
			fetchUserAchievementsHandler(result);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x00112910 File Offset: 0x00110B10
		public void FetchUserAchievements(AchievementManager.FetchUserAchievementsHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.FetchUserAchievements(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new AchievementManager.FFIMethods.FetchUserAchievementsCallback(AchievementManager.FetchUserAchievementsCallbackImpl));
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0011294C File Offset: 0x00110B4C
		public int CountUserAchievements()
		{
			int num = 0;
			this.Methods.CountUserAchievements(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x00112974 File Offset: 0x00110B74
		public UserAchievement GetUserAchievement(long userAchievementId)
		{
			UserAchievement userAchievement = default(UserAchievement);
			Result result = this.Methods.GetUserAchievement(this.MethodsPtr, userAchievementId, ref userAchievement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return userAchievement;
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x001129B0 File Offset: 0x00110BB0
		public UserAchievement GetUserAchievementAt(int index)
		{
			UserAchievement userAchievement = default(UserAchievement);
			Result result = this.Methods.GetUserAchievementAt(this.MethodsPtr, index, ref userAchievement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return userAchievement;
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x001129EC File Offset: 0x00110BEC
		[MonoPInvokeCallback]
		private static void OnUserAchievementUpdateImpl(IntPtr ptr, ref UserAchievement userAchievement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.AchievementManagerInstance.OnUserAchievementUpdate != null)
			{
				discord.AchievementManagerInstance.OnUserAchievementUpdate(ref userAchievement);
			}
		}

		// Token: 0x0400307F RID: 12415
		private IntPtr MethodsPtr;

		// Token: 0x04003080 RID: 12416
		private object MethodsStructure;

		// Token: 0x02000778 RID: 1912
		internal struct FFIEvents
		{
			// Token: 0x04003082 RID: 12418
			internal AchievementManager.FFIEvents.UserAchievementUpdateHandler OnUserAchievementUpdate;

			// Token: 0x02000779 RID: 1913
			// (Invoke) Token: 0x06002962 RID: 10594
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void UserAchievementUpdateHandler(IntPtr ptr, ref UserAchievement userAchievement);
		}

		// Token: 0x0200077A RID: 1914
		internal struct FFIMethods
		{
			// Token: 0x04003083 RID: 12419
			internal AchievementManager.FFIMethods.SetUserAchievementMethod SetUserAchievement;

			// Token: 0x04003084 RID: 12420
			internal AchievementManager.FFIMethods.FetchUserAchievementsMethod FetchUserAchievements;

			// Token: 0x04003085 RID: 12421
			internal AchievementManager.FFIMethods.CountUserAchievementsMethod CountUserAchievements;

			// Token: 0x04003086 RID: 12422
			internal AchievementManager.FFIMethods.GetUserAchievementMethod GetUserAchievement;

			// Token: 0x04003087 RID: 12423
			internal AchievementManager.FFIMethods.GetUserAchievementAtMethod GetUserAchievementAt;

			// Token: 0x0200077B RID: 1915
			// (Invoke) Token: 0x06002966 RID: 10598
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementCallback(IntPtr ptr, Result result);

			// Token: 0x0200077C RID: 1916
			// (Invoke) Token: 0x0600296A RID: 10602
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetUserAchievementMethod(IntPtr methodsPtr, long achievementId, byte percentComplete, IntPtr callbackData, AchievementManager.FFIMethods.SetUserAchievementCallback callback);

			// Token: 0x0200077D RID: 1917
			// (Invoke) Token: 0x0600296E RID: 10606
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsCallback(IntPtr ptr, Result result);

			// Token: 0x0200077E RID: 1918
			// (Invoke) Token: 0x06002972 RID: 10610
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchUserAchievementsMethod(IntPtr methodsPtr, IntPtr callbackData, AchievementManager.FFIMethods.FetchUserAchievementsCallback callback);

			// Token: 0x0200077F RID: 1919
			// (Invoke) Token: 0x06002976 RID: 10614
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountUserAchievementsMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000780 RID: 1920
			// (Invoke) Token: 0x0600297A RID: 10618
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementMethod(IntPtr methodsPtr, long userAchievementId, ref UserAchievement userAchievement);

			// Token: 0x02000781 RID: 1921
			// (Invoke) Token: 0x0600297E RID: 10622
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetUserAchievementAtMethod(IntPtr methodsPtr, int index, ref UserAchievement userAchievement);
		}

		// Token: 0x02000782 RID: 1922
		// (Invoke) Token: 0x06002982 RID: 10626
		public delegate void SetUserAchievementHandler(Result result);

		// Token: 0x02000783 RID: 1923
		// (Invoke) Token: 0x06002986 RID: 10630
		public delegate void FetchUserAchievementsHandler(Result result);

		// Token: 0x02000784 RID: 1924
		// (Invoke) Token: 0x0600298A RID: 10634
		public delegate void UserAchievementUpdateHandler(ref UserAchievement userAchievement);
	}
}
