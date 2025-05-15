using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000205 RID: 517
public class GabrielOutro : MonoBehaviour
{
	// Token: 0x06000AB7 RID: 2743 RVA: 0x0004C7AB File Offset: 0x0004A9AB
	private void Start()
	{
		if (!this.target)
		{
			this.target = MonoSingleton<NewMovement>.Instance.transform;
		}
		this.anim = base.GetComponent<Animator>();
		base.Invoke("ToEndAnimation", 0.75f);
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x0004C7E6 File Offset: 0x0004A9E6
	private void OnDisable()
	{
		base.CancelInvoke();
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x0004C7F0 File Offset: 0x0004A9F0
	private void Update()
	{
		if (this.tracking)
		{
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z) - base.transform.position, Vector3.up);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * Mathf.Clamp(Quaternion.Angle(base.transform.rotation, quaternion) * 2f, 1f, 90f));
		}
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0004C8A0 File Offset: 0x0004AAA0
	public void SetSource(Transform tf)
	{
		if (!this.target)
		{
			this.target = MonoSingleton<NewMovement>.Instance.transform;
		}
		base.transform.position = tf.position;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0004C91C File Offset: 0x0004AB1C
	public void ToEndAnimation()
	{
		int num = Mathf.RoundToInt(Vector3.Distance(base.transform.position, this.middlePosition) / 2.5f);
		for (int i = 0; i < num; i++)
		{
			if (this.gabe)
			{
				this.gabe.CreateDecoy(Vector3.Lerp(base.transform.position, this.middlePosition, (float)i / (float)num), (float)i / (float)num + 0.1f, this.anim);
			}
			else
			{
				this.gabe2.CreateDecoy(Vector3.Lerp(base.transform.position, this.middlePosition, (float)i / (float)num), (float)i / (float)num + 0.1f, this.anim);
			}
		}
		base.transform.position = this.middlePosition;
		Object.Instantiate<GameObject>(this.gabe ? this.gabe.teleportSound : this.gabe2.teleportSound, base.transform.position, Quaternion.identity);
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.anim.Play("Outro");
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0004CA78 File Offset: 0x0004AC78
	public void EnableWings()
	{
		UnityEvent unityEvent = this.onEnableWings;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		this.tracking = true;
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0004CA92 File Offset: 0x0004AC92
	public void RageStart()
	{
		UnityEvent unityEvent = this.onRageStart;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0004CAA4 File Offset: 0x0004ACA4
	public void Disappear()
	{
		UnityEvent unityEvent = this.onDisappear;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04000E45 RID: 3653
	private Transform target;

	// Token: 0x04000E46 RID: 3654
	private Animator anim;

	// Token: 0x04000E47 RID: 3655
	public Vector3 middlePosition;

	// Token: 0x04000E48 RID: 3656
	public Gabriel gabe;

	// Token: 0x04000E49 RID: 3657
	public GabrielSecond gabe2;

	// Token: 0x04000E4A RID: 3658
	public UnityEvent onEnableWings;

	// Token: 0x04000E4B RID: 3659
	public UnityEvent onRageStart;

	// Token: 0x04000E4C RID: 3660
	public UnityEvent onDisappear;

	// Token: 0x04000E4D RID: 3661
	private bool tracking;
}
