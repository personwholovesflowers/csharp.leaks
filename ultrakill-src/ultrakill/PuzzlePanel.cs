using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200036E RID: 878
public class PuzzlePanel : MonoBehaviour
{
	// Token: 0x06001468 RID: 5224 RVA: 0x000A56F4 File Offset: 0x000A38F4
	private void Start()
	{
		this.img = base.GetComponent<Image>();
		this.defaultSprite = this.img.sprite;
		this.pc = base.GetComponentInParent<PuzzleController>();
		this.pl = base.transform.GetChild(0).GetComponent<PuzzleLine>();
		if (this.pl != null)
		{
			this.pl.transform.SetParent(base.transform.parent, true);
		}
		if (!base.TryGetComponent<ControllerPointer>(out this.pointer))
		{
			this.pointer = base.gameObject.AddComponent<ControllerPointer>();
		}
		this.pointer.OnEnter.AddListener(delegate
		{
			this.pc.Hovered(this);
		});
		this.pointer.OnPressed.AddListener(delegate
		{
			this.pc.Clicked(this);
		});
		this.pointer.OnReleased.AddListener(new UnityAction(this.pc.Unclicked));
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x000A57E4 File Offset: 0x000A39E4
	public void Activate(TileColor color)
	{
		if (this.tileType == TileType.End)
		{
			base.transform.GetChild(0).GetComponent<Image>().fillCenter = true;
		}
		this.activated = true;
		Color color2 = this.pl.TranslateColor(color);
		this.img.color = new Color(color2.r, color2.g, color2.b, 1f);
		this.img.sprite = this.activeSprite;
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x000A5860 File Offset: 0x000A3A60
	public void DeActivate()
	{
		if (this.tileType == TileType.End)
		{
			base.transform.GetChild(0).GetComponent<Image>().fillCenter = false;
		}
		this.activated = false;
		this.img.color = new Color(1f, 1f, 1f, 1f);
		this.img.sprite = this.defaultSprite;
	}

	// Token: 0x04001C07 RID: 7175
	public GameObject[] tileTypePrefabs;

	// Token: 0x04001C08 RID: 7176
	public Color[] tileColors;

	// Token: 0x04001C09 RID: 7177
	public GameObject currentPanel;

	// Token: 0x04001C0A RID: 7178
	public TileType tileType;

	// Token: 0x04001C0B RID: 7179
	public TileColor tileColor;

	// Token: 0x04001C0C RID: 7180
	private Image img;

	// Token: 0x04001C0D RID: 7181
	private Sprite defaultSprite;

	// Token: 0x04001C0E RID: 7182
	[SerializeField]
	private Sprite activeSprite;

	// Token: 0x04001C0F RID: 7183
	private bool activated;

	// Token: 0x04001C10 RID: 7184
	private PuzzleController pc;

	// Token: 0x04001C11 RID: 7185
	[HideInInspector]
	public PuzzleLine pl;

	// Token: 0x04001C12 RID: 7186
	private ControllerPointer pointer;
}
