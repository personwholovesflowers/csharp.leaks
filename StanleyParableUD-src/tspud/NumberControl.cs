using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000142 RID: 322
public class NumberControl : Selectable
{
	// Token: 0x06000788 RID: 1928 RVA: 0x000267E7 File Offset: 0x000249E7
	public override void Select()
	{
		base.Select();
		this.selected = true;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x000267F6 File Offset: 0x000249F6
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		this.selected = false;
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00026808 File Offset: 0x00024A08
	public override void OnMove(AxisEventData eventData)
	{
		MoveDirection moveDir = eventData.moveDir;
		if (moveDir == MoveDirection.Up)
		{
			this.UpdateNumberValue(1);
			return;
		}
		if (moveDir != MoveDirection.Down)
		{
			base.OnMove(eventData);
			return;
		}
		this.UpdateNumberValue(-1);
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0002683D File Offset: 0x00024A3D
	public void UpdateNumberValue(int changeValue)
	{
		this.UpdateNumberValue(changeValue, false);
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00026848 File Offset: 0x00024A48
	public void UpdateNumberValue(int changeValue, bool silent)
	{
		if (!silent)
		{
			this.valueChangedEvent.Call();
		}
		this.numberValue += changeValue;
		NumberControl.NumberTypes numberTypes = this.numberType;
		if (numberTypes != NumberControl.NumberTypes.Minute)
		{
			if (numberTypes == NumberControl.NumberTypes.Hour)
			{
				if (this.numberValue >= 24)
				{
					this.numberValue = 0;
				}
				if (this.numberValue < 0)
				{
					this.numberValue = 23;
				}
			}
		}
		else
		{
			if (this.numberValue >= 60)
			{
				this.numberValue = 0;
				NumberControl numberControl = this.otherNumberControl;
				if (numberControl != null)
				{
					numberControl.UpdateNumberValue(1, true);
				}
			}
			if (this.numberValue < 0)
			{
				this.numberValue = 59;
				NumberControl numberControl2 = this.otherNumberControl;
				if (numberControl2 != null)
				{
					numberControl2.UpdateNumberValue(-1, true);
				}
			}
		}
		this.UpdateText();
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x000268F4 File Offset: 0x00024AF4
	private void UpdateText()
	{
		NumberControl.NumberTypes numberTypes = this.numberType;
		if (numberTypes == NumberControl.NumberTypes.Minute)
		{
			this.numberDisplay.text = IntroTimeDisplay.GetMinuteString(this.numberValue);
			return;
		}
		if (numberTypes != NumberControl.NumberTypes.Hour)
		{
			return;
		}
		string text;
		this.numberDisplay.text = IntroTimeDisplay.GetHourString(this.numberValue, out text);
		if (this.ampmDisplay != null)
		{
			this.ampmDisplay.text = text;
		}
	}

	// Token: 0x040007AC RID: 1964
	[Space]
	[SerializeField]
	private NumberControl.NumberTypes numberType;

	// Token: 0x040007AD RID: 1965
	private int numberValue;

	// Token: 0x040007AE RID: 1966
	[SerializeField]
	private TextMeshProUGUI numberDisplay;

	// Token: 0x040007AF RID: 1967
	[Header("used to update hours if minutes goes below or above 59|00 mark")]
	[SerializeField]
	private NumberControl otherNumberControl;

	// Token: 0x040007B0 RID: 1968
	[Header("leave null for 24 hour clock, otherwise 12 hour clock")]
	[SerializeField]
	private TextMeshProUGUI ampmDisplay;

	// Token: 0x040007B1 RID: 1969
	[SerializeField]
	private SimpleEvent valueChangedEvent;

	// Token: 0x040007B2 RID: 1970
	private bool selected;

	// Token: 0x020003DD RID: 989
	public enum NumberTypes
	{
		// Token: 0x04001447 RID: 5191
		Minute,
		// Token: 0x04001448 RID: 5192
		Hour
	}
}
