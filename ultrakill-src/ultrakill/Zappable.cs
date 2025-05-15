using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class Zappable : MonoBehaviour
{
	// Token: 0x06001C22 RID: 7202 RVA: 0x000E98ED File Offset: 0x000E7AED
	private void OnEnable()
	{
		MonoSingleton<ObjectTracker>.Instance.AddZappable(this);
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x000E98FA File Offset: 0x000E7AFA
	private void OnDisable()
	{
		if (MonoSingleton<ObjectTracker>.Instance)
		{
			MonoSingleton<ObjectTracker>.Instance.RemoveZappable(this);
		}
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x000E9913 File Offset: 0x000E7B13
	public IEnumerator Zap(List<GameObject> alreadyHitObjects, float damage = 1f, GameObject sourceWeapon = null)
	{
		if (this.beenZapped)
		{
			yield break;
		}
		this.beenZapped = true;
		alreadyHitObjects.Add(base.gameObject);
		yield return new WaitForSeconds(0.25f);
		EnemyIdentifier.Zap(base.transform.position, damage / 2f, alreadyHitObjects, sourceWeapon, null, null, false);
		yield return new WaitForSeconds(1f);
		this.beenZapped = false;
		yield break;
	}

	// Token: 0x040027AD RID: 10157
	private bool beenZapped;
}
