using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class GameState
{
	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00051ED4 File Offset: 0x000500D4
	[CanBeNull]
	public GameObject trackedObject { get; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00051EDC File Offset: 0x000500DC
	[CanBeNull]
	public GameObject[] trackedObjects { get; }

	// Token: 0x06000B95 RID: 2965 RVA: 0x00051EE4 File Offset: 0x000500E4
	public GameState(string key, GameObject trackedObject)
	{
		this.key = key;
		this.trackedObject = trackedObject;
		this.tracked = trackedObject != null;
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x00051F0E File Offset: 0x0005010E
	public GameState(string key, GameObject[] trackedObjects)
	{
		this.key = key;
		this.trackedObjects = trackedObjects;
		this.tracked = trackedObjects != null;
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00051F35 File Offset: 0x00050135
	public GameState(string key)
	{
		this.key = key;
		this.tracked = false;
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00051F54 File Offset: 0x00050154
	public bool IsValid()
	{
		if (!this.tracked)
		{
			return true;
		}
		if (this.trackedObjects != null)
		{
			foreach (GameObject gameObject in this.trackedObjects)
			{
				if (gameObject != null && gameObject.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
		}
		return this.trackedObject != null && this.trackedObject.activeInHierarchy;
	}

	// Token: 0x04000F27 RID: 3879
	public string key;

	// Token: 0x04000F2A RID: 3882
	private bool tracked;

	// Token: 0x04000F2B RID: 3883
	public LockMode playerInputLock;

	// Token: 0x04000F2C RID: 3884
	public LockMode cameraInputLock;

	// Token: 0x04000F2D RID: 3885
	public LockMode cursorLock;

	// Token: 0x04000F2E RID: 3886
	public float? timerModifier;

	// Token: 0x04000F2F RID: 3887
	public int priority = 1;
}
