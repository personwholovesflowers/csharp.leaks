using System;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class CursorHijack : MonoBehaviour
{
	// Token: 0x0600068E RID: 1678 RVA: 0x00023416 File Offset: 0x00021616
	private void Awake()
	{
		this.RT = base.GetComponent<RectTransform>();
		this.cursorRT = this.cursor.GetComponent<RectTransform>();
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00023438 File Offset: 0x00021638
	private void Update()
	{
		if (Singleton<GameMaster>.Instance.stanleyActions.Up.IsPressed)
		{
			if (this.pressedTimer <= 0f)
			{
				this.refireCounter++;
				this.pressedTimer = this.refireTime / Mathf.Clamp((float)this.refireCounter, 1f, 10f);
				this.MoveCursor(DPadDir.Up);
			}
			this.pressedTimer -= Time.deltaTime;
			return;
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.Down.IsPressed)
		{
			if (this.pressedTimer <= 0f)
			{
				this.refireCounter++;
				this.pressedTimer = this.refireTime / Mathf.Clamp((float)this.refireCounter, 1f, 5f);
				this.MoveCursor(DPadDir.Down);
			}
			this.pressedTimer -= Time.deltaTime;
			return;
		}
		this.pressedTimer = 0f;
		this.refireCounter = 0;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00023534 File Offset: 0x00021734
	private void MoveCursor(DPadDir direction)
	{
		if (direction == DPadDir.Left || direction == DPadDir.Right)
		{
			return;
		}
		if (this.cursor.hoveringOver == "")
		{
			float num = float.PositiveInfinity;
			int num2 = -1;
			for (int i = 0; i < this.menuItems.Length; i++)
			{
				float num3 = Mathf.Abs(this.cursorRT.anchoredPosition3D.y - (-512f + this.RT.InverseTransformPoint(this.menuItems[i].position).y));
				if (num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
			if (num2 != -1)
			{
				this.currentIndex = num2;
			}
		}
		else
		{
			if (direction == DPadDir.Up)
			{
				this.currentIndex--;
			}
			else if (direction == DPadDir.Down)
			{
				this.currentIndex++;
			}
			if (this.currentIndex < 0)
			{
				this.currentIndex = this.menuItems.Length - 1;
			}
			if (this.currentIndex > this.menuItems.Length - 1)
			{
				this.currentIndex = 0;
			}
		}
		this.cursorRT.anchoredPosition3D = new Vector3(1024f, -512f, 0f) + this.RT.InverseTransformPoint(this.menuItems[this.currentIndex].position);
	}

	// Token: 0x040006E1 RID: 1761
	public MenuCursor cursor;

	// Token: 0x040006E2 RID: 1762
	private RectTransform cursorRT;

	// Token: 0x040006E3 RID: 1763
	[Space(10f)]
	public RectTransform[] menuItems;

	// Token: 0x040006E4 RID: 1764
	private int currentIndex;

	// Token: 0x040006E5 RID: 1765
	private float pressedTimer;

	// Token: 0x040006E6 RID: 1766
	private float refireTime = 0.25f;

	// Token: 0x040006E7 RID: 1767
	private int refireCounter;

	// Token: 0x040006E8 RID: 1768
	private RectTransform RT;
}
