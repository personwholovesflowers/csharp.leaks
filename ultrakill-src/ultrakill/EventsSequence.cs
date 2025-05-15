using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class EventsSequence : MonoBehaviour
{
	// Token: 0x060008F6 RID: 2294 RVA: 0x0003AA16 File Offset: 0x00038C16
	private void Start()
	{
		this.StartEvents();
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0003AA16 File Offset: 0x00038C16
	private void OnEnable()
	{
		this.StartEvents();
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0003AA1E File Offset: 0x00038C1E
	private void OnDisable()
	{
		base.CancelInvoke("ActivateEvent");
		this.active = false;
		if (this.startOverOnEnable)
		{
			this.currentEvent = 0;
		}
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0003AA44 File Offset: 0x00038C44
	public void StartEvents()
	{
		if (this.events.Length == 0)
		{
			Debug.LogError("No events set in EventsSequence on " + base.gameObject.name, base.gameObject);
			return;
		}
		if (!this.active)
		{
			this.active = true;
			base.Invoke("ActivateEvent", this.delay);
			return;
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0003AA9C File Offset: 0x00038C9C
	private void ActivateEvent()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.currentEvent < this.events.Length)
		{
			this.events[this.currentEvent].Invoke("");
		}
		if (this.currentEvent < this.events.Length - 1)
		{
			this.currentEvent++;
			base.Invoke("ActivateEvent", this.delay);
			return;
		}
		if (this.loop)
		{
			this.currentEvent = 0;
			if (this.delay != 0f)
			{
				base.Invoke("ActivateEvent", this.delay);
				return;
			}
			base.Invoke("ActivateEvent", Time.deltaTime);
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0003AB4E File Offset: 0x00038D4E
	public void ChangeDelay(float newDelay)
	{
		this.delay = newDelay;
	}

	// Token: 0x04000B49 RID: 2889
	public float delay = 1f;

	// Token: 0x04000B4A RID: 2890
	public bool loop;

	// Token: 0x04000B4B RID: 2891
	public bool startOverOnEnable;

	// Token: 0x04000B4C RID: 2892
	public UltrakillEvent[] events;

	// Token: 0x04000B4D RID: 2893
	private bool active;

	// Token: 0x04000B4E RID: 2894
	private int currentEvent;
}
