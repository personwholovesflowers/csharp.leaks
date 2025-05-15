using System;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class Pond : MonoBehaviour, IBloodstainReceiver
{
	// Token: 0x06001DA9 RID: 7593 RVA: 0x000F7974 File Offset: 0x000F5B74
	private void Start()
	{
		this.waterComponent = base.GetComponent<Water>();
		this.underwaterColor = this.waterComponent.clr;
		this.waterSurfaceColor = this.waterSurface.sharedMaterials[0].GetColor("_Color");
		this.propertyBlock = new MaterialPropertyBlock();
		this.bloodFillAmountCopy = this.bloodFillAmount;
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000F79D4 File Offset: 0x000F5BD4
	private void Update()
	{
		this.bloodFillAmount = Mathf.Clamp01(this.bloodFillAmount);
		if (this.isDraining)
		{
			this.bloodFillAmount -= this.bloodDrainSpeed * Time.deltaTime;
			this.bloodFillAmount = Mathf.Clamp01(this.bloodFillAmount);
		}
		if (this.bloodFillAmount != this.lastBloodFillAmount)
		{
			this.UpdateVisuals();
		}
		this.lastBloodFillAmount = this.bloodFillAmount;
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x000F7A44 File Offset: 0x000F5C44
	public void StoreBlood()
	{
		this.bloodFillAmountCopy = this.bloodFillAmount;
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x000F7A52 File Offset: 0x000F5C52
	public void RestoreBlood()
	{
		this.bloodFillAmount = this.bloodFillAmountCopy;
		this.UpdateVisuals();
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x000F7A68 File Offset: 0x000F5C68
	private void UpdateVisuals()
	{
		Color color = Color.Lerp(this.waterSurfaceColor, this.surfaceBloodColor, this.bloodFillAmount);
		this.propertyBlock.SetColor("_Color", color);
		this.waterSurface.SetPropertyBlock(this.propertyBlock);
		Color color2 = Color.Lerp(this.underwaterColor, this.underwaterBloodColor, this.bloodFillAmount);
		this.waterComponent.UpdateColor(color2);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x000F7AD3 File Offset: 0x000F5CD3
	public bool HandleBloodstainHit(ref RaycastHit rhit)
	{
		this.bloodFillAmount += this.bloodFillSpeed;
		return true;
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x000F7AEC File Offset: 0x000F5CEC
	private void OnTriggerEnter(Collider col)
	{
		GoreSplatter goreSplatter;
		if (col.TryGetComponent<GoreSplatter>(out goreSplatter))
		{
			goreSplatter.bloodAbsorberCount++;
			MonoSingleton<BloodCheckerManager>.Instance.AddPondGore(goreSplatter);
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (col.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
		{
			enemyIdentifierIdentifier.bloodAbsorberCount++;
			MonoSingleton<BloodCheckerManager>.Instance.AddPondGib(enemyIdentifierIdentifier);
			return;
		}
		UnderwaterController underwaterController;
		if (col.TryGetComponent<UnderwaterController>(out underwaterController))
		{
			MonoSingleton<BloodCheckerManager>.Instance.playerInPond = true;
		}
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x000F7B58 File Offset: 0x000F5D58
	private void OnTriggerExit(Collider col)
	{
		GoreSplatter goreSplatter;
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		UnderwaterController underwaterController;
		if (col.TryGetComponent<GoreSplatter>(out goreSplatter))
		{
			goreSplatter.bloodAbsorberCount--;
			if (StockMapInfo.Instance.removeGibsWithoutAbsorbers)
			{
				goreSplatter.Invoke("RepoolIfNoAbsorber", StockMapInfo.Instance.gibRemoveTime);
				return;
			}
		}
		else if (col.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
		{
			enemyIdentifierIdentifier.bloodAbsorberCount--;
			if (StockMapInfo.Instance.removeGibsWithoutAbsorbers)
			{
				enemyIdentifierIdentifier.SetupForHellBath();
				return;
			}
		}
		else if (col.TryGetComponent<UnderwaterController>(out underwaterController))
		{
			MonoSingleton<BloodCheckerManager>.Instance.playerInPond = false;
		}
	}

	// Token: 0x04002A03 RID: 10755
	public GameObject owningRoom;

	// Token: 0x04002A04 RID: 10756
	public float bloodFillSpeed;

	// Token: 0x04002A05 RID: 10757
	public float bloodDrainSpeed = 0.1f;

	// Token: 0x04002A06 RID: 10758
	public Color surfaceBloodColor = Color.red;

	// Token: 0x04002A07 RID: 10759
	public Color underwaterBloodColor = Color.red;

	// Token: 0x04002A08 RID: 10760
	public float bloodFillAmount;

	// Token: 0x04002A09 RID: 10761
	private Color underwaterColor;

	// Token: 0x04002A0A RID: 10762
	private Color waterSurfaceColor;

	// Token: 0x04002A0B RID: 10763
	private Water waterComponent;

	// Token: 0x04002A0C RID: 10764
	public Renderer waterSurface;

	// Token: 0x04002A0D RID: 10765
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x04002A0E RID: 10766
	public bool isDraining;

	// Token: 0x04002A0F RID: 10767
	public float bloodFillAmountCopy;

	// Token: 0x04002A10 RID: 10768
	private float lastBloodFillAmount = 9999f;
}
