using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[RequireComponent(typeof(BoxCollider))]
public class VolumeBase : MonoBehaviour
{
	// Token: 0x06000337 RID: 823 RVA: 0x00008FD9 File Offset: 0x000071D9
	public virtual ProfileBase GetProfile()
	{
		return null;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00015F22 File Offset: 0x00014122
	private void Awake()
	{
		if (this.BoxCollider == null)
		{
			this.UpdateBoxCollider();
		}
		this.UpdateFeatheredBounds();
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00015F22 File Offset: 0x00014122
	private void OnValidate()
	{
		if (this.BoxCollider == null)
		{
			this.UpdateBoxCollider();
		}
		this.UpdateFeatheredBounds();
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00015F3E File Offset: 0x0001413E
	private void UpdateBoxCollider()
	{
		this.BoxCollider = base.GetComponent<BoxCollider>();
		this.BoxCollider.isTrigger = true;
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00015F58 File Offset: 0x00014158
	private void UpdateFeatheredBounds()
	{
		Vector3 vector = base.transform.TransformPoint(this.BoxCollider.center);
		Vector3 vector2 = this.BoxCollider.size;
		vector2 = base.transform.TransformVector(vector2);
		this.FeatheredBounds = new Bounds(vector, new Vector3(vector2.x * this.FeatherValue, vector2.y, vector2.z * this.FeatherValue));
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00015FC8 File Offset: 0x000141C8
	private void OnDrawGizmos()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		BoxCollider2D component2 = base.GetComponent<BoxCollider2D>();
		if (component != null || component2 != null)
		{
			Vector3 vector;
			Vector3 vector2;
			if (component != null)
			{
				vector = component.center;
				vector2 = component.size;
			}
			else
			{
				vector = component2.offset;
				vector2 = component2.size;
			}
			if (this.Preservation != VolumeBase.VolumePreservation.FeatheredVolume && this.Preservation != VolumeBase.VolumePreservation.FeatheredVolumePreserve)
			{
				if (this.Preservation == VolumeBase.VolumePreservation.DiscardWhenLeaving)
				{
					Gizmos.color = this.GetVolumeBaseColor();
				}
				else
				{
					Gizmos.color = this.GetVolumeBaseColor() * Color.gray;
				}
				Gizmos.matrix = base.transform.localToWorldMatrix;
				Gizmos.DrawCube(vector, vector2);
				return;
			}
			if (this.Preservation == VolumeBase.VolumePreservation.FeatheredVolumePreserve || this.Preservation == VolumeBase.VolumePreservation.FeatheredVolume)
			{
				if (this.Preservation == VolumeBase.VolumePreservation.FeatheredVolume)
				{
					Gizmos.color = this.GetVolumeBaseColor();
				}
				else
				{
					Gizmos.color = this.GetVolumeBaseColor() * Color.gray;
				}
				Gizmos.matrix = base.transform.localToWorldMatrix;
				Gizmos.DrawCube(vector, vector2);
				Gizmos.color = this.GetInnerCubeColor();
				Gizmos.matrix = base.transform.localToWorldMatrix;
				Gizmos.DrawCube(vector, new Vector3(vector2.x * this.FeatherValue, vector2.y, vector2.z * this.FeatherValue));
			}
		}
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001611A File Offset: 0x0001431A
	protected virtual Color GetVolumeBaseColor()
	{
		return Color.green * new Color(1f, 1f, 1f, 0.25f);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0001613F File Offset: 0x0001433F
	protected virtual Color GetInnerCubeColor()
	{
		return Color.yellow * new Color(1f, 1f, 1f, 0.25f);
	}

	// Token: 0x04000332 RID: 818
	public int Priority;

	// Token: 0x04000333 RID: 819
	public VolumeBase.VolumePreservation Preservation;

	// Token: 0x04000334 RID: 820
	public AnimationCurve LerpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x04000335 RID: 821
	public float Duration = 1f;

	// Token: 0x04000336 RID: 822
	[Range(0f, 1f)]
	public float FeatherValue;

	// Token: 0x04000337 RID: 823
	[SerializeField]
	[HideInInspector]
	public BoxCollider BoxCollider;

	// Token: 0x04000338 RID: 824
	[HideInInspector]
	public Bounds FeatheredBounds;

	// Token: 0x02000387 RID: 903
	public enum VolumePreservation
	{
		// Token: 0x040012E2 RID: 4834
		DiscardWhenLeaving,
		// Token: 0x040012E3 RID: 4835
		Preserve,
		// Token: 0x040012E4 RID: 4836
		FeatheredVolume,
		// Token: 0x040012E5 RID: 4837
		FeatheredVolumePreserve
	}
}
