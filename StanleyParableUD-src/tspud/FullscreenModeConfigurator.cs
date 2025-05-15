using System;

// Token: 0x02000042 RID: 66
public class FullscreenModeConfigurator : Configurator
{
	// Token: 0x0600015D RID: 349 RVA: 0x00005444 File Offset: 0x00003644
	public override void ApplyData()
	{
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00009E80 File Offset: 0x00008080
	public override void PrintValue(Configurable _configurable)
	{
		int intValue = this.configurable.GetIntValue();
		string text = "";
		switch (intValue)
		{
		case 0:
			text = "Menu_Fullscreen_Mode_Fullscreen";
			break;
		case 1:
			text = "Menu_Fullscreen_Mode_Borderless";
			break;
		case 2:
			text = "Menu_Fullscreen_Mode_Windowed";
			break;
		}
		this.OnPrintValue.Invoke(text);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00005444 File Offset: 0x00003644
	public void FunctionBecauesTheFuckingThingWontFuckingCallAndIDontKnowWhy(string yeah_the_fucking_string)
	{
	}
}
