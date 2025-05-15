using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class TooltipManager : MonoBehaviour
{
	// Token: 0x06001083 RID: 4227 RVA: 0x0007EB48 File Offset: 0x0007CD48
	private void Awake()
	{
		Canvas componentInParent = base.GetComponentInParent<Canvas>();
		if (componentInParent != null)
		{
			this.canvasRect = componentInParent.GetComponent<RectTransform>();
		}
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x0007EB74 File Offset: 0x0007CD74
	public Guid ShowTooltip(Vector2 position, string text = "")
	{
		Guid guid = Guid.NewGuid();
		GameObject gameObject = Object.Instantiate<GameObject>(this.tooltipTemplate);
		gameObject.transform.SetParent(base.transform, false);
		gameObject.SetActive(true);
		TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
		componentInChildren.text = text;
		componentInChildren.ForceMeshUpdate(false, false);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.position = position;
		Vector2 preferredValues = componentInChildren.GetPreferredValues();
		component.sizeDelta = preferredValues;
		this.EnsureWithinBounds(component);
		this.dict.Add(guid, gameObject);
		return guid;
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x0007EBF8 File Offset: 0x0007CDF8
	public void HideTooltip(Guid id)
	{
		GameObject gameObject;
		if (this.dict.TryGetValue(id, out gameObject))
		{
			Object.Destroy(gameObject);
			this.dict.Remove(id);
		}
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x0007EC28 File Offset: 0x0007CE28
	private void EnsureWithinBounds(RectTransform rect)
	{
		if (this.canvasRect == null || rect == null)
		{
			return;
		}
		Vector2 sizeDelta = this.canvasRect.sizeDelta;
		Vector2 sizeDelta2 = rect.sizeDelta;
		Vector2 anchoredPosition = rect.anchoredPosition;
		if (anchoredPosition.x + sizeDelta2.x > sizeDelta.x)
		{
			anchoredPosition.x = sizeDelta.x - sizeDelta2.x;
		}
		if (anchoredPosition.y - sizeDelta2.y < -sizeDelta.y)
		{
			anchoredPosition.y = -sizeDelta.y + sizeDelta2.y;
		}
		rect.anchoredPosition = anchoredPosition;
	}

	// Token: 0x04001672 RID: 5746
	public GameObject tooltipTemplate;

	// Token: 0x04001673 RID: 5747
	private Dictionary<Guid, GameObject> dict = new Dictionary<Guid, GameObject>();

	// Token: 0x04001674 RID: 5748
	private RectTransform canvasRect;
}
