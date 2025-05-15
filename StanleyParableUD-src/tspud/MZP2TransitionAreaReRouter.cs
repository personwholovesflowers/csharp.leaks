using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
[ExecuteInEditMode]
public class MZP2TransitionAreaReRouter : MonoBehaviour
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00022747 File Offset: 0x00020947
	private int OrderA
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.roomAEditorOrder;
			}
			return this.roomAOrder.GetIntValue() + 1;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x00022764 File Offset: 0x00020964
	private int OrderB
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.roomBEditorOrder;
			}
			return this.roomBOrder.GetIntValue() + 1;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x00022781 File Offset: 0x00020981
	private int OrderC
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.roomCEditorOrder;
			}
			return this.roomCOrder.GetIntValue() + 1;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x0002279E File Offset: 0x0002099E
	private int OrderD
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.roomDEditorOrder;
			}
			return this.roomDOrder.GetIntValue() + 1;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000651 RID: 1617 RVA: 0x000227BB File Offset: 0x000209BB
	private int OrderE
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.roomEEditorOrder;
			}
			return this.roomEOrder.GetIntValue() + 1;
		}
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x000227D8 File Offset: 0x000209D8
	public void ManuallySetConfigurablesAndPlace()
	{
		this.roomAOrder.SetValue(this.roomAEditorOrder - 1);
		this.roomBOrder.SetValue(this.roomBEditorOrder - 1);
		this.roomCOrder.SetValue(this.roomCEditorOrder - 1);
		this.roomDOrder.SetValue(this.roomDEditorOrder - 1);
		this.roomEOrder.SetValue(this.roomEEditorOrder - 1);
		this.roomAOrder.SaveToDiskAll();
		this.roomBOrder.SaveToDiskAll();
		this.roomCOrder.SaveToDiskAll();
		this.roomDOrder.SaveToDiskAll();
		this.roomEOrder.SaveToDiskAll();
		this.MoveAreas();
		this.WentThroughLeftChoiceDoor();
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00022888 File Offset: 0x00020A88
	private Transform GetIntro(int i)
	{
		if (i == this.OrderA)
		{
			return this.voidIntroA;
		}
		if (i == this.OrderB)
		{
			return this.voidIntroB;
		}
		if (i == this.OrderC)
		{
			return this.voidIntroC;
		}
		if (i == this.OrderD)
		{
			return this.voidIntroD;
		}
		if (i == this.OrderE)
		{
			return this.voidIntroE;
		}
		return null;
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x000228E8 File Offset: 0x00020AE8
	private Transform GetExit(int i)
	{
		if (i == this.OrderA)
		{
			return this.exhibitExitA;
		}
		if (i == this.OrderB)
		{
			return this.exhibitExitB;
		}
		if (i == this.OrderC)
		{
			return this.exhibitExitC;
		}
		if (i == this.OrderD)
		{
			return this.exhibitExitD;
		}
		if (i == this.OrderE)
		{
			return this.exhibitExitE;
		}
		return null;
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x00022948 File Offset: 0x00020B48
	private void Start()
	{
		if (Application.isPlaying)
		{
			if (this.GetIntro(1) == null || this.GetIntro(2) == null || this.GetIntro(3) == null || this.GetIntro(4) == null || this.GetIntro(5) == null || this.GetExit(1) == null || this.GetExit(2) == null || this.GetExit(3) == null || this.GetExit(4) == null || this.GetExit(5) == null)
			{
				Debug.LogError("FigleyCollection Order Incorrect, resetting to a default");
				Debug.LogError("OrderA " + this.OrderA);
				Debug.LogError("OrderB " + this.OrderB);
				Debug.LogError("OrderC " + this.OrderC);
				Debug.LogError("OrderD " + this.OrderD);
				Debug.LogError("OrderE " + this.OrderE);
				this.roomAOrder.SetValue(0);
				this.roomBOrder.SetValue(1);
				this.roomCOrder.SetValue(2);
				this.roomDOrder.SetValue(3);
				this.roomEOrder.SetValue(4);
				this.roomAOrder.SaveToDiskAll();
				this.roomBOrder.SaveToDiskAll();
				this.roomCOrder.SaveToDiskAll();
				this.roomDOrder.SaveToDiskAll();
				this.roomEOrder.SaveToDiskAll();
			}
			this.MoveAreas();
		}
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00022B00 File Offset: 0x00020D00
	private void Update()
	{
		if (!Application.isPlaying && this.moveAreasInEditMode)
		{
			this.MoveAreas();
		}
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00022B18 File Offset: 0x00020D18
	private void MoveAreas()
	{
		this.Link(this.preambleAreaExit, this.GetIntro(1), "preambleAreaExit, GetIntro(1)");
		this.Link(this.GetExit(1), this.choiceIntro, "GetExit(1), choiceIntro");
		this.Link(this.choiceLeftExit, this.GetIntro(2), "choiceLeftExit, GetIntro(2)");
		this.Link(this.choiceRightExit, this.GetIntro(3), "choiceRightExit, GetIntro(3)");
		this.PinkIntroBackTrack.gameObject.SetActive(false);
		if (!Application.isPlaying)
		{
			if (this.wentThroughLeftDoor_EDITOR)
			{
				this.WentThroughLeftChoiceDoor();
			}
			else
			{
				this.WentThroughRightChoiceDoor();
			}
			this.PreformBacktrackPortalSwitch();
		}
		this.Link(this.PinkExit, this.GetIntro(4), "PinkExit, GetIntro(4)");
		this.room4VideoPlayer.position = this.GetExit(4).position;
		this.Link(this.GetExit(4), this.shortPathExit, "GetExit(4), shortPathExit");
		this.Link(this.shortPathEnter, this.GetIntro(5), "shortPathEnter, GetIntro(5)");
		this.Link(this.GetExit(5), this.FinalIntro, "GetExit(5), FinalIntro");
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00022C30 File Offset: 0x00020E30
	private void LinkLeft()
	{
		this.Link(this.GetExit(2), this.PinkIntro, "LinkLeft: GetExit(2), PinkIntro");
		this.Link(this.GetExit(3), this.PinkIntroBackTrack, "LinkLeft: GetExit(3), PinkIntroBackTrack");
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00022C62 File Offset: 0x00020E62
	private void LinkRight()
	{
		this.Link(this.GetExit(3), this.PinkIntro, "LinkRight: GetExit(3), PinkIntro");
		this.Link(this.GetExit(2), this.PinkIntroBackTrack, "LinkRight: GetExit(2), PinkIntroBackTrack");
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00022C94 File Offset: 0x00020E94
	public void WentThroughLeftChoiceDoor()
	{
		this.LinkLeft();
		this.wentLeft = true;
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00022CA3 File Offset: 0x00020EA3
	public void WentThroughRightChoiceDoor()
	{
		this.LinkRight();
		this.wentLeft = false;
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00022CB2 File Offset: 0x00020EB2
	public void PreformBacktrackPortalSwitch()
	{
		if (this.reverseLinksOnBackwardTravel)
		{
			if (this.wentLeft)
			{
				this.LinkRight();
			}
			else
			{
				this.LinkLeft();
			}
			this.PinkIntroBackTrack.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00022CE3 File Offset: 0x00020EE3
	public void OG_WentThroughLeftChoiceDoor()
	{
		this.Link(this.GetExit(3), this.PinkIntro, "");
		this.Link(this.GetExit(2), this.PinkIntroBackTrack, "");
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00022D15 File Offset: 0x00020F15
	public void OG_WentThroughRightChoiceDoor()
	{
		this.Link(this.GetExit(2), this.PinkIntro, "");
		this.Link(this.GetExit(3), this.PinkIntroBackTrack, "");
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00022D48 File Offset: 0x00020F48
	private void Link(Transform exit, Transform entrance, string debug = "")
	{
		if (exit == null && entrance == null)
		{
			Debug.LogError(string.Concat(new object[] { "Missing Link - exit: ", exit, ", entrance:", entrance }));
		}
		entrance.position = exit.position;
		entrance.rotation = exit.rotation;
	}

	// Token: 0x040006A2 RID: 1698
	public bool moveAreasInEditMode;

	// Token: 0x040006A3 RID: 1699
	[Header("Preamble Area")]
	public Transform preambleAreaExit;

	// Token: 0x040006A4 RID: 1700
	[Header("A-E")]
	public Transform voidIntroA;

	// Token: 0x040006A5 RID: 1701
	public Transform exhibitExitA;

	// Token: 0x040006A6 RID: 1702
	public Transform voidIntroB;

	// Token: 0x040006A7 RID: 1703
	public Transform exhibitExitB;

	// Token: 0x040006A8 RID: 1704
	public Transform voidIntroC;

	// Token: 0x040006A9 RID: 1705
	public Transform exhibitExitC;

	// Token: 0x040006AA RID: 1706
	public Transform voidIntroD;

	// Token: 0x040006AB RID: 1707
	public Transform exhibitExitD;

	// Token: 0x040006AC RID: 1708
	public Transform voidIntroE;

	// Token: 0x040006AD RID: 1709
	public Transform exhibitExitE;

	// Token: 0x040006AE RID: 1710
	[Header("Short Path")]
	public Transform shortPathExit;

	// Token: 0x040006AF RID: 1711
	public Transform shortPathEnter;

	// Token: 0x040006B0 RID: 1712
	[Header("Choice Room")]
	public Transform choiceIntro;

	// Token: 0x040006B1 RID: 1713
	public Transform choiceLeftExit;

	// Token: 0x040006B2 RID: 1714
	public Transform choiceRightExit;

	// Token: 0x040006B3 RID: 1715
	[Header("Pink Room")]
	public Transform PinkIntro;

	// Token: 0x040006B4 RID: 1716
	public Transform PinkIntroBackTrack;

	// Token: 0x040006B5 RID: 1717
	public Transform PinkExit;

	// Token: 0x040006B6 RID: 1718
	[Header("Pink Room")]
	public Transform FinalIntro;

	// Token: 0x040006B7 RID: 1719
	[Header("VideoPlayer")]
	public Transform room4VideoPlayer;

	// Token: 0x040006B8 RID: 1720
	[Header("Configurables")]
	public IntConfigurable roomAOrder;

	// Token: 0x040006B9 RID: 1721
	public IntConfigurable roomBOrder;

	// Token: 0x040006BA RID: 1722
	public IntConfigurable roomCOrder;

	// Token: 0x040006BB RID: 1723
	public IntConfigurable roomDOrder;

	// Token: 0x040006BC RID: 1724
	public IntConfigurable roomEOrder;

	// Token: 0x040006BD RID: 1725
	[Header("Editor Tools")]
	public bool wentThroughLeftDoor_EDITOR;

	// Token: 0x040006BE RID: 1726
	public int roomAEditorOrder = 1;

	// Token: 0x040006BF RID: 1727
	public int roomBEditorOrder = 2;

	// Token: 0x040006C0 RID: 1728
	public int roomCEditorOrder = 3;

	// Token: 0x040006C1 RID: 1729
	public int roomDEditorOrder = 4;

	// Token: 0x040006C2 RID: 1730
	[InspectorButton("ManuallySetConfigurablesAndPlace", null)]
	public int roomEEditorOrder = 5;

	// Token: 0x040006C3 RID: 1731
	private bool wentLeft = true;

	// Token: 0x040006C4 RID: 1732
	private bool reverseLinksOnBackwardTravel;
}
