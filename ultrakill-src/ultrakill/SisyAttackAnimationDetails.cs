using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003FF RID: 1023
[CreateAssetMenu(menuName = "ULTRAKILL/Attack Animation Details")]
public class SisyAttackAnimationDetails : ScriptableObject
{
	// Token: 0x04002022 RID: 8226
	[Header("Boulder")]
	public float minBoulderSpeed = 0.01f;

	// Token: 0x04002023 RID: 8227
	public float boulderDistanceDivide = 100f;

	// Token: 0x04002024 RID: 8228
	public float maxBoulderSpeed = 1E+10f;

	// Token: 0x04002025 RID: 8229
	[FormerlySerializedAs("durationMulti")]
	public float finalDurationMulti = 1f;

	// Token: 0x04002026 RID: 8230
	[Header("Anim")]
	public float speedDistanceMulti = 1f;

	// Token: 0x04002027 RID: 8231
	[FormerlySerializedAs("minSpeedCap")]
	public float minAnimSpeedCap = 0.1f;

	// Token: 0x04002028 RID: 8232
	[FormerlySerializedAs("maxSpeedCap")]
	public float maxAnimSpeedCap = 1f;
}
