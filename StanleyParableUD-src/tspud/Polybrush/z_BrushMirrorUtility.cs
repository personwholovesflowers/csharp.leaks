using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x02000211 RID: 529
	public static class z_BrushMirrorUtility
	{
		// Token: 0x06000C24 RID: 3108 RVA: 0x00036364 File Offset: 0x00034564
		public static Vector3 ToVector3(this z_BrushMirror mirror)
		{
			Vector3 one = Vector3.one;
			if ((mirror & z_BrushMirror.X) > z_BrushMirror.None)
			{
				one.x = -1f;
			}
			if ((mirror & z_BrushMirror.Y) > z_BrushMirror.None)
			{
				one.y = -1f;
			}
			if ((mirror & z_BrushMirror.Z) > z_BrushMirror.None)
			{
				one.z = -1f;
			}
			return one;
		}
	}
}
