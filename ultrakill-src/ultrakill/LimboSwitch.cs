using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002D4 RID: 724
public class LimboSwitch : MonoBehaviour
{
	// Token: 0x06000FBC RID: 4028 RVA: 0x000755C8 File Offset: 0x000737C8
	private void Start()
	{
		this.mr = base.GetComponent<MeshRenderer>();
		this.block = new MaterialPropertyBlock();
		if (!this.dontSave && ((this.type == SwitchLockType.Limbo && GameProgressSaver.GetLimboSwitch(this.switchNumber - 1)) || (this.type == SwitchLockType.Shotgun && GameProgressSaver.GetShotgunSwitch(this.switchNumber - 1))))
		{
			this.beenPressed = true;
			UnityEvent unityEvent = this.onAlreadyPressed;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.fadeAmount = 2f;
			this.mr.GetPropertyBlock(this.block);
			this.block.SetFloat(UKShaderProperties.EmissiveIntensity, this.fadeAmount);
			this.mr.SetPropertyBlock(this.block);
		}
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x00075684 File Offset: 0x00073884
	private void Update()
	{
		if (this.beenPressed && this.fadeAmount < 2f)
		{
			this.fadeAmount = Mathf.MoveTowards(this.fadeAmount, 2f, Time.deltaTime);
			this.mr.GetPropertyBlock(this.block);
			this.block.SetFloat(UKShaderProperties.EmissiveIntensity, this.fadeAmount);
			this.mr.SetPropertyBlock(this.block);
		}
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x000756FC File Offset: 0x000738FC
	public void Pressed()
	{
		if (!this.beenPressed)
		{
			this.beenPressed = true;
			base.GetComponent<AudioSource>().Play();
			base.Invoke("DelayedEffect", 1f);
			if (!this.dontSave)
			{
				if (this.type == SwitchLockType.Limbo)
				{
					GameProgressSaver.SetLimboSwitch(this.switchNumber - 1);
					return;
				}
				if (this.type == SwitchLockType.Shotgun)
				{
					GameProgressSaver.SetShotgunSwitch(this.switchNumber - 1);
				}
			}
		}
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x00075768 File Offset: 0x00073968
	private void DelayedEffect()
	{
		UnityEvent unityEvent = this.onDelayedEffect;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04001544 RID: 5444
	public SwitchLockType type = SwitchLockType.Limbo;

	// Token: 0x04001545 RID: 5445
	public bool beenPressed;

	// Token: 0x04001546 RID: 5446
	public int switchNumber;

	// Token: 0x04001547 RID: 5447
	private float fadeAmount;

	// Token: 0x04001548 RID: 5448
	public bool dontSave;

	// Token: 0x04001549 RID: 5449
	public UnityEvent onAlreadyPressed;

	// Token: 0x0400154A RID: 5450
	public UnityEvent onDelayedEffect;

	// Token: 0x0400154B RID: 5451
	private MaterialPropertyBlock block;

	// Token: 0x0400154C RID: 5452
	private MeshRenderer mr;
}
