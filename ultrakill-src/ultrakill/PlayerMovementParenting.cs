using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000348 RID: 840
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class PlayerMovementParenting : MonoSingleton<PlayerMovementParenting>
{
	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06001355 RID: 4949 RVA: 0x0009B93D File Offset: 0x00099B3D
	// (set) Token: 0x06001356 RID: 4950 RVA: 0x0009B945 File Offset: 0x00099B45
	public Vector3 currentDelta { get; private set; }

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06001357 RID: 4951 RVA: 0x0009B94E File Offset: 0x00099B4E
	public List<Transform> TrackedObjects
	{
		get
		{
			return this.trackedObjects;
		}
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x0009B956 File Offset: 0x00099B56
	private new void Awake()
	{
		if (this.deltaReceiver == null)
		{
			this.deltaReceiver = base.transform;
		}
		if (!this.rb)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0009B98C File Offset: 0x00099B8C
	private void FixedUpdate()
	{
		this.currentDelta = Vector3.zero;
		if (this.playerTracker == null)
		{
			return;
		}
		if (!MonoSingleton<NewMovement>.Instance.enabled)
		{
			this.DetachPlayer(null);
			return;
		}
		Vector3 position = this.playerTracker.transform.position;
		float y = this.playerTracker.transform.eulerAngles.y;
		Vector3 vector = position - this.lastTrackedPos;
		this.lastTrackedPos = position;
		bool flag = true;
		if (MonoSingleton<NewMovement>.Instance && MonoSingleton<NewMovement>.Instance.groundProperties && MonoSingleton<NewMovement>.Instance.groundProperties.dontRotateCamera)
		{
			flag = false;
		}
		float num = y - this.lastAngle;
		this.lastAngle = y;
		float num2 = Mathf.Abs(num);
		if (num2 > 180f)
		{
			num2 = 360f - num2;
		}
		if (num2 > 5f)
		{
			if (PlayerParentingDebug.Active)
			{
				PlayerMovementParenting.Log.Fine(string.Format("Angle delta too high: {0} degrees", num2), null, null, null);
			}
			this.DetachPlayer(null);
			return;
		}
		if (vector.magnitude > 2f)
		{
			if (PlayerParentingDebug.Active)
			{
				PlayerMovementParenting.Log.Fine(string.Format("Position delta too high: {0} units", vector.magnitude), null, null, null);
			}
			this.DetachPlayer(null);
			return;
		}
		if (this.rb)
		{
			this.rb.MovePosition(this.rb.position + vector);
		}
		else
		{
			this.deltaReceiver.position += vector;
		}
		this.playerTracker.transform.position = this.deltaReceiver.position;
		this.lastTrackedPos = this.playerTracker.transform.position;
		this.currentDelta = vector;
		if (flag)
		{
			MonoSingleton<CameraController>.Instance.rotationY += num;
		}
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x0009BB67 File Offset: 0x00099D67
	public bool IsPlayerTracking()
	{
		return this.playerTracker != null;
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x0009BB75 File Offset: 0x00099D75
	public bool IsObjectTracked(Transform other)
	{
		return this.trackedObjects.Contains(other);
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0009BB84 File Offset: 0x00099D84
	public void AttachPlayer(Transform other)
	{
		if (this.lockParent)
		{
			return;
		}
		this.trackedObjects.Add(other);
		GameObject gameObject = new GameObject("Player Position Proxy")
		{
			transform = 
			{
				parent = other,
				position = this.deltaReceiver.position,
				rotation = this.deltaReceiver.rotation
			}
		};
		this.lastTrackedPos = gameObject.transform.position;
		this.lastAngle = gameObject.transform.eulerAngles.y;
		if (this.playerTracker != null)
		{
			Object.Destroy(this.playerTracker.gameObject);
		}
		this.playerTracker = gameObject.transform;
		this.ClearTrackedNulls();
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x0009BC40 File Offset: 0x00099E40
	public void DetachPlayer([CanBeNull] Transform other = null)
	{
		if (this.lockParent)
		{
			return;
		}
		if (other == null)
		{
			this.trackedObjects.Clear();
		}
		else
		{
			this.trackedObjects.Remove(other);
		}
		if (this.trackedObjects.Count == 0)
		{
			Object.Destroy(this.playerTracker.gameObject);
			this.playerTracker = null;
			return;
		}
		this.ClearTrackedNulls();
		if (this.playerTracker != null && this.trackedObjects.Count > 0)
		{
			this.playerTracker.SetParent(this.trackedObjects.First<Transform>());
		}
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x0009BCD8 File Offset: 0x00099ED8
	private void ClearTrackedNulls()
	{
		for (int i = this.trackedObjects.Count - 1; i >= 0; i--)
		{
			if (this.trackedObjects[i] == null)
			{
				this.trackedObjects.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0009BD1D File Offset: 0x00099F1D
	public void LockMovementParent(bool fuck)
	{
		this.lockParent = fuck;
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x0009BD28 File Offset: 0x00099F28
	public void LockMovementParentTeleport(bool fuck)
	{
		if (this.playerTracker)
		{
			if (fuck)
			{
				this.teleportLockDelta = this.lastTrackedPos - this.playerTracker.position;
			}
			if (this.lockParent && !fuck)
			{
				this.lastTrackedPos = this.playerTracker.position - this.teleportLockDelta;
			}
		}
		else
		{
			this.teleportLockDelta = this.lastTrackedPos;
		}
		this.lockParent = fuck;
	}

	// Token: 0x04001AAD RID: 6829
	private static readonly global::plog.Logger Log = new global::plog.Logger("PlayerMovementParenting");

	// Token: 0x04001AAF RID: 6831
	public Transform deltaReceiver;

	// Token: 0x04001AB0 RID: 6832
	private Vector3 lastTrackedPos;

	// Token: 0x04001AB1 RID: 6833
	private float lastAngle;

	// Token: 0x04001AB2 RID: 6834
	private Transform playerTracker;

	// Token: 0x04001AB3 RID: 6835
	[HideInInspector]
	public bool lockParent;

	// Token: 0x04001AB4 RID: 6836
	private Vector3 teleportLockDelta;

	// Token: 0x04001AB5 RID: 6837
	private Rigidbody rb;

	// Token: 0x04001AB6 RID: 6838
	private List<Transform> trackedObjects = new List<Transform>();
}
