using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class ResolutionConfigurator : Configurator
{
	// Token: 0x0600016F RID: 367 RVA: 0x0000A0A0 File Offset: 0x000082A0
	public new void Start()
	{
		this.PrintValue(this.configurable);
		this.TMProForceMeshUpdate();
		SimpleEvent simpleEvent = this.onResolutionChange;
		simpleEvent.OnCall = (Action)Delegate.Combine(simpleEvent.OnCall, new Action(this.OnResolutionChangedEvent));
		base.Start();
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000A0EC File Offset: 0x000082EC
	private void OnDestroy()
	{
		SimpleEvent simpleEvent = this.onResolutionChange;
		simpleEvent.OnCall = (Action)Delegate.Combine(simpleEvent.OnCall, new Action(this.OnResolutionChangedEvent));
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000A115 File Offset: 0x00008315
	private void OnResolutionChangedEvent()
	{
		this.TMProForceMeshUpdate();
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000A120 File Offset: 0x00008320
	public void TMProForceMeshUpdate()
	{
		TextMeshProUGUI[] componentsInChildren = Singleton<GameMaster>.Instance.MenuManager.GetComponent<Canvas>().GetComponentsInChildren<TextMeshProUGUI>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ForceMeshUpdate(true, true);
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00005444 File Offset: 0x00003644
	public override void ApplyData()
	{
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000A15A File Offset: 0x0000835A
	private static int GCD(int a, int b)
	{
		if (a == 0)
		{
			return b;
		}
		if (b == 0)
		{
			return a;
		}
		if (a == b)
		{
			return a;
		}
		if (a > b)
		{
			return ResolutionConfigurator.GCD(a - b, b);
		}
		return ResolutionConfigurator.GCD(a, b - a);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000A184 File Offset: 0x00008384
	private static string GetAspectRatioString(int w, int h)
	{
		int num = ResolutionConfigurator.GCD(w, h);
		int num2 = w / num;
		int num3 = h / num;
		if (num2 == 8 && num3 == 5)
		{
			num2 *= 2;
			num3 *= 2;
		}
		return num2 + ":" + num3;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000A1C8 File Offset: 0x000083C8
	public override void PrintValue(Configurable _configurable)
	{
		int intValue = this.configurable.GetIntValue();
		Debug.Log("resolutionIndex: " + intValue);
		Debug.Log("ResolutionController.Instance.availableResolutions.Length: " + ResolutionController.Instance.availableResolutions.Length);
		Debug.Log("ResolutionController.Instance.availableResolutions[] = " + string.Join("\n", new List<Resolution>(ResolutionController.Instance.availableResolutions).ConvertAll<string>((Resolution x) => x.ToString())));
		Resolution resolution = ResolutionController.Instance.availableResolutions[intValue];
		string text = (ResolutionController.CompareResolutions(Screen.currentResolution, resolution) ? "" : "*");
		string aspectRatioString = ResolutionConfigurator.GetAspectRatioString(resolution.width, resolution.height);
		this.OnPrintValue.Invoke(string.Format("{0} x {1} @ {2}Hz{3} ({4})", new object[] { resolution.width, resolution.height, resolution.refreshRate, text, aspectRatioString }));
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00005444 File Offset: 0x00003644
	public void FunctionBecauesTheFuckingThingWontFuckingCallAndIDontKnowWhy(string yeah_the_fucking_string)
	{
	}

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private TextMeshProUGUI theLabelThatWontWorkBcItsBeingStupid;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private SimpleEvent onResolutionChange;
}
