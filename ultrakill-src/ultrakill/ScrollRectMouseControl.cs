using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020003DA RID: 986
[RequireComponent(typeof(ScrollRect))]
public class ScrollRectMouseControl : MonoBehaviour
{
	// Token: 0x06001651 RID: 5713 RVA: 0x000B3CE4 File Offset: 0x000B1EE4
	private void OnEnable()
	{
		this.m_ScrollRect = base.GetComponent<ScrollRect>();
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x000B3CF2 File Offset: 0x000B1EF2
	private void Update()
	{
		this.m_ScrollRect.verticalNormalizedPosition += Mouse.current.scroll.y.ReadValue() / this.m_ScrollRect.content.sizeDelta.y;
	}

	// Token: 0x04001EBF RID: 7871
	private ScrollRect m_ScrollRect;
}
