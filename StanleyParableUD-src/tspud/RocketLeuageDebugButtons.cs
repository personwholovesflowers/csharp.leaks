using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class RocketLeuageDebugButtons : MonoBehaviour
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0002A20E File Offset: 0x0002840E
	// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0002A215 File Offset: 0x00028415
	public static RocketLeuageDebugButtons Instance { get; private set; }

	// Token: 0x060008D9 RID: 2265 RVA: 0x0002A21D File Offset: 0x0002841D
	private void Awake()
	{
		RocketLeuageDebugButtons.Instance = this;
		this.registeredObjects = new Dictionary<string, List<GameObject>>();
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0002A230 File Offset: 0x00028430
	public void RegisterObject(string id, GameObject go)
	{
		if (!this.registeredObjects.ContainsKey(id))
		{
			this.registeredObjects[id] = new List<GameObject>();
		}
		this.registeredObjects[id].Add(go);
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0002A264 File Offset: 0x00028464
	private void OnGUI()
	{
		if (this.registeredObjects == null)
		{
			return;
		}
		if (this.guiStyle == null)
		{
			this.guiStyle = new GUIStyle("button");
		}
		GUILayout.Space(this.leadingSpace);
		this.guiStyle.fontSize = this.fontSize * Screen.width / 1920;
		foreach (KeyValuePair<string, List<GameObject>> keyValuePair in this.registeredObjects)
		{
			if (GUILayout.Button(keyValuePair.Key, this.guiStyle, Array.Empty<GUILayoutOption>()))
			{
				foreach (GameObject gameObject in keyValuePair.Value)
				{
					gameObject.SetActive(!gameObject.activeInHierarchy);
				}
			}
		}
	}

	// Token: 0x040008A5 RID: 2213
	private Dictionary<string, List<GameObject>> registeredObjects;

	// Token: 0x040008A6 RID: 2214
	public float leadingSpace = 100f;

	// Token: 0x040008A7 RID: 2215
	public int fontSize = 20;

	// Token: 0x040008A8 RID: 2216
	public bool scaleByResolutionWidth = true;

	// Token: 0x040008A9 RID: 2217
	private GUIStyle guiStyle;
}
