using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
public class BloodAbsorberChild : MonoBehaviour, IBloodstainReceiver
{
	// Token: 0x06001B56 RID: 6998 RVA: 0x000E357E File Offset: 0x000E177E
	private void Start()
	{
		this.bloodGroup = base.GetComponentInParent<BloodAbsorber>();
		this.mRend = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x000E3598 File Offset: 0x000E1798
	private void OnEnable()
	{
		BloodsplatterManager instance = MonoSingleton<BloodsplatterManager>.Instance;
		if (instance != null)
		{
			instance.bloodAbsorberChildren++;
		}
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000E35C4 File Offset: 0x000E17C4
	private void OnDisable()
	{
		BloodsplatterManager instance = MonoSingleton<BloodsplatterManager>.Instance;
		if (instance != null)
		{
			instance.bloodAbsorberChildren--;
		}
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x000E35EE File Offset: 0x000E17EE
	public bool HandleBloodstainHit(ref RaycastHit hit)
	{
		this.bloodGroup.HandleBloodstainHit(ref hit);
		return true;
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000E35FE File Offset: 0x000E17FE
	public void ProcessWasherSpray(ref List<ParticleCollisionEvent> pEvents, Vector3 position)
	{
		this.bloodGroup.ProcessWasherSpray(ref pEvents, position, this.mRend);
	}

	// Token: 0x04002690 RID: 9872
	[HideInInspector]
	public BloodAbsorber bloodGroup;

	// Token: 0x04002691 RID: 9873
	private MeshRenderer mRend;
}
