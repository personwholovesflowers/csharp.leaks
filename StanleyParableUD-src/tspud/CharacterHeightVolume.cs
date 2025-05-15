using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class CharacterHeightVolume : MonoBehaviour
{
	// Token: 0x0600094E RID: 2382 RVA: 0x0002BA44 File Offset: 0x00029C44
	private void Awake()
	{
		this.position = base.transform.position;
		this.halfExtents = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
		this.halfExtents *= 0.5f;
		this.orientation = base.transform.rotation;
		if (this.SetOnAwake)
		{
			StanleyController.Instance.SetCharacterHeightMultiplier(this.characterHeightMultiplier);
		}
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00005444 File Offset: 0x00003644
	private void Update()
	{
	}

	// Token: 0x04000923 RID: 2339
	public float characterHeightMultiplier = 1f;

	// Token: 0x04000924 RID: 2340
	public CharacterHeightVolume.ExitColliderMode exitColliderMode;

	// Token: 0x04000925 RID: 2341
	private Vector3 halfExtents;

	// Token: 0x04000926 RID: 2342
	private Vector3 position;

	// Token: 0x04000927 RID: 2343
	private Quaternion orientation;

	// Token: 0x04000928 RID: 2344
	private LayerMask layerMask = 512;

	// Token: 0x04000929 RID: 2345
	private bool touchingLastFrame;

	// Token: 0x0400092A RID: 2346
	private bool isEnabled = true;

	// Token: 0x0400092B RID: 2347
	[SerializeField]
	private bool SetOnAwake;

	// Token: 0x020003EB RID: 1003
	public enum ExitColliderMode
	{
		// Token: 0x0400146E RID: 5230
		ResetToOneOnExit,
		// Token: 0x0400146F RID: 5231
		DoesNotResetOnExit
	}
}
