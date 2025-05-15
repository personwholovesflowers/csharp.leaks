using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000421 RID: 1057
[CreateAssetMenu(menuName = "ULTRAKILL/Spawnable Object")]
public class SpawnableObject : ScriptableObject
{
	// Token: 0x04002161 RID: 8545
	public string identifier;

	// Token: 0x04002162 RID: 8546
	public SpawnableObject.SpawnableObjectDataType spawnableObjectType;

	// Token: 0x04002163 RID: 8547
	public UnlockableType unlockableType;

	// Token: 0x04002164 RID: 8548
	public bool sandboxOnly;

	// Token: 0x04002165 RID: 8549
	public string objectName;

	// Token: 0x04002166 RID: 8550
	public string type;

	// Token: 0x04002167 RID: 8551
	[TextArea]
	public string description;

	// Token: 0x04002168 RID: 8552
	[TextArea]
	public string strategy;

	// Token: 0x04002169 RID: 8553
	public GameObject gameObject;

	// Token: 0x0400216A RID: 8554
	public GameObject preview;

	// Token: 0x0400216B RID: 8555
	public string iconKey;

	// Token: 0x0400216C RID: 8556
	public Sprite gridIcon;

	// Token: 0x0400216D RID: 8557
	public Color backgroundColor;

	// Token: 0x0400216E RID: 8558
	public EnemyType enemyType;

	// Token: 0x0400216F RID: 8559
	public bool fullEnemyComponent;

	// Token: 0x04002170 RID: 8560
	public SpawnableType spawnableType;

	// Token: 0x04002171 RID: 8561
	public Vector3 armOffset = Vector3.zero;

	// Token: 0x04002172 RID: 8562
	[FormerlySerializedAs("rotationOffset")]
	public Vector3 armRotationOffset = Vector3.zero;

	// Token: 0x04002173 RID: 8563
	public Vector3 menuOffset = Vector3.zero;

	// Token: 0x04002174 RID: 8564
	public Vector3 menuScale = Vector3.one;

	// Token: 0x04002175 RID: 8565
	public float spawnOffset;

	// Token: 0x04002176 RID: 8566
	public bool isWater;

	// Token: 0x04002177 RID: 8567
	public bool triggerOnly;

	// Token: 0x04002178 RID: 8568
	public bool alwaysKinematic;

	// Token: 0x04002179 RID: 8569
	public AlterOption[] defaultSettings;

	// Token: 0x02000422 RID: 1058
	public enum SpawnableObjectDataType
	{
		// Token: 0x0400217B RID: 8571
		Object,
		// Token: 0x0400217C RID: 8572
		Enemy,
		// Token: 0x0400217D RID: 8573
		Tool = 3,
		// Token: 0x0400217E RID: 8574
		Unlockable
	}
}
