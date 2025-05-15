using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class WeaponPos : MonoBehaviour
{
	// Token: 0x06001BF6 RID: 7158 RVA: 0x000E83DA File Offset: 0x000E65DA
	private void Start()
	{
		this.CheckPosition();
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x000E83E2 File Offset: 0x000E65E2
	private void OnEnable()
	{
		this.CheckPosition();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x000E840A File Offset: 0x000E660A
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x000E842C File Offset: 0x000E662C
	private void OnPrefChanged(string key, object value)
	{
		if (key == "weaponHoldPosition")
		{
			this.CheckPosition();
		}
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x000E8444 File Offset: 0x000E6644
	public void CheckPosition()
	{
		if (!this.ready)
		{
			this.ready = true;
			this.defaultPos = base.transform.localPosition;
			this.defaultRot = base.transform.localRotation.eulerAngles;
			this.defaultScale = base.transform.localScale;
			if (this.middleScale == Vector3.zero)
			{
				this.middleScale = this.defaultScale;
			}
			if (this.middleRot == Vector3.zero)
			{
				this.middleRot = this.defaultRot;
			}
			if (this.moveOnMiddlePos != null && this.moveOnMiddlePos.Length != 0)
			{
				for (int i = 0; i < this.moveOnMiddlePos.Length; i++)
				{
					this.defaultPosValues.Add(this.moveOnMiddlePos[i].localPosition);
					this.defaultRotValues.Add(this.moveOnMiddlePos[i].localEulerAngles);
					if (this.middleRotValues[i] == Vector3.zero)
					{
						this.middleRotValues[i] = this.moveOnMiddlePos[i].localEulerAngles;
					}
				}
			}
		}
		if (MonoSingleton<PrefsManager>.Instance.GetInt("weaponHoldPosition", 0) == 1 && (!MonoSingleton<PowerUpMeter>.Instance || MonoSingleton<PowerUpMeter>.Instance.juice <= 0f))
		{
			base.transform.localPosition = this.middlePos;
			base.transform.localRotation = Quaternion.Euler(this.middleRot);
			base.transform.localScale = this.middleScale;
			if (this.moveOnMiddlePos != null && this.moveOnMiddlePos.Length != 0)
			{
				for (int j = 0; j < this.moveOnMiddlePos.Length; j++)
				{
					this.moveOnMiddlePos[j].localPosition = this.middlePosValues[j];
					this.moveOnMiddlePos[j].localEulerAngles = this.middleRotValues[j];
				}
			}
		}
		else
		{
			base.transform.localPosition = this.defaultPos;
			base.transform.localRotation = Quaternion.Euler(this.defaultRot);
			base.transform.localScale = this.defaultScale;
			if (this.moveOnMiddlePos != null && this.moveOnMiddlePos.Length != 0)
			{
				for (int k = 0; k < this.moveOnMiddlePos.Length; k++)
				{
					this.moveOnMiddlePos[k].localPosition = this.defaultPosValues[k];
					this.moveOnMiddlePos[k].localEulerAngles = this.defaultRotValues[k];
				}
			}
		}
		this.currentDefault = base.transform.localPosition;
	}

	// Token: 0x0400276D RID: 10093
	private bool ready;

	// Token: 0x0400276E RID: 10094
	public Vector3 currentDefault;

	// Token: 0x0400276F RID: 10095
	private Vector3 defaultPos;

	// Token: 0x04002770 RID: 10096
	private Vector3 defaultRot;

	// Token: 0x04002771 RID: 10097
	private Vector3 defaultScale;

	// Token: 0x04002772 RID: 10098
	public Vector3 middlePos;

	// Token: 0x04002773 RID: 10099
	public Vector3 middleRot;

	// Token: 0x04002774 RID: 10100
	public Vector3 middleScale;

	// Token: 0x04002775 RID: 10101
	public Transform[] moveOnMiddlePos;

	// Token: 0x04002776 RID: 10102
	public Vector3[] middlePosValues;

	// Token: 0x04002777 RID: 10103
	private List<Vector3> defaultPosValues = new List<Vector3>();

	// Token: 0x04002778 RID: 10104
	public Vector3[] middleRotValues;

	// Token: 0x04002779 RID: 10105
	private List<Vector3> defaultRotValues = new List<Vector3>();
}
