using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
[ExecuteInEditMode]
public class RocketLeaugeShadersWorldColors : MonoBehaviour
{
	// Token: 0x060008D4 RID: 2260 RVA: 0x0002A1E4 File Offset: 0x000283E4
	private void Start()
	{
		this.SetColours();
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0002A1EC File Offset: 0x000283EC
	[ContextMenu("Set Colours")]
	private void SetColours()
	{
		Shader.SetGlobalColor("TeamBlueColour", this.blueTeamColor);
		Shader.SetGlobalColor("TeamOrangeColour", this.orangeTeamColor);
	}

	// Token: 0x040008A2 RID: 2210
	public Color blueTeamColor;

	// Token: 0x040008A3 RID: 2211
	public Color orangeTeamColor;
}
