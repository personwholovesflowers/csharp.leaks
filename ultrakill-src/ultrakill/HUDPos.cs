using System;
using UnityEngine;

// Token: 0x02000256 RID: 598
public class HUDPos : MonoBehaviour
{
	// Token: 0x06000D34 RID: 3380 RVA: 0x00064807 File Offset: 0x00062A07
	private void Start()
	{
		this.CheckPos();
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0006480F File Offset: 0x00062A0F
	private void OnEnable()
	{
		this.CheckPos();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x00064837 File Offset: 0x00062A37
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x00064859 File Offset: 0x00062A59
	private void OnPrefChanged(string key, object value)
	{
		if (key == "weaponHoldPosition")
		{
			this.CheckPos();
		}
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x00064870 File Offset: 0x00062A70
	public void CheckPos()
	{
		if (this.active)
		{
			if (!this.ready)
			{
				this.ready = true;
				if (this.rectTransform)
				{
					this.rect = base.GetComponent<RectTransform>();
					this.anchoredPositionDefault = this.rect.anchoredPosition;
					this.anchorsMaxDefault = this.rect.anchorMax;
					this.anchorsMinDefault = this.rect.anchorMin;
					this.pivotDefault = this.rect.pivot;
				}
				else
				{
					this.defaultPos = base.transform.localPosition;
					this.defaultRot = base.transform.localRotation.eulerAngles;
				}
			}
			if (MonoSingleton<PrefsManager>.Instance.GetInt("weaponHoldPosition", 0) == 2)
			{
				if (this.rectTransform)
				{
					this.rect.anchorMax = this.anchorsMax;
					this.rect.anchorMin = this.anchorsMin;
					this.rect.pivot = this.pivot;
					this.rect.anchoredPosition = this.anchoredPosition;
					return;
				}
				base.transform.localPosition = this.reversePos;
				base.transform.localRotation = Quaternion.Euler(this.reverseRot);
				return;
			}
			else
			{
				if (this.rectTransform)
				{
					this.rect.anchorMax = this.anchorsMaxDefault;
					this.rect.anchorMin = this.anchorsMinDefault;
					this.rect.pivot = this.pivotDefault;
					this.rect.anchoredPosition = this.anchoredPositionDefault;
					return;
				}
				base.transform.localPosition = this.defaultPos;
				base.transform.localRotation = Quaternion.Euler(this.defaultRot);
			}
		}
	}

	// Token: 0x040011BD RID: 4541
	private bool ready;

	// Token: 0x040011BE RID: 4542
	public bool active;

	// Token: 0x040011BF RID: 4543
	private Vector3 defaultPos;

	// Token: 0x040011C0 RID: 4544
	private Vector3 defaultRot;

	// Token: 0x040011C1 RID: 4545
	public Vector3 reversePos;

	// Token: 0x040011C2 RID: 4546
	public Vector3 reverseRot;

	// Token: 0x040011C3 RID: 4547
	[Header("Rect Transform")]
	public bool rectTransform;

	// Token: 0x040011C4 RID: 4548
	private RectTransform rect;

	// Token: 0x040011C5 RID: 4549
	private Vector2 anchorsMaxDefault;

	// Token: 0x040011C6 RID: 4550
	public Vector2 anchorsMax;

	// Token: 0x040011C7 RID: 4551
	private Vector2 anchorsMinDefault;

	// Token: 0x040011C8 RID: 4552
	public Vector2 anchorsMin;

	// Token: 0x040011C9 RID: 4553
	private Vector2 pivotDefault;

	// Token: 0x040011CA RID: 4554
	public Vector2 pivot;

	// Token: 0x040011CB RID: 4555
	private Vector2 anchoredPositionDefault;

	// Token: 0x040011CC RID: 4556
	public Vector2 anchoredPosition;
}
