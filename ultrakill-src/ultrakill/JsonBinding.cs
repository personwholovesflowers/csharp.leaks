using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200026B RID: 619
public class JsonBinding
{
	// Token: 0x06000D98 RID: 3480 RVA: 0x00004ADB File Offset: 0x00002CDB
	private JsonBinding()
	{
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x00066BAC File Offset: 0x00064DAC
	public static List<JsonBinding> FromAction(InputAction action, string group)
	{
		List<JsonBinding> list = new List<JsonBinding>();
		for (int i = 0; i < action.bindings.Count; i++)
		{
			InputBinding inputBinding = action.bindings[i];
			JsonBinding jsonBinding = new JsonBinding();
			if (action.BindingHasGroup(i, group))
			{
				if (inputBinding.isComposite)
				{
					jsonBinding.path = inputBinding.GetNameOfComposite();
					jsonBinding.isComposite = true;
					jsonBinding.parts = new Dictionary<string, string>();
					while (i + 1 < action.bindings.Count)
					{
						if (!action.bindings[i + 1].isPartOfComposite)
						{
							break;
						}
						i++;
						InputBinding inputBinding2 = action.bindings[i];
						Debug.Log("BLEURHG " + inputBinding2.name);
						Debug.Log(inputBinding2.path);
						Debug.Log(inputBinding2.isPartOfComposite);
						jsonBinding.parts.Add(inputBinding2.name, inputBinding2.path);
					}
				}
				else
				{
					jsonBinding.path = inputBinding.path;
				}
				list.Add(jsonBinding);
			}
		}
		return list;
	}

	// Token: 0x04001216 RID: 4630
	public string path;

	// Token: 0x04001217 RID: 4631
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool isComposite;

	// Token: 0x04001218 RID: 4632
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public Dictionary<string, string> parts;
}
