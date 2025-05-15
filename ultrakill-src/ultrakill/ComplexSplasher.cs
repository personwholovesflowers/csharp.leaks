using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class ComplexSplasher : MonoBehaviour
{
	// Token: 0x06000438 RID: 1080 RVA: 0x0001D054 File Offset: 0x0001B254
	private void Awake()
	{
		this.children = new List<SplashingElement>();
		this.currentSplashes = new Dictionary<ParticleCluster, TimeSince>();
		SplashingElement splashingElement = null;
		foreach (SplashingElement splashingElement2 in base.GetComponentsInChildren<SplashingElement>())
		{
			this.children.Add(splashingElement2);
			splashingElement2.previousElement = splashingElement;
			splashingElement = splashingElement2;
		}
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0001D0A8 File Offset: 0x0001B2A8
	private void OnDrawGizmosSelected()
	{
		if (this.children == null)
		{
			return;
		}
		Gizmos.color = Color.red;
		int num = 0;
		while (num < this.children.Count && num + 1 < this.children.Count)
		{
			Component component = this.children[num];
			SplashingElement splashingElement = this.children[num + 1];
			Gizmos.color = ((this.children[num].isSplashing || this.children[num + 1].isSplashing) ? Color.green : Color.red);
			Gizmos.DrawLine(component.transform.position, splashingElement.transform.position);
			num++;
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0001D168 File Offset: 0x0001B368
	private void OnDisable()
	{
		foreach (KeyValuePair<ParticleCluster, TimeSince> keyValuePair in this.currentSplashes)
		{
			keyValuePair.Key.EmissionOff();
		}
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x0001D1C0 File Offset: 0x0001B3C0
	private void FixedUpdate()
	{
		List<ParticleCluster> list = new List<ParticleCluster>();
		foreach (KeyValuePair<ParticleCluster, TimeSince> keyValuePair in this.currentSplashes)
		{
			if (keyValuePair.Value > this.keepAliveFor)
			{
				list.Add(keyValuePair.Key);
			}
			keyValuePair.Key.EmissionOff();
		}
		list.ForEach(delegate(ParticleCluster x)
		{
			Object.Destroy(x.gameObject);
			this.currentSplashes.Remove(x);
		});
		foreach (SplashingElement splashingElement in this.children)
		{
			if (splashingElement.isSplashing)
			{
				ParticleCluster particleCluster = null;
				foreach (KeyValuePair<ParticleCluster, TimeSince> keyValuePair2 in this.currentSplashes)
				{
					if (Vector3.Distance(keyValuePair2.Key.transform.position, splashingElement.splashPosition) <= this.maxSplashDistance)
					{
						particleCluster = keyValuePair2.Key;
						this.currentSplashes[particleCluster] = 0f;
						break;
					}
				}
				if (particleCluster == null)
				{
					particleCluster = Object.Instantiate<ParticleCluster>(this.splashParticles);
					particleCluster.transform.SetParent(GoreZone.ResolveGoreZone(base.transform).transform);
					this.currentSplashes.Add(particleCluster, 0f);
				}
				particleCluster.EmissionOn();
				particleCluster.transform.position = splashingElement.splashPosition + Vector3.up * 3f;
			}
		}
	}

	// Token: 0x04000547 RID: 1351
	[SerializeField]
	private ParticleCluster splashParticles;

	// Token: 0x04000548 RID: 1352
	[SerializeField]
	private float maxSplashDistance = 80f;

	// Token: 0x04000549 RID: 1353
	[SerializeField]
	private float keepAliveFor = 3f;

	// Token: 0x0400054A RID: 1354
	private List<SplashingElement> children;

	// Token: 0x0400054B RID: 1355
	private Dictionary<ParticleCluster, TimeSince> currentSplashes;

	// Token: 0x0400054C RID: 1356
	private int splashElementIndex;
}
