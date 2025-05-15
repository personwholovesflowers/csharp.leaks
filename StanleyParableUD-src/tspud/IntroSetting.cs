using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F4 RID: 244
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class IntroSetting : MonoBehaviour
{
	// Token: 0x060005E0 RID: 1504 RVA: 0x00020537 File Offset: 0x0001E737
	public void SetFadeInOnMessageBoxAdvance(bool b)
	{
		this.fadeInOnMessageBoxAdvance = b;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x00020540 File Offset: 0x0001E740
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
		this.cGroup = base.GetComponent<CanvasGroup>();
		MessageBox messageBox = this.messageBox;
		messageBox.OnAdvanceMessageEvent = (UnityAction)Delegate.Combine(messageBox.OnAdvanceMessageEvent, new UnityAction(this.OnMessageBoxAdvance));
		this.cGroup.interactable = false;
		this.cGroup.blocksRaycasts = false;
		this.ResetToHidden();
		if (this.startsVisible)
		{
			this.FadeIn();
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x000205B8 File Offset: 0x0001E7B8
	public void SetMessageBox()
	{
		this.messageBox.SetMessage(this.messageBoxDialog);
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x000205CB File Offset: 0x0001E7CB
	public void SetMessageBoxDialog(MessageBoxDialogue newMessageBoxDialog)
	{
		this.messageBoxDialog = newMessageBoxDialog;
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x000205D4 File Offset: 0x0001E7D4
	public void BeginSetting()
	{
		this.BeginSetting(null);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x000205E0 File Offset: 0x0001E7E0
	public void BeginSetting(SimpleEvent completeEvent)
	{
		if (this.delayCoroutine != null)
		{
			return;
		}
		if (this.completeEvent == null && completeEvent != null)
		{
			this.completeEvent = completeEvent;
		}
		EventSystem.current.SetSelectedGameObject(null);
		if (this.defaultSelectable != null)
		{
			EventSystem.current.firstSelectedGameObject = this.defaultSelectable.gameObject;
		}
		UnityEvent unityEvent = this.onBeginSetting;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		this.delayCoroutine = base.StartCoroutine(this.DelayedStart());
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x00020665 File Offset: 0x0001E865
	public void EndSetting()
	{
		if (this.delayCoroutine != null)
		{
			return;
		}
		this.delayCoroutine = base.StartCoroutine(this.DelayedEnd());
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x00020682 File Offset: 0x0001E882
	private void MessageBoxBecomesVisible()
	{
		this.messageBox.InformOfVisibility(true);
		this.messageBox.PlayTalkSound();
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0002069B File Offset: 0x0001E89B
	private void ContentBecomesVisible()
	{
		this.cGroup.interactable = true;
		this.cGroup.blocksRaycasts = true;
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x000206B5 File Offset: 0x0001E8B5
	private IEnumerator DelayedStart()
	{
		yield return new WaitForSeconds(this.startEndDelay);
		this.FadeIn();
		this.SetMessageBox();
		this.messageBox.InformOfVisibility(false);
		this.delayCoroutine = null;
		yield break;
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x000206C4 File Offset: 0x0001E8C4
	private IEnumerator DelayedEnd()
	{
		yield return new WaitForSeconds(this.startEndDelay);
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
		}
		this.cGroup.interactable = false;
		this.cGroup.blocksRaycasts = false;
		this.ResetToHidden();
		yield return new WaitForSeconds(this.endEndDelay);
		if (this.completeEvent != null)
		{
			this.completeEvent.Call();
		}
		this.completeEvent = null;
		this.delayCoroutine = null;
		yield break;
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x000206D3 File Offset: 0x0001E8D3
	private void FadeIn()
	{
		if (this.animator.GetBool("Hidden"))
		{
			this.animator.SetBool("Hidden", false);
			return;
		}
		this.animator.SetTrigger("ReFade");
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x00020709 File Offset: 0x0001E909
	private void ResetToHidden()
	{
		this.animator.SetBool("Hidden", true);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0002071C File Offset: 0x0001E91C
	private void OnMessageBoxAdvance()
	{
		if (this.fadeInOnMessageBoxAdvance)
		{
			this.FadeIn();
		}
	}

	// Token: 0x0400061A RID: 1562
	[SerializeField]
	private float startEndDelay = 0.15f;

	// Token: 0x0400061B RID: 1563
	[SerializeField]
	private float endEndDelay = 0.35f;

	// Token: 0x0400061C RID: 1564
	[SerializeField]
	private MessageBoxDialogue messageBoxDialog;

	// Token: 0x0400061D RID: 1565
	[SerializeField]
	private MessageBox messageBox;

	// Token: 0x0400061E RID: 1566
	[SerializeField]
	private Selectable defaultSelectable;

	// Token: 0x0400061F RID: 1567
	[SerializeField]
	private UnityEvent onBeginSetting;

	// Token: 0x04000620 RID: 1568
	[SerializeField]
	private bool startsVisible;

	// Token: 0x04000621 RID: 1569
	[SerializeField]
	private bool fadeInOnMessageBoxAdvance = true;

	// Token: 0x04000622 RID: 1570
	[Header("Null means it should be set by BeginSetting(SimpleEvent)")]
	[SerializeField]
	private SimpleEvent completeEvent;

	// Token: 0x04000623 RID: 1571
	private Coroutine delayCoroutine;

	// Token: 0x04000624 RID: 1572
	private Animator animator;

	// Token: 0x04000625 RID: 1573
	private CanvasGroup cGroup;
}
