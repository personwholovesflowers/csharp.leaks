using System;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class PlatformerEnding : MonoBehaviour
{
	// Token: 0x06001301 RID: 4865 RVA: 0x000972C0 File Offset: 0x000954C0
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject)
		{
			this.Activate();
		}
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x000972F4 File Offset: 0x000954F4
	private void Update()
	{
		if (!this.activated)
		{
			return;
		}
		this.teleportEffect.localScale = Vector3.MoveTowards(this.teleportEffect.localScale, Vector3.up * this.teleportEffect.localScale.y, Time.deltaTime * 2.5f);
		if (this.teleportEffect.localScale.x == 0f && this.teleportEffect.localScale.z == 0f)
		{
			this.Done();
		}
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x00097380 File Offset: 0x00095580
	public void Activate()
	{
		MonoSingleton<PlatformerMovement>.Instance.transform.parent.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
		MonoSingleton<PlatformerMovement>.Instance.gameObject.SetActive(false);
		GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<PlatformerMovement>.Instance.gameObject, MonoSingleton<PlatformerMovement>.Instance.transform.position, MonoSingleton<PlatformerMovement>.Instance.transform.rotation, this.teleportEffect);
		SandboxUtils.StripForPreview(gameObject.transform, null, true);
		gameObject.SetActive(true);
		base.Invoke("DelayedActivateEffect", 0.5f);
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x00097412 File Offset: 0x00095612
	private void DelayedActivateEffect()
	{
		this.activated = true;
		this.onActivate.Invoke("");
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x0009742B File Offset: 0x0009562B
	public void Done()
	{
		this.activated = false;
	}

	// Token: 0x04001A1D RID: 6685
	[SerializeField]
	private Transform teleportEffect;

	// Token: 0x04001A1E RID: 6686
	[SerializeField]
	private UltrakillEvent onActivate;

	// Token: 0x04001A1F RID: 6687
	private bool activated;
}
