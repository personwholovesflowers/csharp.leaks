using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200025F RID: 607
	[Serializable]
	public class MB3_MeshBakerGrouperNone : MB3_MeshBakerGrouperCore
	{
		// Token: 0x06000E6F RID: 3695 RVA: 0x00045FC0 File Offset: 0x000441C0
		public MB3_MeshBakerGrouperNone(GrouperData d)
		{
			this.d = d;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00045FD0 File Offset: 0x000441D0
		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			Debug.Log("Filtering into groups none");
			Dictionary<string, List<Renderer>> dictionary = new Dictionary<string, List<Renderer>>();
			List<Renderer> list = new List<Renderer>();
			for (int i = 0; i < selection.Count; i++)
			{
				if (selection[i] != null)
				{
					list.Add(selection[i].GetComponent<Renderer>());
				}
			}
			dictionary.Add("MeshBaker", list);
			return dictionary;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00005444 File Offset: 0x00003644
		public override void DrawGizmos(Bounds sourceObjectBounds)
		{
		}
	}
}
