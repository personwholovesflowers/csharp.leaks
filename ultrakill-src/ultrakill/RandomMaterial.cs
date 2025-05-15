using System;
using UnityEngine;

// Token: 0x02000377 RID: 887
public class RandomMaterial : MonoBehaviour
{
	// Token: 0x06001498 RID: 5272 RVA: 0x000A6C81 File Offset: 0x000A4E81
	private void Start()
	{
		this.renderer = base.GetComponent<Renderer>();
		this.previousChange = 0f;
		if (!this.activated && this.instantOnFirstTime)
		{
			this.Randomize();
		}
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x000A6CB5 File Offset: 0x000A4EB5
	private void Update()
	{
		if (this.oneTime && this.activated)
		{
			return;
		}
		if (this.previousChange > (float)this.delay)
		{
			this.previousChange = 0f;
			this.Randomize();
		}
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x000A6CF2 File Offset: 0x000A4EF2
	public void Randomize()
	{
		this.activated = true;
		this.renderer.sharedMaterial = this.materials[Random.Range(0, this.materials.Length)];
	}

	// Token: 0x04001C5C RID: 7260
	private Renderer renderer;

	// Token: 0x04001C5D RID: 7261
	public Material[] materials;

	// Token: 0x04001C5E RID: 7262
	public int delay = 1;

	// Token: 0x04001C5F RID: 7263
	public bool instantOnFirstTime;

	// Token: 0x04001C60 RID: 7264
	private TimeSince previousChange;

	// Token: 0x04001C61 RID: 7265
	public bool oneTime;

	// Token: 0x04001C62 RID: 7266
	private bool activated;
}
