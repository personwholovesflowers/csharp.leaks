using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class SimpleFSM
{
	// Token: 0x06000AE1 RID: 2785 RVA: 0x00033662 File Offset: 0x00031862
	public SimpleFSM(SimpleFSMState _initialState)
	{
		this.initialState = _initialState;
		this.currentState = this.initialState;
		this.AddState(this.initialState);
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0003369B File Offset: 0x0003189B
	public IEnumerator StartFSM()
	{
		this.alive = true;
		this.inTransitionOrNotReady = true;
		this.currentState = this.initialState;
		yield return this.currentState.cntrl.StartCoroutine(this.currentState.Begin());
		this.inTransitionOrNotReady = false;
		yield break;
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x000336AA File Offset: 0x000318AA
	public void StopFSM()
	{
		this.currentState.cntrl.StopAllCoroutines();
		this.inTransitionOrNotReady = false;
		this.alive = false;
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x000336CA File Offset: 0x000318CA
	public void AddState(SimpleFSMState stateToAdd)
	{
		this._states.Add(stateToAdd.GetType(), stateToAdd);
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x000336E0 File Offset: 0x000318E0
	public void ChangeState<R>() where R : SimpleFSMState
	{
		Type typeFromHandle = typeof(R);
		this.inTransitionOrNotReady = true;
		this.currentState.cntrl.StartCoroutine(this.PerformChangeState(this._states[typeFromHandle]));
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00033722 File Offset: 0x00031922
	public void ChangeState(SimpleFSMState newState)
	{
		this.inTransitionOrNotReady = true;
		this.currentState.cntrl.StartCoroutine(this.PerformChangeState(newState));
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x00033743 File Offset: 0x00031943
	public IEnumerator PerformChangeState(SimpleFSMState newState)
	{
		this.nextState = newState.GetType();
		this.lastState = this.currentState.GetType();
		yield return this.currentState.cntrl.StartCoroutine(this.currentState.Exit());
		this.currentState = this._states[newState.GetType()];
		yield return this.currentState.cntrl.StartCoroutine(this.currentState.Begin());
		this.inTransitionOrNotReady = false;
		yield return null;
		yield break;
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00033759 File Offset: 0x00031959
	public void UpdateState()
	{
		if (this.currentState != null && !this.inTransitionOrNotReady)
		{
			this.currentState.Reason();
			this.currentState.DoUpdate();
		}
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00033781 File Offset: 0x00031981
	public void LateUpdateState()
	{
		if (this.currentState != null && !this.inTransitionOrNotReady)
		{
			this.currentState.DoLateUpdate();
		}
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0003379E File Offset: 0x0003199E
	public void FixedUpdateState()
	{
		if (this.currentState != null && !this.inTransitionOrNotReady)
		{
			this.currentState.DoFixedUpdate();
		}
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x000337BB File Offset: 0x000319BB
	public void OnCollisionEnterState(Collision col)
	{
		this.currentState.OnCollisionState(col);
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x000337C9 File Offset: 0x000319C9
	public void OnTriggerEnterState(Collider col)
	{
		this.currentState.OnTriggerEnterState(col);
	}

	// Token: 0x04000AD8 RID: 2776
	public SimpleFSMState currentState;

	// Token: 0x04000AD9 RID: 2777
	public Type lastState;

	// Token: 0x04000ADA RID: 2778
	public Type nextState;

	// Token: 0x04000ADB RID: 2779
	private Dictionary<Type, SimpleFSMState> _states = new Dictionary<Type, SimpleFSMState>();

	// Token: 0x04000ADC RID: 2780
	public bool inTransitionOrNotReady = true;

	// Token: 0x04000ADD RID: 2781
	public bool alive;

	// Token: 0x04000ADE RID: 2782
	private SimpleFSMState initialState;
}
