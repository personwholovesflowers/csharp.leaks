using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000095 RID: 149
public class BreakChunks : MonoBehaviour
{
	// Token: 0x060002E9 RID: 745 RVA: 0x000112F4 File Offset: 0x0000F4F4
	private void Start()
	{
		if (this.chunks.Length != 0)
		{
			GoreZone componentInParent = base.transform.GetComponentInParent<GoreZone>();
			Material material = null;
			bool flag = this.getEnviroMaterial && this.GetMaterial(out material);
			GameObject[] array = this.chunks.ToAssets();
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(array[i], base.transform.position, Random.rotation);
				Vector3 vector = new Vector3((float)Random.Range(-45, 45), (float)Random.Range(-45, 45), (float)Random.Range(-45, 45));
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.AddForce(vector, ForceMode.VelocityChange);
				}
				if (componentInParent != null)
				{
					gameObject.transform.SetParent(componentInParent.gibZone);
				}
				MeshRenderer meshRenderer;
				if (flag && gameObject.TryGetComponent<MeshRenderer>(out meshRenderer))
				{
					meshRenderer.material = material;
				}
			}
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000113DC File Offset: 0x0000F5DC
	private bool GetMaterial(out Material mat)
	{
		Vector3 vector = this.getDirection.normalized;
		if (this.relativeDirection)
		{
			vector = base.transform.InverseTransformDirection(vector);
		}
		Debug.Log("May need upgrading for blended surfaces", base.gameObject);
		SceneHelper.HitSurfaceData hitSurfaceData;
		if (!MonoSingleton<SceneHelper>.Instance.TryGetSurfaceData(base.transform.position - vector, vector, 5f, out hitSurfaceData))
		{
			mat = hitSurfaceData.material;
			return false;
		}
		mat = hitSurfaceData.material;
		return true;
	}

	// Token: 0x04000389 RID: 905
	public AssetReference[] chunks;

	// Token: 0x0400038A RID: 906
	public bool getEnviroMaterial;

	// Token: 0x0400038B RID: 907
	public Vector3 getDirection = Vector3.down;

	// Token: 0x0400038C RID: 908
	public bool relativeDirection = true;
}
