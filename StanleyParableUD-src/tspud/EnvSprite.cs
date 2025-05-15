using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class EnvSprite : HammerEntity
{
	// Token: 0x0600043A RID: 1082 RVA: 0x000199B4 File Offset: 0x00017BB4
	private void Awake()
	{
		this._renderer = base.GetComponent<MeshRenderer>();
		if (this._renderer != null)
		{
			this._renderer.material.SetColor("_Color", this.color);
		}
		this.baseAlpha = this.color.a;
		this.alpha = 0f;
		this.isEnabled = this.startOn;
		if (!this.isEnabled && this._renderer != null)
		{
			this._renderer.enabled = false;
		}
		if (StanleyController.Instance == null)
		{
			this.originTransform = Object.FindObjectOfType<Camera>().transform;
			return;
		}
		this.originTransform = StanleyController.Instance.camTransform;
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00019A6E File Offset: 0x00017C6E
	private void OnBecameVisible()
	{
		this.rendering = true;
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00019A77 File Offset: 0x00017C77
	private void OnBecameInvisible()
	{
		this.rendering = false;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00019A80 File Offset: 0x00017C80
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(base.transform.position, this.proxyRadius);
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00019A98 File Offset: 0x00017C98
	private void FixedUpdate()
	{
		if (!this.rendering)
		{
			return;
		}
		this.FireRays(4);
		float num = 0f;
		for (int i = 0; i < this.rayHistory.Length; i++)
		{
			if (this.rayHistory[i])
			{
				num += 1f / (float)this.rayHistory.Length;
			}
		}
		this.alpha = 0.5f * (this.alpha + num * this.baseAlpha);
		if (this.alpha != this.color.a)
		{
			this.color.a = this.alpha;
			if (this._renderer != null)
			{
				this._renderer.material.SetColor("_Color", this.color);
			}
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00019B54 File Offset: 0x00017D54
	private void FireRays(int amt)
	{
		Vector3 position = this.originTransform.position;
		for (int i = 0; i < amt; i++)
		{
			Vector3 vector = base.transform.position;
			Vector3 vector2 = Quaternion.Euler(TimMaths.SphereRandom(), Random.Range(0f, 180f), 0f) * Vector3.forward * this.proxyRadius;
			vector2 = Quaternion.FromToRotation(Vector3.right, position - vector) * vector2;
			vector += vector2;
			Vector3 vector3 = vector - position;
			float num = Vector3.Distance(position, vector);
			RaycastHit raycastHit;
			if (Physics.Raycast(position, vector3, out raycastHit, num, this.raycastMask))
			{
				this.rayHistory[this.rayIndex] = false;
			}
			else
			{
				this.rayHistory[this.rayIndex] = true;
			}
			this.rayIndex++;
			if (this.rayIndex >= this.rayHistory.Length)
			{
				this.rayIndex = 0;
			}
		}
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00019C50 File Offset: 0x00017E50
	public void Input_Color(string newColor)
	{
		string[] array = newColor.Split(new char[] { ' ' });
		if (array.Length != 3)
		{
			return;
		}
		Color color = new Color(float.Parse(array[0]) / 255f, float.Parse(array[1]) / 255f, float.Parse(array[2]) / 255f);
		this.color = color;
		this._renderer.material.SetColor("_Color", this.color);
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x00019CC9 File Offset: 0x00017EC9
	public void Input_ShowSprite()
	{
		base.Input_Enable();
		if (this._renderer != null)
		{
			this._renderer.enabled = true;
		}
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x00019CEB File Offset: 0x00017EEB
	public void Input_HideSprite()
	{
		base.Input_Disable();
		if (this._renderer != null)
		{
			this._renderer.enabled = false;
		}
	}

	// Token: 0x0400042D RID: 1069
	public bool startOn = true;

	// Token: 0x0400042E RID: 1070
	public Color color = Color.white;

	// Token: 0x0400042F RID: 1071
	private float baseAlpha = 1f;

	// Token: 0x04000430 RID: 1072
	private float alpha = 1f;

	// Token: 0x04000431 RID: 1073
	public float proxyRadius = 0.02f;

	// Token: 0x04000432 RID: 1074
	private MeshRenderer _renderer;

	// Token: 0x04000433 RID: 1075
	private bool rendering;

	// Token: 0x04000434 RID: 1076
	private LayerMask raycastMask = 1;

	// Token: 0x04000435 RID: 1077
	private bool[] rayHistory = new bool[32];

	// Token: 0x04000436 RID: 1078
	private int rayIndex;

	// Token: 0x04000437 RID: 1079
	private Transform originTransform;
}
