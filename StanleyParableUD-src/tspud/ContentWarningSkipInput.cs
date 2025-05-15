using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A2 RID: 162
public class ContentWarningSkipInput : MonoBehaviour
{
	// Token: 0x060003E5 RID: 997 RVA: 0x00018A8C File Offset: 0x00016C8C
	private void Start()
	{
		BooleanConfigurable booleanConfigurable = this.contentWarningsToggle;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConfigurables));
		StringConfigurable stringConfigurable = this.contentWarningsShown;
		stringConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(stringConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConfigurables));
		this.CheckConfigurables(null);
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00018AF0 File Offset: 0x00016CF0
	private void OnDestroy()
	{
		BooleanConfigurable booleanConfigurable = this.contentWarningsToggle;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConfigurables));
		StringConfigurable stringConfigurable = this.contentWarningsShown;
		stringConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(stringConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConfigurables));
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x00018B4B File Offset: 0x00016D4B
	private void CheckConfigurables(LiveData ld)
	{
		this.contentWarningEnabled = this.contentWarningsToggle.GetBooleanValue() && this.contentWarningsShown.GetStringValue() != "";
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00018B78 File Offset: 0x00016D78
	private void Update()
	{
		if (!GameMaster.PAUSEMENUACTIVE && Singleton<GameMaster>.Instance.stanleyActions.ExtraAction(0, false).IsPressed)
		{
			this.timeRemaining -= Time.smoothDeltaTime;
		}
		else
		{
			this.timeRemaining = this.timeToWait;
		}
		if (!this.contentWarningEnabled)
		{
			return;
		}
		if (this.timeRemaining < 0f)
		{
			UnityEvent unityEvent = this.onSkip;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x040003E0 RID: 992
	public float timeToWait = 3f;

	// Token: 0x040003E1 RID: 993
	public UnityEvent onSkip;

	// Token: 0x040003E2 RID: 994
	private float timeRemaining = -1f;

	// Token: 0x040003E3 RID: 995
	public BooleanConfigurable contentWarningsToggle;

	// Token: 0x040003E4 RID: 996
	public StringConfigurable contentWarningsShown;

	// Token: 0x040003E5 RID: 997
	[Header("DEBUG")]
	public bool contentWarningEnabled;
}
