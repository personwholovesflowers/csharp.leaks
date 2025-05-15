using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x020006A1 RID: 1697
	public class ApplicationManager
	{
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x0010F8A1 File Offset: 0x0010DAA1
		private ApplicationManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ApplicationManager.FFIMethods));
				}
				return (ApplicationManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x0010F8D4 File Offset: 0x0010DAD4
		internal ApplicationManager(IntPtr ptr, IntPtr eventsPtr, ref ApplicationManager.FFIEvents events)
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

		// Token: 0x060025A3 RID: 9635 RVA: 0x0010F923 File Offset: 0x0010DB23
		private void InitEvents(IntPtr eventsPtr, ref ApplicationManager.FFIEvents events)
		{
			Marshal.StructureToPtr<ApplicationManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x0010F934 File Offset: 0x0010DB34
		[MonoPInvokeCallback]
		private static void ValidateOrExitCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.ValidateOrExitHandler validateOrExitHandler = (ApplicationManager.ValidateOrExitHandler)gchandle.Target;
			gchandle.Free();
			validateOrExitHandler(result);
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x0010F964 File Offset: 0x0010DB64
		public void ValidateOrExit(ApplicationManager.ValidateOrExitHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ValidateOrExit(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ApplicationManager.FFIMethods.ValidateOrExitCallback(ApplicationManager.ValidateOrExitCallbackImpl));
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x0010F9A0 File Offset: 0x0010DBA0
		public string GetCurrentLocale()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			this.Methods.GetCurrentLocale(this.MethodsPtr, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x0010F9D8 File Offset: 0x0010DBD8
		public string GetCurrentBranch()
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			this.Methods.GetCurrentBranch(this.MethodsPtr, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x0010FA10 File Offset: 0x0010DC10
		[MonoPInvokeCallback]
		private static void GetOAuth2TokenCallbackImpl(IntPtr ptr, Result result, ref OAuth2Token oauth2Token)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.GetOAuth2TokenHandler getOAuth2TokenHandler = (ApplicationManager.GetOAuth2TokenHandler)gchandle.Target;
			gchandle.Free();
			getOAuth2TokenHandler(result, ref oauth2Token);
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x0010FA40 File Offset: 0x0010DC40
		public void GetOAuth2Token(ApplicationManager.GetOAuth2TokenHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.GetOAuth2Token(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ApplicationManager.FFIMethods.GetOAuth2TokenCallback(ApplicationManager.GetOAuth2TokenCallbackImpl));
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x0010FA7C File Offset: 0x0010DC7C
		[MonoPInvokeCallback]
		private static void GetTicketCallbackImpl(IntPtr ptr, Result result, ref string data)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ApplicationManager.GetTicketHandler getTicketHandler = (ApplicationManager.GetTicketHandler)gchandle.Target;
			gchandle.Free();
			getTicketHandler(result, ref data);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x0010FAAC File Offset: 0x0010DCAC
		public void GetTicket(ApplicationManager.GetTicketHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.GetTicket(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new ApplicationManager.FFIMethods.GetTicketCallback(ApplicationManager.GetTicketCallbackImpl));
		}

		// Token: 0x04002FEB RID: 12267
		private IntPtr MethodsPtr;

		// Token: 0x04002FEC RID: 12268
		private object MethodsStructure;

		// Token: 0x020006A2 RID: 1698
		internal struct FFIEvents
		{
		}

		// Token: 0x020006A3 RID: 1699
		internal struct FFIMethods
		{
			// Token: 0x04002FED RID: 12269
			internal ApplicationManager.FFIMethods.ValidateOrExitMethod ValidateOrExit;

			// Token: 0x04002FEE RID: 12270
			internal ApplicationManager.FFIMethods.GetCurrentLocaleMethod GetCurrentLocale;

			// Token: 0x04002FEF RID: 12271
			internal ApplicationManager.FFIMethods.GetCurrentBranchMethod GetCurrentBranch;

			// Token: 0x04002FF0 RID: 12272
			internal ApplicationManager.FFIMethods.GetOAuth2TokenMethod GetOAuth2Token;

			// Token: 0x04002FF1 RID: 12273
			internal ApplicationManager.FFIMethods.GetTicketMethod GetTicket;

			// Token: 0x020006A4 RID: 1700
			// (Invoke) Token: 0x060025AD RID: 9645
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitCallback(IntPtr ptr, Result result);

			// Token: 0x020006A5 RID: 1701
			// (Invoke) Token: 0x060025B1 RID: 9649
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ValidateOrExitMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.ValidateOrExitCallback callback);

			// Token: 0x020006A6 RID: 1702
			// (Invoke) Token: 0x060025B5 RID: 9653
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentLocaleMethod(IntPtr methodsPtr, StringBuilder locale);

			// Token: 0x020006A7 RID: 1703
			// (Invoke) Token: 0x060025B9 RID: 9657
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetCurrentBranchMethod(IntPtr methodsPtr, StringBuilder branch);

			// Token: 0x020006A8 RID: 1704
			// (Invoke) Token: 0x060025BD RID: 9661
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenCallback(IntPtr ptr, Result result, ref OAuth2Token oauth2Token);

			// Token: 0x020006A9 RID: 1705
			// (Invoke) Token: 0x060025C1 RID: 9665
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetOAuth2TokenMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.GetOAuth2TokenCallback callback);

			// Token: 0x020006AA RID: 1706
			// (Invoke) Token: 0x060025C5 RID: 9669
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketCallback(IntPtr ptr, Result result, [MarshalAs(UnmanagedType.LPStr)] ref string data);

			// Token: 0x020006AB RID: 1707
			// (Invoke) Token: 0x060025C9 RID: 9673
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void GetTicketMethod(IntPtr methodsPtr, IntPtr callbackData, ApplicationManager.FFIMethods.GetTicketCallback callback);
		}

		// Token: 0x020006AC RID: 1708
		// (Invoke) Token: 0x060025CD RID: 9677
		public delegate void ValidateOrExitHandler(Result result);

		// Token: 0x020006AD RID: 1709
		// (Invoke) Token: 0x060025D1 RID: 9681
		public delegate void GetOAuth2TokenHandler(Result result, ref OAuth2Token oauth2Token);

		// Token: 0x020006AE RID: 1710
		// (Invoke) Token: 0x060025D5 RID: 9685
		public delegate void GetTicketHandler(Result result, ref string data);
	}
}
