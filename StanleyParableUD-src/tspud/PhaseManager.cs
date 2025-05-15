using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class PhaseManager : MonoBehaviour
{
	// Token: 0x060002F8 RID: 760 RVA: 0x000148A2 File Offset: 0x00012AA2
	private void Awake()
	{
		this.phaseArray = base.GetComponentsInChildren<Phase>();
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x000148B0 File Offset: 0x00012AB0
	private void Start()
	{
		if (this.AutoStart)
		{
			this.StartFirstPhase();
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000148C0 File Offset: 0x00012AC0
	public void Disable()
	{
		this.disabled = true;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x000148CC File Offset: 0x00012ACC
	public void StartFirstPhase()
	{
		if (this.disabled)
		{
			return;
		}
		this.currentPhase = this.phaseArray[0];
		if (this.currentSwitchRoutine != null)
		{
			base.StopCoroutine(this.currentSwitchRoutine);
		}
		this.currentSwitchRoutine = base.StartCoroutine(this.SwitchRoutine(this.currentPhase, true));
		this.currentPhaseIndex = 0;
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00014924 File Offset: 0x00012B24
	public void AdvanceToNextPhase()
	{
		if (this.disabled)
		{
			return;
		}
		if (this.phaseArray.Length > this.currentPhaseIndex + 1)
		{
			this.currentPhaseIndex++;
			this.EnterPhase(this.phaseArray[this.currentPhaseIndex]);
		}
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00014962 File Offset: 0x00012B62
	public void AdvanceToLastPhase()
	{
		if (this.disabled)
		{
			return;
		}
		if (this.currentPhaseIndex > 0)
		{
			this.currentPhaseIndex--;
			this.EnterPhase(this.phaseArray[this.currentPhaseIndex]);
		}
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00014997 File Offset: 0x00012B97
	public void EnterPhase(Phase newPhase)
	{
		if (this.disabled)
		{
			return;
		}
		if (this.currentSwitchRoutine != null)
		{
			base.StopCoroutine(this.currentSwitchRoutine);
		}
		this.currentSwitchRoutine = base.StartCoroutine(this.SwitchRoutine(newPhase, false));
		this.currentPhaseIndex = newPhase.Index;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x000149D6 File Offset: 0x00012BD6
	private IEnumerator SwitchRoutine(Phase newPhase, bool first = false)
	{
		if (this.currentPhaseRoutine != null)
		{
			base.StopCoroutine(this.currentPhaseRoutine);
		}
		if (this.disabled)
		{
			yield break;
		}
		if (!first && this.currentPhase != null)
		{
			this.currentPhaseRoutine = base.StartCoroutine(this.PhaseRoutine(this.currentPhase.LeaveItems));
			yield return this.currentPhaseRoutine;
		}
		this.currentPhase = newPhase;
		this.currentPhaseRoutine = base.StartCoroutine(this.PhaseRoutine(this.currentPhase.EnterItems));
		yield return this.currentPhaseRoutine;
		yield break;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000149F3 File Offset: 0x00012BF3
	private IEnumerator PhaseRoutine(PhaseItem[] itemArray)
	{
		while (Time.timeScale == 0f)
		{
			yield return null;
		}
		int num;
		for (int i = 0; i < itemArray.Length; i = num + 1)
		{
			if (itemArray[i].Mode != PhaseItem.PhaseMode.Disabled)
			{
				itemArray[i].PhaseEvent.Invoke();
				float itemDuration = itemArray[i].Duration;
				if (itemDuration > 0f)
				{
					float itemCompletion = 0f;
					while (itemCompletion < itemDuration)
					{
						itemCompletion += Time.deltaTime * PhaseManager.GameSpeed;
						yield return null;
					}
				}
			}
			num = i;
		}
		this.currentPhaseRoutine = null;
		yield break;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00014A09 File Offset: 0x00012C09
	private void OnDestroy()
	{
		if (this.currentSwitchRoutine != null)
		{
			base.StopCoroutine(this.currentSwitchRoutine);
		}
		if (this.currentPhaseRoutine != null)
		{
			base.StopCoroutine(this.currentPhaseRoutine);
		}
	}

	// Token: 0x04000310 RID: 784
	public static float GameSpeed = 1f;

	// Token: 0x04000311 RID: 785
	private Phase[] phaseArray;

	// Token: 0x04000312 RID: 786
	private Phase currentPhase;

	// Token: 0x04000313 RID: 787
	private int currentPhaseIndex;

	// Token: 0x04000314 RID: 788
	public bool AutoStart = true;

	// Token: 0x04000315 RID: 789
	private bool disabled;

	// Token: 0x04000316 RID: 790
	private Coroutine currentPhaseRoutine;

	// Token: 0x04000317 RID: 791
	private Coroutine currentSwitchRoutine;
}
