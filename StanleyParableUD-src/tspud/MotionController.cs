using System;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class MotionController : MonoBehaviour
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000762 RID: 1890 RVA: 0x00026252 File Offset: 0x00024452
	public float ScreenShakeAmplitudeMultiplier
	{
		get
		{
			return (float)(this.InMenu ? 0 : (this.ReducedMotionMode ? 0 : 1));
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x0002626C File Offset: 0x0002446C
	public bool ReducedMotionMode
	{
		get
		{
			return this.reducedMotionConfigurable.GetBooleanValue();
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000764 RID: 1892 RVA: 0x00026279 File Offset: 0x00024479
	private bool InMenu
	{
		get
		{
			return GameMaster.ONMAINMENUORSETTINGS || GameMaster.PAUSEMENUACTIVE;
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000765 RID: 1893 RVA: 0x0002628C File Offset: 0x0002448C
	// (remove) Token: 0x06000766 RID: 1894 RVA: 0x000262C0 File Offset: 0x000244C0
	public static event MotionController.ReducedMotionValueChangedEvent OnReducedMotionValueChanged;

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000767 RID: 1895 RVA: 0x000262F3 File Offset: 0x000244F3
	// (set) Token: 0x06000768 RID: 1896 RVA: 0x000262FA File Offset: 0x000244FA
	public static MotionController Instance { get; private set; }

	// Token: 0x06000769 RID: 1897 RVA: 0x00026302 File Offset: 0x00024502
	private void Awake()
	{
		MotionController.Instance = this;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0002630A File Offset: 0x0002450A
	private void Start()
	{
		BooleanConfigurable booleanConfigurable = this.reducedMotionConfigurable;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.ReduceMotionValueChanged));
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x00026333 File Offset: 0x00024533
	private void ReduceMotionValueChanged(LiveData ld)
	{
		MotionController.ReducedMotionValueChangedEvent onReducedMotionValueChanged = MotionController.OnReducedMotionValueChanged;
		if (onReducedMotionValueChanged == null)
		{
			return;
		}
		onReducedMotionValueChanged();
	}

	// Token: 0x0400078E RID: 1934
	public BooleanConfigurable reducedMotionConfigurable;

	// Token: 0x020003DA RID: 986
	// (Invoke) Token: 0x06001776 RID: 6006
	public delegate void ReducedMotionValueChangedEvent();
}
