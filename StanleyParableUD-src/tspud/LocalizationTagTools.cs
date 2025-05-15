using System;
using UnityEngine;

// Token: 0x02000101 RID: 257
public static class LocalizationTagTools
{
	// Token: 0x06000620 RID: 1568 RVA: 0x00021D1C File Offset: 0x0001FF1C
	public static StanleyPlatform GetNextMostGeneralPlatformVariation(StanleyPlatform specificPlatform)
	{
		if (specificPlatform <= StanleyPlatform.XBOX360)
		{
			if (specificPlatform <= StanleyPlatform.Playstation4)
			{
				if (specificPlatform == StanleyPlatform.PC)
				{
					return StanleyPlatform.NoVariation;
				}
				if (specificPlatform == StanleyPlatform.Playstation4)
				{
					return StanleyPlatform.Playstation;
				}
			}
			else
			{
				if (specificPlatform == StanleyPlatform.Playstation5)
				{
					return StanleyPlatform.Playstation;
				}
				if (specificPlatform == StanleyPlatform.Playstation)
				{
					return StanleyPlatform.Console;
				}
				if (specificPlatform == StanleyPlatform.XBOX360)
				{
					return StanleyPlatform.XBOX;
				}
			}
		}
		else if (specificPlatform <= StanleyPlatform.XBOX)
		{
			if (specificPlatform == StanleyPlatform.XBOXone)
			{
				return StanleyPlatform.XBOX;
			}
			if (specificPlatform == StanleyPlatform.XBOX)
			{
				return StanleyPlatform.Console;
			}
		}
		else
		{
			if (specificPlatform == StanleyPlatform.Switch)
			{
				return StanleyPlatform.Console;
			}
			if (specificPlatform == StanleyPlatform.Console)
			{
				return StanleyPlatform.Port;
			}
			if (specificPlatform == StanleyPlatform.Mobile)
			{
				return StanleyPlatform.Port;
			}
		}
		return StanleyPlatform.Invalid;
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00021D95 File Offset: 0x0001FF95
	public static string GetVoiceAudioClipBaseName(string audioClipBasename, PlatformTag[] platformVariations, bool useBucketIfAvailable, bool hasBucketClip)
	{
		return LocalizationTagTools.GetVoiceAudioClipBaseName(audioClipBasename, PlatformSettings.GetCurrentRunningPlatform(), platformVariations, useBucketIfAvailable, hasBucketClip);
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00021DA5 File Offset: 0x0001FFA5
	public static string GetVoiceAudioClipBaseName(string audioClipBasename, RuntimePlatform runtimePlatform, PlatformTag[] platformVariations, bool useBucketIfAvailable, bool hasBucketClip)
	{
		return LocalizationTagTools.GetVoiceAudioClipBaseName(audioClipBasename, PlatformSettings.GetStanleyPlatform(runtimePlatform), platformVariations, useBucketIfAvailable, hasBucketClip);
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00021DB8 File Offset: 0x0001FFB8
	public static string GetVoiceAudioClipBaseName(string audioClipBasename, StanleyPlatform runtimePlatform, PlatformTag[] platformVariations, bool useBucketIfAvailable, bool hasBucketClip)
	{
		int length = audioClipBasename.Length;
		PlatformTag platformTag = LocalizationTagTools.GetPlatformTag(runtimePlatform, platformVariations);
		bool flag = platformTag != null && platformTag.tag.Contains(">");
		bool flag2 = platformTag != null && !flag;
		bool flag3 = useBucketIfAvailable && hasBucketClip;
		string text = "";
		if (flag2)
		{
			text = "_" + platformTag.tag;
		}
		if (flag3)
		{
			text += LocalizationTagTools.BUCKETSUFFIX;
		}
		string text2;
		if (length > 3 && char.IsNumber(audioClipBasename[length - 1]) && char.IsNumber(audioClipBasename[length - 2]) && audioClipBasename[length - 3] == '_')
		{
			text2 = audioClipBasename.Insert(length - 3, text);
		}
		else
		{
			text2 = audioClipBasename + text;
		}
		if (flag)
		{
			string[] array = platformTag.tag.Split(new char[] { '>' });
			text2 = LocalizationTagTools.ReplaceLastOnly(text2, array[0], array[1]);
		}
		return text2;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00021EA4 File Offset: 0x000200A4
	public static PlatformTag GetPlatformTag(StanleyPlatform platform, PlatformTag[] platformVariations)
	{
		if (platformVariations.Length == 0)
		{
			return null;
		}
		StanleyPlatform platformToCheck = platform;
		Predicate<PlatformTag> <>9__0;
		while (platformToCheck != StanleyPlatform.NoVariation && platformToCheck != StanleyPlatform.Invalid)
		{
			Predicate<PlatformTag> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (PlatformTag x) => x.platform == platformToCheck);
			}
			PlatformTag platformTag = Array.Find<PlatformTag>(platformVariations, predicate);
			if (platformTag != null)
			{
				return platformTag;
			}
			platformToCheck = LocalizationTagTools.GetNextMostGeneralPlatformVariation(platformToCheck);
		}
		return null;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00021F18 File Offset: 0x00020118
	private static string ReplaceLastOnly(string orig, string target, string replacement)
	{
		int num = orig.LastIndexOf(target);
		if (num == -1)
		{
			return orig;
		}
		return orig.Substring(0, num) + replacement + orig.Substring(num + target.Length);
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x00021F50 File Offset: 0x00020150
	[Obsolete]
	public static string GetBucketVoiceClipName(string baseName)
	{
		int length = baseName.Length;
		if (length > 3 && char.IsNumber(baseName[length - 1]) && char.IsNumber(baseName[length - 2]) && baseName[length - 3] == '_')
		{
			baseName = baseName.Insert(length - 3, LocalizationTagTools.BUCKETSUFFIX);
		}
		else
		{
			baseName += LocalizationTagTools.BUCKETSUFFIX;
		}
		return baseName;
	}

	// Token: 0x04000689 RID: 1673
	public static readonly string BUCKETSUFFIX = "_BUCKET";
}
