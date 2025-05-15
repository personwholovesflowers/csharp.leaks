using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200002E RID: 46
[RequireComponent(typeof(Toggle))]
public class CategoryButton : MonoBehaviour
{
	// Token: 0x060000F7 RID: 247 RVA: 0x00008910 File Offset: 0x00006B10
	private void Awake()
	{
		this.toggle = base.GetComponent<Toggle>();
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetCategory));
		this.toggle.onValueChanged.Invoke(this.toggle.isOn);
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00008960 File Offset: 0x00006B60
	private void OnDestroy()
	{
		this.toggle.onValueChanged.RemoveAllListeners();
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00008972 File Offset: 0x00006B72
	private void SetCategory(bool value)
	{
		if (this.categoryRoot != null)
		{
			this.categoryRoot.SetActive(value);
		}
	}

	// Token: 0x04000167 RID: 359
	[SerializeField]
	private GameObject categoryRoot;

	// Token: 0x04000168 RID: 360
	private Toggle toggle;
}
