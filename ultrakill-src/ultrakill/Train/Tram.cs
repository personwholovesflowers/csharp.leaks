using System;
using System.Linq;
using UnityEngine;

namespace Train
{
	// Token: 0x02000525 RID: 1317
	public class Tram : MonoBehaviour
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x000F958B File Offset: 0x000F778B
		// (set) Token: 0x06001DED RID: 7661 RVA: 0x000F9593 File Offset: 0x000F7793
		public bool canGoForward { get; private set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x000F959C File Offset: 0x000F779C
		// (set) Token: 0x06001DEF RID: 7663 RVA: 0x000F95A4 File Offset: 0x000F77A4
		public bool canGoBackward { get; private set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x000F95AD File Offset: 0x000F77AD
		public TramMovementDirection movementDirection
		{
			get
			{
				if (this.speed > 0f)
				{
					return TramMovementDirection.Forward;
				}
				if (this.speed >= 0f)
				{
					return TramMovementDirection.None;
				}
				return TramMovementDirection.Backward;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001DF1 RID: 7665 RVA: 0x000F95CE File Offset: 0x000F77CE
		public float directionMod
		{
			get
			{
				return (float)((this.speed > 0f) ? 1 : (-1));
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x000F95E2 File Offset: 0x000F77E2
		public float computedSpeed
		{
			get
			{
				return this.speed * this.inheritedSpeedMultiplier;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x000F95F4 File Offset: 0x000F77F4
		public float inheritedSpeedMultiplier
		{
			get
			{
				if (this.zapAmount > 0f)
				{
					TramPath tramPath = this.currentPath;
					return Mathf.Lerp((tramPath != null) ? tramPath.MaxSpeedMultiplier(this.movementDirection, this.speed) : 1f, 2f, this.zapAmount);
				}
				TramPath tramPath2 = this.currentPath;
				if (tramPath2 == null)
				{
					return 1f;
				}
				return tramPath2.MaxSpeedMultiplier(this.movementDirection, this.speed);
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x000F9664 File Offset: 0x000F7864
		public float backwardOffset
		{
			get
			{
				if (this.connectedTrams != null && this.connectedTrams.Length != 0)
				{
					return this.connectedTrams.Sum((ConnectedTram tram) => tram.offset);
				}
				return 0f;
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000F96B4 File Offset: 0x000F78B4
		public void TurnOn()
		{
			this.poweredOn = true;
			if (this.screenActivators != null && this.screenActivators.Length != 0)
			{
				ScreenZone[] array = this.screenActivators;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000F96FC File Offset: 0x000F78FC
		public void ShutDown()
		{
			this.poweredOn = false;
			if (this.screenActivators != null && this.screenActivators.Length != 0)
			{
				foreach (ScreenZone screenZone in this.screenActivators)
				{
					ObjectActivator[] components = screenZone.GetComponents<ObjectActivator>();
					if (components != null && components.Length != 0)
					{
						foreach (ObjectActivator objectActivator in components)
						{
							if (objectActivator.events.toActivateObjects != null && objectActivator.events.toActivateObjects.Length != 0)
							{
								GameObject[] toActivateObjects = objectActivator.events.toActivateObjects;
								for (int k = 0; k < toActivateObjects.Length; k++)
								{
									toActivateObjects[k].SetActive(false);
								}
							}
						}
					}
					screenZone.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000F97C8 File Offset: 0x000F79C8
		public void StopAndTeleport(TrainTrackPoint point)
		{
			this.currentPoint = point;
			this.currentPath = null;
			this.speed = 0f;
			TrainTrackPoint destination = this.currentPoint.GetDestination(true);
			TrainTrackPoint destination2 = this.currentPoint.GetDestination(false);
			TramPath tramPath = null;
			if (destination)
			{
				tramPath = new TramPath(this.currentPoint, destination);
			}
			else if (destination2)
			{
				tramPath = new TramPath(destination2, this.currentPoint);
				tramPath.distanceTravelled = tramPath.DistanceTotal;
			}
			if (tramPath != null)
			{
				this.currentPath = tramPath;
				this.UpdateWorldRotation();
				ConnectedTram[] array = this.connectedTrams;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].UpdateTram(this.currentPath);
				}
				this.currentPath = null;
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000F9881 File Offset: 0x000F7A81
		private void Awake()
		{
			this.aud = base.GetComponent<AudioSource>();
			this.screenActivators = base.GetComponentsInChildren<ScreenZone>();
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x000F98A7 File Offset: 0x000F7AA7
		private void Update()
		{
			this.UpdateAudio();
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x000F98B0 File Offset: 0x000F7AB0
		private void FixedUpdate()
		{
			if (this.currentPath == null && this.currentPoint != null)
			{
				this.rb.MovePosition(this.currentPoint.transform.position);
				this.canGoForward = this.currentPoint.GetDestination(true) != null;
				this.canGoBackward = this.currentPoint.GetDestination(false) != null;
			}
			if (this.speed != 0f)
			{
				if (this.currentPath == null && this.currentPoint != null)
				{
					this.ReceiveNewPath();
				}
				if (this.currentPath != null)
				{
					this.TraversePath();
				}
			}
			if (this.deathZones)
			{
				this.deathZones.SetActive(this.speed != 0f);
			}
			if (this.currentPath != null)
			{
				this.UpdateWorldPosition();
				if (this.movementDirection != TramMovementDirection.None)
				{
					this.UpdateWorldRotation();
				}
				this.DrawPathPreview();
				ConnectedTram[] array = this.connectedTrams;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].UpdateTram(this.currentPath);
				}
			}
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000F99C4 File Offset: 0x000F7BC4
		private void DrawPathPreview()
		{
			if (!Debug.isDebugBuild)
			{
				return;
			}
			int num = 16;
			Vector3 position = this.rb.position;
			for (int i = 0; i < num; i++)
			{
				float num2 = this.currentPath.Progress + (float)i / (float)num * this.directionMod;
				this.currentPath.GetPointOnPath(num2);
				Vector3.up * 1f + new Vector3(0f, 0.125f, 0f);
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000F9A44 File Offset: 0x000F7C44
		private void TraversePath()
		{
			this.currentPath.distanceTravelled += this.computedSpeed * Time.deltaTime;
			if (!this.IsAtEndOfPath())
			{
				this.canGoForward = true;
				this.canGoBackward = true;
				return;
			}
			float num = this.currentPath.distanceTravelled;
			if (this.movementDirection == TramMovementDirection.Forward)
			{
				num -= this.currentPath.DistanceTotal;
			}
			if (this.movementDirection == TramMovementDirection.Backward && this.backwardOffset != 0f && this.currentPath.IsDeadEnd(this.movementDirection))
			{
				this.speed = 0f;
				this.canGoBackward = false;
				Object.Instantiate<GameObject>(this.bonkSound, this.rb.position, Quaternion.identity);
				return;
			}
			this.currentPoint = ((this.movementDirection == TramMovementDirection.Forward) ? this.currentPath.end : this.currentPath.start);
			this.currentPoint != null;
			this.currentPath = null;
			this.ReceiveNewPath();
			if (this.currentPath == null)
			{
				if (this.currentPoint.stopBehaviour == StopBehaviour.InstantClank || this.zapAmount > 0f)
				{
					Object.Instantiate<GameObject>(this.bonkSound, this.rb.position, Quaternion.identity);
				}
				if (this.movementDirection == TramMovementDirection.Forward)
				{
					this.canGoForward = false;
				}
				else
				{
					this.canGoBackward = false;
				}
				this.speed = 0f;
				return;
			}
			this.currentPath.distanceTravelled += num;
			if (this.movementDirection == TramMovementDirection.Forward)
			{
				this.canGoForward = true;
				return;
			}
			this.canGoBackward = true;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000F9BCC File Offset: 0x000F7DCC
		private bool IsAtEndOfPath()
		{
			if (this.currentPath == null)
			{
				return false;
			}
			float distanceTotal = this.currentPath.DistanceTotal;
			float num = 0f;
			if (this.movementDirection == TramMovementDirection.Backward && this.backwardOffset != 0f && this.currentPath.start.GetDestination(false) == null)
			{
				num += this.backwardOffset;
			}
			if (this.movementDirection != TramMovementDirection.Forward)
			{
				return this.currentPath.distanceTravelled <= num;
			}
			return this.currentPath.distanceTravelled >= distanceTotal;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000F9C58 File Offset: 0x000F7E58
		public void UpdateWorldPosition()
		{
			if (this.currentPath == null)
			{
				return;
			}
			Vector3 pointOnPath = this.currentPath.GetPointOnPath(this.currentPath.Progress);
			this.rb.MovePosition(pointOnPath);
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000F9C94 File Offset: 0x000F7E94
		public void UpdateWorldRotation()
		{
			if (this.currentPath == null)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(this.currentPath.MovementDirection(), Vector3.up);
			this.rb.rotation = quaternion;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000F9CCC File Offset: 0x000F7ECC
		private void ReceiveNewPath()
		{
			if (this.currentPoint == null)
			{
				return;
			}
			bool flag = this.movementDirection == TramMovementDirection.Forward;
			TrainTrackPoint destination = this.currentPoint.GetDestination(flag);
			if (destination == null)
			{
				return;
			}
			TrainTrackPoint trainTrackPoint = (flag ? this.currentPoint : destination);
			TrainTrackPoint trainTrackPoint2 = (flag ? destination : this.currentPoint);
			TramPath tramPath = new TramPath(trainTrackPoint, trainTrackPoint2);
			if (!flag)
			{
				tramPath.distanceTravelled = tramPath.DistanceTotal;
			}
			this.currentPath = tramPath;
			this.currentPoint = null;
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000F9D48 File Offset: 0x000F7F48
		private void UpdateAudio()
		{
			if (this.computedSpeed != 0f && !this.aud.isPlaying)
			{
				this.aud.Play();
			}
			else if (this.computedSpeed == 0f && this.aud.isPlaying)
			{
				this.aud.Stop();
			}
			float num;
			if (Mathf.Abs(this.computedSpeed) >= 50f)
			{
				num = ((this.zapAmount > 0f) ? Mathf.Lerp(1f, 1.5f, this.zapAmount) : 1f);
			}
			else
			{
				num = Mathf.Abs(this.computedSpeed) * 0.02f;
			}
			this.aud.volume = num;
			this.aud.pitch = num * 2f;
		}

		// Token: 0x04002A6E RID: 10862
		public bool poweredOn = true;

		// Token: 0x04002A6F RID: 10863
		private AudioSource aud;

		// Token: 0x04002A70 RID: 10864
		public GameObject bonkSound;

		// Token: 0x04002A71 RID: 10865
		public GameObject deathZones;

		// Token: 0x04002A72 RID: 10866
		[HideInInspector]
		public float zapAmount;

		// Token: 0x04002A75 RID: 10869
		public float speed;

		// Token: 0x04002A76 RID: 10870
		public ConnectedTram[] connectedTrams;

		// Token: 0x04002A77 RID: 10871
		[Space]
		public TrainTrackPoint currentPoint;

		// Token: 0x04002A78 RID: 10872
		public TramPath currentPath;

		// Token: 0x04002A79 RID: 10873
		private ScreenZone[] screenActivators;

		// Token: 0x04002A7A RID: 10874
		[HideInInspector]
		public TramControl controller;

		// Token: 0x04002A7B RID: 10875
		private Rigidbody rb;
	}
}
