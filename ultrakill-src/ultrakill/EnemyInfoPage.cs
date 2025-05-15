using System;
using plog;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000192 RID: 402
public class EnemyInfoPage : ListComponent<EnemyInfoPage>
{
	// Token: 0x060007FF RID: 2047 RVA: 0x0003744A File Offset: 0x0003564A
	private void Start()
	{
		this.UpdateInfo();
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00037454 File Offset: 0x00035654
	public void UpdateInfo()
	{
		if (this.enemyList.childCount > 1)
		{
			for (int i = this.enemyList.childCount - 1; i > 0; i--)
			{
				Object.Destroy(this.enemyList.GetChild(i).gameObject);
			}
		}
		SpawnableObject[] enemies = this.objects.enemies;
		for (int j = 0; j < enemies.Length; j++)
		{
			SpawnableObject spawnableObject = enemies[j];
			if (spawnableObject == null)
			{
				EnemyInfoPage.Log.Warning("Spawnable object in enemy list is null!", null, null, null);
			}
			else
			{
				bool flag = MonoSingleton<BestiaryData>.Instance.GetEnemy(spawnableObject.enemyType) >= 1;
				if (flag)
				{
					this.buttonTemplateWickedNoise.enabled = spawnableObject.enemyType == EnemyType.Wicked;
					this.buttonTemplateBackground.color = spawnableObject.backgroundColor;
					this.buttonTemplateForeground.sprite = spawnableObject.gridIcon;
				}
				else
				{
					this.buttonTemplateWickedNoise.enabled = false;
					this.buttonTemplateBackground.color = Color.gray;
					this.buttonTemplateForeground.sprite = this.lockedSprite;
				}
				GameObject gameObject = Object.Instantiate<GameObject>(this.buttonTemplate, this.enemyList);
				gameObject.SetActive(true);
				if (flag)
				{
					gameObject.GetComponentInChildren<ShopButton>().deactivated = false;
					gameObject.GetComponentInChildren<Button>().onClick.AddListener(delegate
					{
						this.currentSpawnable = spawnableObject;
						this.DisplayInfo(spawnableObject);
					});
				}
				else
				{
					gameObject.GetComponentInChildren<ShopButton>().deactivated = true;
				}
			}
		}
		this.buttonTemplate.SetActive(false);
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x000375F0 File Offset: 0x000357F0
	private void SwapLayers(Transform target, int layer)
	{
		foreach (object obj in target)
		{
			Transform transform = (Transform)obj;
			transform.gameObject.layer = layer;
			if (transform.childCount > 0)
			{
				this.SwapLayers(transform, layer);
			}
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0003765C File Offset: 0x0003585C
	private void DisplayInfo(SpawnableObject source)
	{
		this.enemyPageTitle.text = source.objectName;
		this.enemyEntryTitle.text = source.objectName;
		string text = "<color=#FF4343>TYPE:</color> " + source.type + "\n\n<color=#FF4343>DATA:</color>\n";
		if (MonoSingleton<BestiaryData>.Instance.GetEnemy(source.enemyType) > 1)
		{
			text += source.description;
		}
		else
		{
			text += "???";
		}
		text = text + "\n\n<color=#FF4343>STRATEGY:</color>\n" + source.strategy;
		this.enemyPageContent.text = text;
		this.enemyPageContent.rectTransform.localPosition = new Vector3(this.enemyPageContent.rectTransform.localPosition.x, 0f, this.enemyPageContent.rectTransform.localPosition.z);
		for (int i = 0; i < this.enemyPreviewWrapper.childCount; i++)
		{
			Object.Destroy(this.enemyPreviewWrapper.GetChild(i).gameObject);
		}
		if (source.enemyType == EnemyType.Wicked)
		{
			this.wickedNoise.SetActive(true);
			return;
		}
		this.wickedNoise.SetActive(false);
		GameObject gameObject = Object.Instantiate<GameObject>(source.preview, this.enemyPreviewWrapper);
		int layer = this.enemyPreviewWrapper.gameObject.layer;
		this.SwapLayers(gameObject.transform, layer);
		gameObject.layer = layer;
		gameObject.transform.localPosition = source.menuOffset;
		gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, source.menuScale);
		Spin spin = gameObject.AddComponent<Spin>();
		spin.spinDirection = new Vector3(0f, 1f, 0f);
		spin.speed = 10f;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x00037813 File Offset: 0x00035A13
	public void DisplayInfo()
	{
		if (this.currentSpawnable == null)
		{
			return;
		}
		this.DisplayInfo(this.currentSpawnable);
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00037830 File Offset: 0x00035A30
	public void UndisplayInfo()
	{
		this.currentSpawnable = null;
	}

	// Token: 0x04000A99 RID: 2713
	private static readonly global::plog.Logger Log = new global::plog.Logger("EnemyInfoPage");

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private TMP_Text enemyPageTitle;

	// Token: 0x04000A9B RID: 2715
	[SerializeField]
	private TMP_Text enemyEntryTitle;

	// Token: 0x04000A9C RID: 2716
	[SerializeField]
	private TMP_Text enemyPageContent;

	// Token: 0x04000A9D RID: 2717
	[SerializeField]
	private Transform enemyPreviewWrapper;

	// Token: 0x04000A9E RID: 2718
	[SerializeField]
	private GameObject wickedNoise;

	// Token: 0x04000A9F RID: 2719
	[Space]
	[SerializeField]
	private Transform enemyList;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private GameObject buttonTemplate;

	// Token: 0x04000AA1 RID: 2721
	[SerializeField]
	private Image buttonTemplateBackground;

	// Token: 0x04000AA2 RID: 2722
	[SerializeField]
	private Image buttonTemplateForeground;

	// Token: 0x04000AA3 RID: 2723
	[SerializeField]
	private Image buttonTemplateWickedNoise;

	// Token: 0x04000AA4 RID: 2724
	[SerializeField]
	private Sprite lockedSprite;

	// Token: 0x04000AA5 RID: 2725
	[Space]
	[SerializeField]
	private SpawnableObjectsDatabase objects;

	// Token: 0x04000AA6 RID: 2726
	private SpawnableObject currentSpawnable;
}
