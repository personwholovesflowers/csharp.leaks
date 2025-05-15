using System;
using UnityEngine;

namespace AmazingAssets.CurvedWorld
{
	// Token: 0x0200033F RID: 831
	[ExecuteAlways]
	public class CurvedWorldController : MonoBehaviour
	{
		// Token: 0x06001556 RID: 5462 RVA: 0x00071205 File Offset: 0x0006F405
		private void OnDisable()
		{
			this.DisableBend();
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00071205 File Offset: 0x0006F405
		private void OnDestroy()
		{
			this.DisableBend();
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0007120D File Offset: 0x0006F40D
		private void OnEnable()
		{
			this.EnableBend();
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00071215 File Offset: 0x0006F415
		private void Start()
		{
			this.GenerateShaderPropertyIDs();
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0007121D File Offset: 0x0006F41D
		private void Update()
		{
			if (this.manualUpdate)
			{
				return;
			}
			this.UpdateShaderdata();
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00071230 File Offset: 0x0006F430
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			BEND_TYPE bend_TYPE = this.bendType;
			if (bend_TYPE - BEND_TYPE.TwistedSpiral_X_Positive <= 3)
			{
				Gizmos.DrawRay(this.bendPivotPointPosition, this.bendRotationAxis.normalized * 10f);
			}
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x00071278 File Offset: 0x0006F478
		private void UpdateShaderdata()
		{
			this.CheckBendChanging();
			if (base.isActiveAndEnabled)
			{
				if (this.disableInEditor && Application.isEditor && !Application.isPlaying)
				{
					this.UpdateShaderDataDisabled();
					return;
				}
				if (this.bendPivotPoint != null)
				{
					this.bendPivotPointPosition = this.bendPivotPoint.transform.position;
				}
				if (this.bendRotationCenter != null)
				{
					this.bendRotationCenterPosition = this.bendRotationCenter.position;
				}
				if (this.bendRotationCenter2 != null)
				{
					this.bendRotationCenter2Position = this.bendRotationCenter2.position;
				}
				switch (this.bendType)
				{
				case BEND_TYPE.ClassicRunner_X_Positive:
				case BEND_TYPE.ClassicRunner_X_Negative:
				case BEND_TYPE.ClassicRunner_Z_Positive:
				case BEND_TYPE.ClassicRunner_Z_Negative:
					Shader.SetGlobalVector(this.materialPropertyID_PivotPoint, this.bendPivotPointPosition);
					Shader.SetGlobalVector(this.materialPropertyID_BendSize, new Vector2(this.bendVerticalSize, this.bendHorizontalSize));
					Shader.SetGlobalVector(this.materialPropertyID_BendOffset, new Vector2(this.bendVerticalOffset, this.bendHorizontalOffset));
					return;
				case BEND_TYPE.LittlePlanet_X:
				case BEND_TYPE.LittlePlanet_Y:
				case BEND_TYPE.LittlePlanet_Z:
				case BEND_TYPE.CylindricalTower_X:
				case BEND_TYPE.CylindricalTower_Z:
				case BEND_TYPE.CylindricalRolloff_X:
				case BEND_TYPE.CylindricalRolloff_Z:
					Shader.SetGlobalVector(this.materialPropertyID_PivotPoint, this.bendPivotPointPosition);
					Shader.SetGlobalFloat(this.materialPropertyID_BendSize, this.bendCurvatureSize);
					Shader.SetGlobalFloat(this.materialPropertyID_BendOffset, this.bendCurvatureOffset);
					return;
				case BEND_TYPE.SpiralHorizontal_X_Positive:
				case BEND_TYPE.SpiralHorizontal_X_Negative:
				case BEND_TYPE.SpiralHorizontal_Z_Positive:
				case BEND_TYPE.SpiralHorizontal_Z_Negative:
				case BEND_TYPE.SpiralVertical_X_Positive:
				case BEND_TYPE.SpiralVertical_X_Negative:
				case BEND_TYPE.SpiralVertical_Z_Positive:
				case BEND_TYPE.SpiralVertical_Z_Negative:
					Shader.SetGlobalVector(this.materialPropertyID_PivotPoint, this.bendPivotPointPosition);
					Shader.SetGlobalVector(this.materialPropertyID_RotationCenter, this.bendRotationCenterPosition);
					Shader.SetGlobalFloat(this.materialPropertyID_BendAngle, this.bendAngle);
					Shader.SetGlobalFloat(this.materialPropertyID_BendMinimumRadius, this.bendMinimumRadius);
					return;
				case BEND_TYPE.SpiralHorizontalDouble_X:
				case BEND_TYPE.SpiralHorizontalDouble_Z:
				case BEND_TYPE.SpiralVerticalDouble_X:
				case BEND_TYPE.SpiralVerticalDouble_Z:
					Shader.SetGlobalVector(this.materialPropertyID_PivotPoint, this.bendPivotPointPosition);
					Shader.SetGlobalVector(this.materialPropertyID_RotationCenter, this.bendRotationCenterPosition);
					Shader.SetGlobalVector(this.materialPropertyID_RotationCenter2, this.bendRotationCenter2Position);
					Shader.SetGlobalVector(this.materialPropertyID_BendAngle, new Vector2(this.bendAngle, this.bendAngle2));
					Shader.SetGlobalVector(this.materialPropertyID_BendMinimumRadius, new Vector2(this.bendMinimumRadius, this.bendMinimumRadius2));
					return;
				case BEND_TYPE.SpiralHorizontalRolloff_X:
				case BEND_TYPE.SpiralHorizontalRolloff_Z:
				case BEND_TYPE.SpiralVerticalRolloff_X:
				case BEND_TYPE.SpiralVerticalRolloff_Z:
					Shader.SetGlobalVector(this.materialPropertyID_PivotPoint, this.bendPivotPointPosition);
					Shader.SetGlobalVector(this.materialPropertyID_RotationCenter, this.bendRotationCenterPosition);
					Shader.SetGlobalFloat(this.materialPropertyID_BendAngle, this.bendAngle);
					Shader.SetGlobalFloat(this.materialPropertyID_BendMinimumRadius, this.bendMinimumRadius);
					Shader.SetGlobalFloat(this.materialPropertyID_BendRolloff, this.bendRolloff);
					return;
				case BEND_TYPE.TwistedSpiral_X_Positive:
				case BEND_TYPE.TwistedSpiral_X_Negative:
				case BEND_TYPE.TwistedSpiral_Z_Positive:
				case BEND_TYPE.TwistedSpiral_Z_Negative:
					switch (this.bendRotationAxisType)
					{
					case CurvedWorldController.AXIS_TYPE.Transform:
						if (this.bendPivotPoint == null)
						{
							switch (this.bendType)
							{
							case BEND_TYPE.ClassicRunner_X_Positive:
								this.bendRotationAxis = Vector3.left;
								break;
							case BEND_TYPE.ClassicRunner_X_Negative:
								this.bendRotationAxis = Vector3.right;
								break;
							case BEND_TYPE.ClassicRunner_Z_Positive:
								this.bendRotationAxis = Vector3.back;
								break;
							case BEND_TYPE.ClassicRunner_Z_Negative:
								this.bendRotationAxis = Vector3.forward;
								break;
							}
						}
						else
						{
							this.bendRotationAxis = this.bendPivotPoint.forward;
						}
						break;
					case CurvedWorldController.AXIS_TYPE.CustomNormalized:
						this.bendRotationAxis = this.bendRotationAxis.normalized;
						break;
					}
					Shader.SetGlobalVector(this.materialPropertyID_PivotPoint, this.bendPivotPointPosition);
					Shader.SetGlobalVector(this.materialPropertyID_RotationAxis, this.bendRotationAxis);
					Shader.SetGlobalVector(this.materialPropertyID_BendSize, new Vector3(this.bendCurvatureSize, this.bendVerticalSize, this.bendHorizontalSize));
					Shader.SetGlobalVector(this.materialPropertyID_BendOffset, new Vector3(this.bendCurvatureOffset, this.bendVerticalOffset, this.bendHorizontalOffset));
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00071684 File Offset: 0x0006F884
		private void UpdateShaderDataDisabled()
		{
			Shader.SetGlobalVector(this.materialPropertyID_PivotPoint, Vector3.zero);
			Shader.SetGlobalVector(this.materialPropertyID_RotationCenter, Vector3.zero);
			Shader.SetGlobalVector(this.materialPropertyID_RotationCenter2, Vector3.zero);
			Shader.SetGlobalVector(this.materialPropertyID_RotationAxis, Vector3.zero);
			Shader.SetGlobalVector(this.materialPropertyID_BendSize, Vector3.zero);
			Shader.SetGlobalFloat(this.materialPropertyID_BendSize, 0f);
			Shader.SetGlobalVector(this.materialPropertyID_BendOffset, Vector3.zero);
			Shader.SetGlobalFloat(this.materialPropertyID_BendOffset, 0f);
			Shader.SetGlobalVector(this.materialPropertyID_BendAngle, Vector2.zero);
			Shader.SetGlobalFloat(this.materialPropertyID_BendAngle, 0f);
			Shader.SetGlobalVector(this.materialPropertyID_BendMinimumRadius, Vector2.zero);
			Shader.SetGlobalFloat(this.materialPropertyID_BendMinimumRadius, 0f);
			Shader.SetGlobalFloat(this.materialPropertyID_BendRolloff, 10f);
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00071789 File Offset: 0x0006F989
		public void DisableBend()
		{
			this.GenerateShaderPropertyIDs();
			this.UpdateShaderDataDisabled();
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00071797 File Offset: 0x0006F997
		public void EnableBend()
		{
			this.GenerateShaderPropertyIDs();
			this.previousBentType = this.bendType;
			this.previousID = this.bendID;
			this.UpdateShaderdata();
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x000717BD File Offset: 0x0006F9BD
		public void ManualUpdate()
		{
			this.UpdateShaderdata();
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x000717C8 File Offset: 0x0006F9C8
		private void CheckBendChanging()
		{
			if (this.previousBentType != this.bendType || this.previousID != this.bendID)
			{
				this.DisableBend();
				this.previousBentType = this.bendType;
				if (this.bendID < 1)
				{
					this.bendID = 1;
				}
				this.previousID = this.bendID;
				this.GenerateShaderPropertyIDs();
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00071828 File Offset: 0x0006FA28
		private void GenerateShaderPropertyIDs()
		{
			this.materialPropertyID_PivotPoint = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_PivotPoint", this.bendType, this.bendID));
			this.materialPropertyID_RotationCenter = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_RotationCenter", this.bendType, this.bendID));
			this.materialPropertyID_RotationCenter2 = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_RotationCenter2", this.bendType, this.bendID));
			this.materialPropertyID_RotationAxis = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_RotationAxis", this.bendType, this.bendID));
			this.materialPropertyID_BendSize = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_BendSize", this.bendType, this.bendID));
			this.materialPropertyID_BendOffset = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_BendOffset", this.bendType, this.bendID));
			this.materialPropertyID_BendAngle = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_BendAngle", this.bendType, this.bendID));
			this.materialPropertyID_BendMinimumRadius = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_BendMinimumRadius", this.bendType, this.bendID));
			this.materialPropertyID_BendRolloff = Shader.PropertyToID(string.Format("CurvedWorld_{0}_ID{1}_BendRolloff", this.bendType, this.bendID));
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x000719B8 File Offset: 0x0006FBB8
		public Vector3 TransformPosition(Vector3 vertex)
		{
			if (!base.enabled || !base.gameObject.activeSelf)
			{
				return vertex;
			}
			return CurvedWorldUtilities.TransformPosition(vertex, this);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x000719D8 File Offset: 0x0006FBD8
		public Quaternion TransformRotation(Vector3 vertex, Vector3 forwardVector, Vector3 rightVector)
		{
			Vector3 vector = this.TransformPosition(vertex);
			Vector3 vector2 = this.TransformPosition(vertex + forwardVector);
			Vector3 vector3 = this.TransformPosition(vertex + rightVector);
			Vector3 vector4 = Vector3.Normalize(vector2 - vector);
			Vector3 vector5 = Vector3.Normalize(vector3 - vector);
			Vector3 vector6 = Vector3.Cross(vector4, vector5);
			if (vector4.sqrMagnitude < 0.01f && vector6.sqrMagnitude < 0.01f)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(vector4, vector6);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00071A54 File Offset: 0x0006FC54
		public void SetBendVerticalSize(float value)
		{
			this.bendVerticalSize = value;
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00071A5D File Offset: 0x0006FC5D
		public void SetBendVerticalOffset(float value)
		{
			this.bendVerticalOffset = value;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00071A66 File Offset: 0x0006FC66
		public void SetBendHorizontalSize(float value)
		{
			this.bendHorizontalSize = value;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00071A6F File Offset: 0x0006FC6F
		public void SetBendHorizontalOffset(float value)
		{
			this.bendHorizontalOffset = value;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00071A78 File Offset: 0x0006FC78
		public void SetBendCurvatureSize(float value)
		{
			this.bendCurvatureSize = value;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00071A81 File Offset: 0x0006FC81
		public void SetBendCurvatureOffset(float value)
		{
			this.bendCurvatureOffset = value;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00071A8A File Offset: 0x0006FC8A
		public void SetBendAngle(float value)
		{
			this.bendAngle = value;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00071A93 File Offset: 0x0006FC93
		public void SetBendAngle2(float value)
		{
			this.bendAngle2 = value;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00071A9C File Offset: 0x0006FC9C
		public void SetBendMinimumRadius(float value)
		{
			this.bendMinimumRadius = value;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00071AA5 File Offset: 0x0006FCA5
		public void SetBendMinimumRadius2(float value)
		{
			this.bendMinimumRadius2 = value;
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00071AAE File Offset: 0x0006FCAE
		public void SetBendRolloff(float value)
		{
			this.bendRolloff = value;
		}

		// Token: 0x0400118B RID: 4491
		public BEND_TYPE bendType;

		// Token: 0x0400118C RID: 4492
		[Range(1f, 32f)]
		public int bendID = 1;

		// Token: 0x0400118D RID: 4493
		public Transform bendPivotPoint;

		// Token: 0x0400118E RID: 4494
		public Vector3 bendPivotPointPosition;

		// Token: 0x0400118F RID: 4495
		public Transform bendRotationCenter;

		// Token: 0x04001190 RID: 4496
		public Vector3 bendRotationCenterPosition;

		// Token: 0x04001191 RID: 4497
		public Vector3 bendRotationAxis;

		// Token: 0x04001192 RID: 4498
		public CurvedWorldController.AXIS_TYPE bendRotationAxisType;

		// Token: 0x04001193 RID: 4499
		public Transform bendRotationCenter2;

		// Token: 0x04001194 RID: 4500
		public Vector3 bendRotationCenter2Position;

		// Token: 0x04001195 RID: 4501
		public float bendVerticalSize;

		// Token: 0x04001196 RID: 4502
		public float bendVerticalOffset;

		// Token: 0x04001197 RID: 4503
		public float bendHorizontalSize;

		// Token: 0x04001198 RID: 4504
		public float bendHorizontalOffset;

		// Token: 0x04001199 RID: 4505
		public float bendCurvatureSize;

		// Token: 0x0400119A RID: 4506
		public float bendCurvatureOffset;

		// Token: 0x0400119B RID: 4507
		public float bendAngle;

		// Token: 0x0400119C RID: 4508
		public float bendAngle2;

		// Token: 0x0400119D RID: 4509
		public float bendMinimumRadius;

		// Token: 0x0400119E RID: 4510
		public float bendMinimumRadius2;

		// Token: 0x0400119F RID: 4511
		public float bendRolloff;

		// Token: 0x040011A0 RID: 4512
		public bool disableInEditor;

		// Token: 0x040011A1 RID: 4513
		public bool manualUpdate;

		// Token: 0x040011A2 RID: 4514
		private BEND_TYPE previousBentType;

		// Token: 0x040011A3 RID: 4515
		private int previousID;

		// Token: 0x040011A4 RID: 4516
		private int materialPropertyID_PivotPoint;

		// Token: 0x040011A5 RID: 4517
		private int materialPropertyID_RotationCenter;

		// Token: 0x040011A6 RID: 4518
		private int materialPropertyID_RotationCenter2;

		// Token: 0x040011A7 RID: 4519
		private int materialPropertyID_RotationAxis;

		// Token: 0x040011A8 RID: 4520
		private int materialPropertyID_BendSize;

		// Token: 0x040011A9 RID: 4521
		private int materialPropertyID_BendOffset;

		// Token: 0x040011AA RID: 4522
		private int materialPropertyID_BendAngle;

		// Token: 0x040011AB RID: 4523
		private int materialPropertyID_BendMinimumRadius;

		// Token: 0x040011AC RID: 4524
		private int materialPropertyID_BendRolloff;

		// Token: 0x020004C3 RID: 1219
		public enum AXIS_TYPE
		{
			// Token: 0x040017E2 RID: 6114
			Transform,
			// Token: 0x040017E3 RID: 6115
			Custom,
			// Token: 0x040017E4 RID: 6116
			CustomNormalized
		}
	}
}
