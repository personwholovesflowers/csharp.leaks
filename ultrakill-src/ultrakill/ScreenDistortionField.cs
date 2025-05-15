using System;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class ScreenDistortionField : MonoBehaviour
{
	// Token: 0x06001610 RID: 5648 RVA: 0x000B281D File Offset: 0x000B0A1D
	private void Start()
	{
		this.col = base.GetComponent<Collider>();
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x000B282B File Offset: 0x000B0A2B
	private void OnEnable()
	{
		if (!MonoSingleton<ScreenDistortionController>.Instance.fields.Contains(this))
		{
			MonoSingleton<ScreenDistortionController>.Instance.fields.Add(this);
		}
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x000B284F File Offset: 0x000B0A4F
	private void OnDisable()
	{
		if (MonoSingleton<ScreenDistortionController>.Instance.fields.Contains(this))
		{
			MonoSingleton<ScreenDistortionController>.Instance.fields.Remove(this);
		}
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x000B2874 File Offset: 0x000B0A74
	private void Update()
	{
		Vector3 position = MonoSingleton<PlayerTracker>.Instance.GetPlayer().position;
		Vector3 vector = (this.col ? this.col.ClosestPoint(position) : base.transform.position);
		float num = Vector3.Distance(position, vector);
		if (num < this.distance)
		{
			float num2 = Mathf.Pow((this.distance - num) / this.distance, 2f);
			this.currentStrength = num2 * this.strength;
			return;
		}
		if (this.currentStrength != 0f)
		{
			this.currentStrength = 0f;
		}
	}

	// Token: 0x04001E60 RID: 7776
	private Collider col;

	// Token: 0x04001E61 RID: 7777
	public float distance;

	// Token: 0x04001E62 RID: 7778
	public float strength = 1f;

	// Token: 0x04001E63 RID: 7779
	public float currentStrength;
}
