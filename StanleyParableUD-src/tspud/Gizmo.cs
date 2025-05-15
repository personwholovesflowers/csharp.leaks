using System;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class Gizmo : MonoBehaviour
{
	// Token: 0x06000A6A RID: 2666 RVA: 0x00030F5C File Offset: 0x0002F15C
	private void OnDrawGizmos()
	{
		if (Camera.current && Vector3.Distance(Camera.current.transform.position, base.transform.position) < 7.5f)
		{
			Gizmos.DrawIcon(base.transform.position, this.Icons[(int)this.icon]);
		}
	}

	// Token: 0x04000A59 RID: 2649
	private string[] Icons = new string[]
	{
		"obsolete.tga", "choreo_scene.tga", "env_shake.tga", "env_fade.tga", "env_soundscape.tga", "env_spark.tga", "env_cubemap.tga", "env_texturetoggle.tga", "game_text.tga", "info_landmark.tga",
		"logic_auto.tga", "logic_branch.tga", "logic_case.tga", "logic_compare.tga", "logic_relay.tga", "logic_script.tga", "logic_timer.tga", "math_counter.tga", "phys_ballsocket.tga"
	};

	// Token: 0x04000A5A RID: 2650
	public Gizmo.SourceIcons icon;

	// Token: 0x02000402 RID: 1026
	public enum SourceIcons
	{
		// Token: 0x040014EE RID: 5358
		obsolete,
		// Token: 0x040014EF RID: 5359
		choreo_scene,
		// Token: 0x040014F0 RID: 5360
		env_shake,
		// Token: 0x040014F1 RID: 5361
		env_fade,
		// Token: 0x040014F2 RID: 5362
		env_soundscape,
		// Token: 0x040014F3 RID: 5363
		env_spark,
		// Token: 0x040014F4 RID: 5364
		env_cubemap,
		// Token: 0x040014F5 RID: 5365
		env_texturetoggle,
		// Token: 0x040014F6 RID: 5366
		game_text,
		// Token: 0x040014F7 RID: 5367
		info_landmark,
		// Token: 0x040014F8 RID: 5368
		logic_auto,
		// Token: 0x040014F9 RID: 5369
		logic_branch,
		// Token: 0x040014FA RID: 5370
		logic_case,
		// Token: 0x040014FB RID: 5371
		logic_compare,
		// Token: 0x040014FC RID: 5372
		logic_relay,
		// Token: 0x040014FD RID: 5373
		logic_script,
		// Token: 0x040014FE RID: 5374
		logic_timer,
		// Token: 0x040014FF RID: 5375
		math_counter,
		// Token: 0x04001500 RID: 5376
		phys_ballsocket
	}
}
