using System;
using UnityEngine;

namespace Train
{
	// Token: 0x02000528 RID: 1320
	public class TramPath
	{
		// Token: 0x06001E06 RID: 7686 RVA: 0x000F9E38 File Offset: 0x000F8038
		public TramPath(TrainTrackPoint start, bool forward)
		{
			this.start = start;
			this.end = start.GetDestination(forward);
			this.DistanceTotal = this.CalculateFullDistance();
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x000F9E60 File Offset: 0x000F8060
		public TramPath(TrainTrackPoint start, TrainTrackPoint end)
		{
			this.start = start;
			this.end = end;
			this.DistanceTotal = this.CalculateFullDistance();
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000F9E82 File Offset: 0x000F8082
		private float CalculateFullDistance()
		{
			return this.CalculateFullDistance(this.start, this.end);
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000F9E98 File Offset: 0x000F8098
		private float CalculateFullDistance(TrainTrackPoint startPoint, TrainTrackPoint endPoint)
		{
			PathInterpolation curve = startPoint.forwardCurveSettings.curve;
			if (curve == PathInterpolation.Linear)
			{
				return Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
			}
			if (curve - PathInterpolation.SphericalManual > 1)
			{
				return 0f;
			}
			float num = 0f;
			Vector3 vector = startPoint.transform.position;
			for (int i = 0; i < 16; i++)
			{
				Vector3 pointOnSimulatedPath = TramPath.GetPointOnSimulatedPath((float)i / 16f, startPoint, endPoint);
				float num2 = Vector3.Distance(vector, pointOnSimulatedPath);
				vector = pointOnSimulatedPath;
				if (i > 0)
				{
					num += num2;
				}
			}
			return num;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000F9F25 File Offset: 0x000F8125
		public Vector3 GetPointOnPath(float progress)
		{
			return TramPath.GetPointOnSimulatedPath(progress, this.start, this.end);
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000F9F3C File Offset: 0x000F813C
		public static Vector3 GetPointOnSimulatedPath(float progress, TrainTrackPoint startPoint, TrainTrackPoint endPoint)
		{
			Vector3 position = startPoint.transform.position;
			Vector3 position2 = endPoint.transform.position;
			TrackCurveSettings forwardCurveSettings = startPoint.forwardCurveSettings;
			PathInterpolation curve = forwardCurveSettings.curve;
			Vector3 vector4;
			if (curve != PathInterpolation.SphericalManual)
			{
				if (curve == PathInterpolation.SphericalAutomatic)
				{
					float angle = forwardCurveSettings.angle;
					bool flipCurve = forwardCurveSettings.flipCurve;
					Vector3 vector = position;
					Vector3 vector2 = position2;
					float num = angle;
					Vector3 vector3 = PathTools.ComputeSphericalCurveCenter(vector, vector2, flipCurve, num);
					vector4 = PathTools.InterpolateAlongCircle(position, position2, vector3, progress);
				}
				else
				{
					vector4 = Vector3.Lerp(position, position2, progress);
				}
			}
			else
			{
				Transform handle = forwardCurveSettings.handle;
				vector4 = PathTools.InterpolateAlongCircle(position, position2, handle.position, progress);
			}
			return vector4;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000F9FCC File Offset: 0x000F81CC
		public float MaxSpeedMultiplier(TramMovementDirection direction, float speed)
		{
			if (!this.IsDeadEnd(direction))
			{
				return 1f;
			}
			if (this.GetNextPoint(direction).stopBehaviour != StopBehaviour.EaseOut)
			{
				return 1f;
			}
			float num = 1.5f;
			float num2 = 0.85f;
			float num3 = ((direction == TramMovementDirection.Forward) ? (this.DistanceTotal - this.distanceTravelled) : this.distanceTravelled);
			num3 = Mathf.Abs(num3);
			if (num3 < speed * num)
			{
				return Mathf.Clamp(Mathf.Pow(num3 / (speed * num), num2), 0.0125f, 1f);
			}
			return 1f;
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000FA050 File Offset: 0x000F8250
		private Vector3 CalculateCurrentMovementDirection()
		{
			float num = this.Progress + 0.05f / this.DistanceTotal;
			Vector3 vector;
			if (num > 1f)
			{
				TrainTrackPoint destination = this.end.GetDestination(true);
				if (destination != null)
				{
					float num2 = num - 1f;
					float num3 = this.CalculateFullDistance(this.end, destination);
					vector = TramPath.GetPointOnSimulatedPath(num2 * this.DistanceTotal / num3, this.end, destination);
				}
				else
				{
					vector = this.GetPointOnPath(Mathf.Clamp01(num));
				}
			}
			else
			{
				vector = this.GetPointOnPath(num);
			}
			float num4 = this.Progress - 0.05f / this.DistanceTotal;
			Vector3 vector2;
			if (num4 < 0f)
			{
				TrainTrackPoint destination2 = this.start.GetDestination(false);
				if (destination2 != null)
				{
					float num5 = -num4;
					float num6 = this.CalculateFullDistance(destination2, this.start);
					float num7 = num5 * this.DistanceTotal / num6;
					vector2 = TramPath.GetPointOnSimulatedPath(1f - num7, destination2, this.start);
				}
				else
				{
					vector2 = this.GetPointOnPath(Mathf.Clamp01(num4));
				}
			}
			else
			{
				vector2 = this.GetPointOnPath(num4);
			}
			return (vector - vector2).normalized;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000FA16C File Offset: 0x000F836C
		public string PrintPathDirectional(TramMovementDirection direction)
		{
			if (direction == TramMovementDirection.None)
			{
				return string.Concat(new string[]
				{
					"(",
					this.start.gameObject.name,
					") --- (",
					this.end.gameObject.name,
					")"
				});
			}
			if (direction == TramMovementDirection.Forward)
			{
				return string.Concat(new string[]
				{
					"(",
					this.start.gameObject.name,
					") --> (",
					this.end.gameObject.name,
					")"
				});
			}
			return string.Concat(new string[]
			{
				"(",
				this.start.gameObject.name,
				") <-- (",
				this.end.gameObject.name,
				")"
			});
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000FA25D File Offset: 0x000F845D
		public bool IsDeadEnd(TramMovementDirection direction)
		{
			return direction != TramMovementDirection.None && this.GetNextPoint(direction).GetDestination(direction == TramMovementDirection.Forward) == null;
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000FA27B File Offset: 0x000F847B
		public TrainTrackPoint GetNextPoint(TramMovementDirection direction)
		{
			if (direction == TramMovementDirection.None)
			{
				return null;
			}
			if (direction != TramMovementDirection.Forward)
			{
				return this.start;
			}
			return this.end;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001E11 RID: 7697 RVA: 0x000FA293 File Offset: 0x000F8493
		// (set) Token: 0x06001E12 RID: 7698 RVA: 0x000FA29B File Offset: 0x000F849B
		public float DistanceTotal { get; private set; }

		// Token: 0x06001E13 RID: 7699 RVA: 0x000FA2A4 File Offset: 0x000F84A4
		public Vector3 MovementDirection()
		{
			return this.CalculateCurrentMovementDirection();
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001E14 RID: 7700 RVA: 0x000FA2AC File Offset: 0x000F84AC
		public float Progress
		{
			get
			{
				if (this.distanceTravelled != 0f || this.DistanceTotal != 0f)
				{
					return this.distanceTravelled / this.DistanceTotal;
				}
				return 0f;
			}
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000FA2DC File Offset: 0x000F84DC
		public override bool Equals(object obj)
		{
			TramPath tramPath = obj as TramPath;
			return tramPath != null && this.start == tramPath.start && this.end == tramPath.end;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000FA31C File Offset: 0x000F851C
		public override int GetHashCode()
		{
			return new ValueTuple<TrainTrackPoint, TrainTrackPoint>(this.start, this.end).GetHashCode();
		}

		// Token: 0x04002A82 RID: 10882
		private const int CurveDistanceCalculationSteps = 16;

		// Token: 0x04002A83 RID: 10883
		private const float TramDirectionCalcStepLength = 0.05f;

		// Token: 0x04002A84 RID: 10884
		private const float MinSpeedMultiplier = 0.0125f;

		// Token: 0x04002A85 RID: 10885
		public readonly TrainTrackPoint start;

		// Token: 0x04002A86 RID: 10886
		public readonly TrainTrackPoint end;

		// Token: 0x04002A88 RID: 10888
		public float distanceTravelled;
	}
}
