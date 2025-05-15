using System;
using Train;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200048A RID: 1162
public class TramControl : MonoBehaviour
{
	// Token: 0x06001A99 RID: 6809 RVA: 0x000DA9DE File Offset: 0x000D8BDE
	private void Awake()
	{
		if (this.targetTram)
		{
			this.targetTram.controller = this;
		}
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000DA9FC File Offset: 0x000D8BFC
	public void SpeedUp()
	{
		if (this.SpeedUp(1))
		{
			if (this.clickSound)
			{
				Object.Instantiate<GameObject>(this.clickSound, base.transform.position, Quaternion.identity, base.transform);
				return;
			}
		}
		else if (this.clickFailSound)
		{
			Object.Instantiate<GameObject>(this.clickFailSound, base.transform.position, Quaternion.identity, base.transform);
		}
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x000DAA74 File Offset: 0x000D8C74
	public void SpeedDown()
	{
		if (this.SpeedDown(1))
		{
			if (this.clickSound)
			{
				Object.Instantiate<GameObject>(this.clickDownSound, base.transform.position, Quaternion.identity, base.transform);
				return;
			}
		}
		else if (this.clickFailSound)
		{
			Object.Instantiate<GameObject>(this.clickFailSound, base.transform.position, Quaternion.identity, base.transform);
		}
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x000DAAEC File Offset: 0x000D8CEC
	public bool SpeedUp(int amount)
	{
		if (!this.targetTram.poweredOn)
		{
			return false;
		}
		if (this.targetTram.currentPoint != null && this.targetTram.currentPoint.GetDestination(true) == null)
		{
			return false;
		}
		if (this.zapAmount > 0f)
		{
			this.currentSpeedStep = this.maxSpeedStep;
		}
		else if (this.currentSpeedStep < this.maxSpeedStep)
		{
			if (this.currentSpeedStep + amount <= this.maxSpeedStep)
			{
				this.currentSpeedStep += amount;
			}
			else
			{
				this.currentSpeedStep = this.maxSpeedStep;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000DAB90 File Offset: 0x000D8D90
	public bool SpeedDown(int amount)
	{
		if (!this.targetTram.poweredOn)
		{
			return false;
		}
		if (this.targetTram.currentPoint != null && this.targetTram.currentPoint.GetDestination(false) == null)
		{
			return false;
		}
		if (this.zapAmount > 0f)
		{
			this.currentSpeedStep = this.minSpeedStep;
		}
		else if (this.currentSpeedStep > this.minSpeedStep)
		{
			if (this.currentSpeedStep - amount >= this.minSpeedStep)
			{
				this.currentSpeedStep -= amount;
			}
			else
			{
				this.currentSpeedStep = this.minSpeedStep;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x000DAC34 File Offset: 0x000D8E34
	private void LateUpdate()
	{
		if (this.targetTram == null || !base.enabled || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.zapAmount > 0f)
		{
			this.zapAmount = Mathf.MoveTowards(this.zapAmount, 0f, Time.deltaTime);
			this.targetTram.zapAmount = this.zapAmount;
			if (this.currentSpeedStep != 0)
			{
				this.currentSpeedStep = ((this.currentSpeedStep > 0) ? this.maxSpeedStep : this.minSpeedStep);
			}
			this.targetTram.speed = (float)this.currentSpeedStep * (this.speedMultiplier / 10f);
		}
		else
		{
			this.targetTram.speed = Mathf.MoveTowards(this.targetTram.speed, (float)this.currentSpeedStep * (this.speedMultiplier / 10f), this.speedMultiplier / 10f * Time.deltaTime);
		}
		this.UpdateZapEffects();
		if (this.currentSpeedStep != 0)
		{
			if (!this.targetTram.poweredOn)
			{
				this.currentSpeedStep = 0;
			}
			else if (this.targetTram.movementDirection == TramMovementDirection.Forward && !this.targetTram.canGoForward)
			{
				this.currentSpeedStep = 0;
				this.targetTram.speed = 0f;
			}
			else if (this.targetTram.movementDirection == TramMovementDirection.Backward && !this.targetTram.canGoBackward)
			{
				this.currentSpeedStep = 0;
				this.targetTram.speed = 0f;
			}
		}
		if (this.lastSpeedStep != this.currentSpeedStep)
		{
			this.lastSpeedStep = this.currentSpeedStep;
			this.UpdateSpeedIndicators();
		}
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x000DADCE File Offset: 0x000D8FCE
	private void FixedUpdate()
	{
		if (this.maxPlayerDistance != 0f && Vector3.Distance(base.transform.position, MonoSingleton<PlayerTracker>.Instance.GetPlayer().position) > this.maxPlayerDistance)
		{
			this.currentSpeedStep = 0;
		}
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x000DAE0C File Offset: 0x000D900C
	private void UpdateSpeedIndicators()
	{
		for (int i = 0; i < this.speedIndicators.Length; i++)
		{
			this.speedIndicators[i].color = ((i == this.currentSpeedStep - this.minSpeedStep) ? this.speedOnColor : this.speedOffColor);
		}
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x000DAE57 File Offset: 0x000D9057
	public void Zap()
	{
		this.zapAmount = 5f;
		this.targetTram.zapAmount = this.zapAmount;
		this.UpdateZapEffects();
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x000DAE7C File Offset: 0x000D907C
	private void UpdateZapEffects()
	{
		if (this.zapAmount > 0f && !this.zapEffects.activeSelf)
		{
			this.zapEffects.SetActive(true);
		}
		else if (this.zapAmount <= 0f && this.zapEffects.activeSelf)
		{
			this.zapEffects.SetActive(false);
		}
		this.zapLight.intensity = Mathf.Lerp(0f, 10f, this.zapAmount);
		this.zapSprite.color = new Color(this.zapSprite.color.r, this.zapSprite.color.g, this.zapSprite.color.b, Mathf.Lerp(0f, 1f, this.zapAmount));
		this.zapSound.volume = Mathf.Lerp(0f, 0.5f, this.zapAmount);
		this.zapSound.pitch = Mathf.Lerp(0f, 1f, this.zapAmount);
	}

	// Token: 0x04002548 RID: 9544
	[SerializeField]
	private Tram targetTram;

	// Token: 0x04002549 RID: 9545
	[Space]
	[SerializeField]
	private GameObject clickSound;

	// Token: 0x0400254A RID: 9546
	[SerializeField]
	private GameObject clickDownSound;

	// Token: 0x0400254B RID: 9547
	[SerializeField]
	private GameObject clickFailSound;

	// Token: 0x0400254C RID: 9548
	[Space]
	[SerializeField]
	private int maxSpeedStep;

	// Token: 0x0400254D RID: 9549
	[SerializeField]
	private int minSpeedStep;

	// Token: 0x0400254E RID: 9550
	[SerializeField]
	private float speedMultiplier;

	// Token: 0x0400254F RID: 9551
	[HideInInspector]
	public float zapAmount;

	// Token: 0x04002550 RID: 9552
	[SerializeField]
	private Image[] speedIndicators;

	// Token: 0x04002551 RID: 9553
	public Color speedOffColor;

	// Token: 0x04002552 RID: 9554
	public Color speedOnColor;

	// Token: 0x04002553 RID: 9555
	public float maxPlayerDistance = 15f;

	// Token: 0x04002554 RID: 9556
	public int currentSpeedStep;

	// Token: 0x04002555 RID: 9557
	private int lastSpeedStep;

	// Token: 0x04002556 RID: 9558
	[SerializeField]
	private GameObject zapEffects;

	// Token: 0x04002557 RID: 9559
	[SerializeField]
	private Light zapLight;

	// Token: 0x04002558 RID: 9560
	[SerializeField]
	private SpriteRenderer zapSprite;

	// Token: 0x04002559 RID: 9561
	[SerializeField]
	private AudioSource zapSound;
}
