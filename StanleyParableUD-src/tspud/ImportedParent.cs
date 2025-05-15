using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class ImportedParent : MonoBehaviour
{
	// Token: 0x06000A6D RID: 2669 RVA: 0x000310A4 File Offset: 0x0002F2A4
	public void Cleanup()
	{
		List<ImportedObject> list = new List<ImportedObject>();
		for (int i = 0; i < this.importedObjects.Count; i++)
		{
			if (this.importedObjects[i])
			{
				list.Add(this.importedObjects[i]);
			}
		}
		this.importedObjects = list;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x000310FC File Offset: 0x0002F2FC
	public GameObject Find(string name)
	{
		for (int i = 0; i < this.importedObjects.Count; i++)
		{
			if (this.importedObjects[i].gameObject.name == name)
			{
				return this.importedObjects[i].gameObject;
			}
		}
		return null;
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00031150 File Offset: 0x0002F350
	public GameObject[] FindWild(string name)
	{
		List<GameObject> list = new List<GameObject>();
		if (name[name.Length - 1] == '*')
		{
			string text = name.Substring(0, name.Length - 1);
			for (int i = 0; i < this.importedObjects.Count; i++)
			{
				if (this.importedObjects[i].gameObject.name.Substring(0, this.importedObjects[i].gameObject.name.Length - 1) == text)
				{
					list.Add(this.importedObjects[i].gameObject);
				}
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	// Token: 0x04000A61 RID: 2657
	[HideInInspector]
	public List<ImportedObject> importedObjects = new List<ImportedObject>();
}
