using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[RequireComponent(typeof(BoxCollider2D))]
[AddComponentMenu("Image Effects/Amplify Color Volume 2D")]
public class AmplifyColorVolume2D : AmplifyColorVolumeBase
{
	// Token: 0x0600003A RID: 58 RVA: 0x00003CE8 File Offset: 0x00001EE8
	private void OnTriggerEnter2D(Collider2D other)
	{
		AmplifyColorTriggerProxy2D component = other.GetComponent<AmplifyColorTriggerProxy2D>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & (1 << base.gameObject.layer)) != 0)
		{
			component.OwnerEffect.EnterVolume(this);
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003D44 File Offset: 0x00001F44
	private void OnTriggerExit2D(Collider2D other)
	{
		AmplifyColorTriggerProxy2D component = other.GetComponent<AmplifyColorTriggerProxy2D>();
		if (component != null && component.OwnerEffect.UseVolumes && (component.OwnerEffect.VolumeCollisionMask & (1 << base.gameObject.layer)) != 0)
		{
			component.OwnerEffect.ExitVolume(this);
		}
	}
}
