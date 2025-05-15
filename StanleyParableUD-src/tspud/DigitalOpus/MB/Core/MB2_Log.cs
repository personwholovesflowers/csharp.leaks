using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000275 RID: 629
	public class MB2_Log
	{
		// Token: 0x06000EC4 RID: 3780 RVA: 0x000480C8 File Offset: 0x000462C8
		public static void Log(MB2_LogLevel l, string msg, MB2_LogLevel currentThreshold)
		{
			if (l <= currentThreshold)
			{
				if (l == MB2_LogLevel.error)
				{
					Debug.LogError(msg);
				}
				if (l == MB2_LogLevel.warn)
				{
					Debug.LogWarning(string.Format("frm={0} WARN {1}", Time.frameCount, msg));
				}
				if (l == MB2_LogLevel.info)
				{
					Debug.Log(string.Format("frm={0} INFO {1}", Time.frameCount, msg));
				}
				if (l == MB2_LogLevel.debug)
				{
					Debug.Log(string.Format("frm={0} DEBUG {1}", Time.frameCount, msg));
				}
				if (l == MB2_LogLevel.trace)
				{
					Debug.Log(string.Format("frm={0} TRACE {1}", Time.frameCount, msg));
				}
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00048160 File Offset: 0x00046360
		public static string Error(string msg, params object[] args)
		{
			string text = string.Format(msg, args);
			string text2 = string.Format("f={0} ERROR {1}", Time.frameCount, text);
			Debug.LogError(text2);
			return text2;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00048190 File Offset: 0x00046390
		public static string Warn(string msg, params object[] args)
		{
			string text = string.Format(msg, args);
			string text2 = string.Format("f={0} WARN {1}", Time.frameCount, text);
			Debug.LogWarning(text2);
			return text2;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000481C0 File Offset: 0x000463C0
		public static string Info(string msg, params object[] args)
		{
			string text = string.Format(msg, args);
			string text2 = string.Format("f={0} INFO {1}", Time.frameCount, text);
			Debug.Log(text2);
			return text2;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x000481F0 File Offset: 0x000463F0
		public static string LogDebug(string msg, params object[] args)
		{
			string text = string.Format(msg, args);
			string text2 = string.Format("f={0} DEBUG {1}", Time.frameCount, text);
			Debug.Log(text2);
			return text2;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00048220 File Offset: 0x00046420
		public static string Trace(string msg, params object[] args)
		{
			string text = string.Format(msg, args);
			string text2 = string.Format("f={0} TRACE {1}", Time.frameCount, text);
			Debug.Log(text2);
			return text2;
		}
	}
}
