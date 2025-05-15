using System;
using UnityEngine;

namespace AmazingAssets.CurvedWorld
{
	// Token: 0x02000340 RID: 832
	public static class CurvedWorldUtilities
	{
		// Token: 0x06001571 RID: 5489 RVA: 0x00071AC8 File Offset: 0x0006FCC8
		public static Vector3 TransformPosition(Vector3 vertex, CurvedWorldController controller)
		{
			if (controller == null || (controller.disableInEditor && Application.isEditor && !Application.isPlaying))
			{
				return vertex;
			}
			switch (controller.bendType)
			{
			case BEND_TYPE.ClassicRunner_X_Positive:
			case BEND_TYPE.ClassicRunner_X_Negative:
			case BEND_TYPE.ClassicRunner_Z_Positive:
			case BEND_TYPE.ClassicRunner_Z_Negative:
				return CurvedWorldUtilities.TransformPosition(vertex, controller.bendType, controller.bendPivotPointPosition, new Vector2(controller.bendVerticalSize, controller.bendHorizontalSize), new Vector2(controller.bendVerticalOffset, controller.bendHorizontalOffset));
			case BEND_TYPE.LittlePlanet_X:
			case BEND_TYPE.LittlePlanet_Y:
			case BEND_TYPE.LittlePlanet_Z:
			case BEND_TYPE.CylindricalTower_X:
			case BEND_TYPE.CylindricalTower_Z:
			case BEND_TYPE.CylindricalRolloff_X:
			case BEND_TYPE.CylindricalRolloff_Z:
				return CurvedWorldUtilities.TransformPosition(vertex, controller.bendType, controller.bendPivotPointPosition, controller.bendCurvatureSize, controller.bendCurvatureOffset);
			case BEND_TYPE.SpiralHorizontal_X_Positive:
			case BEND_TYPE.SpiralHorizontal_X_Negative:
			case BEND_TYPE.SpiralHorizontal_Z_Positive:
			case BEND_TYPE.SpiralHorizontal_Z_Negative:
			case BEND_TYPE.SpiralVertical_X_Positive:
			case BEND_TYPE.SpiralVertical_X_Negative:
			case BEND_TYPE.SpiralVertical_Z_Positive:
			case BEND_TYPE.SpiralVertical_Z_Negative:
				return CurvedWorldUtilities.TransformPosition(vertex, controller.bendType, controller.bendPivotPointPosition, controller.bendRotationCenterPosition, controller.bendAngle, controller.bendMinimumRadius);
			case BEND_TYPE.SpiralHorizontalDouble_X:
			case BEND_TYPE.SpiralHorizontalDouble_Z:
			case BEND_TYPE.SpiralVerticalDouble_X:
			case BEND_TYPE.SpiralVerticalDouble_Z:
				return CurvedWorldUtilities.TransformPosition(vertex, controller.bendType, controller.bendPivotPointPosition, controller.bendRotationCenterPosition, controller.bendRotationCenter2Position, controller.bendAngle, controller.bendMinimumRadius, controller.bendAngle2, controller.bendMinimumRadius2);
			case BEND_TYPE.SpiralHorizontalRolloff_X:
			case BEND_TYPE.SpiralHorizontalRolloff_Z:
			case BEND_TYPE.SpiralVerticalRolloff_X:
			case BEND_TYPE.SpiralVerticalRolloff_Z:
				return CurvedWorldUtilities.TransformPosition(vertex, controller.bendType, controller.bendPivotPointPosition, controller.bendRotationCenterPosition, controller.bendAngle, controller.bendMinimumRadius, controller.bendRolloff);
			case BEND_TYPE.TwistedSpiral_X_Positive:
			case BEND_TYPE.TwistedSpiral_X_Negative:
			case BEND_TYPE.TwistedSpiral_Z_Positive:
			case BEND_TYPE.TwistedSpiral_Z_Negative:
				return CurvedWorldUtilities.TransformPosition(vertex, controller.bendType, controller.bendPivotPointPosition, controller.bendRotationAxis, new Vector3(controller.bendCurvatureSize, controller.bendVerticalSize, controller.bendHorizontalSize), new Vector3(controller.bendCurvatureOffset, controller.bendVerticalOffset, controller.bendHorizontalOffset));
			default:
				return vertex;
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x00071CA8 File Offset: 0x0006FEA8
		private static Vector3 TransformPosition(Vector3 vertex, BEND_TYPE bendType, Vector3 pivotPoint, Vector2 bendSize, Vector2 bendOffset)
		{
			switch (bendType)
			{
			case BEND_TYPE.ClassicRunner_X_Positive:
			{
				Vector3 vector = vertex - pivotPoint;
				float num = Mathf.Max(0f, vector.x - bendOffset.x);
				float num2 = Mathf.Max(0f, vector.x - bendOffset.y);
				vector = new Vector3(0f, bendSize.x * num * num, -bendSize.y * num2 * num2) * 0.001f;
				return vertex + vector;
			}
			case BEND_TYPE.ClassicRunner_X_Negative:
			{
				Vector3 vector2 = vertex - pivotPoint;
				float num3 = Mathf.Min(0f, vector2.x + bendOffset.x);
				float num4 = Mathf.Min(0f, vector2.x + bendOffset.y);
				vector2 = new Vector3(0f, bendSize.x * num3 * num3, bendSize.y * num4 * num4) * 0.001f;
				return vertex + vector2;
			}
			case BEND_TYPE.ClassicRunner_Z_Positive:
			{
				Vector3 vector3 = vertex - pivotPoint;
				float num5 = Mathf.Max(0f, vector3.z - bendOffset.x);
				float num6 = Mathf.Max(0f, vector3.z - bendOffset.y);
				vector3 = new Vector3(bendSize.y * num6 * num6, bendSize.x * num5 * num5, 0f) * 0.001f;
				return vertex + vector3;
			}
			case BEND_TYPE.ClassicRunner_Z_Negative:
			{
				Vector3 vector4 = vertex - pivotPoint;
				float num7 = Mathf.Min(0f, vector4.z + bendOffset.x);
				float num8 = Mathf.Min(0f, vector4.z + bendOffset.y);
				vector4 = new Vector3(-bendSize.y * num8 * num8, bendSize.x * num7 * num7, 0f) * 0.001f;
				return vertex + vector4;
			}
			default:
				return vertex;
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00071E9C File Offset: 0x0007009C
		private static Vector3 TransformPosition(Vector3 vertex, BEND_TYPE bendType, Vector3 pivotPoint, float bendSize, float bendOffset)
		{
			switch (bendType)
			{
			case BEND_TYPE.LittlePlanet_X:
			{
				Vector3 vector = vertex - pivotPoint;
				float num = Mathf.Max(0f, Mathf.Abs(vector.y) - ((bendOffset < 0f) ? 0f : bendOffset)) * ((vector.y < 0f) ? (-1f) : 1f);
				float num2 = Mathf.Max(0f, Mathf.Abs(vector.z) - ((bendOffset < 0f) ? 0f : bendOffset)) * ((vector.z < 0f) ? (-1f) : 1f);
				vector = new Vector3(-(bendSize * num * num + bendSize * num2 * num2) * 0.001f, 0f, 0f);
				return vertex + vector;
			}
			case BEND_TYPE.LittlePlanet_Y:
			{
				Vector3 vector2 = vertex - pivotPoint;
				float num3 = Mathf.Max(0f, Mathf.Abs(vector2.x) - ((bendOffset < 0f) ? 0f : bendOffset)) * ((vector2.x < 0f) ? (-1f) : 1f);
				float num4 = Mathf.Max(0f, Mathf.Abs(vector2.z) - ((bendOffset < 0f) ? 0f : bendOffset)) * ((vector2.z < 0f) ? (-1f) : 1f);
				vector2 = new Vector3(0f, -(bendSize * num4 * num4 + bendSize * num3 * num3) * 0.001f, 0f);
				return vertex + vector2;
			}
			case BEND_TYPE.LittlePlanet_Z:
			{
				Vector3 vector3 = vertex - pivotPoint;
				float num5 = Mathf.Max(0f, Mathf.Abs(vector3.x) - ((bendOffset < 0f) ? 0f : bendOffset)) * ((vector3.x < 0f) ? (-1f) : 1f);
				float num6 = Mathf.Max(0f, Mathf.Abs(vector3.y) - ((bendOffset < 0f) ? 0f : bendOffset)) * ((vector3.y < 0f) ? (-1f) : 1f);
				vector3 = new Vector3(0f, 0f, -(bendSize * num5 * num5 + bendSize * num6 * num6) * 0.001f);
				return vertex + vector3;
			}
			case BEND_TYPE.CylindricalTower_X:
			{
				Vector3 vector4 = vertex - pivotPoint;
				float num7 = Mathf.Max(0f, Mathf.Abs(vector4.x) - bendOffset) * ((vector4.x < 0f) ? (-1f) : 1f);
				vector4 = new Vector3(0f, 0f, bendSize * num7 * num7 * 0.001f);
				return vertex + vector4;
			}
			case BEND_TYPE.CylindricalTower_Z:
			{
				Vector3 vector5 = vertex - pivotPoint;
				float num8 = Mathf.Max(0f, Mathf.Abs(vector5.z) - bendOffset) * ((vector5.z < 0f) ? (-1f) : 1f);
				vector5 = new Vector3(bendSize * num8 * num8 * 0.001f, 0f, 0f);
				return vertex + vector5;
			}
			case BEND_TYPE.CylindricalRolloff_X:
			{
				Vector3 vector6 = vertex - pivotPoint;
				float num9 = Mathf.Max(0f, Mathf.Abs(vector6.x) - bendOffset) * ((vector6.x < 0f) ? (-1f) : 1f);
				vector6 = new Vector3(0f, -bendSize * num9 * num9 * 0.001f, 0f);
				return vertex + vector6;
			}
			case BEND_TYPE.CylindricalRolloff_Z:
			{
				Vector3 vector7 = vertex - pivotPoint;
				float num10 = Mathf.Max(0f, Mathf.Abs(vector7.z) - bendOffset) * ((vector7.z < 0f) ? (-1f) : 1f);
				vector7 = new Vector3(0f, -bendSize * num10 * num10 * 0.001f, 0f);
				return vertex + vector7;
			}
			default:
				return vertex;
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000722AC File Offset: 0x000704AC
		private static Vector3 TransformPosition(Vector3 vertex, BEND_TYPE bendType, Vector3 pivotPoint, Vector3 rotationCenter, float bendAngle, float bendMinimumRadius)
		{
			switch (bendType)
			{
			case BEND_TYPE.SpiralHorizontal_X_Positive:
			{
				Vector3 vector = vertex;
				vector -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector.x > rotationCenter.x)
				{
					rotationCenter.z = ((Mathf.Abs(rotationCenter.z) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.z)) : rotationCenter.z);
					float z = rotationCenter.z;
					float num = bendAngle * CurvedWorldUtilities.Sign(z);
					float num2 = 6.2831855f * z * (num / 360f);
					float num3 = Mathf.Abs(rotationCenter.x - vector.x) / num2;
					float num4 = CurvedWorldUtilities.Smooth(num3);
					CurvedWorldUtilities.Spiral_H_Rotate_X_Negative(ref vector, rotationCenter, num3, num4, num2, num);
				}
				vector += pivotPoint;
				return vector;
			}
			case BEND_TYPE.SpiralHorizontal_X_Negative:
			{
				Vector3 vector2 = vertex;
				vector2 -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector2.x < rotationCenter.x)
				{
					rotationCenter.z = ((Mathf.Abs(rotationCenter.z) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.z)) : rotationCenter.z);
					float z2 = rotationCenter.z;
					float num5 = bendAngle * CurvedWorldUtilities.Sign(z2);
					float num6 = 6.2831855f * z2 * (num5 / 360f);
					float num7 = Mathf.Abs(rotationCenter.x - vector2.x) / num6;
					float num8 = CurvedWorldUtilities.Smooth(num7);
					CurvedWorldUtilities.Spiral_H_Rotate_X_Positive(ref vector2, rotationCenter, num7, num8, num6, num5);
				}
				vector2 += pivotPoint;
				return vector2;
			}
			case BEND_TYPE.SpiralHorizontal_Z_Positive:
			{
				Vector3 vector3 = vertex;
				vector3 -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector3.z > rotationCenter.z)
				{
					rotationCenter.x = ((Mathf.Abs(rotationCenter.x) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.x)) : rotationCenter.x);
					float x = rotationCenter.x;
					float num9 = bendAngle * CurvedWorldUtilities.Sign(x);
					float num10 = 6.2831855f * x * (num9 / 360f);
					float num11 = Mathf.Abs(rotationCenter.z - vector3.z) / num10;
					float num12 = CurvedWorldUtilities.Smooth(num11);
					CurvedWorldUtilities.Spiral_H_Rotate_Z_Positive(ref vector3, rotationCenter, num11, num12, num10, num9);
				}
				vector3 += pivotPoint;
				return vector3;
			}
			case BEND_TYPE.SpiralHorizontal_Z_Negative:
			{
				Vector3 vector4 = vertex;
				vector4 -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector4.z < rotationCenter.z)
				{
					rotationCenter.x = ((Mathf.Abs(rotationCenter.x) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.x)) : rotationCenter.x);
					float x2 = rotationCenter.x;
					float num13 = bendAngle * CurvedWorldUtilities.Sign(x2);
					float num14 = 6.2831855f * x2 * (num13 / 360f);
					float num15 = Mathf.Abs(rotationCenter.z - vector4.z) / num14;
					float num16 = CurvedWorldUtilities.Smooth(num15);
					CurvedWorldUtilities.Spiral_H_Rotate_Z_Negative(ref vector4, rotationCenter, num15, num16, num14, num13);
				}
				vector4 += pivotPoint;
				return vector4;
			}
			case BEND_TYPE.SpiralVertical_X_Positive:
			{
				Vector3 vector5 = vertex;
				vector5 -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector5.x > rotationCenter.x)
				{
					rotationCenter.y = ((Mathf.Abs(rotationCenter.y) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.y)) : rotationCenter.y);
					float y = rotationCenter.y;
					float num17 = bendAngle * CurvedWorldUtilities.Sign(y);
					float num18 = 6.2831855f * y * (num17 / 360f);
					float num19 = Mathf.Abs(rotationCenter.x - vector5.x) / num18;
					float num20 = CurvedWorldUtilities.Smooth(num19);
					CurvedWorldUtilities.Spiral_V_Rotate_X_Negative(ref vector5, rotationCenter, num19, num20, num18, num17);
				}
				vector5 += pivotPoint;
				return vector5;
			}
			case BEND_TYPE.SpiralVertical_X_Negative:
			{
				Vector3 vector6 = vertex;
				vector6 -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector6.x < rotationCenter.x)
				{
					rotationCenter.y = ((Mathf.Abs(rotationCenter.y) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.y)) : rotationCenter.y);
					float y2 = rotationCenter.y;
					float num21 = bendAngle * CurvedWorldUtilities.Sign(y2);
					float num22 = 6.2831855f * y2 * (num21 / 360f);
					float num23 = Mathf.Abs(rotationCenter.x - vector6.x) / num22;
					float num24 = CurvedWorldUtilities.Smooth(num23);
					CurvedWorldUtilities.Spiral_V_Rotate_X_Positive(ref vector6, rotationCenter, num23, num24, num22, num21);
				}
				vector6 += pivotPoint;
				return vector6;
			}
			case BEND_TYPE.SpiralVertical_Z_Positive:
			{
				Vector3 vector7 = vertex;
				vector7 -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector7.z > rotationCenter.z)
				{
					rotationCenter.y = ((Mathf.Abs(rotationCenter.y) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.y)) : rotationCenter.y);
					float y3 = rotationCenter.y;
					float num25 = bendAngle * CurvedWorldUtilities.Sign(y3);
					float num26 = 6.2831855f * y3 * (num25 / 360f);
					float num27 = Mathf.Abs(rotationCenter.z - vector7.z) / num26;
					float num28 = CurvedWorldUtilities.Smooth(num27);
					CurvedWorldUtilities.Spiral_V_Rotate_Z_Positive(ref vector7, rotationCenter, num27, num28, num26, num25);
				}
				vector7 += pivotPoint;
				return vector7;
			}
			case BEND_TYPE.SpiralVertical_Z_Negative:
			{
				Vector3 vector8 = vertex;
				vector8 -= pivotPoint;
				rotationCenter -= pivotPoint;
				if (vector8.z < rotationCenter.z)
				{
					rotationCenter.y = ((Mathf.Abs(rotationCenter.y) < bendMinimumRadius) ? (bendMinimumRadius * CurvedWorldUtilities.Sign(rotationCenter.y)) : rotationCenter.y);
					float y4 = rotationCenter.y;
					float num29 = bendAngle * CurvedWorldUtilities.Sign(y4);
					float num30 = 6.2831855f * y4 * (num29 / 360f);
					float num31 = Mathf.Abs(rotationCenter.z - vector8.z) / num30;
					float num32 = CurvedWorldUtilities.Smooth(num31);
					CurvedWorldUtilities.Spiral_V_Rotate_Z_Negative(ref vector8, rotationCenter, num31, num32, num30, num29);
				}
				vector8 += pivotPoint;
				return vector8;
			}
			}
			return vertex;
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x000728AC File Offset: 0x00070AAC
		private static Vector3 TransformPosition(Vector3 vertex, BEND_TYPE bendType, Vector3 pivotPoint, Vector3 rotationCenter, Vector3 rotationCenter2, float bendAngle, float bendMinimumRadius, float bendAngle2, float bendMinimumRadius2)
		{
			if (bendType <= BEND_TYPE.SpiralHorizontalDouble_Z)
			{
				if (bendType != BEND_TYPE.SpiralHorizontalDouble_X)
				{
					if (bendType == BEND_TYPE.SpiralHorizontalDouble_Z)
					{
						if (vertex.z > pivotPoint.z)
						{
							return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_Z_Positive, pivotPoint, rotationCenter2, bendAngle2, bendMinimumRadius2);
						}
						if (vertex.z < pivotPoint.z)
						{
							return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_Z_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
						}
						return vertex;
					}
				}
				else
				{
					if (vertex.x < pivotPoint.x)
					{
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_X_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
					}
					if (vertex.x > pivotPoint.x)
					{
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_X_Positive, pivotPoint, rotationCenter2, bendAngle2, bendMinimumRadius2);
					}
					return vertex;
				}
			}
			else if (bendType != BEND_TYPE.SpiralVerticalDouble_X)
			{
				if (bendType == BEND_TYPE.SpiralVerticalDouble_Z)
				{
					if (vertex.z > pivotPoint.z)
					{
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_Z_Positive, pivotPoint, rotationCenter2, bendAngle2, bendMinimumRadius2);
					}
					if (vertex.z < pivotPoint.z)
					{
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_Z_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
					}
					return vertex;
				}
			}
			else
			{
				if (vertex.x < pivotPoint.x)
				{
					return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_X_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
				}
				if (vertex.x > pivotPoint.x)
				{
					return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_X_Positive, pivotPoint, rotationCenter2, bendAngle2, bendMinimumRadius2);
				}
				return vertex;
			}
			return vertex;
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000729E0 File Offset: 0x00070BE0
		private static Vector3 TransformPosition(Vector3 vertex, BEND_TYPE bendType, Vector3 pivotPoint, Vector3 rotationCenter, float bendAngle, float bendMinimumRadius, float bendRolloff)
		{
			if (bendType <= BEND_TYPE.SpiralHorizontalRolloff_Z)
			{
				if (bendType != BEND_TYPE.SpiralHorizontalRolloff_X)
				{
					if (bendType == BEND_TYPE.SpiralHorizontalRolloff_Z)
					{
						if (vertex.z > rotationCenter.z + bendRolloff)
						{
							rotationCenter.z += bendRolloff;
							return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_Z_Positive, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
						}
						if (vertex.z < rotationCenter.z - bendRolloff)
						{
							rotationCenter.z -= bendRolloff;
							return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_Z_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
						}
						return vertex;
					}
				}
				else
				{
					if (vertex.x < rotationCenter.x - bendRolloff)
					{
						rotationCenter.x -= bendRolloff;
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_X_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
					}
					if (vertex.x > rotationCenter.x + bendRolloff)
					{
						rotationCenter.x += bendRolloff;
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralHorizontal_X_Positive, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
					}
					return vertex;
				}
			}
			else if (bendType != BEND_TYPE.SpiralVerticalRolloff_X)
			{
				if (bendType == BEND_TYPE.SpiralVerticalRolloff_Z)
				{
					if (vertex.z > rotationCenter.z + bendRolloff)
					{
						rotationCenter.z += bendRolloff;
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_Z_Positive, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
					}
					if (vertex.z < rotationCenter.z - bendRolloff)
					{
						rotationCenter.z -= bendRolloff;
						return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_Z_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
					}
					return vertex;
				}
			}
			else
			{
				if (vertex.x < rotationCenter.x - bendRolloff)
				{
					rotationCenter.x -= bendRolloff;
					return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_X_Negative, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
				}
				if (vertex.x > rotationCenter.x + bendRolloff)
				{
					rotationCenter.x += bendRolloff;
					return CurvedWorldUtilities.TransformPosition(vertex, BEND_TYPE.SpiralVertical_X_Positive, pivotPoint, rotationCenter, bendAngle, bendMinimumRadius);
				}
				return vertex;
			}
			return vertex;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00072B88 File Offset: 0x00070D88
		private static Vector3 TransformPosition(Vector3 vertex, BEND_TYPE bendType, Vector3 pivotPoint, Vector3 rotationAxis, Vector3 bendSize, Vector3 bendOffset)
		{
			switch (bendType)
			{
			case BEND_TYPE.TwistedSpiral_X_Positive:
			{
				Vector3 vector = vertex;
				vector -= pivotPoint;
				float num = Mathf.Max(0f, vector.x - bendOffset.x);
				num = CurvedWorldUtilities.SmoothTwistedPositive(num, 100f);
				float num2 = bendSize.x * num;
				CurvedWorldUtilities.RotateVertex(ref vector, pivotPoint + new Vector3(bendOffset.x, 0f, 0f), rotationAxis, num2);
				float num3 = Mathf.Max(0f, vector.x - bendOffset.y);
				float num4 = Mathf.Max(0f, vector.x - bendOffset.z);
				vector += new Vector3(0f, bendSize.y * num3 * num3, -bendSize.z * num4 * num4) * 0.001f;
				vector += pivotPoint;
				return vector;
			}
			case BEND_TYPE.TwistedSpiral_X_Negative:
			{
				Vector3 vector2 = vertex;
				vector2 -= pivotPoint;
				float num5 = Mathf.Min(0f, vector2.x + bendOffset.x);
				num5 = CurvedWorldUtilities.SmoothTwistedNegative(num5, -100f);
				float num6 = bendSize.x * num5;
				CurvedWorldUtilities.RotateVertex(ref vector2, pivotPoint - new Vector3(bendOffset.x, 0f, 0f), rotationAxis, num6);
				float num7 = Mathf.Min(0f, vector2.x + bendOffset.y);
				float num8 = Mathf.Min(0f, vector2.x + bendOffset.z);
				vector2 += new Vector3(0f, bendSize.y * num7 * num7, bendSize.z * num8 * num8) * 0.001f;
				vector2 += pivotPoint;
				return vector2;
			}
			case BEND_TYPE.TwistedSpiral_Z_Positive:
			{
				Vector3 vector3 = vertex;
				vector3 -= pivotPoint;
				float num9 = Mathf.Max(0f, vector3.z - bendOffset.x);
				num9 = CurvedWorldUtilities.SmoothTwistedPositive(num9, 100f);
				float num10 = bendSize.x * num9;
				CurvedWorldUtilities.RotateVertex(ref vector3, pivotPoint + new Vector3(0f, 0f, bendOffset.x), rotationAxis, num10);
				float num11 = Mathf.Max(0f, vector3.z - bendOffset.z);
				float num12 = Mathf.Max(0f, vector3.z - bendOffset.y);
				vector3 += new Vector3(bendSize.z * num11 * num11, bendSize.y * num12 * num12, 0f) * 0.001f;
				vector3 += pivotPoint;
				return vector3;
			}
			case BEND_TYPE.TwistedSpiral_Z_Negative:
			{
				Vector3 vector4 = vertex;
				vector4 -= pivotPoint;
				float num13 = Mathf.Min(0f, vector4.z + bendOffset.x);
				num13 = CurvedWorldUtilities.SmoothTwistedNegative(num13, -100f);
				float num14 = bendSize.x * num13;
				CurvedWorldUtilities.RotateVertex(ref vector4, pivotPoint - new Vector3(0f, 0f, bendOffset.x), rotationAxis, num14);
				float num15 = Mathf.Min(0f, vector4.z + bendOffset.z);
				float num16 = Mathf.Min(0f, vector4.z + bendOffset.y);
				vector4 += new Vector3(-bendSize.z * num15 * num15, bendSize.y * num16 * num16, 0f) * 0.001f;
				vector4 += pivotPoint;
				return vector4;
			}
			default:
				return vertex;
			}
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00072F2C File Offset: 0x0007112C
		private static float Smooth(float x)
		{
			float num = Mathf.Cos(x * 1.5707964f);
			return 1f - num * num;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x00072F50 File Offset: 0x00071150
		private static float SmoothTwistedPositive(float x, float scale)
		{
			float num = x / scale;
			float num2 = num * num;
			float num3 = Mathf.Lerp(num2, num, num2) * scale;
			if (x >= scale)
			{
				return x;
			}
			return num3;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00072F78 File Offset: 0x00071178
		private static float SmoothTwistedNegative(float x, float scale)
		{
			float num = x / scale;
			float num2 = num * num;
			float num3 = Mathf.Lerp(num2, num, num2) * scale;
			if (x >= scale)
			{
				return num3;
			}
			return x;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00072F9F File Offset: 0x0007119F
		private static float Sign(float a)
		{
			if (a >= 0f)
			{
				return 1f;
			}
			return -1f;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00072FB4 File Offset: 0x000711B4
		private static float Abs(float a)
		{
			if (a > 0f)
			{
				return a;
			}
			if (a < 0f)
			{
				return -a;
			}
			return 0f;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00072FD0 File Offset: 0x000711D0
		private static void RotateVertex(ref Vector3 vertex, Vector3 pivot, Vector3 axis, float angle)
		{
			angle *= 0.008726646f;
			float num = Mathf.Sin(angle);
			float num2 = Mathf.Cos(angle);
			Vector3 vector = axis * num;
			vertex -= pivot;
			vertex += Vector3.Cross(vector, Vector3.Cross(vector, vertex) + vertex * num2) * 2f;
			vertex += pivot;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00073060 File Offset: 0x00071260
		private static void Spiral_H_Rotate_X_Positive(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.x = pivot.x;
				vertex.y += pivot.y * smoothValue;
			}
			else
			{
				vertex.x += l;
				vertex.y += pivot.y;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(0f, 1f, 0f), angle * Mathf.Clamp01(absoluteValue));
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x000730D8 File Offset: 0x000712D8
		private static void Spiral_H_Rotate_X_Negative(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.x = pivot.x;
				vertex.y += pivot.y * smoothValue;
			}
			else
			{
				vertex.x += -l;
				vertex.y += pivot.y;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(0f, -1f, 0f), angle * Mathf.Clamp01(absoluteValue));
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00073150 File Offset: 0x00071350
		private static void Spiral_H_Rotate_Z_Positive(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.z = pivot.z;
				vertex.y += pivot.y * smoothValue;
			}
			else
			{
				vertex.z += -l;
				vertex.y += pivot.y;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(0f, 1f, 0f), angle * Mathf.Clamp01(absoluteValue));
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x000731C8 File Offset: 0x000713C8
		private static void Spiral_H_Rotate_Z_Negative(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.z = pivot.z;
				vertex.y += pivot.y * smoothValue;
			}
			else
			{
				vertex.z += l;
				vertex.y += pivot.y;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(0f, -1f, 0f), angle * Mathf.Clamp01(absoluteValue));
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00073240 File Offset: 0x00071440
		private static void Spiral_V_Rotate_X_Positive(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.x = pivot.x;
				vertex.z += pivot.z * smoothValue;
			}
			else
			{
				vertex.x += l;
				vertex.z += pivot.z;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(0f, 0f, -1f), angle * Mathf.Clamp01(absoluteValue));
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x000732B8 File Offset: 0x000714B8
		private static void Spiral_V_Rotate_X_Negative(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.x = pivot.x;
				vertex.z += pivot.z * smoothValue;
			}
			else
			{
				vertex.x += -l;
				vertex.z += pivot.z;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(0f, 0f, 1f), angle * Mathf.Clamp01(absoluteValue));
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00073330 File Offset: 0x00071530
		private static void Spiral_V_Rotate_Z_Positive(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.z = pivot.z;
				vertex.x += pivot.x * smoothValue;
			}
			else
			{
				vertex.z += -l;
				vertex.x += pivot.x;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(-1f, 0f, 0f), angle * Mathf.Clamp01(absoluteValue));
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x000733A8 File Offset: 0x000715A8
		private static void Spiral_V_Rotate_Z_Negative(ref Vector3 vertex, Vector3 pivot, float absoluteValue, float smoothValue, float l, float angle)
		{
			if (absoluteValue < 1f)
			{
				vertex.z = pivot.z;
				vertex.x += pivot.x * smoothValue;
			}
			else
			{
				vertex.z += l;
				vertex.x += pivot.x;
			}
			CurvedWorldUtilities.RotateVertex(ref vertex, pivot, new Vector3(1f, 0f, 0f), angle * Mathf.Clamp01(absoluteValue));
		}
	}
}
