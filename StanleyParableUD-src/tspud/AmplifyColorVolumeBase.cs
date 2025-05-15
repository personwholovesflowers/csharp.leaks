using System;
using AmplifyColor;
using UnityEngine;

// Token: 0x0200000B RID: 11
[ExecuteInEditMode]
[AddComponentMenu("")]
public class AmplifyColorVolumeBase : MonoBehaviour
{
	// Token: 0x0600003D RID: 61 RVA: 0x00003DA0 File Offset: 0x00001FA0
	private void OnDrawGizmos()
	{
		if (this.ShowInSceneView)
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
				Gizmos.color = Color.green;
				Gizmos.matrix = base.transform.localToWorldMatrix;
				Gizmos.DrawWireCube(vector, vector2);
			}
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003E28 File Offset: 0x00002028
	private void OnDrawGizmosSelected()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		BoxCollider2D component2 = base.GetComponent<BoxCollider2D>();
		if (component != null || component2 != null)
		{
			Color green = Color.green;
			green.a = 0.2f;
			Gizmos.color = green;
			Gizmos.matrix = base.transform.localToWorldMatrix;
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
			Gizmos.DrawCube(vector, vector2);
		}
	}

	// Token: 0x04000056 RID: 86
	public Texture2D LutTexture;

	// Token: 0x04000057 RID: 87
	public float Exposure = 1f;

	// Token: 0x04000058 RID: 88
	public float EnterBlendTime = 1f;

	// Token: 0x04000059 RID: 89
	public int Priority;

	// Token: 0x0400005A RID: 90
	public bool ShowInSceneView = true;

	// Token: 0x0400005B RID: 91
	[HideInInspector]
	public VolumeEffectContainer EffectContainer = new VolumeEffectContainer();
}
