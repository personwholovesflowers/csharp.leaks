using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Nest.Components;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E2 RID: 226
public class HammerEntity : MonoBehaviour
{
	// Token: 0x0600057E RID: 1406 RVA: 0x0001EFC8 File Offset: 0x0001D1C8
	protected void FireOutput(Outputs output)
	{
		this.FireOutput(output, "", float.NaN);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0001EFDB File Offset: 0x0001D1DB
	protected void FireOutput(Outputs output, string parameterOverride)
	{
		this.FireOutput(output, parameterOverride, float.NaN);
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001EFEA File Offset: 0x0001D1EA
	protected void FireOutput(Outputs output, float parameterFloat)
	{
		this.FireOutput(output, "", parameterFloat);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0001EFFC File Offset: 0x0001D1FC
	protected void FireOutput(Outputs output, string parameterString, float parameterFloat)
	{
		if (!this.sorted)
		{
			this.SortOutputsByDelay();
			this.sorted = true;
		}
		for (int i = 0; i < this.expandedConnections.Count; i++)
		{
			if (this.expandedConnections[i].output == output && this.expandedConnections[i].nestInput)
			{
				if (parameterString != "")
				{
					if (this.expandedConnections[i].nestInput.CurrentEventType == NestInput.EventType.String && this.expandedConnections[i].nestInput._eventString.GetPersistentEventCount() > 0)
					{
						object persistentTarget = this.expandedConnections[i].nestInput._eventString.GetPersistentTarget(0);
						MethodInfo method = persistentTarget.GetType().GetMethod(this.expandedConnections[i].nestInput._eventString.GetPersistentMethodName(0), new Type[] { typeof(string) });
						this.expandedConnections[i].nestInput._eventString = new NestInput.StringEvent();
						UnityAction<string> unityAction = Delegate.CreateDelegate(typeof(UnityAction<string>), persistentTarget, method) as UnityAction<string>;
						this.expandedConnections[i].nestInput._eventString.AddListener(unityAction);
					}
					this.expandedConnections[i].nestInput._parameterString = parameterString;
				}
				if (!float.IsNaN(parameterFloat))
				{
					if (this.expandedConnections[i].nestInput.CurrentEventType == NestInput.EventType.Float && this.expandedConnections[i].nestInput._eventValue.GetPersistentEventCount() > 0)
					{
						object persistentTarget2 = this.expandedConnections[i].nestInput._eventValue.GetPersistentTarget(0);
						MethodInfo method2 = persistentTarget2.GetType().GetMethod(this.expandedConnections[i].nestInput._eventValue.GetPersistentMethodName(0), new Type[] { typeof(float) });
						this.expandedConnections[i].nestInput._eventValue = new NestInput.ValueEvent();
						UnityAction<float> unityAction2 = Delegate.CreateDelegate(typeof(UnityAction<float>), persistentTarget2, method2) as UnityAction<float>;
						this.expandedConnections[i].nestInput._eventValue.AddListener(unityAction2);
					}
					this.expandedConnections[i].nestInput._parameterFloat = parameterFloat;
				}
				this.expandedConnections[i].nestInput.Invoke();
			}
		}
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001F29C File Offset: 0x0001D49C
	private void SortOutputsByDelay()
	{
		List<HammerConnection> list = new List<HammerConnection>();
		while (this.expandedConnections.Count > 0)
		{
			float num = float.MaxValue;
			int num2 = -1;
			for (int i = 0; i < this.expandedConnections.Count; i++)
			{
				if (this.expandedConnections[i].delay < num)
				{
					num = this.expandedConnections[i].delay;
					num2 = i;
				}
			}
			if (num2 != -1)
			{
				list.Add(this.expandedConnections[num2]);
				this.expandedConnections.Remove(this.expandedConnections[num2]);
			}
		}
		this.expandedConnections = list;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0001F340 File Offset: 0x0001D540
	private void Validate()
	{
		int i;
		int j;
		for (i = 0; i < this.expandedConnections.Count; i = j + 1)
		{
			new List<Component>(base.GetComponents<Component>()).FindIndex((Component x) => x == this.expandedConnections[i].nestInput);
			j = i;
		}
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0001F3A3 File Offset: 0x0001D5A3
	private IEnumerator LogOutput(HammerConnection connection, string output, string filter)
	{
		yield return new WaitForGameSeconds(connection.nestInput.Delay);
		string text = " ::: ";
		string text2 = "                              ";
		string text3 = text + "              OUTPUT              " + text;
		string text4;
		if (connection.recipientObject == null)
		{
			text4 = "DESTROYED";
		}
		else
		{
			text4 = connection.recipientObject.name;
		}
		string text5;
		if (connection.nestInput._parameterString != "")
		{
			text5 = connection.nestInput._parameterString;
		}
		else
		{
			text5 = connection.nestInput._parameterFloat.ToString();
		}
		text3 = text3 + base.name + text2.Substring(base.name.Length) + text;
		text3 = text3 + output + text2.Substring(output.Length) + text;
		text3 = text3 + text4 + text2.Substring(text4.Length) + text;
		text3 = text3 + connection.input.ToString() + text2.Substring(connection.input.ToString().Length) + text;
		text3 = text3 + text5 + text2.Substring(text5.Length) + text;
		text3 += connection.nestInput.Delay.ToString();
		if (!(filter == ""))
		{
			text3.Contains(filter);
		}
		yield break;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void Use()
	{
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0001F3C7 File Offset: 0x0001D5C7
	public virtual void Input_Enable()
	{
		this.isEnabled = true;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0001F3D0 File Offset: 0x0001D5D0
	public virtual void Input_Disable()
	{
		this.isEnabled = false;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00019283 File Offset: 0x00017483
	public virtual void Input_Kill()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0001F3D9 File Offset: 0x0001D5D9
	public void Input_FireUser1()
	{
		this.FireOutput(Outputs.OnUser1);
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0001F3E3 File Offset: 0x0001D5E3
	public void Input_FireUser2()
	{
		this.FireOutput(Outputs.OnUser2);
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0001F3ED File Offset: 0x0001D5ED
	public void Input_FireUser3()
	{
		this.FireOutput(Outputs.OnUser3);
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0001F3F7 File Offset: 0x0001D5F7
	public void Input_FireUser4()
	{
		this.FireOutput(Outputs.OnUser4);
	}

	// Token: 0x040005C8 RID: 1480
	public List<HammerConnection> connections = new List<HammerConnection>();

	// Token: 0x040005C9 RID: 1481
	public List<HammerConnection> expandedConnections = new List<HammerConnection>();

	// Token: 0x040005CA RID: 1482
	[HideInInspector]
	public GameObject parent;

	// Token: 0x040005CB RID: 1483
	public bool isEnabled = true;

	// Token: 0x040005CC RID: 1484
	private bool sorted;
}
