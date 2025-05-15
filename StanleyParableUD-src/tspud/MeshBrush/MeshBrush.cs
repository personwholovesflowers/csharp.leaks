using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace MeshBrush
{
	// Token: 0x02000251 RID: 593
	public class MeshBrush : MonoBehaviour
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0003EC64 File Offset: 0x0003CE64
		public Transform CachedTransform
		{
			get
			{
				if (this.cachedTransform == null)
				{
					this.cachedTransform = base.transform;
				}
				return this.cachedTransform;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0003EC86 File Offset: 0x0003CE86
		public Collider CachedCollider
		{
			get
			{
				if (this.cachedCollider == null)
				{
					this.cachedCollider = base.GetComponent<Collider>();
				}
				return this.cachedCollider;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0003ECA8 File Offset: 0x0003CEA8
		public GameObject Brush
		{
			get
			{
				if (this.brush == null)
				{
					this.CheckBrush();
				}
				return this.brush;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x0003ECC4 File Offset: 0x0003CEC4
		public Transform BrushTransform
		{
			get
			{
				if (this.brushTransform == null)
				{
					this.CheckBrush();
				}
				return this.brushTransform;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0003ECE0 File Offset: 0x0003CEE0
		public Transform HolderObj
		{
			get
			{
				if (this.holderObj == null)
				{
					this.CheckHolder();
				}
				return this.holderObj;
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003ECFC File Offset: 0x0003CEFC
		public void OnValidate()
		{
			this.ValidateKeyBindings();
			this.ValidateRangeLimits();
			if (this.meshes.Count == 0)
			{
				this.meshes.Add(null);
			}
			if (this.layerMask.Length != 32)
			{
				this.layerMask = new bool[32];
				for (int i = this.layerMask.Length - 1; i >= 0; i--)
				{
					this.layerMask[i] = true;
				}
			}
			if (this.layerMask[2])
			{
				this.layerMask[2] = false;
			}
			if (this.radius < 0.01f)
			{
				this.radius = 0.01f;
			}
			this.radius = (float)Math.Round((double)this.radius, 3);
			VectorClampingUtility.ClampVector(ref this.quantityRange, 1f, (float)this.maxQuantityLimit, 1f, (float)this.maxQuantityLimit);
			VectorClampingUtility.ClampVector(ref this.densityRange, 0.1f, this.maxDensityLimit, 0.1f, this.maxDensityLimit);
			this.delay = Mathf.Clamp(this.delay, 0.03f, this.maxDelayLimit);
			this.randomScaleCurveVariation = Mathf.Clamp(this.randomScaleCurveVariation, 0f, 3f);
			VectorClampingUtility.ClampVector(ref this.offsetRange, this.minOffsetLimit, this.maxOffsetLimit, this.minOffsetLimit, this.maxOffsetLimit);
			VectorClampingUtility.ClampVector(ref this.scatteringRange, 0f, 100f, 0f, 100f);
			VectorClampingUtility.ClampVector(ref this.slopeInfluenceRange, 0f, 100f, 0f, 100f);
			VectorClampingUtility.ClampVector(ref this.angleThresholdRange, 1f, 180f, 1f, 180f);
			VectorClampingUtility.ClampVector(ref this.minimumAbsoluteDistanceRange, 0f, this.maxMinimumAbsoluteDistanceLimit, 0f, this.maxMinimumAbsoluteDistanceLimit);
			VectorClampingUtility.ClampVector(ref this.randomScaleRange, 0.01f, this.maxRandomScaleLimit, 0f, this.maxRandomScaleLimit);
			VectorClampingUtility.ClampVector(ref this.randomScaleRangeX, 0.01f, this.maxRandomScaleLimit, 0f, this.maxRandomScaleLimit);
			VectorClampingUtility.ClampVector(ref this.randomScaleRangeY, 0.01f, this.maxRandomScaleLimit, 0f, this.maxRandomScaleLimit);
			VectorClampingUtility.ClampVector(ref this.randomScaleRangeZ, 0.01f, this.maxRandomScaleLimit, 0f, this.maxRandomScaleLimit);
			VectorClampingUtility.ClampVector(ref this.randomRotationRangeY, 0f, 100f, 0f, 100f);
			VectorClampingUtility.ClampVector(ref this.additiveScaleRange, -0.9f, this.maxAdditiveScaleLimit, -0.9f, this.maxAdditiveScaleLimit);
			VectorClampingUtility.ClampVector(ref this.additiveScaleNonUniform, -0.9f, this.maxAdditiveScaleLimit, -0.9f, this.maxAdditiveScaleLimit, -0.9f, this.maxAdditiveScaleLimit);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0003EFB0 File Offset: 0x0003D1B0
		private void ValidateRangeLimits()
		{
			this.maxQuantityLimit = Mathf.Clamp(this.maxQuantityLimit, 1, 1000);
			this.maxDensityLimit = Mathf.Clamp(this.maxDensityLimit, 1f, 1000f);
			this.maxDelayLimit = Mathf.Clamp(this.maxDelayLimit, 1f, 10f);
			this.minOffsetLimit = Mathf.Clamp(this.minOffsetLimit, -1000f, -1f);
			this.maxOffsetLimit = Mathf.Clamp(this.maxOffsetLimit, 1f, 1000f);
			this.maxMinimumAbsoluteDistanceLimit = Mathf.Clamp(this.maxMinimumAbsoluteDistanceLimit, 3f, 1000f);
			this.maxAdditiveScaleLimit = Mathf.Clamp(this.maxAdditiveScaleLimit, 3f, 1000f);
			this.maxRandomScaleLimit = Mathf.Clamp(this.maxRandomScaleLimit, 3f, 1000f);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0003F094 File Offset: 0x0003D294
		private void ValidateKeyBindings()
		{
			if (this.paintKey == KeyCode.None)
			{
				this.paintKey = KeyCode.P;
			}
			if (this.deleteKey == KeyCode.None)
			{
				this.deleteKey = KeyCode.L;
			}
			if (this.randomizeKey == KeyCode.None)
			{
				this.randomizeKey = KeyCode.J;
			}
			if (this.combineKey == KeyCode.None)
			{
				this.combineKey = KeyCode.K;
			}
			if (this.increaseRadiusKey == KeyCode.None)
			{
				this.increaseRadiusKey = KeyCode.O;
			}
			if (this.decreaseRadiusKey == KeyCode.None)
			{
				this.decreaseRadiusKey = KeyCode.I;
			}
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0003F101 File Offset: 0x0003D301
		public void GatherPaintedMeshes()
		{
			this.paintedMeshes = this.HolderObj.GetComponentsInChildren<Transform>().ToList<Transform>();
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0003F11C File Offset: 0x0003D31C
		public void CleanSetOfMeshesToPaint()
		{
			if (this.meshes.Count <= 1)
			{
				return;
			}
			for (int i = this.meshes.Count - 1; i >= 0; i--)
			{
				if (this.meshes[i] == null)
				{
					this.meshes.RemoveAt(i);
				}
			}
			if (this.meshes.Count == 0)
			{
				this.meshes.Add(null);
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0003F18C File Offset: 0x0003D38C
		private void GatherMeshesInsideBrushArea(RaycastHit brushLocation)
		{
			this.paintedMeshesInsideBrushArea.Clear();
			foreach (Transform transform in this.paintedMeshes)
			{
				if (transform != null && transform != this.BrushTransform && transform != this.HolderObj && Vector3.Distance(brushLocation.point, transform.position) < this.radius)
				{
					this.paintedMeshesInsideBrushArea.Add(transform);
				}
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0003F230 File Offset: 0x0003D430
		public void PaintMeshes(RaycastHit brushLocation)
		{
			if (this.nextFeasibleStrokeTime >= Time.realtimeSinceStartup)
			{
				return;
			}
			this.nextFeasibleStrokeTime = Time.realtimeSinceStartup + this.delay;
			this.CheckBrush();
			this.brushStrokeDirection = brushLocation.point - this.lastPaintLocation;
			int num = (this.useDensity ? ((int)(this.radius * this.radius * 3.1415927f * Random.Range(this.densityRange.x, this.densityRange.y))) : ((int)Random.Range(this.quantityRange.x, this.quantityRange.y + 1f)));
			if (num <= 0)
			{
				num = 1;
			}
			if (this.useOverlapFilter)
			{
				this.GatherMeshesInsideBrushArea(brushLocation);
			}
			bool flag = false;
			for (int i = num; i > 0; i--)
			{
				float num2 = this.radius * 0.01f * Random.Range(this.scatteringRange.x, this.scatteringRange.y);
				this.brushTransform.position = brushLocation.point + brushLocation.normal * 0.5f;
				this.brushTransform.rotation = Quaternion.LookRotation(brushLocation.normal);
				this.brushTransform.up = this.brushTransform.forward;
				if (num > 1)
				{
					this.brushTransform.Translate(Random.Range(-Random.insideUnitCircle.x * num2, Random.insideUnitCircle.x * num2), 0f, Random.Range(-Random.insideUnitCircle.y * num2, Random.insideUnitCircle.y * num2), Space.Self);
				}
				RaycastHit raycastHit;
				if (this.globalPaintingMode ? Physics.Raycast(new Ray(this.brushTransform.position, -brushLocation.normal), out raycastHit, 2.5f) : this.CachedCollider.Raycast(new Ray(this.brushTransform.position, -brushLocation.normal), out raycastHit, 2.5f))
				{
					float num3 = (this.useSlopeFilter ? Vector3.Angle(raycastHit.normal, this.manualReferenceVectorSampling ? this.slopeReferenceVector : Vector3.up) : (this.inverseSlopeFilter ? 180f : 0f));
					if ((this.inverseSlopeFilter ? (num3 > Random.Range(this.angleThresholdRange.x, this.angleThresholdRange.y)) : (num3 < Random.Range(this.angleThresholdRange.x, this.angleThresholdRange.y))) && (!this.useOverlapFilter || !this.CheckOverlap(raycastHit.point)))
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.meshes[Random.Range(0, this.meshes.Count)]);
						if (gameObject == null)
						{
							if (!flag)
							{
								flag = true;
								Debug.LogError("MeshBrush: one or more fields in the set of meshes to paint is null. Please assign all fields before painting (or remove empty ones).");
							}
						}
						else
						{
							if (this.autoIgnoreRaycast)
							{
								gameObject.layer = 2;
							}
							Transform transform = gameObject.transform;
							this.OrientPaintedMesh(transform, raycastHit);
							if (Mathf.Abs(this.offsetRange.x) > 1E-45f || Mathf.Abs(this.offsetRange.y) > 1E-45f)
							{
								MeshTransformationUtility.ApplyMeshOffset(transform, Random.Range(this.offsetRange.x, this.offsetRange.y), brushLocation.normal);
							}
							if (this.uniformRandomScale)
							{
								if (Mathf.Abs(this.randomScaleRange.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRange.y - 1f) > 1E-45f)
								{
									MeshTransformationUtility.ApplyRandomScale(transform, this.randomScaleRange);
								}
							}
							else if (Mathf.Abs(this.randomScaleRangeX.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeX.y - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeY.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeY.y - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeZ.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeZ.y - 1f) > 1E-45f)
							{
								MeshTransformationUtility.ApplyRandomScale(transform, this.randomScaleRangeX, this.randomScaleRangeY, this.randomScaleRangeZ);
							}
							transform.localScale *= Mathf.Abs(this.randomScaleCurve.Evaluate(Vector3.Distance(transform.position, brushLocation.point) / this.radius) + Random.Range(-this.randomScaleCurveVariation, this.randomScaleCurveVariation));
							if (this.uniformAdditiveScale)
							{
								if (Mathf.Abs(this.additiveScaleRange.x) > 1E-45f || Mathf.Abs(this.additiveScaleRange.y) > 1E-45f)
								{
									MeshTransformationUtility.AddConstantScale(transform, this.additiveScaleRange);
								}
							}
							else if (Mathf.Abs(this.additiveScaleNonUniform.x) > 1E-45f || Mathf.Abs(this.additiveScaleNonUniform.y) > 1E-45f || Mathf.Abs(this.additiveScaleNonUniform.z) > 1E-45f)
							{
								MeshTransformationUtility.AddConstantScale(transform, this.additiveScaleNonUniform.x, this.additiveScaleNonUniform.y, this.additiveScaleNonUniform.z);
							}
							if (this.randomRotationRangeX.x > 0f || this.randomRotationRangeX.y > 0f || this.randomRotationRangeY.x > 0f || this.randomRotationRangeY.y > 0f || this.randomRotationRangeZ.x > 0f || this.randomRotationRangeZ.y > 0f)
							{
								MeshTransformationUtility.ApplyRandomRotation(transform, Random.Range(this.randomRotationRangeX.x, this.randomRotationRangeX.y), Random.Range(this.randomRotationRangeY.x, this.randomRotationRangeY.y), Random.Range(this.randomRotationRangeZ.x, this.randomRotationRangeZ.y));
							}
							transform.parent = this.HolderObj;
							gameObject.isStatic |= this.autoStatic;
							this.paintedMeshes.Add(transform);
						}
					}
				}
			}
			this.lastPaintLocation = brushLocation.point;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0003F8C0 File Offset: 0x0003DAC0
		public void RandomizeMeshes(RaycastHit brushLocation)
		{
			if (this.nextFeasibleStrokeTime >= Time.realtimeSinceStartup)
			{
				return;
			}
			this.nextFeasibleStrokeTime = Time.realtimeSinceStartup + this.delay;
			this.GatherMeshesInsideBrushArea(brushLocation);
			for (int i = this.paintedMeshesInsideBrushArea.Count - 1; i >= 0; i--)
			{
				Transform transform = this.paintedMeshesInsideBrushArea[i];
				if (transform != null)
				{
					if (this.positionBrushRandomizer)
					{
						float num = this.radius * 0.01f * Random.Range(this.scatteringRange.x, this.scatteringRange.y);
						this.brushTransform.position = brushLocation.point + brushLocation.normal * 0.5f;
						this.brushTransform.rotation = Quaternion.LookRotation(brushLocation.normal);
						this.brushTransform.up = this.brushTransform.forward;
						this.brushTransform.Translate(Random.Range(-Random.insideUnitCircle.x * num, Random.insideUnitCircle.x * num), 0f, Random.Range(-Random.insideUnitCircle.y * num, Random.insideUnitCircle.y * num), Space.Self);
						RaycastHit raycastHit;
						if (this.globalPaintingMode ? Physics.Raycast(new Ray(this.brushTransform.position, -brushLocation.normal), out raycastHit, 2.5f) : this.CachedCollider.Raycast(new Ray(this.brushTransform.position, -brushLocation.normal), out raycastHit, 2.5f))
						{
							this.OrientPaintedMesh(transform, raycastHit);
						}
						if (Mathf.Abs(this.offsetRange.x) > 1E-45f || Mathf.Abs(this.offsetRange.y) > 1E-45f)
						{
							MeshTransformationUtility.ApplyMeshOffset(transform, Random.Range(this.offsetRange.x, this.offsetRange.y), brushLocation.normal);
						}
					}
					if (this.rotationBrushRandomizer && (this.randomRotationRangeX.x > 0f || this.randomRotationRangeX.y > 0f || this.randomRotationRangeY.x > 0f || this.randomRotationRangeY.y > 0f || this.randomRotationRangeZ.x > 0f || this.randomRotationRangeZ.y > 0f))
					{
						MeshTransformationUtility.ApplyRandomRotation(transform, Random.Range(this.randomRotationRangeX.x, this.randomRotationRangeX.y), Random.Range(this.randomRotationRangeY.x, this.randomRotationRangeY.y), Random.Range(this.randomRotationRangeZ.x, this.randomRotationRangeZ.y));
					}
					if (this.scaleBrushRandomizer)
					{
						if (this.uniformRandomScale)
						{
							if (Mathf.Abs(this.randomScaleRange.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRange.y - 1f) > 1E-45f)
							{
								MeshTransformationUtility.ApplyRandomScale(transform, this.randomScaleRange);
							}
						}
						else if (Mathf.Abs(this.randomScaleRangeX.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeX.y - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeY.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeY.y - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeZ.x - 1f) > 1E-45f || Mathf.Abs(this.randomScaleRangeZ.y - 1f) > 1E-45f)
						{
							MeshTransformationUtility.ApplyRandomScale(transform, this.randomScaleRangeX, this.randomScaleRangeY, this.randomScaleRangeZ);
						}
						transform.localScale *= Mathf.Abs(this.randomScaleCurve.Evaluate(Vector3.Distance(transform.position, brushLocation.point) / this.radius) + Random.Range(-this.randomScaleCurveVariation, this.randomScaleCurveVariation));
					}
				}
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0003FD08 File Offset: 0x0003DF08
		public void DeleteMeshes(RaycastHit brushLocation)
		{
			if (this.nextFeasibleStrokeTime >= Time.realtimeSinceStartup)
			{
				return;
			}
			this.nextFeasibleStrokeTime = Time.realtimeSinceStartup + this.delay;
			this.GatherMeshesInsideBrushArea(brushLocation);
			for (int i = this.paintedMeshesInsideBrushArea.Count - 1; i >= 0; i--)
			{
				this.paintedMeshes.Remove(this.paintedMeshesInsideBrushArea[i]);
				GameObject gameObject = this.paintedMeshesInsideBrushArea[i].gameObject;
				if (gameObject.transform.parent == this.HolderObj.transform)
				{
					Object.Destroy(gameObject);
				}
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0003FDA4 File Offset: 0x0003DFA4
		public void CombineMeshes(RaycastHit brushLocation)
		{
			if (this.nextFeasibleStrokeTime >= Time.realtimeSinceStartup)
			{
				return;
			}
			this.nextFeasibleStrokeTime = Time.realtimeSinceStartup + this.delay;
			this.GatherMeshesInsideBrushArea(brushLocation);
			if (this.paintedMeshesInsideBrushArea.Count > 0)
			{
				this.HolderObj.GetComponent<MeshBrushParent>().CombinePaintedMeshes(this.autoSelectOnCombine, this.paintedMeshesInsideBrushArea.Select((Transform mesh) => mesh.GetComponent<MeshFilter>()).ToArray<MeshFilter>());
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0003FE2B File Offset: 0x0003E02B
		public void SampleReferenceVector(Vector3 referenceVector, Vector3 sampleLocation)
		{
			this.slopeReferenceVector = referenceVector;
			this.slopeReferenceVectorSampleLocation = sampleLocation;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0003FE3C File Offset: 0x0003E03C
		private void OrientPaintedMesh(Transform targetTransform, RaycastHit targetLocation)
		{
			targetTransform.position = targetLocation.point;
			targetTransform.rotation = Quaternion.LookRotation(targetLocation.normal);
			Vector3 vector = Vector3.Lerp(this.yAxisTangent ? targetTransform.up : Vector3.up, targetTransform.forward, Random.Range(this.slopeInfluenceRange.x, this.slopeInfluenceRange.y) * 0.01f);
			Vector3 vector2 = ((this.strokeAlignment && this.brushStrokeDirection != Vector3.zero && this.lastPaintLocation != Vector3.zero) ? this.brushStrokeDirection : targetTransform.forward);
			Vector3.OrthoNormalize(ref vector, ref vector2);
			targetTransform.rotation = Quaternion.LookRotation(vector2, vector);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0003FEFC File Offset: 0x0003E0FC
		private bool CheckOverlap(Vector3 objPos)
		{
			if (this.paintedMeshes == null || this.paintedMeshes.Count < 1)
			{
				return false;
			}
			foreach (Transform transform in this.paintedMeshes)
			{
				if (transform != null && transform != this.BrushTransform && transform != this.HolderObj && Vector3.Distance(objPos, transform.position) < Random.Range(this.minimumAbsoluteDistanceRange.x, this.minimumAbsoluteDistanceRange.y))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0003FFB8 File Offset: 0x0003E1B8
		private void CheckHolder()
		{
			MeshBrushParent[] componentsInChildren = base.GetComponentsInChildren<MeshBrushParent>();
			if (componentsInChildren.Length != 0)
			{
				this.holderObj = null;
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i] != null && string.CompareOrdinal(componentsInChildren[i].name, this.groupName) == 0)
					{
						this.holderObj = componentsInChildren[i].transform;
					}
				}
				if (this.holderObj == null)
				{
					this.CreateHolder();
					return;
				}
			}
			else
			{
				this.CreateHolder();
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0004002E File Offset: 0x0003E22E
		private void CheckBrush()
		{
			this.CheckHolder();
			this.brushTransform = this.holderObj.Find("Brush");
			if (this.brushTransform == null)
			{
				this.CreateBrush();
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00040060 File Offset: 0x0003E260
		private void CreateHolder()
		{
			GameObject gameObject = new GameObject(this.groupName);
			gameObject.AddComponent<MeshBrushParent>();
			gameObject.transform.rotation = this.CachedTransform.rotation;
			gameObject.transform.parent = this.CachedTransform;
			gameObject.transform.localPosition = Vector3.zero;
			this.holderObj = gameObject.transform;
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x000400C4 File Offset: 0x0003E2C4
		private void CreateBrush()
		{
			this.brush = new GameObject("Brush");
			this.brushTransform = this.brush.transform;
			this.brushTransform.position = this.CachedTransform.position;
			this.brushTransform.parent = this.holderObj;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00040119 File Offset: 0x0003E319
		public void ResetKeyBindings()
		{
			this.paintKey = KeyCode.P;
			this.deleteKey = KeyCode.L;
			this.combineKey = KeyCode.K;
			this.randomizeKey = KeyCode.J;
			this.increaseRadiusKey = KeyCode.O;
			this.decreaseRadiusKey = KeyCode.I;
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0004014C File Offset: 0x0003E34C
		public void ResetSlopeSettings()
		{
			this.slopeInfluenceRange = new Vector2(95f, 100f);
			this.angleThresholdRange = new Vector2(25f, 30f);
			this.useSlopeFilter = false;
			this.inverseSlopeFilter = false;
			this.manualReferenceVectorSampling = false;
			this.showReferenceVectorInSceneView = true;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000401A0 File Offset: 0x0003E3A0
		public void ResetRandomizers()
		{
			this.randomScaleRange = Vector2.one;
			this.randomScaleRangeX = (this.randomScaleRangeY = (this.randomScaleRangeZ = Vector2.one));
			this.randomScaleCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
			this.randomScaleCurveVariation = 0f;
			this.randomRotationRangeY = new Vector2(0f, 5f);
			this.randomRotationRangeX = (this.randomRotationRangeZ = Vector2.zero);
			this.positionBrushRandomizer = false;
			this.rotationBrushRandomizer = true;
			this.scaleBrushRandomizer = true;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0004023D File Offset: 0x0003E43D
		public void ResetAdditiveScale()
		{
			this.uniformRandomScale = true;
			this.additiveScaleRange = Vector2.zero;
			this.additiveScaleNonUniform = Vector3.zero;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0004025C File Offset: 0x0003E45C
		public void ResetOverlapFilterSettings()
		{
			this.useOverlapFilter = false;
			this.minimumAbsoluteDistanceRange = new Vector2(0.25f, 0.5f);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0004027C File Offset: 0x0003E47C
		public XDocument SaveTemplate(string filePath)
		{
			object[] array = new object[1];
			int num = 0;
			XName xname = "meshBrushTemplate";
			object[] array2 = new object[13];
			array2[0] = new XAttribute("version", 2f);
			array2[1] = new XElement("instance", new object[]
			{
				new XElement("active", this.active),
				new XElement("name", this.groupName),
				new XElement("stats", this.stats),
				new XElement("lockSceneView", this.lockSceneView)
			});
			array2[2] = new XElement("meshes", new XElement("ui", new object[]
			{
				new XElement("style", this.classicUI ? "classic" : "modern"),
				new XElement("iconSize", this.previewIconSize)
			}));
			array2[3] = new XElement("keyBindings", new object[]
			{
				new XElement("paint", this.paintKey),
				new XElement("delete", this.deleteKey),
				new XElement("combine", this.combineKey),
				new XElement("randomize", this.randomizeKey),
				new XElement("increaseRadius", this.increaseRadiusKey),
				new XElement("decreaseRadius", this.decreaseRadiusKey)
			});
			array2[4] = new XElement("brush", new object[]
			{
				new XElement("radius", this.radius),
				new XElement("color", new object[]
				{
					new XElement("r", this.color.r),
					new XElement("g", this.color.g),
					new XElement("b", this.color.b),
					new XElement("a", this.color.a)
				}),
				new XElement("quantity", new object[]
				{
					new XElement("min", this.quantityRange.x),
					new XElement("max", this.quantityRange.y)
				}),
				new XElement("useDensity", this.useDensity),
				new XElement("density", new object[]
				{
					new XElement("min", this.densityRange.x),
					new XElement("max", this.densityRange.y)
				}),
				new XElement("offset", new object[]
				{
					new XElement("min", this.offsetRange.x),
					new XElement("max", this.offsetRange.y)
				}),
				new XElement("scattering", new object[]
				{
					new XElement("min", this.scatteringRange.x),
					new XElement("max", this.scatteringRange.y)
				}),
				new XElement("delay", this.delay),
				new XElement("yAxisTangent", this.yAxisTangent),
				new XElement("strokeAlignment", this.strokeAlignment)
			});
			array2[5] = new XElement("slopes", new object[]
			{
				new XElement("slopeInfluence", new object[]
				{
					new XElement("min", this.slopeInfluenceRange.x),
					new XElement("max", this.slopeInfluenceRange.y)
				}),
				new XElement("slopeFilter", new object[]
				{
					new XElement("enabled", this.useSlopeFilter),
					new XElement("inverse", this.inverseSlopeFilter),
					new XElement("angleThreshold", new object[]
					{
						new XElement("min", this.angleThresholdRange.x),
						new XElement("max", this.angleThresholdRange.y)
					}),
					new XElement("manualReferenceVectorSampling", this.manualReferenceVectorSampling),
					new XElement("showReferenceVectorInSceneView", this.showReferenceVectorInSceneView),
					new XElement("referenceVector", new object[]
					{
						new XElement("x", this.slopeReferenceVector.x),
						new XElement("y", this.slopeReferenceVector.y),
						new XElement("z", this.slopeReferenceVector.z)
					}),
					new XElement("referenceVectorSampleLocation", new object[]
					{
						new XElement("x", this.slopeReferenceVectorSampleLocation.x),
						new XElement("y", this.slopeReferenceVectorSampleLocation.y),
						new XElement("z", this.slopeReferenceVectorSampleLocation.z)
					})
				})
			});
			int num2 = 6;
			XName xname2 = "randomizers";
			object[] array3 = new object[3];
			int num3 = 0;
			XName xname3 = "scale";
			object[] array4 = new object[4];
			array4[0] = new XElement("scaleUniformly", this.uniformRandomScale);
			array4[1] = new XElement("uniform", new object[]
			{
				new XElement("min", this.randomScaleRange.x),
				new XElement("max", this.randomScaleRange.y)
			});
			array4[2] = new XElement("nonUniform", new object[]
			{
				new XElement("x", new object[]
				{
					new XElement("min", this.randomScaleRangeX.x),
					new XElement("max", this.randomScaleRangeX.y)
				}),
				new XElement("y", new object[]
				{
					new XElement("min", this.randomScaleRangeY.x),
					new XElement("max", this.randomScaleRangeY.y)
				}),
				new XElement("z", new object[]
				{
					new XElement("min", this.randomScaleRangeZ.x),
					new XElement("max", this.randomScaleRangeZ.y)
				})
			});
			int num4 = 3;
			XName xname4 = "curve";
			object[] array5 = new object[2];
			array5[0] = new XElement("variation", this.randomScaleCurveVariation);
			array5[1] = new XElement("keys", this.randomScaleCurve.keys.Select((Keyframe key) => new XElement("key", new object[]
			{
				new XElement("time", key.time),
				new XElement("value", key.value),
				new XElement("inTangent", key.inTangent),
				new XElement("outTangent", key.outTangent)
			})));
			array4[num4] = new XElement(xname4, array5);
			array3[num3] = new XElement(xname3, array4);
			array3[1] = new XElement("rotation", new object[]
			{
				new XElement("x", new object[]
				{
					new XElement("min", this.randomRotationRangeX.x),
					new XElement("max", this.randomRotationRangeX.y)
				}),
				new XElement("y", new object[]
				{
					new XElement("min", this.randomRotationRangeY.x),
					new XElement("max", this.randomRotationRangeY.y)
				}),
				new XElement("z", new object[]
				{
					new XElement("min", this.randomRotationRangeZ.x),
					new XElement("max", this.randomRotationRangeZ.y)
				})
			});
			array3[2] = new XElement("randomizerBrush", new object[]
			{
				new XElement("position", this.positionBrushRandomizer),
				new XElement("rotation", this.rotationBrushRandomizer),
				new XElement("scale", this.scaleBrushRandomizer)
			});
			array2[num2] = new XElement(xname2, array3);
			array2[7] = new XElement("overlapFilter", new object[]
			{
				new XElement("enabled", this.useOverlapFilter),
				new XElement("minimumAbsoluteDistance", new object[]
				{
					new XElement("min", this.minimumAbsoluteDistanceRange.x),
					new XElement("max", this.minimumAbsoluteDistanceRange.y)
				})
			});
			array2[8] = new XElement("additiveScale", new object[]
			{
				new XElement("scaleUniformly", this.uniformAdditiveScale),
				new XElement("uniform", new object[]
				{
					new XElement("min", this.additiveScaleRange.x),
					new XElement("max", this.additiveScaleRange.y)
				}),
				new XElement("nonUniform", new object[]
				{
					new XElement("x", this.additiveScaleNonUniform.x),
					new XElement("y", this.additiveScaleNonUniform.y),
					new XElement("z", this.additiveScaleNonUniform.z)
				})
			});
			array2[9] = new XElement("optimization", new object[]
			{
				new XElement("autoIgnoreRaycast", this.autoIgnoreRaycast),
				new XElement("autoSelectOnCombine", this.autoSelectOnCombine),
				new XElement("autoStatic", this.autoStatic)
			});
			array2[10] = new XElement("rangeLimits", new object[]
			{
				new XElement("quantity", new XElement("max", this.maxQuantityLimit)),
				new XElement("density", new XElement("max", this.maxDensityLimit)),
				new XElement("offset", new object[]
				{
					new XElement("min", this.minOffsetLimit),
					new XElement("max", this.maxOffsetLimit)
				}),
				new XElement("delay", new XElement("max", this.maxDelayLimit)),
				new XElement("minimumAbsoluteDistance", new XElement("max", this.maxMinimumAbsoluteDistanceLimit)),
				new XElement("randomScale", new XElement("max", this.maxRandomScaleLimit)),
				new XElement("additiveScale", new XElement("max", this.maxAdditiveScaleLimit))
			});
			array2[11] = new XElement("inspectorFoldouts", new object[]
			{
				new XElement("help", this.helpFoldout),
				new XElement("templatesHelp", this.helpTemplatesFoldout),
				new XElement("generalUsageHelp", this.helpGeneralUsageFoldout),
				new XElement("optimizationHelp", this.helpOptimizationFoldout),
				new XElement("meshes", this.meshesFoldout),
				new XElement("templates", this.templatesFoldout),
				new XElement("keyBindings", this.keyBindingsFoldout),
				new XElement("brush", this.brushFoldout),
				new XElement("slopes", this.slopesFoldout),
				new XElement("randomizers", this.randomizersFoldout),
				new XElement("overlapFilter", this.overlapFilterFoldout),
				new XElement("additiveScale", this.additiveScaleFoldout),
				new XElement("optimization", this.optimizationFoldout)
			});
			int num5 = 12;
			XName xname5 = "globalPaintingMode";
			object[] array6 = new object[2];
			array6[0] = new XElement("enabled", this.globalPaintingMode);
			array6[1] = new XElement("layerMask", this.layerMask.Select((bool layer, int index) => new XElement("layer", new object[]
			{
				new XAttribute("index", index),
				layer
			})));
			array2[num5] = new XElement(xname5, array6);
			array[num] = new XElement(xname, array2);
			XDocument xdocument = new XDocument(array);
			xdocument.Save(filePath);
			return xdocument;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0004130C File Offset: 0x0003F50C
		public bool LoadTemplate(string filePath)
		{
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				Debug.LogError("MeshBrush: the specified template file path is invalid or does not exist! Cancelling loading procedure...");
				return false;
			}
			XDocument xdocument = XDocument.Load(filePath);
			if (xdocument == null || xdocument.Root == null)
			{
				Debug.LogError("MeshBrush: the specified template file couldn't be loaded.");
				return false;
			}
			float num = 2f;
			if (!float.TryParse(xdocument.Root.FirstAttribute.Value, out num))
			{
				Debug.LogWarning("MeshBrush: The template you just loaded doesn't seem to contain a MeshBrush version number. Loading procedure might yield unpredictable results in cross-version situations. File path: " + filePath);
			}
			foreach (XElement xelement in xdocument.Root.Elements())
			{
				string text = xelement.Name.LocalName;
				uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num2 <= 1693723192U)
				{
					if (num2 <= 386306721U)
					{
						if (num2 != 193386898U)
						{
							if (num2 != 363276849U)
							{
								if (num2 != 386306721U)
								{
									continue;
								}
								if (!(text == "slopes"))
								{
									continue;
								}
								goto IL_0928;
							}
							else
							{
								if (!(text == "additiveScale"))
								{
									continue;
								}
								goto IL_12A4;
							}
						}
						else
						{
							if (!(text == "instance"))
							{
								continue;
							}
							using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									XElement xelement2 = enumerator2.Current;
									text = xelement2.Name.LocalName;
									if (!(text == "active"))
									{
										if (!(text == "name"))
										{
											if (!(text == "stats"))
											{
												if (text == "lockSceneView")
												{
													this.lockSceneView = string.CompareOrdinal(xelement2.Value, "true") == 0;
												}
											}
											else
											{
												this.stats = string.CompareOrdinal(xelement2.Value, "true") == 0;
											}
										}
										else
										{
											this.groupName = xelement2.Value;
										}
									}
									else
									{
										this.active = string.CompareOrdinal(xelement2.Value, "true") == 0;
									}
								}
								continue;
							}
						}
					}
					else if (num2 != 938501116U)
					{
						if (num2 != 1351500805U)
						{
							if (num2 != 1693723192U)
							{
								continue;
							}
							if (!(text == "meshes"))
							{
								continue;
							}
						}
						else
						{
							if (!(text == "globalPaintingMode"))
							{
								continue;
							}
							goto IL_1AC3;
						}
					}
					else
					{
						if (!(text == "inspectorFoldouts"))
						{
							continue;
						}
						goto IL_1704;
					}
					using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							XElement xelement3 = enumerator2.Current;
							text = xelement3.Name.LocalName;
							if (text == "ui")
							{
								this.classicUI = string.CompareOrdinal(xelement3.Element("style").Value, "classic") == 0;
								this.previewIconSize = float.Parse(xelement3.Element("iconSize").Value);
							}
						}
						continue;
					}
				}
				else if (num2 <= 2670881530U)
				{
					if (num2 != 1955283044U)
					{
						if (num2 != 2413813947U)
						{
							if (num2 != 2670881530U)
							{
								continue;
							}
							if (!(text == "overlapFilter"))
							{
								continue;
							}
							goto IL_11E3;
						}
						else
						{
							if (!(text == "randomizers"))
							{
								continue;
							}
							goto IL_0C69;
						}
					}
					else
					{
						if (!(text == "optimization"))
						{
							continue;
						}
						goto IL_13D7;
					}
				}
				else if (num2 != 3653235718U)
				{
					if (num2 != 3862843026U)
					{
						if (num2 != 4207177203U)
						{
							continue;
						}
						if (!(text == "brush"))
						{
							continue;
						}
						goto IL_0554;
					}
					else if (!(text == "keyBindings"))
					{
						continue;
					}
				}
				else
				{
					if (!(text == "rangeLimits"))
					{
						continue;
					}
					goto IL_149B;
				}
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement4 = enumerator2.Current;
						text = xelement4.Name.LocalName;
						if (!(text == "paint"))
						{
							if (!(text == "delete"))
							{
								if (!(text == "combine"))
								{
									if (!(text == "randomize"))
									{
										if (!(text == "increaseRadius"))
										{
											if (text == "decreaseRadius")
											{
												this.decreaseRadiusKey = (KeyCode)Enum.Parse(typeof(KeyCode), xelement4.Value);
											}
										}
										else
										{
											this.increaseRadiusKey = (KeyCode)Enum.Parse(typeof(KeyCode), xelement4.Value);
										}
									}
									else
									{
										this.randomizeKey = (KeyCode)Enum.Parse(typeof(KeyCode), xelement4.Value);
									}
								}
								else
								{
									this.combineKey = (KeyCode)Enum.Parse(typeof(KeyCode), xelement4.Value);
								}
							}
							else
							{
								this.deleteKey = (KeyCode)Enum.Parse(typeof(KeyCode), xelement4.Value);
							}
						}
						else
						{
							this.paintKey = (KeyCode)Enum.Parse(typeof(KeyCode), xelement4.Value);
						}
					}
					continue;
				}
				IL_0554:
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement5 = enumerator2.Current;
						text = xelement5.Name.LocalName;
						num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num2 <= 1054015406U)
						{
							if (num2 <= 348705738U)
							{
								if (num2 != 230313139U)
								{
									if (num2 == 348705738U)
									{
										if (text == "offset")
										{
											this.offsetRange = new Vector2(float.Parse(xelement5.Element("min").Value), float.Parse(xelement5.Element("max").Value));
										}
									}
								}
								else if (text == "radius")
								{
									this.radius = float.Parse(xelement5.Value);
								}
							}
							else if (num2 != 996569730U)
							{
								if (num2 != 1031692888U)
								{
									if (num2 == 1054015406U)
									{
										if (text == "strokeAlignment")
										{
											this.strokeAlignment = string.CompareOrdinal(xelement5.Value, "true") == 0;
										}
									}
								}
								else if (text == "color")
								{
									this.color = new Color(float.Parse(xelement5.Element("r").Value), float.Parse(xelement5.Element("g").Value), float.Parse(xelement5.Element("b").Value), float.Parse(xelement5.Element("a").Value));
								}
							}
							else if (text == "yAxisTangent")
							{
								this.yAxisTangent = string.CompareOrdinal(xelement5.Value, "true") == 0;
							}
						}
						else if (num2 <= 1322381784U)
						{
							if (num2 != 1265799128U)
							{
								if (num2 == 1322381784U)
								{
									if (text == "delay")
									{
										this.delay = float.Parse(xelement5.Value);
									}
								}
							}
							else if (text == "quantity")
							{
								this.quantityRange = new Vector2(float.Parse(xelement5.Element("min").Value), float.Parse(xelement5.Element("max").Value));
							}
						}
						else if (num2 != 1924728219U)
						{
							if (num2 != 2567109148U)
							{
								if (num2 == 2799640479U)
								{
									if (text == "scattering")
									{
										this.scatteringRange = new Vector2(float.Parse(xelement5.Element("min").Value), float.Parse(xelement5.Element("max").Value));
									}
								}
							}
							else if (text == "useDensity")
							{
								this.useDensity = string.CompareOrdinal(xelement5.Value, "true") == 0;
							}
						}
						else if (text == "density")
						{
							this.densityRange = new Vector2(float.Parse(xelement5.Element("min").Value), float.Parse(xelement5.Element("max").Value));
						}
					}
					continue;
				}
				IL_0928:
				using (IEnumerator<XElement> enumerator2 = xelement.Descendants().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement6 = enumerator2.Current;
						text = xelement6.Name.LocalName;
						num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num2 <= 2320977079U)
						{
							if (num2 <= 1067638908U)
							{
								if (num2 != 49525662U)
								{
									if (num2 == 1067638908U)
									{
										if (text == "referenceVectorSampleLocation")
										{
											this.slopeReferenceVectorSampleLocation = new Vector3(float.Parse(xelement6.Element("x").Value), float.Parse(xelement6.Element("y").Value), float.Parse(xelement6.Element("z").Value));
										}
									}
								}
								else if (text == "enabled")
								{
									this.useSlopeFilter = string.CompareOrdinal(xelement6.Value, "true") == 0;
								}
							}
							else if (num2 != 1458647825U)
							{
								if (num2 == 2320977079U)
								{
									if (text == "slopeInfluence")
									{
										this.slopeInfluenceRange = new Vector2(float.Parse(xelement6.Element("min").Value), float.Parse(xelement6.Element("max").Value));
									}
								}
							}
							else if (text == "referenceVector")
							{
								this.slopeReferenceVector = new Vector3(float.Parse(xelement6.Element("x").Value), float.Parse(xelement6.Element("y").Value), float.Parse(xelement6.Element("z").Value));
							}
						}
						else if (num2 <= 2986472067U)
						{
							if (num2 != 2660031338U)
							{
								if (num2 == 2986472067U)
								{
									if (text == "inverse")
									{
										this.inverseSlopeFilter = string.CompareOrdinal(xelement6.Value, "true") == 0;
									}
								}
							}
							else if (text == "showReferenceVectorInSceneView")
							{
								this.showReferenceVectorInSceneView = string.CompareOrdinal(xelement6.Value, "true") == 0;
							}
						}
						else if (num2 != 3432447995U)
						{
							if (num2 == 3727491022U)
							{
								if (text == "manualReferenceVectorSampling")
								{
									this.manualReferenceVectorSampling = string.CompareOrdinal(xelement6.Value, "true") == 0;
								}
							}
						}
						else if (text == "angleThreshold")
						{
							this.angleThresholdRange = new Vector2(float.Parse(xelement6.Element("min").Value), float.Parse(xelement6.Element("max").Value));
						}
					}
					continue;
				}
				IL_0C69:
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement7 = enumerator2.Current;
						text = xelement7.Name.LocalName;
						if (!(text == "scale"))
						{
							if (!(text == "rotation"))
							{
								if (!(text == "randomizerBrush"))
								{
									continue;
								}
								XElement xelement8 = xelement7.Element("position");
								if (xelement8 != null)
								{
									this.positionBrushRandomizer = string.CompareOrdinal(xelement8.Value, "true") == 0;
								}
								xelement8 = xelement7.Element("rotation");
								if (xelement8 != null)
								{
									this.rotationBrushRandomizer = string.CompareOrdinal(xelement8.Value, "true") == 0;
								}
								xelement8 = xelement7.Element("scale");
								if (xelement8 != null)
								{
									this.scaleBrushRandomizer = string.CompareOrdinal(xelement8.Value, "true") == 0;
									continue;
								}
								continue;
							}
						}
						else
						{
							using (IEnumerator<XElement> enumerator3 = xelement7.Descendants().GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									XElement xelement9 = enumerator3.Current;
									text = xelement9.Name.LocalName;
									num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
									if (num2 <= 3728169555U)
									{
										if (num2 != 266295870U)
										{
											if (num2 != 2245370192U)
											{
												if (num2 == 3728169555U)
												{
													if (text == "uniform")
													{
														this.randomScaleRange = new Vector2(float.Parse(xelement9.Element("min").Value), float.Parse(xelement9.Element("max").Value));
													}
												}
											}
											else if (text == "scaleUniformly")
											{
												this.uniformRandomScale = string.CompareOrdinal(xelement9.Value, "true") == 0;
											}
										}
										else if (text == "variation")
										{
											this.randomScaleCurveVariation = float.Parse(xelement9.Value);
										}
									}
									else if (num2 <= 4228665076U)
									{
										if (num2 != 4182378701U)
										{
											if (num2 == 4228665076U)
											{
												if (text == "y")
												{
													this.randomScaleRangeY = new Vector2(float.Parse(xelement9.Element("min").Value), float.Parse(xelement9.Element("max").Value));
												}
											}
										}
										else if (text == "keys")
										{
											this.randomScaleCurve = new AnimationCurve((from key in xelement9.Descendants("key")
												select new Keyframe(float.Parse(key.Element("time").Value), float.Parse(key.Element("value").Value), float.Parse(key.Element("inTangent").Value), float.Parse(key.Element("outTangent").Value))).ToArray<Keyframe>());
										}
									}
									else if (num2 != 4245442695U)
									{
										if (num2 == 4278997933U)
										{
											if (text == "z")
											{
												this.randomScaleRangeZ = new Vector2(float.Parse(xelement9.Element("min").Value), float.Parse(xelement9.Element("max").Value));
											}
										}
									}
									else if (text == "x")
									{
										this.randomScaleRangeX = new Vector2(float.Parse(xelement9.Element("min").Value), float.Parse(xelement9.Element("max").Value));
									}
								}
								continue;
							}
						}
						if (num < 2f)
						{
							if (string.CompareOrdinal(xelement7.Parent.Name.LocalName, "randomizerBrush") != 0)
							{
								this.randomRotationRangeY = new Vector2(float.Parse(xelement7.Element("min").Value), float.Parse(xelement7.Element("max").Value));
							}
						}
						else if (string.CompareOrdinal(xelement7.Parent.Name.LocalName, "randomizerBrush") != 0)
						{
							XElement xelement10 = xelement7.Element("x");
							this.randomRotationRangeX = new Vector2(float.Parse(xelement10.Element("min").Value), float.Parse(xelement10.Element("max").Value));
							xelement10 = xelement7.Element("y");
							this.randomRotationRangeY = new Vector2(float.Parse(xelement10.Element("min").Value), float.Parse(xelement10.Element("max").Value));
							xelement10 = xelement7.Element("z");
							this.randomRotationRangeZ = new Vector2(float.Parse(xelement10.Element("min").Value), float.Parse(xelement10.Element("max").Value));
						}
					}
					continue;
				}
				IL_11E3:
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement11 = enumerator2.Current;
						text = xelement11.Name.LocalName;
						if (!(text == "enabled"))
						{
							if (text == "minimumAbsoluteDistance")
							{
								this.minimumAbsoluteDistanceRange = new Vector2(float.Parse(xelement11.Element("min").Value), float.Parse(xelement11.Element("max").Value));
							}
						}
						else
						{
							this.useOverlapFilter = string.CompareOrdinal(xelement11.Value, "true") == 0;
						}
					}
					continue;
				}
				IL_12A4:
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement12 = enumerator2.Current;
						text = xelement12.Name.LocalName;
						if (!(text == "scaleUniformly"))
						{
							if (!(text == "uniform"))
							{
								if (text == "nonUniform")
								{
									this.additiveScaleNonUniform = new Vector3(float.Parse(xelement12.Element("x").Value), float.Parse(xelement12.Element("y").Value), float.Parse(xelement12.Element("z").Value));
								}
							}
							else
							{
								this.additiveScaleRange = new Vector2(float.Parse(xelement12.Element("min").Value), float.Parse(xelement12.Element("max").Value));
							}
						}
						else
						{
							this.uniformAdditiveScale = string.CompareOrdinal(xelement12.Value, "true") == 0;
						}
					}
					continue;
				}
				IL_13D7:
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement13 = enumerator2.Current;
						text = xelement13.Name.LocalName;
						if (!(text == "autoIgnoreRaycast"))
						{
							if (!(text == "autoSelectOnCombine"))
							{
								if (text == "autoStatic")
								{
									this.autoStatic = string.CompareOrdinal(xelement13.Value, "true") == 0;
								}
							}
							else
							{
								this.autoSelectOnCombine = string.CompareOrdinal(xelement13.Value, "true") == 0;
							}
						}
						else
						{
							this.autoIgnoreRaycast = string.CompareOrdinal(xelement13.Value, "true") == 0;
						}
					}
					continue;
				}
				IL_149B:
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement14 = enumerator2.Current;
						text = xelement14.Name.LocalName;
						num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num2 <= 1265799128U)
						{
							if (num2 != 348705738U)
							{
								if (num2 != 363276849U)
								{
									if (num2 == 1265799128U)
									{
										if (text == "quantity")
										{
											this.maxQuantityLimit = int.Parse(xelement14.Element("max").Value);
										}
									}
								}
								else if (text == "additiveScale")
								{
									this.maxAdditiveScaleLimit = float.Parse(xelement14.Element("max").Value);
								}
							}
							else if (text == "offset")
							{
								this.minOffsetLimit = float.Parse(xelement14.Element("min").Value);
								this.maxOffsetLimit = float.Parse(xelement14.Element("max").Value);
							}
						}
						else if (num2 <= 1924728219U)
						{
							if (num2 != 1322381784U)
							{
								if (num2 == 1924728219U)
								{
									if (text == "density")
									{
										this.maxDensityLimit = float.Parse(xelement14.Element("max").Value);
									}
								}
							}
							else if (text == "delay")
							{
								this.maxDelayLimit = float.Parse(xelement14.Element("max").Value);
							}
						}
						else if (num2 != 2046989233U)
						{
							if (num2 == 2547116664U)
							{
								if (text == "randomScale")
								{
									this.maxRandomScaleLimit = float.Parse(xelement14.Element("max").Value);
								}
							}
						}
						else if (text == "minimumAbsoluteDistance")
						{
							this.maxMinimumAbsoluteDistanceLimit = float.Parse(xelement14.Element("max").Value);
						}
					}
					continue;
				}
				IL_1704:
				using (IEnumerator<XElement> enumerator2 = xelement.Elements().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						XElement xelement15 = enumerator2.Current;
						text = xelement15.Name.LocalName;
						num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num2 <= 1693723192U)
						{
							if (num2 <= 946971642U)
							{
								if (num2 != 363276849U)
								{
									if (num2 != 386306721U)
									{
										if (num2 == 946971642U)
										{
											if (text == "help")
											{
												this.helpFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
											}
										}
									}
									else if (text == "slopes")
									{
										this.slopesFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
									}
								}
								else if (text == "additiveScale")
								{
									this.additiveScaleFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
								}
							}
							else if (num2 != 948591336U)
							{
								if (num2 != 1399642493U)
								{
									if (num2 == 1693723192U)
									{
										if (text == "meshes")
										{
											this.meshesFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
										}
									}
								}
								else if (text == "generalUsageHelp")
								{
									this.helpGeneralUsageFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
								}
							}
							else if (text == "templates")
							{
								this.templatesFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
							}
						}
						else if (num2 <= 2413813947U)
						{
							if (num2 != 1821279675U)
							{
								if (num2 != 1955283044U)
								{
									if (num2 == 2413813947U)
									{
										if (text == "randomizers")
										{
											this.randomizersFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
										}
									}
								}
								else if (text == "optimization")
								{
									this.optimizationFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
								}
							}
							else if (text == "templatesHelp")
							{
								this.helpTemplatesFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
							}
						}
						else if (num2 <= 2805944327U)
						{
							if (num2 != 2670881530U)
							{
								if (num2 == 2805944327U)
								{
									if (text == "optimizationHelp")
									{
										this.helpOptimizationFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
									}
								}
							}
							else if (text == "overlapFilter")
							{
								this.overlapFilterFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
							}
						}
						else if (num2 != 3862843026U)
						{
							if (num2 == 4207177203U)
							{
								if (text == "brush")
								{
									this.brushFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
								}
							}
						}
						else if (text == "keyBindings")
						{
							this.keyBindingsFoldout = string.CompareOrdinal(xelement15.Value, "true") == 0;
						}
					}
					continue;
				}
				IL_1AC3:
				this.globalPaintingMode = string.CompareOrdinal(xelement.Element("enabled").Value, "true") == 0;
				this.layerMask = (from layerElement in xelement.Descendants("layer")
					select string.CompareOrdinal(layerElement.Value, "false") != 0).ToArray<bool>();
			}
			return true;
		}

		// Token: 0x04000C86 RID: 3206
		public const float version = 2f;

		// Token: 0x04000C87 RID: 3207
		public bool active = true;

		// Token: 0x04000C88 RID: 3208
		public string groupName = "<group name>";

		// Token: 0x04000C89 RID: 3209
		public bool showGlobalPaintingLayersInspector = true;

		// Token: 0x04000C8A RID: 3210
		public bool[] layerMask = new bool[]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, true, true, true, true, true, true,
			true, true
		};

		// Token: 0x04000C8B RID: 3211
		public float radius = 0.3f;

		// Token: 0x04000C8C RID: 3212
		public Color color = Color.white;

		// Token: 0x04000C8D RID: 3213
		public Vector2 quantityRange = Vector2.one;

		// Token: 0x04000C8E RID: 3214
		public bool useDensity;

		// Token: 0x04000C8F RID: 3215
		public Vector2 densityRange = new Vector2(0.5f, 0.5f);

		// Token: 0x04000C90 RID: 3216
		public float delay = 0.25f;

		// Token: 0x04000C91 RID: 3217
		public Vector2 offsetRange;

		// Token: 0x04000C92 RID: 3218
		public Vector2 slopeInfluenceRange = new Vector2(95f, 100f);

		// Token: 0x04000C93 RID: 3219
		public bool useSlopeFilter;

		// Token: 0x04000C94 RID: 3220
		public Vector2 angleThresholdRange = new Vector2(25f, 30f);

		// Token: 0x04000C95 RID: 3221
		public bool inverseSlopeFilter;

		// Token: 0x04000C96 RID: 3222
		public Vector3 slopeReferenceVector = Vector3.up;

		// Token: 0x04000C97 RID: 3223
		public Vector3 slopeReferenceVectorSampleLocation = Vector3.zero;

		// Token: 0x04000C98 RID: 3224
		public bool yAxisTangent;

		// Token: 0x04000C99 RID: 3225
		public bool strokeAlignment;

		// Token: 0x04000C9A RID: 3226
		public bool autoIgnoreRaycast;

		// Token: 0x04000C9B RID: 3227
		public Vector2 scatteringRange = new Vector2(70f, 80f);

		// Token: 0x04000C9C RID: 3228
		public bool useOverlapFilter;

		// Token: 0x04000C9D RID: 3229
		public Vector2 minimumAbsoluteDistanceRange = new Vector2(0.25f, 0.5f);

		// Token: 0x04000C9E RID: 3230
		public bool uniformRandomScale = true;

		// Token: 0x04000C9F RID: 3231
		public bool uniformAdditiveScale = true;

		// Token: 0x04000CA0 RID: 3232
		public Vector2 randomScaleRange = Vector2.one;

		// Token: 0x04000CA1 RID: 3233
		public Vector2 randomScaleRangeX = Vector2.one;

		// Token: 0x04000CA2 RID: 3234
		public Vector2 randomScaleRangeY = Vector2.one;

		// Token: 0x04000CA3 RID: 3235
		public Vector2 randomScaleRangeZ = Vector2.one;

		// Token: 0x04000CA4 RID: 3236
		public Vector2 additiveScaleRange = Vector2.zero;

		// Token: 0x04000CA5 RID: 3237
		public Vector3 additiveScaleNonUniform = Vector3.zero;

		// Token: 0x04000CA6 RID: 3238
		public AnimationCurve randomScaleCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04000CA7 RID: 3239
		public float randomScaleCurveVariation;

		// Token: 0x04000CA8 RID: 3240
		public Vector2 randomRotationRangeX = new Vector2(0f, 0f);

		// Token: 0x04000CA9 RID: 3241
		public Vector2 randomRotationRangeY = new Vector2(0f, 5f);

		// Token: 0x04000CAA RID: 3242
		public Vector2 randomRotationRangeZ = new Vector2(0f, 0f);

		// Token: 0x04000CAB RID: 3243
		public bool positionBrushRandomizer;

		// Token: 0x04000CAC RID: 3244
		public bool rotationBrushRandomizer = true;

		// Token: 0x04000CAD RID: 3245
		public bool scaleBrushRandomizer = true;

		// Token: 0x04000CAE RID: 3246
		public KeyCode paintKey = KeyCode.P;

		// Token: 0x04000CAF RID: 3247
		public KeyCode deleteKey = KeyCode.L;

		// Token: 0x04000CB0 RID: 3248
		public KeyCode combineKey = KeyCode.K;

		// Token: 0x04000CB1 RID: 3249
		public KeyCode randomizeKey = KeyCode.J;

		// Token: 0x04000CB2 RID: 3250
		public KeyCode increaseRadiusKey = KeyCode.O;

		// Token: 0x04000CB3 RID: 3251
		public KeyCode decreaseRadiusKey = KeyCode.I;

		// Token: 0x04000CB4 RID: 3252
		[SerializeField]
		private int maxQuantityLimit = 100;

		// Token: 0x04000CB5 RID: 3253
		[SerializeField]
		private float maxDelayLimit = 1f;

		// Token: 0x04000CB6 RID: 3254
		[SerializeField]
		private float maxDensityLimit = 10f;

		// Token: 0x04000CB7 RID: 3255
		[SerializeField]
		private float minOffsetLimit = -50f;

		// Token: 0x04000CB8 RID: 3256
		[SerializeField]
		private float maxOffsetLimit = 50f;

		// Token: 0x04000CB9 RID: 3257
		[SerializeField]
		private float maxMinimumAbsoluteDistanceLimit = 3f;

		// Token: 0x04000CBA RID: 3258
		[SerializeField]
		private float maxAdditiveScaleLimit = 3f;

		// Token: 0x04000CBB RID: 3259
		[SerializeField]
		private float maxRandomScaleLimit = 3f;

		// Token: 0x04000CBC RID: 3260
		public bool helpFoldout;

		// Token: 0x04000CBD RID: 3261
		public bool helpTemplatesFoldout;

		// Token: 0x04000CBE RID: 3262
		public bool helpGeneralUsageFoldout;

		// Token: 0x04000CBF RID: 3263
		public bool helpOptimizationFoldout;

		// Token: 0x04000CC0 RID: 3264
		public bool meshesFoldout = true;

		// Token: 0x04000CC1 RID: 3265
		public bool templatesFoldout = true;

		// Token: 0x04000CC2 RID: 3266
		public bool keyBindingsFoldout;

		// Token: 0x04000CC3 RID: 3267
		public bool brushFoldout = true;

		// Token: 0x04000CC4 RID: 3268
		public bool slopesFoldout = true;

		// Token: 0x04000CC5 RID: 3269
		public bool randomizersFoldout = true;

		// Token: 0x04000CC6 RID: 3270
		public bool overlapFilterFoldout = true;

		// Token: 0x04000CC7 RID: 3271
		public bool additiveScaleFoldout = true;

		// Token: 0x04000CC8 RID: 3272
		public bool optimizationFoldout = true;

		// Token: 0x04000CC9 RID: 3273
		[SerializeField]
		private bool globalPaintingMode;

		// Token: 0x04000CCA RID: 3274
		public bool collapsed;

		// Token: 0x04000CCB RID: 3275
		public bool stats;

		// Token: 0x04000CCC RID: 3276
		public bool lockSceneView;

		// Token: 0x04000CCD RID: 3277
		public bool classicUI;

		// Token: 0x04000CCE RID: 3278
		public float previewIconSize = 60f;

		// Token: 0x04000CCF RID: 3279
		public bool manualReferenceVectorSampling;

		// Token: 0x04000CD0 RID: 3280
		public bool showReferenceVectorInSceneView = true;

		// Token: 0x04000CD1 RID: 3281
		public bool autoStatic;

		// Token: 0x04000CD2 RID: 3282
		public bool autoSelectOnCombine = true;

		// Token: 0x04000CD3 RID: 3283
		private Transform cachedTransform;

		// Token: 0x04000CD4 RID: 3284
		private Collider cachedCollider;

		// Token: 0x04000CD5 RID: 3285
		private GameObject brush;

		// Token: 0x04000CD6 RID: 3286
		private Transform brushTransform;

		// Token: 0x04000CD7 RID: 3287
		private Transform holderObj;

		// Token: 0x04000CD8 RID: 3288
		private const string minString = "min";

		// Token: 0x04000CD9 RID: 3289
		private const string maxString = "max";

		// Token: 0x04000CDA RID: 3290
		private const string trueString = "true";

		// Token: 0x04000CDB RID: 3291
		private const string falseString = "false";

		// Token: 0x04000CDC RID: 3292
		private const string enabledString = "enabled";

		// Token: 0x04000CDD RID: 3293
		public Vector3 lastPaintLocation;

		// Token: 0x04000CDE RID: 3294
		public Vector3 brushStrokeDirection;

		// Token: 0x04000CDF RID: 3295
		[SerializeField]
		private List<GameObject> meshes = new List<GameObject>(5) { null };

		// Token: 0x04000CE0 RID: 3296
		[SerializeField]
		private List<GameObject> deactivatedMeshes = new List<GameObject>(2);

		// Token: 0x04000CE1 RID: 3297
		private List<Transform> paintedMeshes = new List<Transform>(200);

		// Token: 0x04000CE2 RID: 3298
		private List<Transform> paintedMeshesInsideBrushArea = new List<Transform>(50);

		// Token: 0x04000CE3 RID: 3299
		private float nextFeasibleStrokeTime;
	}
}
