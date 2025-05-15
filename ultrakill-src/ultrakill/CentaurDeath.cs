using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E4 RID: 1252
public class CentaurDeath : MonoBehaviour
{
	// Token: 0x06001CB6 RID: 7350 RVA: 0x000F0D90 File Offset: 0x000EEF90
	private void Start()
	{
		this.propBlock = new MaterialPropertyBlock();
		this.propBlock.SetTexture("_BurningTex", this.burningTex);
		this.mRends = base.GetComponentsInChildren<MeshRenderer>(true);
		Bounds bounds = this.mRends[0].bounds;
		foreach (MeshRenderer meshRenderer in this.mRends)
		{
			bounds.Encapsulate(meshRenderer.bounds);
		}
		foreach (MeshRenderer meshRenderer2 in this.mRends)
		{
			this.allMaterials.AddRange(meshRenderer2.materials);
			Vector4 vector = bounds.size;
			this.propBlock.SetVector("_MeshScale", vector);
			meshRenderer2.SetPropertyBlock(this.propBlock);
		}
		foreach (Material material in this.allMaterials)
		{
			material.EnableKeyword("BURNING");
		}
		this.burnTimeFade = 1f;
		Rigidbody[] componentsInChildren = base.GetComponentsInChildren<Rigidbody>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].AddForceAtPosition(Vector3.one * this.forceStrength, bounds.center, ForceMode.Impulse);
		}
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x000F0EE8 File Offset: 0x000EF0E8
	private void Update()
	{
		this.realTime += Time.deltaTime;
		this.burnTime += Time.deltaTime * 0.05f;
		float num = this.timeCurve.Evaluate(this.burnTime);
		if (this.realTime >= 10f)
		{
			this.burnTimeFade -= Time.deltaTime * 0.1f;
		}
		for (int i = 0; i < this.mRends.Length; i++)
		{
			MeshRenderer meshRenderer = this.mRends[i];
			meshRenderer.GetPropertyBlock(this.propBlock);
			this.propBlock.SetFloat(this.burnID, num);
			this.propBlock.SetFloat(this.burnFadeID, this.burnTimeFade);
			meshRenderer.SetPropertyBlock(this.propBlock);
		}
	}

	// Token: 0x040028BB RID: 10427
	public float forceStrength = 100f;

	// Token: 0x040028BC RID: 10428
	public AnimationCurve timeCurve;

	// Token: 0x040028BD RID: 10429
	public Texture burningTex;

	// Token: 0x040028BE RID: 10430
	private MeshRenderer[] mRends;

	// Token: 0x040028BF RID: 10431
	private List<Material> allMaterials = new List<Material>();

	// Token: 0x040028C0 RID: 10432
	private MaterialPropertyBlock propBlock;

	// Token: 0x040028C1 RID: 10433
	private int burnID = Shader.PropertyToID("_BurnTime");

	// Token: 0x040028C2 RID: 10434
	private int burnFadeID = Shader.PropertyToID("_BurnTimeFade");

	// Token: 0x040028C3 RID: 10435
	private float realTime;

	// Token: 0x040028C4 RID: 10436
	private float burnTime;

	// Token: 0x040028C5 RID: 10437
	private float burnTimeFade = 1f;
}
