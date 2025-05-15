using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

// Token: 0x02000138 RID: 312
[CreateAssetMenu(fileName = "New Message Box Dialouge", menuName = "Stanley/MessageBoxDialouge")]
public class MessageBoxDialogue : ScriptableObject
{
	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000751 RID: 1873 RVA: 0x00025F35 File Offset: 0x00024135
	public string[] Tags
	{
		get
		{
			return this.keys;
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00025F40 File Offset: 0x00024140
	private void DisplayMessagesForPlatform(StanleyPlatform platform)
	{
		if (this.localizationType == MessageBoxDialogue.LocalizeType.UnlocalizedStings)
		{
			new List<string>(this.messages);
			return;
		}
		new List<string>(this.PlatformAdjustedKeys(platform)).ConvertAll<string>((string t) => t + "\n\t\t\"" + LocalizationManager.GetTranslation(t, true, 0, true, false, null, null) + "\"");
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00025F93 File Offset: 0x00024193
	private void DisplayAllMessages()
	{
		this.DisplayMessagesForPlatform(PlatformSettings.GetStanleyPlatform(Application.platform));
		this.DisplayMessagesForPlatform(StanleyPlatform.PC);
		this.DisplayMessagesForPlatform(StanleyPlatform.Playstation);
		this.DisplayMessagesForPlatform(StanleyPlatform.XBOX);
		this.DisplayMessagesForPlatform(StanleyPlatform.Switch);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x00025FC4 File Offset: 0x000241C4
	public IEnumerable<string> PlatformAdjustedKeys()
	{
		foreach (string text in this.Keys())
		{
			yield return LocalizationTagTools.GetVoiceAudioClipBaseName(text, this.platformVariations, false, false);
		}
		IEnumerator<string> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00025FD4 File Offset: 0x000241D4
	public IEnumerable<string> PlatformAdjustedKeys(StanleyPlatform platform)
	{
		foreach (string text in this.Keys())
		{
			yield return LocalizationTagTools.GetVoiceAudioClipBaseName(text, platform, this.platformVariations, false, false);
		}
		IEnumerator<string> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00025FEB File Offset: 0x000241EB
	public IEnumerable<string> Keys()
	{
		switch (this.localizationType)
		{
		case MessageBoxDialogue.LocalizeType.UnlocalizedStings:
			yield break;
		case MessageBoxDialogue.LocalizeType.KeyArray:
		{
			foreach (string text in this.keys)
			{
				yield return text;
			}
			string[] array = null;
			yield break;
		}
		case MessageBoxDialogue.LocalizeType.KeyBaseAndCount:
		{
			int num;
			for (int i = 0; i < this.keyBaseCount; i = num + 1)
			{
				yield return string.Format("{0}_{1:00}", this.keyBaseString, i);
				num = i;
			}
			yield break;
		}
		case MessageBoxDialogue.LocalizeType.LastKeyInSequence:
		{
			string baseString = this.lastKey.Substring(0, this.lastKey.Length - 2);
			int count = int.Parse(this.lastKey.Substring(this.lastKey.Length - 2));
			int num;
			for (int i = 0; i <= count; i = num + 1)
			{
				yield return string.Format("{0}{1:00}", baseString, i);
				num = i;
			}
			yield break;
		}
		default:
		{
			string baseString = null;
			yield break;
		}
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00025FFC File Offset: 0x000241FC
	public string[] GetMessages()
	{
		if (this.localizationType == MessageBoxDialogue.LocalizeType.UnlocalizedStings)
		{
			return new List<string>(this.messages).ToArray();
		}
		return new List<string>(this.PlatformAdjustedKeys()).ConvertAll<string>((string t) => LocalizationManager.GetTranslation(t, true, 0, true, false, null, null)).ToArray();
	}

	// Token: 0x04000782 RID: 1922
	[InspectorButton("DisplayAllMessages", "Display Tags and Messages ")]
	[SerializeField]
	private MessageBoxDialogue.LocalizeType localizationType;

	// Token: 0x04000783 RID: 1923
	[Header("UnlocalizedStings")]
	[SerializeField]
	private string[] messages;

	// Token: 0x04000784 RID: 1924
	[Header("KeyArray")]
	[SerializeField]
	private string[] keys;

	// Token: 0x04000785 RID: 1925
	[Header("KeyBaseAndCount")]
	[SerializeField]
	private string keyBaseString;

	// Token: 0x04000786 RID: 1926
	[SerializeField]
	private int keyBaseCount;

	// Token: 0x04000787 RID: 1927
	[Header("LastKeyInSequence")]
	[SerializeField]
	private string lastKey;

	// Token: 0x04000788 RID: 1928
	[Header("Platform variations")]
	public PlatformTag[] platformVariations = new PlatformTag[0];

	// Token: 0x020003D4 RID: 980
	public enum LocalizeType
	{
		// Token: 0x0400141F RID: 5151
		UnlocalizedStings,
		// Token: 0x04001420 RID: 5152
		KeyArray,
		// Token: 0x04001421 RID: 5153
		KeyBaseAndCount,
		// Token: 0x04001422 RID: 5154
		LastKeyInSequence
	}
}
