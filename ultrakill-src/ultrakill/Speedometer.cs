using System;
using TMPro;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public class Speedometer : MonoBehaviour
{
	// Token: 0x06001804 RID: 6148 RVA: 0x000C3914 File Offset: 0x000C1B14
	private void Awake()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x000C3936 File Offset: 0x000C1B36
	private void OnEnable()
	{
		this.type = MonoSingleton<PrefsManager>.Instance.GetInt("speedometer", 0);
		base.gameObject.SetActive(this.type > 0);
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x000C3962 File Offset: 0x000C1B62
	private void OnDestroy()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x000C3984 File Offset: 0x000C1B84
	private void OnPrefChanged(string id, object value)
	{
		if (id == "speedometer" && value is int)
		{
			int num = (int)value;
			base.gameObject.SetActive(num > 0);
			this.type = num;
		}
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x000C39C4 File Offset: 0x000C1BC4
	private void FixedUpdate()
	{
		float num = 0f;
		string text = "";
		switch (this.type)
		{
		case 0:
			return;
		case 1:
			num = MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(true).magnitude;
			text = "u";
			break;
		case 2:
			num = Vector3.ProjectOnPlane(MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(true), Vector3.up).magnitude;
			text = "hu";
			break;
		case 3:
			num = Mathf.Abs(MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(true).y);
			text = "vu";
			break;
		}
		if (this.lastUpdate > 0.064f)
		{
			if (this.classicVersion)
			{
				this.textMesh.text = string.Format("{0:0}", num);
			}
			else
			{
				this.textMesh.text = string.Format("SPEED: {0:0.00} {1}/s", num, text);
			}
			this.lastUpdate = 0f;
		}
	}

	// Token: 0x040021A8 RID: 8616
	public TextMeshProUGUI textMesh;

	// Token: 0x040021A9 RID: 8617
	public Vector3 lastPos;

	// Token: 0x040021AA RID: 8618
	public bool classicVersion;

	// Token: 0x040021AB RID: 8619
	private TimeSince lastUpdate;

	// Token: 0x040021AC RID: 8620
	public RectTransform rect;

	// Token: 0x040021AD RID: 8621
	private int type;
}
