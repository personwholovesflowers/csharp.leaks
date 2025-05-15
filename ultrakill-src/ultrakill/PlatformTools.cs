using System;

// Token: 0x02000340 RID: 832
public static class PlatformTools
{
	// Token: 0x06001330 RID: 4912 RVA: 0x0009ACC8 File Offset: 0x00098EC8
	public static string ResolveArg(string key)
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (!(commandLineArgs[i] != key) && commandLineArgs.Length > i + 1)
			{
				return commandLineArgs[i + 1];
			}
		}
		return null;
	}
}
