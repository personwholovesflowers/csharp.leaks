using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class DoorController : MonoBehaviour
{
	// Token: 0x0600054C RID: 1356 RVA: 0x00023CEB File Offset: 0x00021EEB
	private void Start()
	{
		this.dc = base.transform.parent.GetComponentInChildren<Door>();
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00023D04 File Offset: 0x00021F04
	private void OnDrawGizmos()
	{
		Collider component = base.GetComponent<Collider>();
		if (!component)
		{
			return;
		}
		Bounds bounds = component.bounds;
		Gizmos.color = new Color(0.2f, 0.2f, 1f, 1f);
		Gizmos.DrawWireCube(bounds.center, bounds.size);
		Gizmos.color = new Color(0.2f, 0.2f, 1f, 0.15f);
		Gizmos.DrawCube(bounds.center, bounds.size);
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00023D8A File Offset: 0x00021F8A
	private void OnDisable()
	{
		if (this.playerIn && this.open && !this.dc.locked && this.type == 0)
		{
			this.Close();
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00023DB8 File Offset: 0x00021FB8
	private void Update()
	{
		if ((this.playerIn || this.enemyIn) && !this.open && !this.dc.locked)
		{
			this.open = true;
			if (this.reverseDirection)
			{
				this.dc.reverseDirection = true;
			}
			else
			{
				this.dc.reverseDirection = false;
			}
			if (this.playerIn)
			{
				this.dc.Optimize();
			}
			if (this.type == 0)
			{
				if (!this.playerIn)
				{
					this.dc.Open(true, false);
				}
				else
				{
					this.dc.Open(false, false);
				}
			}
			else if (this.type == 1)
			{
				if (!this.playerIn)
				{
					this.dc.Open(true, false);
				}
				else
				{
					this.dc.Open(false, false);
				}
				Object.Destroy(this);
			}
			else if (this.type == 2)
			{
				this.dc.Close(false);
				Object.Destroy(this);
			}
		}
		else if (this.open && !this.dc.locked && !this.playerIn && !this.enemyIn)
		{
			this.Close();
		}
		if (this.enemyIn && this.doorUsers.Count > 0)
		{
			for (int i = this.doorUsers.Count - 1; i >= 0; i--)
			{
				if (this.doorUsers[i] == null || this.doorUsers[i].dead || !this.doorUsers[i].gameObject.activeInHierarchy)
				{
					this.doorUsers.RemoveAt(i);
				}
				if (this.doorUsers.Count <= 0)
				{
					this.enemyIn = false;
					if (!this.playerIn)
					{
						this.Close();
					}
				}
			}
		}
		if (!this.playerIn && !this.enemyIn && this.dc.transform.localPosition == this.dc.closedPos)
		{
			this.open = false;
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00023FB8 File Offset: 0x000221B8
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.playerIn = true;
			return;
		}
		if (other.gameObject.CompareTag("Enemy") && !this.open)
		{
			EnemyIdentifier component = other.gameObject.GetComponent<EnemyIdentifier>();
			if (component != null && !component.dead && component.enemyClass == EnemyClass.Machine && component.enemyType != EnemyType.Drone && !this.doorUsers.Contains(component))
			{
				this.enemyIn = true;
				this.doorUsers.Add(component);
			}
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0002404C File Offset: 0x0002224C
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.playerIn = false;
			return;
		}
		if (other.gameObject.CompareTag("Enemy"))
		{
			EnemyIdentifier component = other.gameObject.GetComponent<EnemyIdentifier>();
			if (component != null && component.enemyClass == EnemyClass.Machine && component.enemyType != EnemyType.Drone)
			{
				if (this.doorUsers.Contains(component))
				{
					this.doorUsers.Remove(component);
				}
				if (this.doorUsers.Count <= 0)
				{
					this.enemyIn = false;
				}
			}
		}
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x000240DC File Offset: 0x000222DC
	public void Close()
	{
		this.open = false;
		this.dc.Close(false);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x000240F1 File Offset: 0x000222F1
	public void ForcePlayerOut()
	{
		this.playerIn = false;
	}

	// Token: 0x0400075D RID: 1885
	public int type;

	// Token: 0x0400075E RID: 1886
	private Door dc;

	// Token: 0x0400075F RID: 1887
	private bool open;

	// Token: 0x04000760 RID: 1888
	private bool playerIn;

	// Token: 0x04000761 RID: 1889
	public bool enemyIn;

	// Token: 0x04000762 RID: 1890
	public bool reverseDirection;

	// Token: 0x04000763 RID: 1891
	public bool dontDeactivateOnAltarControl;

	// Token: 0x04000764 RID: 1892
	public List<EnemyIdentifier> doorUsers = new List<EnemyIdentifier>();

	// Token: 0x04000765 RID: 1893
	private List<EnemyIdentifier> doorUsersToDelete = new List<EnemyIdentifier>();
}
