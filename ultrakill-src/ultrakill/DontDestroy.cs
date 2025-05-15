using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class DontDestroy : MonoBehaviour
{
	// Token: 0x06000536 RID: 1334 RVA: 0x000229EB File Offset: 0x00020BEB
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
