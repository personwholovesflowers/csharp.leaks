using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class LogicBranchListener : HammerEntity
{
	// Token: 0x06000634 RID: 1588 RVA: 0x00022058 File Offset: 0x00020258
	private void OnValidate()
	{
		this.branches = new List<LogicBranch>();
		this.values = new List<int>();
		for (int i = 0; i < this.branchStrings.Length; i++)
		{
			GameObject gameObject = GameObject.Find(this.branchStrings[i]);
			if (gameObject)
			{
				LogicBranch component = gameObject.GetComponent<LogicBranch>();
				if (component)
				{
					this.branches.Add(component);
					this.values.Add(component.initialValue);
				}
			}
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x000220D0 File Offset: 0x000202D0
	private void Update()
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < this.branches.Count; i++)
		{
			if (this.branches[i] != null)
			{
				if (this.branches[i].value != this.values[i])
				{
					this.values[i] = this.branches[i].value;
					flag = true;
				}
				num += this.values[i];
			}
		}
		if (flag)
		{
			if (num == this.values.Count)
			{
				base.FireOutput(Outputs.OnAllTrue);
				return;
			}
			if (num == 0)
			{
				base.FireOutput(Outputs.OnAllFalse);
				return;
			}
			base.FireOutput(Outputs.OnMixed);
		}
	}

	// Token: 0x0400068C RID: 1676
	public string[] branchStrings;

	// Token: 0x0400068D RID: 1677
	public List<LogicBranch> branches;

	// Token: 0x0400068E RID: 1678
	[SerializeField]
	private List<int> values;
}
