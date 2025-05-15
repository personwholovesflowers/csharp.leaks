using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class LogicCase : HammerEntity
{
	// Token: 0x06000637 RID: 1591 RVA: 0x00022188 File Offset: 0x00020388
	private void Awake()
	{
		for (int i = 0; i < this.expandedConnections.Count; i++)
		{
			int j = 0;
			while (j < this.outCases.Length)
			{
				if (this.outCases[j] == this.expandedConnections[i].output)
				{
					if (!this.firableRandomCases.Contains(this.expandedConnections[i].output))
					{
						this.firableRandomCases.Add(this.expandedConnections[i].output);
						break;
					}
					break;
				}
				else
				{
					j++;
				}
			}
		}
		if (this.debugPickRandomOverrideConfigurable != null)
		{
			this.debugPickRandomOverrideConfigurable.SetNewMaxValue(this.firableRandomCases.Count);
		}
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0002223C File Offset: 0x0002043C
	public void Input_InValue(string value)
	{
		bool flag = true;
		for (int i = 0; i < this.cases.Length; i++)
		{
			if (this.cases[i] == value)
			{
				base.FireOutput(this.outCases[i]);
			}
			flag = false;
		}
		if (flag)
		{
			base.FireOutput(Outputs.OnDefault);
		}
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0002228C File Offset: 0x0002048C
	public int GetRandomFirableCaseIndex()
	{
		LogicCaseCustomWeights component = base.GetComponent<LogicCaseCustomWeights>();
		if (component == null)
		{
			return Random.Range(0, this.firableRandomCases.Count);
		}
		Dictionary<Outputs, float> dictionary = new Dictionary<Outputs, float>();
		foreach (Outputs outputs in this.firableRandomCases)
		{
			dictionary[outputs] = 1f;
		}
		foreach (LogicCaseCustomWeights.CaseWeight caseWeight in component.caseWeightOverrides)
		{
			if (dictionary.ContainsKey(caseWeight.caseNumber))
			{
				dictionary[caseWeight.caseNumber] = caseWeight.weight;
			}
		}
		float num = 0f;
		foreach (Outputs outputs2 in this.firableRandomCases)
		{
			num += dictionary[outputs2];
		}
		float num2 = Random.Range(0f, num);
		for (int i = 0; i < this.firableRandomCases.Count; i++)
		{
			Outputs outputs3 = this.firableRandomCases[i];
			num2 -= dictionary[outputs3];
			if (num2 <= 0f)
			{
				return i;
			}
		}
		Debug.LogError("Problem in weighted random case picker" + base.name, this);
		return 0;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00022420 File Offset: 0x00020620
	public void Input_PickRandom()
	{
		int num = this.GetRandomFirableCaseIndex();
		if (this.debugPickRandomOverrideConfigurable != null && this.debugPickRandomOverrideConfigurable.GetIntValue() != 0)
		{
			num = this.debugPickRandomOverrideConfigurable.GetIntValue() - 1;
		}
		base.FireOutput(this.firableRandomCases[num]);
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00022478 File Offset: 0x00020678
	public void Input_PickRandomShuffle()
	{
		if (this.shuffleIndex == this.shuffledCases.Count)
		{
			this.shuffledCases = new List<Outputs>();
			List<Outputs> list = new List<Outputs>(this.firableRandomCases);
			while (list.Count > 0)
			{
				int num = Random.Range(0, list.Count);
				this.shuffledCases.Add(list[num]);
				list.RemoveAt(num);
			}
			this.shuffleIndex = 0;
		}
		base.FireOutput(this.shuffledCases[this.shuffleIndex]);
		this.shuffleIndex++;
	}

	// Token: 0x0400068F RID: 1679
	public string[] cases = new string[16];

	// Token: 0x04000690 RID: 1680
	[Header("Overrides case (0 = pick random as normal, 1-16 is OnCase01-16")]
	public IntConfigurable debugPickRandomOverrideConfigurable;

	// Token: 0x04000691 RID: 1681
	private Outputs[] outCases = new Outputs[]
	{
		Outputs.OnCase01,
		Outputs.OnCase02,
		Outputs.OnCase03,
		Outputs.OnCase04,
		Outputs.OnCase05,
		Outputs.OnCase06,
		Outputs.OnCase07,
		Outputs.OnCase08,
		Outputs.OnCase09,
		Outputs.OnCase10,
		Outputs.OnCase11,
		Outputs.OnCase12,
		Outputs.OnCase13,
		Outputs.OnCase14,
		Outputs.OnCase15,
		Outputs.OnCase16
	};

	// Token: 0x04000692 RID: 1682
	private List<Outputs> firableRandomCases = new List<Outputs>();

	// Token: 0x04000693 RID: 1683
	private List<Outputs> shuffledCases = new List<Outputs>();

	// Token: 0x04000694 RID: 1684
	private int shuffleIndex;
}
