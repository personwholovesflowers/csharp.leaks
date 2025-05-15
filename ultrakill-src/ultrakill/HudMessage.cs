using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000252 RID: 594
public class HudMessage : MonoBehaviour
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00063DA0 File Offset: 0x00061FA0
	private string PlayerPref
	{
		get
		{
			string text = this.playerPref;
			if (text == "SecMisTut")
			{
				return "secretMissionPopup";
			}
			if (!(text == "ShoUseTut"))
			{
				return this.playerPref;
			}
			return "hideShotgunPopup";
		}
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x00063DE4 File Offset: 0x00061FE4
	private void Start()
	{
		if (base.GetComponent<Collider>() == null)
		{
			this.colliderless = true;
			if (this.PlayerPref == "" || this.playerPref == null)
			{
				this.PlayMessage(false);
				return;
			}
			if (!MonoSingleton<PrefsManager>.Instance.GetBool(this.PlayerPref, false))
			{
				MonoSingleton<PrefsManager>.Instance.SetBool(this.PlayerPref, true);
				this.PlayMessage(false);
			}
		}
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x00063E54 File Offset: 0x00062054
	private void OnEnable()
	{
		if (this.colliderless && (!this.activated || this.notOneTime))
		{
			if (this.PlayerPref == "")
			{
				this.PlayMessage(false);
				return;
			}
			if (!MonoSingleton<PrefsManager>.Instance.GetBool(this.PlayerPref, false))
			{
				MonoSingleton<PrefsManager>.Instance.SetBool(this.PlayerPref, true);
				this.PlayMessage(false);
			}
		}
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00063EC0 File Offset: 0x000620C0
	private void OnDisable()
	{
		if (base.gameObject.scene.isLoaded && this.deactiveOnDisable && this.activated)
		{
			this.Done();
		}
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x00063EF8 File Offset: 0x000620F8
	private void Update()
	{
		if (this.activated && this.timed)
		{
			MonoSingleton<HudMessageReceiver>.Instance.ForceEnable();
		}
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x00063F14 File Offset: 0x00062114
	private void OnTriggerEnter(Collider other)
	{
		if (!this.dontActivateOnTriggerEnter && other.gameObject.CompareTag("Player") && (!this.activated || this.notOneTime))
		{
			if (this.PlayerPref == "")
			{
				this.PlayMessage(false);
				return;
			}
			if (!MonoSingleton<PrefsManager>.Instance.GetBool(this.PlayerPref, false))
			{
				MonoSingleton<PrefsManager>.Instance.SetBool(this.PlayerPref, true);
				this.PlayMessage(false);
			}
		}
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00063F90 File Offset: 0x00062190
	private void OnTriggerExit(Collider other)
	{
		if (!this.dontActivateOnTriggerEnter && other.gameObject.CompareTag("Player") && this.activated && this.deactiveOnTriggerExit)
		{
			this.Done();
		}
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00063FC2 File Offset: 0x000621C2
	private void Done()
	{
		this.activated = false;
		MonoSingleton<HudMessageReceiver>.Instance.ClearMessage();
		this.Begone();
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x00063FDB File Offset: 0x000621DB
	private void Begone()
	{
		if (!this.notOneTime)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x00063FEC File Offset: 0x000621EC
	public void PlayMessage(bool hasToBeEnabled = false)
	{
		if (this.deactivating)
		{
			this.Done();
			return;
		}
		if ((this.activated && !this.notOneTime) || (hasToBeEnabled && (!base.gameObject.activeInHierarchy || !base.enabled)))
		{
			return;
		}
		this.activated = true;
		if (this.actionReference == null)
		{
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage(this.message, "", this.message2, 0, this.silent, true, false);
		}
		else
		{
			string text = MonoSingleton<InputManager>.Instance.GetBindingString(this.actionReference.action.id);
			if (string.IsNullOrEmpty(text))
			{
				text = "NO BINDING";
			}
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage(this.message, text, this.message2, 0, this.silent, true, false);
		}
		if (this.timed && this.notOneTime)
		{
			base.CancelInvoke("Done");
			base.Invoke("Done", this.timerTime);
			return;
		}
		if (this.timed)
		{
			base.Invoke("Done", this.timerTime);
			return;
		}
		if (!this.deactiveOnTriggerExit && !this.deactiveOnDisable)
		{
			base.Invoke("Begone", 1f);
		}
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x0006411C File Offset: 0x0006231C
	public void ChangeMessage(string newMessage)
	{
		this.message = newMessage;
		this.actionReference = null;
		this.message2 = "";
	}

	// Token: 0x04001194 RID: 4500
	public InputActionReference actionReference;

	// Token: 0x04001195 RID: 4501
	public bool timed;

	// Token: 0x04001196 RID: 4502
	public bool deactivating;

	// Token: 0x04001197 RID: 4503
	public bool notOneTime;

	// Token: 0x04001198 RID: 4504
	public bool dontActivateOnTriggerEnter;

	// Token: 0x04001199 RID: 4505
	public bool silent;

	// Token: 0x0400119A RID: 4506
	public bool deactiveOnTriggerExit;

	// Token: 0x0400119B RID: 4507
	public bool deactiveOnDisable;

	// Token: 0x0400119C RID: 4508
	private bool activated;

	// Token: 0x0400119D RID: 4509
	public string message;

	// Token: 0x0400119E RID: 4510
	public string message2;

	// Token: 0x0400119F RID: 4511
	public string playerPref;

	// Token: 0x040011A0 RID: 4512
	private bool colliderless;

	// Token: 0x040011A1 RID: 4513
	public float timerTime = 5f;
}
