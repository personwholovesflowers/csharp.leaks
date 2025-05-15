using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E4 RID: 484
public class UIUtility : MonoBehaviour
{
	// Token: 0x06000B21 RID: 2849 RVA: 0x00033CC4 File Offset: 0x00031EC4
	public void SelectSelectable(Selectable selectable)
	{
		base.StartCoroutine(UIUtility.Select(selectable));
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x00033CD3 File Offset: 0x00031ED3
	public void Deselect()
	{
		EventSystem.current.SetSelectedGameObject(null);
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x00033CE0 File Offset: 0x00031EE0
	public static IEnumerator Select(Selectable selectable)
	{
		yield return null;
		EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();
		new PointerEventData(eventSystem);
		if (eventSystem == null)
		{
			yield break;
		}
		eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(selectable.gameObject);
		yield return null;
		if (selectable is Toggle)
		{
			(selectable as Toggle).isOn = true;
		}
		selectable.OnSelect(null);
		yield break;
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x00005444 File Offset: 0x00003644
	public void OnMoveTest(AxisEventData baseData)
	{
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x00033CF0 File Offset: 0x00031EF0
	public void OnMove(BaseEventData baseData)
	{
		Configurator component = base.GetComponent<Configurator>();
		if (component != null)
		{
			AxisEventData axisEventData = baseData as AxisEventData;
			if (axisEventData != null)
			{
				MoveDirection moveDir = axisEventData.moveDir;
				if (moveDir == MoveDirection.Left)
				{
					component.DecreaseValue();
					return;
				}
				if (moveDir != MoveDirection.Right)
				{
					return;
				}
				component.IncreaseValue();
				return;
			}
			else
			{
				Debug.Log("Not a move input!");
			}
		}
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00033D40 File Offset: 0x00031F40
	public void OnSubmit(BaseEventData baseData)
	{
		Configurator component = base.GetComponent<Configurator>();
		if (component != null)
		{
			component.IncreaseValue();
		}
	}
}
