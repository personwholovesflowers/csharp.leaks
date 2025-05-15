using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class BloodstainParent : MonoBehaviour
{
	// Token: 0x06000297 RID: 663 RVA: 0x0000EF84 File Offset: 0x0000D184
	public void OnStep()
	{
		this.matrixAtStep = this.GetMatrix();
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000EF92 File Offset: 0x0000D192
	public Matrix4x4 GetMatrix()
	{
		return Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(1f, 1f, 1f));
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
	private void Start()
	{
		this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		this.parentIndex = this.bsm.CreateParent(this.GetMatrix());
		this.OnStep();
		this.bsm.reuseStainIndex += this.OnStainIndexReuse;
		this.bsm.reuseParentIndex += this.OnParentIndexReuse;
		this.bsm.StainsCleared += this.OnStainsCleared;
		this.bsm.PostCollisionStep += this.OnStep;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000F055 File Offset: 0x0000D255
	public void OnStainsCleared()
	{
		this.parentIndex = -1;
		this.children.Clear();
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000F06C File Offset: 0x0000D26C
	public void CreateChild(Vector3 pos, Vector3 norm, bool clipToSurface, bool fromStep)
	{
		if (this.parentIndex == -1)
		{
			this.parentIndex = this.bsm.CreateParent(this.GetMatrix());
		}
		if (this.bsm.usedComputeShadersAtStart)
		{
			Vector3 vector = base.transform.InverseTransformPoint(pos);
			Vector3 vector2 = base.transform.InverseTransformDirection(norm);
			int num = this.bsm.CreateBloodstain(vector, vector2, clipToSurface, this.parentIndex);
			this.children.Add(num);
			return;
		}
		int num2 = this.bsm.CreateBloodstain(pos, norm, clipToSurface, this.parentIndex);
		this.children.Add(num2);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000F105 File Offset: 0x0000D305
	private void OnStainIndexReuse(int index)
	{
		this.children.Remove(index);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0000F114 File Offset: 0x0000D314
	private void OnParentIndexReuse(int index)
	{
		if (index == this.parentIndex)
		{
			this.parentIndex = -1;
			this.ClearChildren();
		}
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0000F12C File Offset: 0x0000D32C
	private void Update()
	{
		if (this.parentIndex != -1 && base.transform.hasChanged)
		{
			this.bsm.parents[this.parentIndex] = this.GetMatrix();
			base.transform.hasChanged = false;
		}
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0000F16C File Offset: 0x0000D36C
	private void OnDestroy()
	{
		this.ClearChildren();
		if (this.bsm)
		{
			this.bsm.reuseStainIndex -= this.OnStainIndexReuse;
			this.bsm.reuseParentIndex -= this.OnParentIndexReuse;
			this.bsm.PostCollisionStep -= this.OnStep;
			this.bsm.StainsCleared -= this.OnStainsCleared;
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000F1E8 File Offset: 0x0000D3E8
	private void OnEnable()
	{
		if (this.parentIndex < 0)
		{
			return;
		}
		this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		if (this.bsm == null)
		{
			return;
		}
		if (this.parentIndex >= this.bsm.parents.Length)
		{
			return;
		}
		if (this.bsm.parents.IsCreated)
		{
			this.bsm.parents[this.parentIndex] = this.GetMatrix();
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000F260 File Offset: 0x0000D460
	private void OnDisable()
	{
		if (this.parentIndex < 0)
		{
			return;
		}
		if (this.bsm == null)
		{
			return;
		}
		if (this.parentIndex >= this.bsm.parents.Length)
		{
			return;
		}
		if (this.bsm.parents.IsCreated)
		{
			this.bsm.parents[this.parentIndex] = float4x4.Scale(0f);
		}
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000F2D8 File Offset: 0x0000D4D8
	public void ClearChildren()
	{
		if (this.bsm == null)
		{
			this.children.Clear();
			return;
		}
		foreach (int num in new HashSet<int>(this.children))
		{
			this.bsm.DeleteBloodstain(num);
		}
		this.children.Clear();
	}

	// Token: 0x04000323 RID: 803
	public int parentIndex;

	// Token: 0x04000324 RID: 804
	public Matrix4x4 matrixAtStep;

	// Token: 0x04000325 RID: 805
	private HashSet<int> children = new HashSet<int>();

	// Token: 0x04000326 RID: 806
	private BloodsplatterManager bsm;
}
