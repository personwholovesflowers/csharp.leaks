using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class PathTrack : HammerEntity
{
	// Token: 0x06000791 RID: 1937 RVA: 0x0002696E File Offset: 0x00024B6E
	private void OnDrawGizmos()
	{
		if (this.nextPathTrack != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(base.transform.position, this.nextPathTrack.transform.position);
		}
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x000269A8 File Offset: 0x00024BA8
	private void OnValidate()
	{
		if (this.nextPathTrack == null || this.nextPathTrack.name != this.nextPath)
		{
			GameObject gameObject = GameObject.Find(this.nextPath);
			if (gameObject)
			{
				this.nextPathTrack = gameObject.GetComponent<PathTrack>();
				return;
			}
			this.nextPathTrack = null;
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00026A03 File Offset: 0x00024C03
	public void Passed()
	{
		base.FireOutput(Outputs.OnPass);
	}

	// Token: 0x040007B4 RID: 1972
	public string nextPath = "";

	// Token: 0x040007B5 RID: 1973
	public PathTrack nextPathTrack;

	// Token: 0x040007B6 RID: 1974
	public float newSpeed;

	// Token: 0x040007B7 RID: 1975
	public PathTrack.OrientationTypes orientationType;

	// Token: 0x040007B8 RID: 1976
	private bool checkedRecently;

	// Token: 0x020003DE RID: 990
	public enum OrientationTypes
	{
		// Token: 0x0400144A RID: 5194
		NoChange,
		// Token: 0x0400144B RID: 5195
		DirectionOfMotion,
		// Token: 0x0400144C RID: 5196
		ThisTracksAngles
	}
}
