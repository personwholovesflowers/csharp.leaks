using System;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class MorphTarget : MonoBehaviour
{
	// Token: 0x06001185 RID: 4485 RVA: 0x00088662 File Offset: 0x00086862
	private void Awake()
	{
		this.rend = base.GetComponent<SkinnedMeshRenderer>();
		this.skinmesh = this.rend.sharedMesh;
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x00088681 File Offset: 0x00086881
	private void Update()
	{
		this.rend.SetBlendShapeWeight(0, this.blend);
		this.blend = (this.blend + Time.deltaTime * this.speed * 60f) % 100f;
	}

	// Token: 0x040017C6 RID: 6086
	public float speed = 1f;

	// Token: 0x040017C7 RID: 6087
	private SkinnedMeshRenderer rend;

	// Token: 0x040017C8 RID: 6088
	private Mesh skinmesh;

	// Token: 0x040017C9 RID: 6089
	private float blend;

	// Token: 0x040017CA RID: 6090
	private int blendshapecount;
}
