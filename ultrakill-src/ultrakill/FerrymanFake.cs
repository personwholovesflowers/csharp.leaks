using System;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class FerrymanFake : MonoBehaviour
{
	// Token: 0x06000949 RID: 2377 RVA: 0x0003EB31 File Offset: 0x0003CD31
	public void CoinCatch()
	{
		this.originalRotation = base.transform.rotation;
		base.GetComponent<Animator>().SetTrigger("CatchCoin");
		this.trackPlayer = true;
		this.activated = true;
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0003EB64 File Offset: 0x0003CD64
	private void Update()
	{
		if (this.trackPlayer)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(MonoSingleton<NewMovement>.Instance.transform.position.x, base.transform.position.y, MonoSingleton<NewMovement>.Instance.transform.position.z) - base.transform.position), Time.deltaTime * 1200f);
		}
		else if (this.activated)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.originalRotation, Time.deltaTime * 10f * Mathf.Max(Mathf.Min(Quaternion.Angle(base.transform.rotation, this.originalRotation), 20f), 0.01f));
		}
		if (this.jumping)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, new Vector3(this.realFerryman.transform.position.x, base.transform.position.y, this.realFerryman.transform.position.z), Time.deltaTime * 10f);
			if (base.transform.position.y < this.realFerryman.transform.position.y + 1f)
			{
				this.OnLand();
			}
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0003ECF5 File Offset: 0x0003CEF5
	private void FixedUpdate()
	{
		if (this.jumping)
		{
			this.rb.AddForce(Vector3.down * 9.81f * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0003ED24 File Offset: 0x0003CF24
	public void ReturnToRotation()
	{
		this.trackPlayer = false;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0003ED2D File Offset: 0x0003CF2D
	public void BlowCoin()
	{
		this.onCoinBlow.Invoke("");
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0003ED40 File Offset: 0x0003CF40
	public void StartFight()
	{
		Animator animator;
		if (base.TryGetComponent<Animator>(out animator))
		{
			animator.SetTrigger("Jump");
			base.GetComponent<Collider>().enabled = false;
			this.rb = base.GetComponent<Rigidbody>();
			if (this.rb)
			{
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
				this.rb.AddForce(base.transform.up * 25f, ForceMode.VelocityChange);
			}
			this.jumping = true;
			return;
		}
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003EDCA File Offset: 0x0003CFCA
	public void OnLand()
	{
		this.realFerryman.SetActive(true);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000BCB RID: 3019
	private bool activated;

	// Token: 0x04000BCC RID: 3020
	private bool trackPlayer;

	// Token: 0x04000BCD RID: 3021
	private bool jumping;

	// Token: 0x04000BCE RID: 3022
	private Quaternion originalRotation;

	// Token: 0x04000BCF RID: 3023
	public UltrakillEvent onCoinBlow;

	// Token: 0x04000BD0 RID: 3024
	public GameObject realFerryman;

	// Token: 0x04000BD1 RID: 3025
	private Rigidbody rb;
}
