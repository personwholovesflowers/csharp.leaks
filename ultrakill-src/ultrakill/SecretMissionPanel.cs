using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003E1 RID: 993
public class SecretMissionPanel : MonoBehaviour
{
	// Token: 0x0600166D RID: 5741 RVA: 0x000B4CC0 File Offset: 0x000B2EC0
	private void Start()
	{
		this.GotEnabled();
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x000B4CC0 File Offset: 0x000B2EC0
	private void OnEnable()
	{
		this.GotEnabled();
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x000B4CC8 File Offset: 0x000B2EC8
	private void Setup()
	{
		if (this.img == null)
		{
			this.img = base.GetComponent<Image>();
		}
		if (this.origSprite == null && this.img)
		{
			this.origSprite = this.img.sprite;
		}
		if (this.txt == null)
		{
			this.txt = base.GetComponentInChildren<TMP_Text>();
		}
		if (this.btn == null)
		{
			this.btn = base.GetComponent<Button>();
		}
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x000B4D50 File Offset: 0x000B2F50
	public void GotEnabled()
	{
		this.Setup();
		int secretMission = GameProgressSaver.GetSecretMission(this.missionNumber);
		if (secretMission == 2)
		{
			this.img.sprite = this.spriteOnComplete;
			this.txt.color = Color.black;
			this.btn.interactable = true;
			this.layerSelect.SecretMissionDone();
			return;
		}
		if (secretMission == 1)
		{
			this.img.sprite = this.origSprite;
			this.txt.color = Color.white;
			this.btn.interactable = true;
			return;
		}
		this.img.sprite = this.origSprite;
		this.txt.color = new Color(0.5f, 0.5f, 0.5f);
		this.btn.interactable = false;
	}

	// Token: 0x04001EFD RID: 7933
	public LayerSelect layerSelect;

	// Token: 0x04001EFE RID: 7934
	public int missionNumber;

	// Token: 0x04001EFF RID: 7935
	[HideInInspector]
	public Image img;

	// Token: 0x04001F00 RID: 7936
	[HideInInspector]
	public Sprite origSprite;

	// Token: 0x04001F01 RID: 7937
	public Sprite spriteOnComplete;

	// Token: 0x04001F02 RID: 7938
	[HideInInspector]
	public TMP_Text txt;

	// Token: 0x04001F03 RID: 7939
	[HideInInspector]
	public Button btn;
}
