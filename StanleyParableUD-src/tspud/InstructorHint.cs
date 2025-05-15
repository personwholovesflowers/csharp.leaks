using System;
using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class InstructorHint : HammerEntity
{
	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060005CB RID: 1483 RVA: 0x00020069 File Offset: 0x0001E269
	// (set) Token: 0x060005CC RID: 1484 RVA: 0x00020071 File Offset: 0x0001E271
	public bool waiting { get; private set; }

	// Token: 0x060005CD RID: 1485 RVA: 0x0002007C File Offset: 0x0001E27C
	private void Start()
	{
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.UpdateHintVisualsCallback;
		IntConfigurable languageProfile = Singleton<GameMaster>.Instance.languageProfile;
		languageProfile.OnValueChanged = (Action<LiveData>)Delegate.Combine(languageProfile.OnValueChanged, new Action<LiveData>(this.UpdateHintVisualsCallback));
		BooleanConfigurable simplifiedControlsConfigurable = Singleton<GameMaster>.Instance.simplifiedControlsConfigurable;
		simplifiedControlsConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(simplifiedControlsConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateHintVisualsCallback));
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsLoaded += this.UpdateHintVisualsCallback;
		Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsSaved += this.UpdateHintVisualsCallback;
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0002012C File Offset: 0x0001E32C
	private void OnDestroy()
	{
		if (Singleton<GameMaster>.Instance != null)
		{
			Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged -= this.UpdateHintVisualsCallback;
			IntConfigurable languageProfile = Singleton<GameMaster>.Instance.languageProfile;
			languageProfile.OnValueChanged = (Action<LiveData>)Delegate.Remove(languageProfile.OnValueChanged, new Action<LiveData>(this.UpdateHintVisualsCallback));
			BooleanConfigurable simplifiedControlsConfigurable = Singleton<GameMaster>.Instance.simplifiedControlsConfigurable;
			simplifiedControlsConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(simplifiedControlsConfigurable.OnValueChanged, new Action<LiveData>(this.UpdateHintVisualsCallback));
			Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsLoaded -= this.UpdateHintVisualsCallback;
			Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsSaved -= this.UpdateHintVisualsCallback;
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x000201EB File Offset: 0x0001E3EB
	private void UpdateHintVisualsCallback(GameMaster.InputDevice inptDevice)
	{
		this.UpdateHintVisualsCallback();
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x000201EB File Offset: 0x0001E3EB
	private void UpdateHintVisualsCallback(LiveData liveData)
	{
		this.UpdateHintVisualsCallback();
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x000201F3 File Offset: 0x0001E3F3
	private void UpdateHintVisualsCallback()
	{
		if (this.currentMessageKeyIndex >= 0)
		{
			this.UpdateHintVisuals();
		}
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00020204 File Offset: 0x0001E404
	private void UpdateHintVisuals()
	{
		string text;
		if (Singleton<GameMaster>.Instance.InputDeviceType == GameMaster.InputDevice.KeyboardAndMouse)
		{
			text = LocalizationManager.GetTranslation("TSP_Apartment_Device_Keyboard", true, 0, true, false, null, null);
		}
		else
		{
			text = LocalizationManager.GetTranslation("TSP_Apartment_Device_Gamepad", true, 0, true, false, null, null);
		}
		string text2 = this.messageKeys[this.currentMessageKeyIndex];
		if (this.autoSuffixMessageKeysOnBucketHeld && BucketController.HASBUCKET)
		{
			text2 += "_BUCKET";
		}
		string text3 = UITextSwitcher.GetLocalizedSpritedText(text2, this.currentRandomInputIndex);
		text3 = text3.Replace("%!D!%", text);
		text3 = text3.Replace("%! D!%", text);
		this.instructorHintText.text = text3;
		this.canvas.enabled = true;
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x000202A9 File Offset: 0x0001E4A9
	public void ShowHintNoInput(int index)
	{
		this.ShowHint(index, false);
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x000202B4 File Offset: 0x0001E4B4
	public void ShowHint(int index, bool takeInput = true)
	{
		if (index < 0)
		{
			return;
		}
		if (index > this.messageKeys.Length - 1)
		{
			return;
		}
		StanleyActions stanleyActions = Singleton<GameMaster>.Instance.stanleyActions;
		this.currentMessageKeyIndex = index;
		this.currentRandomInputIndex = Random.Range(0, Singleton<GameMaster>.Instance.stanleyActions.GetExtraActionInputsLength());
		this.UpdateHintVisuals();
		if (takeInput)
		{
			this.waiting = true;
			base.StartCoroutine(this.WaitForKey());
		}
		base.StartCoroutine(this.HintInOut(this.startYPos, this.holdYPos, 0f, 1f, this.inDuration));
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00020347 File Offset: 0x0001E547
	public void HideHint()
	{
		base.StartCoroutine(this.HintInOut(this.holdYPos, this.outYPos, 1f, 0f, this.outDuration));
		this.canvas.enabled = false;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002037E File Offset: 0x0001E57E
	private IEnumerator WaitForKey()
	{
		yield return new WaitForGameSeconds(this.inDuration);
		while (GameMaster.PAUSEMENUACTIVE || !Singleton<GameMaster>.Instance.stanleyActions.ExtraAction(this.currentRandomInputIndex, false).WasPressed)
		{
			yield return null;
		}
		this.waiting = false;
		this.HideHint();
		yield break;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0002038D File Offset: 0x0001E58D
	private IEnumerator HintInOut(float pos1, float pos2, float a1, float a2, float duration)
	{
		TimePair timer = new TimePair(duration);
		this.color.a = a1;
		this.instructorHintText.color = this.color;
		this.instructorHintText.rectTransform.anchoredPosition3D = new Vector3(0f, pos1, 0f);
		while (!timer.IsFinished())
		{
			float num = timer.InverseLerp();
			float num2 = num;
			this.instructorHintText.rectTransform.anchoredPosition3D = new Vector3(0f, Mathf.Lerp(pos1, pos2, num), 0f);
			this.color.a = num2;
			this.instructorHintText.color = this.color;
			yield return new WaitForEndOfFrame();
		}
		this.instructorHintText.rectTransform.anchoredPosition3D = new Vector3(0f, pos2, 0f);
		this.color.a = a2;
		this.instructorHintText.color = this.color;
		yield break;
	}

	// Token: 0x04000608 RID: 1544
	public Color color = Color.white;

	// Token: 0x04000609 RID: 1545
	public float inDuration = 0.5f;

	// Token: 0x0400060A RID: 1546
	public float outDuration = 0.1f;

	// Token: 0x0400060B RID: 1547
	public float startYPos = 60f;

	// Token: 0x0400060C RID: 1548
	public float holdYPos = -60f;

	// Token: 0x0400060D RID: 1549
	public float outYPos = -75f;

	// Token: 0x0400060E RID: 1550
	public string[] messageKeys;

	// Token: 0x0400060F RID: 1551
	public bool autoSuffixMessageKeysOnBucketHeld = true;

	// Token: 0x04000610 RID: 1552
	public LogicRelay[] apartmentEndingRelays;

	// Token: 0x04000611 RID: 1553
	public bool noInputOnLastRelay = true;

	// Token: 0x04000613 RID: 1555
	private int currentMessageKeyIndex = -1;

	// Token: 0x04000614 RID: 1556
	private int currentRandomInputIndex = -1;

	// Token: 0x04000615 RID: 1557
	public Canvas canvas;

	// Token: 0x04000616 RID: 1558
	public TextMeshProUGUI instructorHintText;
}
