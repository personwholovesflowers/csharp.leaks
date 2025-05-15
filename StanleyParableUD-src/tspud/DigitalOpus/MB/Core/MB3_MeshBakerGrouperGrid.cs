using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000260 RID: 608
	[Serializable]
	public class MB3_MeshBakerGrouperGrid : MB3_MeshBakerGrouperCore
	{
		// Token: 0x06000E72 RID: 3698 RVA: 0x00045FC0 File Offset: 0x000441C0
		public MB3_MeshBakerGrouperGrid(GrouperData d)
		{
			this.d = d;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00046034 File Offset: 0x00044234
		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			Dictionary<string, List<Renderer>> dictionary = new Dictionary<string, List<Renderer>>();
			if (this.d.cellSize.x <= 0f || this.d.cellSize.y <= 0f || this.d.cellSize.z <= 0f)
			{
				Debug.LogError("cellSize x,y,z must all be greater than zero.");
				return dictionary;
			}
			Debug.Log("Collecting renderers in each cell");
			foreach (GameObject gameObject in selection)
			{
				if (!(gameObject == null))
				{
					Renderer component = gameObject.GetComponent<Renderer>();
					if (component is MeshRenderer || component is SkinnedMeshRenderer)
					{
						Vector3 center = component.bounds.center;
						center.x = Mathf.Floor((center.x - this.d.origin.x) / this.d.cellSize.x) * this.d.cellSize.x;
						center.y = Mathf.Floor((center.y - this.d.origin.y) / this.d.cellSize.y) * this.d.cellSize.y;
						center.z = Mathf.Floor((center.z - this.d.origin.z) / this.d.cellSize.z) * this.d.cellSize.z;
						string text = center.ToString();
						List<Renderer> list;
						if (dictionary.ContainsKey(text))
						{
							list = dictionary[text];
						}
						else
						{
							list = new List<Renderer>();
							dictionary.Add(text, list);
						}
						if (!list.Contains(component))
						{
							list.Add(component);
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00046244 File Offset: 0x00044444
		public override void DrawGizmos(Bounds sourceObjectBounds)
		{
			Vector3 cellSize = this.d.cellSize;
			if (cellSize.x <= 1E-05f || cellSize.y <= 1E-05f || cellSize.z <= 1E-05f)
			{
				return;
			}
			Vector3 vector = sourceObjectBounds.center - sourceObjectBounds.extents;
			Vector3 origin = this.d.origin;
			origin.x %= cellSize.x;
			origin.y %= cellSize.y;
			origin.z %= cellSize.z;
			vector.x = Mathf.Round(vector.x / cellSize.x) * cellSize.x + origin.x;
			vector.y = Mathf.Round(vector.y / cellSize.y) * cellSize.y + origin.y;
			vector.z = Mathf.Round(vector.z / cellSize.z) * cellSize.z + origin.z;
			if (vector.x > sourceObjectBounds.center.x - sourceObjectBounds.extents.x)
			{
				vector.x -= cellSize.x;
			}
			if (vector.y > sourceObjectBounds.center.y - sourceObjectBounds.extents.y)
			{
				vector.y -= cellSize.y;
			}
			if (vector.z > sourceObjectBounds.center.z - sourceObjectBounds.extents.z)
			{
				vector.z -= cellSize.z;
			}
			Vector3 vector2 = vector;
			if (Mathf.CeilToInt(sourceObjectBounds.size.x / cellSize.x + sourceObjectBounds.size.y / cellSize.y + sourceObjectBounds.size.z / cellSize.z) > 200)
			{
				Gizmos.DrawWireCube(this.d.origin + cellSize / 2f, cellSize);
				return;
			}
			while (vector.x < sourceObjectBounds.center.x + sourceObjectBounds.extents.x)
			{
				vector.y = vector2.y;
				while (vector.y < sourceObjectBounds.center.y + sourceObjectBounds.extents.y)
				{
					vector.z = vector2.z;
					while (vector.z < sourceObjectBounds.center.z + sourceObjectBounds.extents.z)
					{
						Gizmos.DrawWireCube(vector + cellSize / 2f, cellSize);
						vector.z += cellSize.z;
					}
					vector.y += cellSize.y;
				}
				vector.x += cellSize.x;
			}
		}
	}
}
