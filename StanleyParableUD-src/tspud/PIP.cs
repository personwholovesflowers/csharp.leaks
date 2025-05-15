using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011C RID: 284
public class PIP : MonoBehaviour
{
	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060006CB RID: 1739 RVA: 0x000243F2 File Offset: 0x000225F2
	// (set) Token: 0x060006CC RID: 1740 RVA: 0x000243FC File Offset: 0x000225FC
	private float aspectRatio
	{
		get
		{
			return this._aspectRatio;
		}
		set
		{
			if (value != this._aspectRatio)
			{
				if ((double)this._aspectRatio >= 1.5 && (double)value < 1.5)
				{
					this.SetAspect(PIP.Aspect.Narrow);
				}
				else if ((double)this._aspectRatio < 1.5 && (double)value >= 1.5)
				{
					this.SetAspect(PIP.Aspect.Wide);
				}
				this._aspectRatio = value;
			}
		}
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00024468 File Offset: 0x00022668
	private void Awake()
	{
		Transform[] componentsInChildren = this.level_0.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == "Main-version")
			{
				componentsInChildren[i].gameObject.SetActive(true);
			}
			if (componentsInChildren[i].name == "Pause-version")
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
		}
		this.CreatePIPLevels(this.level_0);
		this.PIPBasePos = this.PIPRT.anchoredPosition3D;
		this.PIPBaseScale = this.PIPRT.localScale;
		this.canvasBaseScale = this.canvasRT.localScale;
		this.canvasBaseRect = new Rect(this.BGRT.rect);
		this.BGRT.GetComponent<RawImage>().material.SetFloat("_Strength", 0f);
		this.backgroundImages.Add(this.BGRT.GetComponent<RawImage>());
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00024560 File Offset: 0x00022760
	private void Update()
	{
		this.BGRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.canvasRT.rect.width);
		this.BGRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.canvasRT.rect.height);
		Vector3 vector = new Vector3(this.PIPBasePos.x * (this.canvasRT.rect.width / this.canvasBaseRect.width), this.PIPBasePos.y * (this.canvasRT.rect.height / this.canvasBaseRect.height), 0f);
		this.PIPRT.anchoredPosition3D = vector;
		this.PIPRT.localScale = new Vector3(this.PIPBaseScale.x * (this.canvasRT.rect.width / this.canvasBaseRect.width), this.PIPBaseScale.y * (this.canvasRT.rect.height / this.canvasBaseRect.height), 1f);
		this.aspectRatio = (float)Screen.width / (float)Screen.height;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00024697 File Offset: 0x00022897
	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.finalTex);
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x000246C0 File Offset: 0x000228C0
	private void SetAspect(PIP.Aspect aspect)
	{
		Texture2D texture2D = this.widescreenTex;
		if (aspect == PIP.Aspect.Narrow)
		{
			texture2D = this.squarescreenTex;
		}
		for (int i = 0; i < this.backgroundImages.Count; i++)
		{
			this.backgroundImages[i].texture = texture2D;
		}
		for (int j = 0; j < this.PIPsWide.Count; j++)
		{
			this.PIPsWide[j].SetActive(aspect == PIP.Aspect.Wide);
		}
		for (int k = 0; k < this.PIPsNarrow.Count; k++)
		{
			this.PIPsNarrow[k].SetActive(aspect == PIP.Aspect.Narrow);
		}
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00024760 File Offset: 0x00022960
	private void CreatePIPLevels(GameObject orig)
	{
		GameObject gameObject = orig;
		float num = 3f;
		int num2 = 0;
		while ((float)num2 < num)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject);
			gameObject2.name = "Menu_Level_" + (num2 + 1);
			gameObject2.transform.position += Vector3.right * 300f;
			int num3 = (int)Mathf.Pow((float)(num2 + 2), 2f);
			RenderTexture renderTexture = new RenderTexture(2048 / num3, 1024 / num3, 0);
			gameObject2.GetComponentInChildren<CanvasScaler>().scaleFactor = 1f / (float)num3;
			RawImage[] componentsInChildren = gameObject.transform.GetComponentsInChildren<RawImage>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name == "PIP-WIDE")
				{
					componentsInChildren[i].texture = renderTexture;
					if (!this.PIPsWide.Contains(componentsInChildren[i].gameObject))
					{
						this.PIPsWide.Add(componentsInChildren[i].gameObject);
					}
				}
				else if (componentsInChildren[i].name == "PIP-NARROW")
				{
					componentsInChildren[i].texture = renderTexture;
					if (!this.PIPsNarrow.Contains(componentsInChildren[i].gameObject))
					{
						this.PIPsNarrow.Add(componentsInChildren[i].gameObject);
					}
				}
			}
			gameObject2.transform.GetComponentInChildren<Camera>().targetTexture = renderTexture;
			RawImage[] componentsInChildren2 = gameObject2.transform.GetComponentsInChildren<RawImage>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (componentsInChildren2[j].name == "Office-BG")
				{
					this.backgroundImages.Add(componentsInChildren2[j]);
				}
				else if ((float)num2 == num - 1f && componentsInChildren2[j].name == "PIP-WIDE")
				{
					this.backgroundImages.Add(componentsInChildren2[j]);
					if (!this.PIPsWide.Contains(componentsInChildren2[j].gameObject))
					{
						this.PIPsWide.Add(componentsInChildren2[j].gameObject);
					}
				}
				else if ((float)num2 == num - 1f && componentsInChildren2[j].name == "PIP-NARROW")
				{
					this.backgroundImages.Add(componentsInChildren2[j]);
					if (!this.PIPsNarrow.Contains(componentsInChildren2[j].gameObject))
					{
						this.PIPsNarrow.Add(componentsInChildren2[j].gameObject);
					}
				}
			}
			MenuButton[] componentsInChildren3 = gameObject2.transform.GetComponentsInChildren<MenuButton>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].original = false;
			}
			gameObject = gameObject2;
			num2++;
		}
	}

	// Token: 0x04000719 RID: 1817
	public GameObject level_0;

	// Token: 0x0400071A RID: 1818
	[Space(10f)]
	public Texture2D widescreenTex;

	// Token: 0x0400071B RID: 1819
	public Texture2D squarescreenTex;

	// Token: 0x0400071C RID: 1820
	private List<RawImage> backgroundImages = new List<RawImage>();

	// Token: 0x0400071D RID: 1821
	private List<GameObject> PIPsWide = new List<GameObject>();

	// Token: 0x0400071E RID: 1822
	private List<GameObject> PIPsNarrow = new List<GameObject>();

	// Token: 0x0400071F RID: 1823
	[Space(10f)]
	public RectTransform BGRT;

	// Token: 0x04000720 RID: 1824
	public RectTransform canvasRT;

	// Token: 0x04000721 RID: 1825
	public RectTransform PIPRT;

	// Token: 0x04000722 RID: 1826
	private Vector3 PIPBaseScale;

	// Token: 0x04000723 RID: 1827
	private Vector3 PIPBasePos;

	// Token: 0x04000724 RID: 1828
	private Vector3 canvasBaseScale;

	// Token: 0x04000725 RID: 1829
	private Rect canvasBaseRect;

	// Token: 0x04000726 RID: 1830
	public Texture finalTex;

	// Token: 0x04000727 RID: 1831
	private float _aspectRatio;

	// Token: 0x020003CD RID: 973
	private enum Aspect
	{
		// Token: 0x0400140B RID: 5131
		Wide,
		// Token: 0x0400140C RID: 5132
		Narrow
	}
}
