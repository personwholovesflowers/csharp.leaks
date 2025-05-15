using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020001EF RID: 495
[CreateAssetMenu(fileName = "New Fish", menuName = "ULTRAKILL/Fish")]
public class FishObject : ScriptableObject
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x000458C6 File Offset: 0x00043AC6
	public GameObject InstantiateWorld(Vector3 position = default(Vector3))
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.worldObject, position, Quaternion.identity);
		gameObject.name = this.fishName;
		return gameObject;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x000458E8 File Offset: 0x00043AE8
	public GameObject InstantiateDumb()
	{
		return this.InstantiateWorld(default(Vector3));
	}

	// Token: 0x04000D18 RID: 3352
	public string fishName;

	// Token: 0x04000D19 RID: 3353
	public GameObject worldObject;

	// Token: 0x04000D1A RID: 3354
	[FormerlySerializedAs("pickup")]
	public ItemIdentifier customPickup;

	// Token: 0x04000D1B RID: 3355
	public Sprite icon;

	// Token: 0x04000D1C RID: 3356
	public Sprite blockedIcon;

	// Token: 0x04000D1D RID: 3357
	public bool canBeCooked = true;

	// Token: 0x04000D1E RID: 3358
	[TextArea]
	public string description;

	// Token: 0x04000D1F RID: 3359
	public float previewSizeMulti = 1f;
}
