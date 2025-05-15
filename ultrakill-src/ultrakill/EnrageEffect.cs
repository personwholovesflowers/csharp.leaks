using System;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class EnrageEffect : MonoBehaviour
{
	// Token: 0x06000862 RID: 2146 RVA: 0x0003A0FC File Offset: 0x000382FC
	private void Start()
	{
		if (!this.activated)
		{
			this.activated = true;
			CameraController instance = MonoSingleton<CameraController>.Instance;
			MonoSingleton<StyleHUD>.Instance.AddPoints(250, string.IsNullOrEmpty(this.styleNameOverride) ? "ultrakill.enraged" : ("<color=red>" + this.styleNameOverride.ToUpper() + "</color>"), null, null, -1, "", "");
			instance.CameraShake(1f);
		}
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0003A174 File Offset: 0x00038374
	private void OnDestroy()
	{
		if (!this.noParticle && base.gameObject.scene.isLoaded)
		{
			Object.Instantiate<GameObject>(this.endSound, base.transform.position, base.transform.rotation);
		}
	}

	// Token: 0x04000B2C RID: 2860
	public GameObject endSound;

	// Token: 0x04000B2D RID: 2861
	public bool noParticle;

	// Token: 0x04000B2E RID: 2862
	[HideInInspector]
	public bool activated;

	// Token: 0x04000B2F RID: 2863
	public string styleNameOverride;
}
