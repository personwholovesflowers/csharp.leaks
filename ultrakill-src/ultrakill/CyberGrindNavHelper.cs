using System;
using Unity.AI.Navigation;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class CyberGrindNavHelper : MonoBehaviour
{
	// Token: 0x0600072D RID: 1837 RVA: 0x0002EA34 File Offset: 0x0002CC34
	public void ResetLinks()
	{
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0002EA44 File Offset: 0x0002CC44
	public void GenerateLinks(EndlessCube[][] cbs)
	{
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002EA54 File Offset: 0x0002CC54
	private void CheckNeighbors(EndlessCube[][] cbs, int x, int y)
	{
		bool flag = x - 1 >= 0;
		bool flag2 = x - 2 >= 0;
		bool flag3 = x + 1 < 16;
		bool flag4 = x + 2 < 16;
		bool flag5 = y - 1 >= 0;
		bool flag6 = y - 2 >= 0;
		bool flag7 = y + 1 < 16;
		bool flag8 = y + 2 < 16;
		if (flag)
		{
			this.CheckNeighbors(cbs[x][y], cbs[x - 1][y]);
		}
		if (flag2)
		{
			this.CheckIfBridge(cbs[x][y], cbs[x - 1][y], cbs[x - 2][y], CyberGrindNavHelper.BridgeDirection.Left);
		}
		if (flag3)
		{
			this.CheckNeighbors(cbs[x][y], cbs[x + 1][y]);
		}
		if (flag4)
		{
			this.CheckIfBridge(cbs[x][y], cbs[x + 1][y], cbs[x + 2][y], CyberGrindNavHelper.BridgeDirection.Right);
		}
		if (flag5)
		{
			this.CheckNeighbors(cbs[x][y], cbs[x][y - 1]);
		}
		if (flag6)
		{
			this.CheckIfBridge(cbs[x][y], cbs[x][y - 1], cbs[x][y - 2], CyberGrindNavHelper.BridgeDirection.Top);
		}
		if (flag7)
		{
			this.CheckNeighbors(cbs[x][y], cbs[x][y + 1]);
		}
		if (flag8)
		{
			this.CheckIfBridge(cbs[x][y], cbs[x][y + 1], cbs[x][y + 2], CyberGrindNavHelper.BridgeDirection.Bottom);
		}
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x0002EB78 File Offset: 0x0002CD78
	private void CheckIfBridge(EndlessCube dom, EndlessCube mid, EndlessCube sub, CyberGrindNavHelper.BridgeDirection dir)
	{
		if (sub.transform.position.y != dom.transform.position.y)
		{
			return;
		}
		if (dom.transform.position.y - mid.transform.position.y <= 10f)
		{
			return;
		}
		if (dir == CyberGrindNavHelper.BridgeDirection.Bottom && this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].BottomBridge)
		{
			return;
		}
		if (dir == CyberGrindNavHelper.BridgeDirection.Top && this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].TopBridge)
		{
			return;
		}
		if (dir == CyberGrindNavHelper.BridgeDirection.Left && this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].LeftBridge)
		{
			return;
		}
		if (dir == CyberGrindNavHelper.BridgeDirection.Right && this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].RightBridge)
		{
			return;
		}
		NavMeshLink navMeshLink = new GameObject("NavLink")
		{
			transform = 
			{
				parent = base.transform,
				position = dom.transform.position + Vector3.up * 25f
			}
		}.AddComponent<NavMeshLink>();
		Vector3 position = sub.transform.position;
		Vector3 position2 = dom.transform.position;
		position.y = 0f;
		position2.y = 0f;
		Vector3 vector = (position - position2).normalized;
		vector *= 1.75f;
		navMeshLink.startPoint = vector;
		navMeshLink.endPoint = sub.transform.position - dom.transform.position - vector;
		navMeshLink.bidirectional = true;
		navMeshLink.width = 3f;
		navMeshLink.UpdateLink();
		if (dir == CyberGrindNavHelper.BridgeDirection.Bottom)
		{
			this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].BottomBridge = true;
			this.bridges[sub.positionOnGrid.x][sub.positionOnGrid.y].TopBridge = true;
			return;
		}
		if (dir == CyberGrindNavHelper.BridgeDirection.Top)
		{
			this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].TopBridge = true;
			this.bridges[sub.positionOnGrid.x][sub.positionOnGrid.y].BottomBridge = true;
			return;
		}
		if (dir == CyberGrindNavHelper.BridgeDirection.Left)
		{
			this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].LeftBridge = true;
			this.bridges[sub.positionOnGrid.x][sub.positionOnGrid.y].RightBridge = true;
			return;
		}
		if (dir == CyberGrindNavHelper.BridgeDirection.Right)
		{
			this.bridges[dom.positionOnGrid.x][dom.positionOnGrid.y].RightBridge = true;
			this.bridges[sub.positionOnGrid.x][sub.positionOnGrid.y].LeftBridge = true;
		}
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002EEBD File Offset: 0x0002D0BD
	private void CheckNeighbors(EndlessCube dom, EndlessCube sub)
	{
		if (sub.blockedByPrefab || dom.blockedByPrefab)
		{
			return;
		}
		this.CheckNeighbors(dom.transform, sub.transform);
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002EEE4 File Offset: 0x0002D0E4
	private void CheckNeighbors(Transform dom, Transform sub)
	{
		if (sub.position.y >= dom.position.y)
		{
			return;
		}
		if (dom.position.y - sub.position.y > 10f)
		{
			return;
		}
		GameObject gameObject = new GameObject("NavLink");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.position = dom.position + Vector3.up * 25f;
		NavMeshLink navMeshLink = gameObject.AddComponent<NavMeshLink>();
		Vector3 position = sub.position;
		Vector3 position2 = dom.position;
		position.y = 0f;
		position2.y = 0f;
		Vector3 vector = (position - position2).normalized;
		vector *= 1.75f;
		navMeshLink.startPoint = vector;
		navMeshLink.endPoint = sub.position - dom.position;
		navMeshLink.bidirectional = true;
		navMeshLink.width = 5f;
		navMeshLink.UpdateLink();
	}

	// Token: 0x04000933 RID: 2355
	private CyberGrindNavHelper.BridgeBlock[][] bridges;

	// Token: 0x02000172 RID: 370
	private enum BridgeDirection
	{
		// Token: 0x04000935 RID: 2357
		Left,
		// Token: 0x04000936 RID: 2358
		Right,
		// Token: 0x04000937 RID: 2359
		Top,
		// Token: 0x04000938 RID: 2360
		Bottom
	}

	// Token: 0x02000173 RID: 371
	private struct BridgeBlock
	{
		// Token: 0x04000939 RID: 2361
		public bool TopBridge;

		// Token: 0x0400093A RID: 2362
		public bool BottomBridge;

		// Token: 0x0400093B RID: 2363
		public bool LeftBridge;

		// Token: 0x0400093C RID: 2364
		public bool RightBridge;
	}
}
