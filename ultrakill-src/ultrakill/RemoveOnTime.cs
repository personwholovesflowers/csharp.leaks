using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class RemoveOnTime : MonoBehaviour
{
	// Token: 0x060014CA RID: 5322 RVA: 0x000A7950 File Offset: 0x000A5B50
	private void Start()
	{
		if (!this.useAudioLength)
		{
			base.Invoke("Remove", this.time + Random.Range(-this.randomizer, this.randomizer));
			return;
		}
		AudioSource component = base.GetComponent<AudioSource>();
		if (!component)
		{
			Debug.LogError("useAudioLength is enabled, but an AudioSource was not found");
			Object.Destroy(this);
			return;
		}
		if (!component.clip)
		{
			Debug.LogError("useAudioLength is enabled without a clip");
			Object.Destroy(this);
			return;
		}
		base.Invoke("Remove", component.clip.length * component.pitch);
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x000A79E8 File Offset: 0x000A5BE8
	private void Remove()
	{
		if (this.affectedByNoCooldowns && NoWeaponCooldown.NoCooldown)
		{
			base.Invoke("Remove", this.time / 2f + Random.Range(-this.randomizer, this.randomizer));
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001C94 RID: 7316
	public bool useAudioLength;

	// Token: 0x04001C95 RID: 7317
	public float time;

	// Token: 0x04001C96 RID: 7318
	public float randomizer;

	// Token: 0x04001C97 RID: 7319
	public bool affectedByNoCooldowns;
}
