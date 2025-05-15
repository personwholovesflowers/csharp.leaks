using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Image Effects/Amplify Color Volume")]
public class AmplifyColorVolume : AmplifyColorVolumeBase
{
	// Token: 0x06000037 RID: 55 RVA: 0x00003C28 File Offset: 0x00001E28
	private void OnTriggerEnter(Collider other)
	{
		AmplifyColorTriggerProxy component = other.GetComponent<AmplifyColorTriggerProxy>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & (1 << base.gameObject.layer)) != 0)
		{
			component.OwnerEffect.EnterVolume(this);
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003C84 File Offset: 0x00001E84
	private void OnTriggerExit(Collider other)
	{
		AmplifyColorTriggerProxy component = other.GetComponent<AmplifyColorTriggerProxy>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & (1 << base.gameObject.layer)) != 0)
		{
			component.OwnerEffect.ExitVolume(this);
		}
	}
}
