using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000137 RID: 311
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class MessageBox : MonoBehaviour
{
	// Token: 0x06000746 RID: 1862 RVA: 0x00025A7E File Offset: 0x00023C7E
	public void InformOfVisibility(bool visible)
	{
		this.visibility = visible;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x00025A88 File Offset: 0x00023C88
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
		this.audioSource = base.GetComponent<AudioSource>();
		foreach (MessageBox.KeyReplaceItem keyReplaceItem in this.replacementItems)
		{
			keyReplaceItem.replace.Init();
		}
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x00025AF8 File Offset: 0x00023CF8
	private string[] DefaultDialogueParser(MessageBoxDialogue dialouge)
	{
		return new List<string>(dialouge.GetMessages()).ConvertAll<string>(delegate(string s)
		{
			if (s == null)
			{
				s = "";
			}
			return s;
		}).ToArray();
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00025B30 File Offset: 0x00023D30
	public void SetMessage(MessageBoxDialogue dialogue)
	{
		this.animator.SetBool("Hidden", false);
		IMessageBoxDialogueParser component = base.GetComponent<IMessageBoxDialogueParser>();
		if (component != null)
		{
			this.messageArray = component.ParseDialogue(dialogue);
		}
		else
		{
			this.messageArray = this.DefaultDialogueParser(dialogue);
		}
		for (int i = 0; i < this.messageArray.Length; i++)
		{
			string text = this.messageArray[i];
			foreach (MessageBox.KeyReplaceItem keyReplaceItem in this.replacementItems)
			{
				text = keyReplaceItem.Replace(text);
			}
			IMessageBoxKeyReplacer[] components = base.GetComponents<IMessageBoxKeyReplacer>();
			for (int j = 0; j < components.Length; j++)
			{
				text = components[j].DoReplaceStep(text);
			}
			text = text.Replace("\\n", "\n");
			if (this.lineBreakBehaviour == MessageBox.LineBreakBehaviour.RemoveAllLineBreaks)
			{
				text = text.Replace("\n", " ");
			}
			this.messageArray[i] = text;
		}
		this.currentMessageIndex = 0;
		this.AdvanceMessage();
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00025C44 File Offset: 0x00023E44
	private void SetVisibility(bool visible)
	{
		this.animator.SetBool("Hidden", !visible);
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00025C5C File Offset: 0x00023E5C
	private void Update()
	{
		if (this.complete && this.manualClose && !this.closed && this.GotContinueInput())
		{
			UnityEvent onCompleteEvent = this.OnCompleteEvent;
			if (onCompleteEvent != null)
			{
				onCompleteEvent.Invoke();
			}
			this.closed = true;
		}
		if (this.closed)
		{
			return;
		}
		if (this.messageArray == null)
		{
			return;
		}
		this.inputCooldownTimer += Time.deltaTime;
		if (this.characterIndex <= this.text.text.Length)
		{
			this.FillMessageBox();
			return;
		}
		if (!this.complete && this.currentMessageIndex >= this.messageArray.Length)
		{
			this.complete = true;
			this.messageArray = null;
			return;
		}
		if (this.GotContinueInput() && this.inputCooldownTimer > this.inputCooldown && this.currentMessageIndex < this.messageArray.Length)
		{
			this.AdvanceMessage();
		}
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00025D37 File Offset: 0x00023F37
	private bool GotContinueInput()
	{
		return Singleton<GameMaster>.Instance.stanleyActions.UseAction.WasPressed || Singleton<GameMaster>.Instance.stanleyActions.MenuConfirm.WasPressed;
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x00025D68 File Offset: 0x00023F68
	private void AdvanceMessage()
	{
		this.text.text = this.messageArray[this.currentMessageIndex];
		this.currentMessageIndex++;
		this.text.maxVisibleCharacters = 0;
		this.characterIndex = 0;
		this.text.maxVisibleCharacters = (this.characterIndex = this.text.text.Length);
		this.characterAdvanceTimer = 0f;
		this.inputCooldownTimer = 0f;
		UnityEvent onAdvanceMessage = this.OnAdvanceMessage;
		if (onAdvanceMessage != null)
		{
			onAdvanceMessage.Invoke();
		}
		UnityAction onAdvanceMessageEvent = this.OnAdvanceMessageEvent;
		if (onAdvanceMessageEvent == null)
		{
			return;
		}
		onAdvanceMessageEvent();
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00025E09 File Offset: 0x00024009
	public void PlayTalkSound()
	{
		if (this.talkCollection.SetVolumeAndPitchAndPlayClip(this.audioSource))
		{
			this.talkTimeStamp = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00025E2C File Offset: 0x0002402C
	private void FillMessageBox()
	{
		if (this.GotContinueInput())
		{
			this.characterIndex = this.text.text.Length - 1;
		}
		this.characterAdvanceTimer += Time.deltaTime;
		if (this.characterAdvanceTimer >= this.characterAdvanceLimit)
		{
			if (Time.realtimeSinceStartup - this.talkTimeStamp >= this.talkSoundDelay && this.playTalkOnFill && this.visibility)
			{
				this.PlayTalkSound();
			}
			this.characterIndex++;
			this.text.maxVisibleCharacters = this.characterIndex;
			this.characterAdvanceTimer = 0f;
			if (this.characterIndex == this.text.text.Length)
			{
				this.endCollection.SetVolumeAndPitchAndPlayClip(this.audioSource);
			}
		}
	}

	// Token: 0x0400076A RID: 1898
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x0400076B RID: 1899
	[SerializeField]
	private Animator animator;

	// Token: 0x0400076C RID: 1900
	[SerializeField]
	private float characterAdvanceLimit = 0.1f;

	// Token: 0x0400076D RID: 1901
	[SerializeField]
	private UnityEvent OnCompleteEvent;

	// Token: 0x0400076E RID: 1902
	[SerializeField]
	private UnityEvent OnAdvanceMessage;

	// Token: 0x0400076F RID: 1903
	public UnityAction OnAdvanceMessageEvent;

	// Token: 0x04000770 RID: 1904
	[SerializeField]
	private float inputCooldown = 1.5f;

	// Token: 0x04000771 RID: 1905
	[Header("Audio")]
	[SerializeField]
	private bool playTalkOnFill = true;

	// Token: 0x04000772 RID: 1906
	[SerializeField]
	private AudioCollection talkCollection;

	// Token: 0x04000773 RID: 1907
	[SerializeField]
	private AudioCollection endCollection;

	// Token: 0x04000774 RID: 1908
	[SerializeField]
	private bool manualClose;

	// Token: 0x04000775 RID: 1909
	[SerializeField]
	private float talkSoundDelay = 0.1f;

	// Token: 0x04000776 RID: 1910
	private MessageBox.LineBreakBehaviour lineBreakBehaviour = MessageBox.LineBreakBehaviour.RemoveAllLineBreaks;

	// Token: 0x04000777 RID: 1911
	[SerializeField]
	private List<MessageBox.KeyReplaceItem> replacementItems;

	// Token: 0x04000778 RID: 1912
	private float talkTimeStamp;

	// Token: 0x04000779 RID: 1913
	private float characterAdvanceTimer;

	// Token: 0x0400077A RID: 1914
	private float inputCooldownTimer;

	// Token: 0x0400077B RID: 1915
	private int characterIndex;

	// Token: 0x0400077C RID: 1916
	private int currentMessageIndex;

	// Token: 0x0400077D RID: 1917
	private string[] messageArray;

	// Token: 0x0400077E RID: 1918
	private AudioSource audioSource;

	// Token: 0x0400077F RID: 1919
	private bool complete;

	// Token: 0x04000780 RID: 1920
	private bool closed;

	// Token: 0x04000781 RID: 1921
	private bool visibility = true;

	// Token: 0x020003D1 RID: 977
	public enum LineBreakBehaviour
	{
		// Token: 0x04001418 RID: 5144
		UseCharacterLineBreaks,
		// Token: 0x04001419 RID: 5145
		RemoveAllLineBreaks
	}

	// Token: 0x020003D2 RID: 978
	[Serializable]
	private class KeyReplaceItem
	{
		// Token: 0x06001752 RID: 5970 RVA: 0x00079071 File Offset: 0x00077271
		public string Replace(string orig)
		{
			return orig.Replace(this.key, this.replace.GetStringValue());
		}

		// Token: 0x0400141A RID: 5146
		public string key;

		// Token: 0x0400141B RID: 5147
		public StringConfigurable replace;
	}
}
