using System;
using UnityEngine;

// Token: 0x02000349 RID: 841
public class PlayerPosInfo : MonoBehaviour
{
	// Token: 0x06001363 RID: 4963 RVA: 0x000229EB File Offset: 0x00020BEB
	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x04001AB7 RID: 6839
	public bool noPosition;

	// Token: 0x04001AB8 RID: 6840
	public Vector3 velocity;

	// Token: 0x04001AB9 RID: 6841
	public Vector3 position;

	// Token: 0x04001ABA RID: 6842
	public float wooshTime;
}
