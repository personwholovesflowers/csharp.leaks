using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmazingAssets.CurvedWorld
{
	// Token: 0x0200033C RID: 828
	[ExecuteInEditMode]
	public class CurvedWorldBoundingBox : MonoBehaviour
	{
		// Token: 0x06001548 RID: 5448 RVA: 0x000709B6 File Offset: 0x0006EBB6
		private void OnEnable()
		{
			this.currentScale = -1f;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x000709C4 File Offset: 0x0006EBC4
		private void OnDisable()
		{
			if (this.bIsSkinned)
			{
				if (this.skinnedMeshRenderer != null)
				{
					this.skinnedMeshRenderer.sharedMesh.bounds = this.originalBounds;
					return;
				}
			}
			else if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
			{
				this.meshFilter.sharedMesh.bounds = this.originalBounds;
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00070A38 File Offset: 0x0006EC38
		private void Start()
		{
			if (CurvedWorldBoundingBox.boundsDictionary == null)
			{
				CurvedWorldBoundingBox.boundsDictionary = new Dictionary<int, Bounds>();
			}
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.skinnedMeshRenderer = base.GetComponent<SkinnedMeshRenderer>();
			if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
			{
				this.bIsSkinned = false;
				if (CurvedWorldBoundingBox.boundsDictionary.ContainsKey(this.meshFilter.sharedMesh.GetInstanceID()))
				{
					this.originalBounds = CurvedWorldBoundingBox.boundsDictionary[this.meshFilter.sharedMesh.GetInstanceID()];
				}
				else
				{
					this.originalBounds = this.meshFilter.sharedMesh.bounds;
					CurvedWorldBoundingBox.boundsDictionary.Add(this.meshFilter.sharedMesh.GetInstanceID(), this.originalBounds);
				}
				this.boundingBoxSize = this.originalBounds.size;
				float num = 1f;
				if (this.boundingBoxSize.x > num)
				{
					num = this.boundingBoxSize.x;
				}
				if (this.boundingBoxSize.y > num)
				{
					num = this.boundingBoxSize.y;
				}
				if (this.boundingBoxSize.z > num)
				{
					num = this.boundingBoxSize.z;
				}
				this.boundingBoxSize.x = (this.boundingBoxSize.y = (this.boundingBoxSize.z = num));
			}
			else if (this.skinnedMeshRenderer != null && this.skinnedMeshRenderer.sharedMesh != null)
			{
				this.bIsSkinned = true;
				if (CurvedWorldBoundingBox.boundsDictionary.ContainsKey(this.skinnedMeshRenderer.sharedMesh.GetInstanceID()))
				{
					this.originalBounds = CurvedWorldBoundingBox.boundsDictionary[this.skinnedMeshRenderer.sharedMesh.GetInstanceID()];
				}
				else
				{
					this.originalBounds = this.skinnedMeshRenderer.sharedMesh.bounds;
					CurvedWorldBoundingBox.boundsDictionary.Add(this.skinnedMeshRenderer.sharedMesh.GetInstanceID(), this.originalBounds);
				}
				this.boundingBoxSize = this.originalBounds.size;
				float num2 = 1f;
				if (this.boundingBoxSize.x > num2)
				{
					num2 = this.boundingBoxSize.x;
				}
				if (this.boundingBoxSize.y > num2)
				{
					num2 = this.boundingBoxSize.y;
				}
				if (this.boundingBoxSize.z > num2)
				{
					num2 = this.boundingBoxSize.z;
				}
				this.boundingBoxSize.x = (this.boundingBoxSize.y = (this.boundingBoxSize.z = num2));
			}
			this.currentScale = 0f;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00070CDC File Offset: 0x0006EEDC
		private void Update()
		{
			if (this.currentScale != this.scale)
			{
				if (this.scale < 0f)
				{
					this.scale = 0f;
				}
				this.currentScale = this.scale;
				if (this.bIsSkinned)
				{
					if (this.skinnedMeshRenderer != null)
					{
						this.skinnedMeshRenderer.localBounds = new Bounds(this.skinnedMeshRenderer.localBounds.center, this.boundingBoxSize * this.scale);
						return;
					}
				}
				else if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
				{
					this.meshFilter.sharedMesh.bounds = new Bounds(this.meshFilter.sharedMesh.bounds.center, this.boundingBoxSize * this.scale);
				}
			}
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00070DCC File Offset: 0x0006EFCC
		private void OnDrawGizmos()
		{
			if (this.visualizeInEditor)
			{
				Gizmos.color = Color.yellow;
				if (this.bIsSkinned && this.skinnedMeshRenderer != null && this.skinnedMeshRenderer.sharedMesh != null)
				{
					Gizmos.DrawWireCube(base.transform.TransformPoint(this.skinnedMeshRenderer.localBounds.center), this.boundingBoxSize * this.scale);
					return;
				}
				if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
				{
					Gizmos.DrawWireCube(base.transform.TransformPoint(this.meshFilter.sharedMesh.bounds.center), this.boundingBoxSize * this.scale);
				}
			}
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00070EA6 File Offset: 0x0006F0A6
		private void Reset()
		{
			this.scale = 1f;
			this.RecalculateBounds();
			this.Update();
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00070EC0 File Offset: 0x0006F0C0
		public void RecalculateBounds()
		{
			if (this.bIsSkinned)
			{
				if (this.skinnedMeshRenderer != null)
				{
					this.skinnedMeshRenderer.sharedMesh.RecalculateBounds();
					this.originalBounds = this.skinnedMeshRenderer.sharedMesh.bounds;
					if (CurvedWorldBoundingBox.boundsDictionary != null && CurvedWorldBoundingBox.boundsDictionary.ContainsKey(this.skinnedMeshRenderer.sharedMesh.GetInstanceID()))
					{
						CurvedWorldBoundingBox.boundsDictionary[this.skinnedMeshRenderer.sharedMesh.GetInstanceID()] = this.originalBounds;
						return;
					}
				}
			}
			else if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
			{
				this.meshFilter.sharedMesh.RecalculateBounds();
				this.originalBounds = this.meshFilter.sharedMesh.bounds;
				if (CurvedWorldBoundingBox.boundsDictionary != null && CurvedWorldBoundingBox.boundsDictionary.ContainsKey(this.meshFilter.sharedMesh.GetInstanceID()))
				{
					CurvedWorldBoundingBox.boundsDictionary[this.meshFilter.sharedMesh.GetInstanceID()] = this.originalBounds;
				}
			}
		}

		// Token: 0x0400115C RID: 4444
		public float scale = 1f;

		// Token: 0x0400115D RID: 4445
		private float currentScale;

		// Token: 0x0400115E RID: 4446
		private Vector3 boundingBoxSize;

		// Token: 0x0400115F RID: 4447
		private Bounds originalBounds;

		// Token: 0x04001160 RID: 4448
		private SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x04001161 RID: 4449
		private MeshFilter meshFilter;

		// Token: 0x04001162 RID: 4450
		private bool bIsSkinned;

		// Token: 0x04001163 RID: 4451
		private static Dictionary<int, Bounds> boundsDictionary;

		// Token: 0x04001164 RID: 4452
		public bool visualizeInEditor;
	}
}
