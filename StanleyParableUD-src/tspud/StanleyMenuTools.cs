﻿using System;
using StanleyUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200012F RID: 303
public static class StanleyMenuTools
{
	// Token: 0x06000723 RID: 1827 RVA: 0x00025497 File Offset: 0x00023697
	public static void StanleyMenuOnPointerEnter(Action<PointerEventData> baseFunction, PointerEventData eventData)
	{
		if (Singleton<GameMaster>.Instance.MouseMoved && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
		{
			baseFunction(eventData);
		}
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x000254BC File Offset: 0x000236BC
	public static void StanleyMenuSelectableOnSelect(Selectable selectable, BaseEventData eventData)
	{
		StanleyInputModuleAssistant.RegisterUIElementSelection(selectable);
		if (!Singleton<GameMaster>.Instance.MouseMoved && !GameMaster.CursorVisible && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
		{
			StanleyMenuTools.SnapToInScrollRect(selectable.GetComponent<RectTransform>());
		}
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x000254F4 File Offset: 0x000236F4
	public static void SnapToInScrollRect(RectTransform target)
	{
		ScrollRect componentInParent = target.GetComponentInParent<ScrollRect>();
		if (componentInParent == null)
		{
			return;
		}
		RectTransform content = componentInParent.content;
		if (content == null)
		{
			return;
		}
		Canvas.ForceUpdateCanvases();
		Vector2 vector = componentInParent.transform.InverseTransformPoint(content.position);
		ref Vector2 ptr = componentInParent.transform.InverseTransformVector(content.transform.TransformVector(content.sizeDelta));
		Vector2 vector2 = componentInParent.transform.InverseTransformPoint(target.position);
		Vector2 sizeDelta = componentInParent.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = 0f;
		float num = (vector - vector2 - sizeDelta / 2f).y;
		float num2 = ptr.y - sizeDelta.y;
		num = Mathf.Clamp(num, 0f, num2);
		content.anchoredPosition = new Vector2(0f, num);
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x000255E8 File Offset: 0x000237E8
	public static Selectable GetPrevActiveSiblingSelectable(Transform trans, params Type[] ignoreList)
	{
		Transform transform = StanleyMenuTools.GetPrevSibling(trans);
		while (!StanleyMenuTools.IsValidSelectable(transform, ignoreList))
		{
			transform = StanleyMenuTools.GetPrevSibling(transform);
		}
		if (!(transform == null))
		{
			return transform.GetComponent<Selectable>();
		}
		return null;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00025620 File Offset: 0x00023820
	public static Selectable GetNextActiveSiblingSelectable(Transform trans, params Type[] ignoreList)
	{
		Transform transform = StanleyMenuTools.GetNextSibling(trans);
		while (!StanleyMenuTools.IsValidSelectable(transform, ignoreList))
		{
			transform = StanleyMenuTools.GetNextSibling(transform);
		}
		if (!(transform == null))
		{
			return transform.GetComponent<Selectable>();
		}
		return null;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00025658 File Offset: 0x00023858
	public static bool IsValidSelectable(Transform t, params Type[] ignoreList)
	{
		if (!(t != null))
		{
			return true;
		}
		Selectable component = t.GetComponent<Selectable>();
		bool flag = component != null;
		bool flag2 = false;
		if (component != null)
		{
			Type type = component.GetType();
			flag2 = Array.FindIndex<Type>(ignoreList, (Type x) => type == x) == -1;
		}
		return flag2 && t.gameObject.activeSelf && flag;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x000256C8 File Offset: 0x000238C8
	public static UIBehaviour GetSiblingThatIsActive(this UIBehaviour ui, int direction)
	{
		UIBehaviour uibehaviour = ui;
		do
		{
			uibehaviour = uibehaviour.GetSibling(direction);
		}
		while (!(uibehaviour == null) && !uibehaviour.gameObject.activeSelf);
		return uibehaviour;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x000256F6 File Offset: 0x000238F6
	public static UIBehaviour GetSibling(this UIBehaviour ui, int direction)
	{
		if (direction == 0)
		{
			return ui;
		}
		if (direction > 0)
		{
			Transform nextSibling = StanleyMenuTools.GetNextSibling(ui.transform);
			if (nextSibling == null)
			{
				return null;
			}
			return nextSibling.GetComponent<UIBehaviour>();
		}
		else
		{
			Transform prevSibling = StanleyMenuTools.GetPrevSibling(ui.transform);
			if (prevSibling == null)
			{
				return null;
			}
			return prevSibling.GetComponent<UIBehaviour>();
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002572E File Offset: 0x0002392E
	public static UIBehaviour GetPrevSibling(this UIBehaviour ui)
	{
		Transform prevSibling = StanleyMenuTools.GetPrevSibling(ui.transform);
		if (prevSibling == null)
		{
			return null;
		}
		return prevSibling.GetComponent<UIBehaviour>();
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00025746 File Offset: 0x00023946
	public static UIBehaviour GetNextSibling(this UIBehaviour ui)
	{
		Transform nextSibling = StanleyMenuTools.GetNextSibling(ui.transform);
		if (nextSibling == null)
		{
			return null;
		}
		return nextSibling.GetComponent<UIBehaviour>();
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00025760 File Offset: 0x00023960
	public static Transform GetPrevSibling(Transform trans)
	{
		int siblingIndex = trans.GetSiblingIndex();
		if (siblingIndex <= 0)
		{
			return null;
		}
		return trans.parent.GetChild(siblingIndex - 1);
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00025788 File Offset: 0x00023988
	public static Transform GetNextSibling(Transform trans)
	{
		int siblingIndex = trans.GetSiblingIndex();
		if (siblingIndex >= trans.parent.childCount - 1)
		{
			return null;
		}
		return trans.parent.GetChild(siblingIndex + 1);
	}
}
