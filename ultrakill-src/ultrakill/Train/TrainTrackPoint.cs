using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Train
{
	// Token: 0x02000520 RID: 1312
	public class TrainTrackPoint : MonoBehaviour
	{
		// Token: 0x06001DE1 RID: 7649 RVA: 0x000F92E8 File Offset: 0x000F74E8
		public TrainTrackPoint GetDestination(bool forward = true)
		{
			TrainTrackPoint trainTrackPoint;
			if (forward)
			{
				if (this.forwardPoints != null && this.forwardPoints.Count != 0)
				{
					if (!this.forwardPoints.All((TrainTrackPoint point) => point == null))
					{
						trainTrackPoint = this.forwardPoints[this.forwardPath];
						goto IL_00AF;
					}
				}
				return null;
			}
			if (this.backwardPoints != null && this.backwardPoints.Count != 0)
			{
				if (!this.backwardPoints.All((TrainTrackPoint point) => point == null))
				{
					trainTrackPoint = this.backwardPoints[this.backwardPath];
					goto IL_00AF;
				}
			}
			return null;
			IL_00AF:
			if (trainTrackPoint == null || !trainTrackPoint.gameObject.activeSelf)
			{
				return null;
			}
			return trainTrackPoint;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000F93BD File Offset: 0x000F75BD
		private void OnDrawGizmos()
		{
			Vector3 position = base.transform.position;
			this.DrawPaths(this.forwardPoints, this.forwardPath, false);
			this.DrawPaths(this.backwardPoints, this.backwardPath, true);
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000F93F1 File Offset: 0x000F75F1
		private void Update()
		{
			this.DrawPaths(this.forwardPoints, this.forwardPath, false);
			this.DrawPaths(this.backwardPoints, this.backwardPath, true);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000F941C File Offset: 0x000F761C
		private void DrawPaths(IReadOnlyList<TrainTrackPoint> points, int path, bool backward)
		{
			Vector3 position = base.transform.position;
			if (points == null)
			{
				return;
			}
			for (int i = 0; i < points.Count; i++)
			{
				TrainTrackPoint trainTrackPoint = points[i];
				if (!(trainTrackPoint == null))
				{
					Vector3 position2 = trainTrackPoint.transform.position;
					bool flag = !trainTrackPoint.gameObject.activeSelf;
					if (path != i || flag)
					{
						Color red = Color.red;
					}
					else if (!backward)
					{
						Color forwardActive = TrainTrackPoint.ForwardActive;
					}
					else
					{
						Color backwardActive = TrainTrackPoint.BackwardActive;
					}
					TrackCurveSettings trackCurveSettings = (backward ? trainTrackPoint.forwardCurveSettings : this.forwardCurveSettings);
					if (trackCurveSettings.curve == PathInterpolation.Linear)
					{
						Vector3.Lerp(position, position2, flag ? 1f : 0.5f);
					}
					else if (trackCurveSettings.curve != PathInterpolation.SphericalManual || !(trackCurveSettings.handle == null))
					{
						int num = 16;
						TrainTrackPoint trainTrackPoint2 = (backward ? trainTrackPoint : this);
						TrainTrackPoint trainTrackPoint3 = (backward ? this : trainTrackPoint);
						for (int j = 0; j <= num; j++)
						{
							float num2 = (float)j / (float)num;
							if (!flag)
							{
								num2 *= 0.5f;
							}
							if (backward)
							{
								num2 = 1f - num2;
							}
							TramPath.GetPointOnSimulatedPath(num2, trainTrackPoint2, trainTrackPoint3);
						}
					}
				}
			}
		}

		// Token: 0x04002A57 RID: 10839
		[HideInInspector]
		public int instanceId;

		// Token: 0x04002A58 RID: 10840
		public List<TrainTrackPoint> forwardPoints;

		// Token: 0x04002A59 RID: 10841
		public List<TrainTrackPoint> backwardPoints;

		// Token: 0x04002A5A RID: 10842
		public StopBehaviour stopBehaviour;

		// Token: 0x04002A5B RID: 10843
		[HideInInspector]
		public int forwardPath;

		// Token: 0x04002A5C RID: 10844
		[HideInInspector]
		public int backwardPath;

		// Token: 0x04002A5D RID: 10845
		[HideInInspector]
		public TrackCurveSettings forwardCurveSettings;

		// Token: 0x04002A5E RID: 10846
		private static readonly Color ForwardActive = Color.green;

		// Token: 0x04002A5F RID: 10847
		private static readonly Color BackwardActive = new Color(0.4f, 0.6f, 0.5f);
	}
}
