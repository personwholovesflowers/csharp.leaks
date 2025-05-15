using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200027F RID: 639
public static class InternalDebug
{
	// Token: 0x06000E0F RID: 3599 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition)
	{
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition, Object context)
	{
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition, object message)
	{
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition, object message, Object context)
	{
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void AssertFormat(bool condition, string format, params object[] args)
	{
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void AssertFormat(bool condition, Object context, string format, params object[] args)
	{
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x000695BA File Offset: 0x000677BA
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void Break()
	{
		Debug.Break();
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x000695C1 File Offset: 0x000677C1
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void ClearDeveloperConsole()
	{
		Debug.ClearDeveloperConsole();
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x000695C8 File Offset: 0x000677C8
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0f, bool depthTest = true)
	{
		Debug.DrawLine(start, end, color, duration, depthTest);
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x000695D5 File Offset: 0x000677D5
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0f, bool depthTest = true)
	{
		Debug.DrawRay(start, dir, color, duration, depthTest);
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x000695E2 File Offset: 0x000677E2
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void Log(object message, bool condition = true)
	{
		if (condition)
		{
			Debug.Log(message);
		}
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x000695ED File Offset: 0x000677ED
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void Log(object message, Object context, bool condition = true)
	{
		if (condition)
		{
			Debug.Log(message, context);
		}
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x000695F9 File Offset: 0x000677F9
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void LogAssertion(object message, bool condition = true)
	{
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x000695FD File Offset: 0x000677FD
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void LogAssertion(object message, Object context, bool condition = true)
	{
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void LogAssertionFormat(string format, params object[] args)
	{
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x000695F9 File Offset: 0x000677F9
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void LogAssertionFormat(string format, bool condition = true, params object[] args)
	{
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x00004AE3 File Offset: 0x00002CE3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void LogAssertionFormat(Object context, string format, params object[] args)
	{
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x000695FD File Offset: 0x000677FD
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	[Conditional("UNITY_ASSERTIONS")]
	public static void LogAssertionFormat(Object context, string format, bool condition = true, params object[] args)
	{
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x00069601 File Offset: 0x00067801
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogError(object message, bool condition = true)
	{
		if (condition)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0006960C File Offset: 0x0006780C
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogError(object message, Object context, bool condition = true)
	{
		if (condition)
		{
			Debug.LogError(message, context);
		}
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x00069618 File Offset: 0x00067818
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogErrorFormat(string format, params object[] args)
	{
		Debug.LogErrorFormat(format, args);
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00069621 File Offset: 0x00067821
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogErrorFormat(string format, bool condition = true, params object[] args)
	{
		if (condition)
		{
			Debug.LogErrorFormat(format, args);
		}
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x0006962D File Offset: 0x0006782D
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogErrorFormat(Object context, string format, params object[] args)
	{
		Debug.LogErrorFormat(context, format, args);
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x00069637 File Offset: 0x00067837
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogErrorFormat(Object context, string format, bool condition = true, params object[] args)
	{
		if (condition)
		{
			Debug.LogErrorFormat(context, format, args);
		}
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x00069644 File Offset: 0x00067844
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogException(Exception exception, bool condition = true)
	{
		if (condition)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0006964F File Offset: 0x0006784F
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogException(Exception exception, Object context, bool condition = true)
	{
		if (condition)
		{
			Debug.LogException(exception, context);
		}
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x0006965B File Offset: 0x0006785B
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogFormat(string format, params object[] args)
	{
		Debug.LogFormat(format, args);
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x00069664 File Offset: 0x00067864
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogFormat(string format, bool condition = true, params object[] args)
	{
		if (condition)
		{
			Debug.LogFormat(format, args);
		}
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x00069670 File Offset: 0x00067870
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogFormat(Object context, string format, params object[] args)
	{
		Debug.LogFormat(context, format, args);
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0006967A File Offset: 0x0006787A
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogFormat(Object context, string format, bool condition = true, params object[] args)
	{
		if (condition)
		{
			Debug.LogFormat(context, format, args);
		}
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x00069687 File Offset: 0x00067887
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarning(object message, bool condition = true)
	{
		if (condition)
		{
			Debug.LogWarning(message);
		}
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x00069692 File Offset: 0x00067892
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarning(object message, Object context, bool condition = true)
	{
		if (condition)
		{
			Debug.LogWarning(message, context);
		}
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x0006969E File Offset: 0x0006789E
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarningFormat(string format, params object[] args)
	{
		Debug.LogWarningFormat(format, args);
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x000696A7 File Offset: 0x000678A7
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarningFormat(string format, bool condition = true, params object[] args)
	{
		if (condition)
		{
			Debug.LogWarningFormat(format, args);
		}
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x000696B3 File Offset: 0x000678B3
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarningFormat(Object context, string format, params object[] args)
	{
		Debug.LogWarningFormat(context, format, args);
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x000696BD File Offset: 0x000678BD
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarningFormat(Object context, string format, bool condition = true, params object[] args)
	{
		if (condition)
		{
			Debug.LogWarningFormat(context, format, args);
		}
	}
}
