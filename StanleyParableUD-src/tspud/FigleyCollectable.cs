using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000C0 RID: 192
public class FigleyCollectable : MonoBehaviour
{
	// Token: 0x0600046A RID: 1130 RVA: 0x0001A1A8 File Offset: 0x000183A8
	private void Awake()
	{
		this.figleyOmegaFoundConfigurable.Init();
		this.figleyOmegaFoundConfigurable.SaveToDiskAll();
		BooleanConfigurable booleanConfigurable = this.figleyOmegaFoundConfigurable;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConditions));
		if (this.figleyCollectionOrderConfigurable != null)
		{
			this.figleyCollectionOrderConfigurable.Init();
			this.figleyCollectionOrderConfigurable.SaveToDiskAll();
			IntConfigurable intConfigurable = this.figleyCollectionOrderConfigurable;
			intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConditions));
		}
		this.figleyPostCompletionCharArrayConfigurable.Init();
		this.figleyPostCompletionCharArrayConfigurable.SaveToDiskAll();
		if (FigleyCollectable.FigleysInCrossHairRange_STATIC == null)
		{
			FigleyCollectable.FigleysInCrossHairRange_STATIC = new HashSet<FigleyCollectable>();
		}
		FigleyCollectable.FigleysInCrossHairRange_STATIC.Clear();
		this.CheckCrosshairStatus();
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0001A274 File Offset: 0x00018474
	private void Start()
	{
		this.CheckConditions(null);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001A27D File Offset: 0x0001847D
	private void OnDisable()
	{
		this.OnCrosshairTriggerExit();
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001A288 File Offset: 0x00018488
	private void OnDestroy()
	{
		this.OnCrosshairTriggerExit();
		if (this.figleyOmegaFoundConfigurable != null)
		{
			BooleanConfigurable booleanConfigurable = this.figleyOmegaFoundConfigurable;
			booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConditions));
		}
		if (this.figleyCollectionOrderConfigurable != null)
		{
			IntConfigurable intConfigurable = this.figleyCollectionOrderConfigurable;
			intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.CheckConditions));
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001A305 File Offset: 0x00018505
	public void OnCrosshairTriggerEnter()
	{
		FigleyCollectable.FigleysInCrossHairRange_STATIC.Add(this);
		this.CheckCrosshairStatus();
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001A319 File Offset: 0x00018519
	public void OnCrosshairTriggerExit()
	{
		FigleyCollectable.FigleysInCrossHairRange_STATIC.Remove(this);
		this.CheckCrosshairStatus();
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001A32D File Offset: 0x0001852D
	public void ClearAllCrosshairs()
	{
		FigleyCollectable.FigleysInCrossHairRange_STATIC.Clear();
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001A339 File Offset: 0x00018539
	private void CheckCrosshairStatus()
	{
		Singleton<GameMaster>.Instance.Crosshair(FigleyCollectable.FigleysInCrossHairRange_STATIC != null && FigleyCollectable.FigleysInCrossHairRange_STATIC.Count != 0);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001A35C File Offset: 0x0001855C
	private void CheckConditions(LiveData __ignored__ = null)
	{
		if (this.IsAPostColletable)
		{
			if (FigleyCollectable.IsPostCollectableCollected(this.figleyPostCompletionCharArrayConfigurable, this.postCollectableIndex))
			{
				this.OnHidden.Invoke();
				return;
			}
			this.OnVisible.Invoke();
			return;
		}
		else if (this.figleyCollectionOrderConfigurable == null)
		{
			if (!this.figleyOmegaFoundConfigurable.GetBooleanValue())
			{
				this.OnVisible.Invoke();
				return;
			}
			this.OnHidden.Invoke();
			return;
		}
		else
		{
			if (this.figleyOmegaFoundConfigurable.GetBooleanValue() && this.figleyCollectionOrderConfigurable.GetIntValue() == -1)
			{
				this.OnVisible.Invoke();
				return;
			}
			this.OnHidden.Invoke();
			return;
		}
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0001A404 File Offset: 0x00018604
	public void StartCollectableRoutine()
	{
		if (this.IsAPostColletable)
		{
			FigleyCollectable.MarkPostCollectableAsCollected(this.figleyPostCompletionCharArrayConfigurable, this.postCollectableIndex);
			FigleyOverlayController.Instance.StartFigleyPostCollection();
			this.OnHidden.Invoke();
			return;
		}
		if (this.figleyCollectionOrderConfigurable == null)
		{
			this.figleyOmegaFoundConfigurable.SetValue(true);
			this.figleyOmegaFoundConfigurable.SaveToDiskAll();
		}
		else
		{
			this.figleyCollectionOrderConfigurable.SetValue(FigleyOverlayController.Instance.FiglysFound);
			this.figleyCollectionOrderConfigurable.SaveToDiskAll();
		}
		FigleyOverlayController.Instance.StartFigleyCollectionRoutine();
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001A494 File Offset: 0x00018694
	public static char[] GetPostCollectableCharArray(StringConfigurable countArray, int referenceIndex)
	{
		List<char> list = new List<char>(countArray.GetStringValue().ToCharArray());
		while (list.Count <= referenceIndex)
		{
			list.Add(FigleyCollectable.NotCollectedChar);
		}
		return list.ToArray();
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000475 RID: 1141 RVA: 0x0001A4CE File Offset: 0x000186CE
	private bool IsAPostColletable
	{
		get
		{
			return this.postCollectableIndex != -1;
		}
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001A4DC File Offset: 0x000186DC
	public static bool IsPostCollectableCollected(StringConfigurable countArray, int referenceIndex)
	{
		return FigleyCollectable.GetPostCollectableCharArray(countArray, referenceIndex)[referenceIndex] == FigleyCollectable.CollectedChar;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0001A4F0 File Offset: 0x000186F0
	public static void MarkPostCollectableAsCollected(StringConfigurable countArray, int referenceIndex)
	{
		char[] postCollectableCharArray = FigleyCollectable.GetPostCollectableCharArray(countArray, referenceIndex);
		if (referenceIndex >= 0)
		{
			postCollectableCharArray[referenceIndex] = FigleyCollectable.CollectedChar;
		}
		countArray.SetValue(new string(postCollectableCharArray));
		countArray.SaveToDiskAll();
	}

	// Token: 0x04000443 RID: 1091
	[SerializeField]
	private BooleanConfigurable figleyOmegaFoundConfigurable;

	// Token: 0x04000444 RID: 1092
	[Header("Keep null if this is OMEGA Figley")]
	[SerializeField]
	private IntConfigurable figleyCollectionOrderConfigurable;

	// Token: 0x04000445 RID: 1093
	[Header("-1 menas not a post collectalbe")]
	[SerializeField]
	[Range(0f, 64f)]
	private int postCollectableIndex = -1;

	// Token: 0x04000446 RID: 1094
	[SerializeField]
	private StringConfigurable figleyPostCompletionCharArrayConfigurable;

	// Token: 0x04000447 RID: 1095
	[SerializeField]
	private UnityEvent OnVisible;

	// Token: 0x04000448 RID: 1096
	[SerializeField]
	private UnityEvent OnHidden;

	// Token: 0x04000449 RID: 1097
	private static HashSet<FigleyCollectable> FigleysInCrossHairRange_STATIC = null;

	// Token: 0x0400044A RID: 1098
	public static char NotCollectedChar = '_';

	// Token: 0x0400044B RID: 1099
	public static char CollectedChar = 'X';
}
