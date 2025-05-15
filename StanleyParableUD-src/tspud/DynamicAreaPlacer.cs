using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
[ExecuteInEditMode]
public class DynamicAreaPlacer : MonoBehaviour
{
	// Token: 0x0600041C RID: 1052 RVA: 0x000192B4 File Offset: 0x000174B4
	public void ManuallySetConfigurablesAndPlace()
	{
		this.configurableAreaOrderA.SetValue(this.editorAreaOrderA);
		this.configurableAreaOrderB.SetValue(this.editorAreaOrderB);
		this.configurableAreaOrderC.SetValue(this.editorAreaOrderC);
		this.configurableAreaOrderD.SetValue(this.editorAreaOrderD);
		this.configurableAreaOrderE.SetValue(this.editorAreaOrderE);
		this.configurableAreaOrderA.SaveToDiskAll();
		this.configurableAreaOrderB.SaveToDiskAll();
		this.configurableAreaOrderC.SaveToDiskAll();
		this.configurableAreaOrderD.SaveToDiskAll();
		this.configurableAreaOrderE.SaveToDiskAll();
		this.PlaceAreas();
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00019354 File Offset: 0x00017554
	private void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.configurableAreaOrderA.Init();
		this.configurableAreaOrderB.Init();
		this.configurableAreaOrderC.Init();
		this.configurableAreaOrderD.Init();
		this.configurableAreaOrderE.Init();
		this.PlaceAreas();
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00005444 File Offset: 0x00003644
	private void Update()
	{
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x000193A8 File Offset: 0x000175A8
	private void PlaceAreas()
	{
		this.dynamicRoomA.transform.position = this.GetNthDRTarget(this.configurableAreaOrderA.GetIntValue()).position;
		this.dynamicRoomB.transform.position = this.GetNthDRTarget(this.configurableAreaOrderB.GetIntValue()).position;
		this.dynamicRoomC.transform.position = this.GetNthDRTarget(this.configurableAreaOrderC.GetIntValue()).position;
		this.dynamicRoomD.transform.position = this.GetNthDRTarget(this.configurableAreaOrderD.GetIntValue()).position;
		this.dynamicRoomE.transform.position = this.GetNthDRTarget(this.configurableAreaOrderE.GetIntValue()).position;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00019474 File Offset: 0x00017674
	private void PlaceAreas_EDITOR()
	{
		this.dynamicRoomA.transform.position = this.GetNthDRTarget(this.editorAreaOrderA).position;
		this.dynamicRoomB.transform.position = this.GetNthDRTarget(this.editorAreaOrderB).position;
		this.dynamicRoomC.transform.position = this.GetNthDRTarget(this.editorAreaOrderC).position;
		this.dynamicRoomD.transform.position = this.GetNthDRTarget(this.editorAreaOrderD).position;
		this.dynamicRoomE.transform.position = this.GetNthDRTarget(this.editorAreaOrderE).position;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00019528 File Offset: 0x00017728
	private Transform GetNthDRTarget(int drTargetIndex)
	{
		switch (drTargetIndex)
		{
		case 0:
			return this.drTarget1;
		case 1:
			return this.drTarget2;
		case 2:
			return this.drTarget3;
		case 3:
			return this.drTarget4;
		case 4:
			return this.drTarget5;
		default:
			return null;
		}
	}

	// Token: 0x04000408 RID: 1032
	[SerializeField]
	private GameObject dynamicRoomA;

	// Token: 0x04000409 RID: 1033
	[SerializeField]
	private GameObject dynamicRoomB;

	// Token: 0x0400040A RID: 1034
	[SerializeField]
	private GameObject dynamicRoomC;

	// Token: 0x0400040B RID: 1035
	[SerializeField]
	private GameObject dynamicRoomD;

	// Token: 0x0400040C RID: 1036
	[SerializeField]
	private GameObject dynamicRoomE;

	// Token: 0x0400040D RID: 1037
	[SerializeField]
	private Transform drTarget1;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private Transform drTarget2;

	// Token: 0x0400040F RID: 1039
	[SerializeField]
	private Transform drTarget3;

	// Token: 0x04000410 RID: 1040
	[SerializeField]
	private Transform drTarget4;

	// Token: 0x04000411 RID: 1041
	[SerializeField]
	private Transform drTarget5;

	// Token: 0x04000412 RID: 1042
	[SerializeField]
	private IntConfigurable configurableAreaOrderA;

	// Token: 0x04000413 RID: 1043
	[SerializeField]
	private IntConfigurable configurableAreaOrderB;

	// Token: 0x04000414 RID: 1044
	[SerializeField]
	private IntConfigurable configurableAreaOrderC;

	// Token: 0x04000415 RID: 1045
	[SerializeField]
	private IntConfigurable configurableAreaOrderD;

	// Token: 0x04000416 RID: 1046
	[SerializeField]
	private IntConfigurable configurableAreaOrderE;

	// Token: 0x04000417 RID: 1047
	[Header("Pick up Order [0-4]")]
	public int editorAreaOrderA;

	// Token: 0x04000418 RID: 1048
	public int editorAreaOrderB = 1;

	// Token: 0x04000419 RID: 1049
	public int editorAreaOrderC = 2;

	// Token: 0x0400041A RID: 1050
	public int editorAreaOrderD = 3;

	// Token: 0x0400041B RID: 1051
	[InspectorButton("ManuallySetConfigurablesAndPlace", null)]
	public int editorAreaOrderE = 4;

	// Token: 0x0400041C RID: 1052
	[SerializeField]
	private bool placeAreasInEditMode;
}
