using System;
using UnityEngine;

// Token: 0x02000222 RID: 546
public class GasolineProjectile : MonoBehaviour
{
	// Token: 0x06000BBF RID: 3007 RVA: 0x00052834 File Offset: 0x00050A34
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 10 || other.gameObject.layer == 11)
		{
			if (this.hitSomething)
			{
				return;
			}
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (other.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && !enemyIdentifierIdentifier.eid.dead)
			{
				this.hitSomething = true;
				enemyIdentifierIdentifier.eid.AddFlammable(0.1f);
				Object.Destroy(base.gameObject);
			}
			return;
		}
		else
		{
			if (!LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
			{
				return;
			}
			Vector3 vector = base.transform.position;
			Vector3 vector2 = -this.rb.velocity;
			Ray ray = new Ray(base.transform.position - this.rb.velocity.normalized * Mathf.Max(2.5f, this.rb.velocity.magnitude * Time.fixedDeltaTime), this.rb.velocity.normalized);
			RaycastHit raycastHit;
			if (other.Raycast(ray, out raycastHit, 10f))
			{
				if (!LayerMaskDefaults.IsMatchingLayer(raycastHit.transform.gameObject.layer, LMD.Environment))
				{
					return;
				}
				vector = raycastHit.point;
				vector2 = raycastHit.normal;
				bool flag = true;
				MeshRenderer meshRenderer;
				if (!MonoSingleton<PostProcessV2_Handler>.Instance.usedComputeShadersAtStart)
				{
					vector += vector2 * 0.2f;
					flag = false;
				}
				else if (other.TryGetComponent<MeshRenderer>(out meshRenderer))
				{
					Material sharedMaterial = meshRenderer.sharedMaterial;
					if (sharedMaterial != null && sharedMaterial.IsKeywordEnabled("VERTEX_DISPLACEMENT"))
					{
						vector += vector2 * 0.2f;
						flag = false;
					}
				}
				GasolineStain gasolineStain = Object.Instantiate<GasolineStain>(this.stain, vector, base.transform.rotation);
				Transform transform = gasolineStain.transform;
				transform.forward = vector2 * -1f;
				transform.Rotate(Vector3.forward * Random.Range(0f, 360f));
				gasolineStain.AttachTo(other, flag);
				Object.Destroy(base.gameObject);
			}
			return;
		}
	}

	// Token: 0x04000F5A RID: 3930
	[SerializeField]
	private GasolineStain stain;

	// Token: 0x04000F5B RID: 3931
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000F5C RID: 3932
	[SerializeField]
	private SphereCollider col;

	// Token: 0x04000F5D RID: 3933
	private bool hitSomething;
}
