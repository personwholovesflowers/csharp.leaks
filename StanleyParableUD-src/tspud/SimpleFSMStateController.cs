using System;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class SimpleFSMStateController : MonoBehaviour
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x000337F6 File Offset: 0x000319F6
	// (set) Token: 0x06000AF9 RID: 2809 RVA: 0x00005444 File Offset: 0x00003644
	public SimpleFSMState CurrentState
	{
		get
		{
			return this.StateMachine.currentState;
		}
		private set
		{
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00033804 File Offset: 0x00031A04
	protected void CreateFSM(SimpleFSMState[] stateArray)
	{
		this.StateMachine = new SimpleFSM(stateArray[0]);
		for (int i = 1; i < stateArray.Length; i++)
		{
			this.StateMachine.AddState(stateArray[i]);
		}
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0003383B File Offset: 0x00031A3B
	public void StartFSM()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.StateMachine.StartFSM());
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00033855 File Offset: 0x00031A55
	public void ChangeState<R>() where R : SimpleFSMState
	{
		this.StateMachine.ChangeState<R>();
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x00033862 File Offset: 0x00031A62
	public void StopFSM()
	{
		base.StopAllCoroutines();
		this.StateMachine.StopFSM();
	}

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	private string currentState;

	// Token: 0x04000AE1 RID: 2785
	protected SimpleFSM StateMachine;
}
