using System;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class GibDestroyer : MonoBehaviour
{
	// Token: 0x06001D97 RID: 7575 RVA: 0x000F75E4 File Offset: 0x000F57E4
	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject == null)
		{
			return;
		}
		GoreSplatter goreSplatter;
		if (col.transform.TryGetComponent<GoreSplatter>(out goreSplatter))
		{
			goreSplatter.Repool();
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (col.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid.dead)
		{
			if (this.lastSound > 0.1f)
			{
				this.lastSound = 0f;
				if (this.rareSoundEffect && Random.Range(0f, 1f) < 0.025f)
				{
					Object.Instantiate<AudioSource>(this.rareSoundEffect, col.transform.position, Quaternion.identity);
				}
				else if (this.soundEffect)
				{
					Object.Instantiate<AudioSource>(this.soundEffect, col.transform.position, Quaternion.identity);
				}
			}
			GibDestroyer.LimbBegone(col);
		}
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x000F76CC File Offset: 0x000F58CC
	public static void LimbBegone(Collider col)
	{
		col.transform.localScale = Vector3.zero;
		if (col.transform.parent != null)
		{
			col.transform.position = col.transform.parent.position;
		}
		else
		{
			col.transform.position = Vector3.zero;
		}
		if (col.attachedRigidbody)
		{
			Joint[] componentsInChildren = col.attachedRigidbody.GetComponentsInChildren<Joint>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.Destroy(componentsInChildren[i]);
			}
			Object.Destroy(col.attachedRigidbody);
		}
		col.gameObject.SetActive(false);
		Object.Destroy(col);
	}

	// Token: 0x040029F2 RID: 10738
	[SerializeField]
	private AudioSource soundEffect;

	// Token: 0x040029F3 RID: 10739
	[SerializeField]
	private AudioSource rareSoundEffect;

	// Token: 0x040029F4 RID: 10740
	private TimeSince lastSound;
}
