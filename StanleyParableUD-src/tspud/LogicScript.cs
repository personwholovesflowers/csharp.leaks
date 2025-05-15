using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class LogicScript : HammerEntity
{
	// Token: 0x06000AB5 RID: 2741 RVA: 0x00031CA4 File Offset: 0x0002FEA4
	private void OnValidate()
	{
		this.groups = new List<LogicScript.GroupList>();
		HammerEntity[] array = Object.FindObjectsOfType<HammerEntity>();
		for (int i = 0; i < this.entityGroups.Length; i++)
		{
			this.groups.Add(new LogicScript.GroupList());
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].name == this.entityGroups[i])
				{
					this.groups[i].group.Add(array[j]);
				}
			}
		}
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00031D23 File Offset: 0x0002FF23
	public void Input_EnableScript()
	{
		this.script.enabled = true;
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00031D31 File Offset: 0x0002FF31
	public void Input_RunScriptCode(string command)
	{
		this.script.ParseCommand(command);
	}

	// Token: 0x04000A98 RID: 2712
	public string[] entityGroups;

	// Token: 0x04000A99 RID: 2713
	public LogicScriptBase script;

	// Token: 0x04000A9A RID: 2714
	public List<LogicScript.GroupList> groups = new List<LogicScript.GroupList>();

	// Token: 0x02000407 RID: 1031
	[Serializable]
	public class GroupList
	{
		// Token: 0x04001515 RID: 5397
		public List<HammerEntity> group = new List<HammerEntity>();
	}
}
