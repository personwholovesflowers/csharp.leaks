using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200020A RID: 522
public class ConsoleWindow : MonoBehaviour
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000B08 RID: 2824 RVA: 0x0004F9EB File Offset: 0x0004DBEB
	// (set) Token: 0x06000B09 RID: 2825 RVA: 0x0004F9F8 File Offset: 0x0004DBF8
	private Vector2 position
	{
		get
		{
			return this.selfFrame.anchoredPosition;
		}
		set
		{
			this.selfFrame.anchoredPosition = value;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0004FA06 File Offset: 0x0004DC06
	// (set) Token: 0x06000B0B RID: 2827 RVA: 0x0004FA13 File Offset: 0x0004DC13
	private Vector2 size
	{
		get
		{
			return this.selfFrame.sizeDelta;
		}
		set
		{
			this.selfFrame.sizeDelta = value;
		}
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0004FA21 File Offset: 0x0004DC21
	private void Awake()
	{
		this.selfFrame = base.GetComponent<RectTransform>();
		this.defaultSize = this.size;
		this.lastResolution = new Vector2Int(Screen.width, Screen.height);
		this.ResetWindow();
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0004FA58 File Offset: 0x0004DC58
	public void ResetWindow()
	{
		this.size = new Vector2(Mathf.Min(this.defaultSize.x, (float)Screen.width), Mathf.Min(this.defaultSize.y, (float)Screen.height));
		this.position = new Vector2((float)Screen.width / 2f - this.size.x / 2f, (float)(-(float)Screen.height) / 2f + this.size.y / 2f);
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0004FAE4 File Offset: 0x0004DCE4
	private void Update()
	{
		if (this.isResizing)
		{
			Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector2 vector2 = vector - this.resizeCursorStart;
			vector2.y = -vector2.y;
			this.size = new Vector2(Mathf.Max(this.minSize.x, this.size.x + vector2.x), Mathf.Max(this.minSize.y, this.size.y + vector2.y));
			if (this.position.x + this.size.x > (float)Screen.width)
			{
				this.size = new Vector2((float)Screen.width - this.position.x, this.size.y);
			}
			if (this.size.y - this.position.y > (float)Screen.height)
			{
				this.size = new Vector2(this.size.x, (float)Screen.height + this.position.y);
			}
			this.resizeCursorStart = vector;
		}
		if (this.isDragging)
		{
			Vector2 vector3 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector2 vector4 = vector3 - this.dragOffset;
			if (this.position.x + vector4.x < 0f)
			{
				vector4.x = 0f;
				this.position = new Vector2(0f, this.position.y);
			}
			else if (this.position.x + vector4.x > (float)Screen.width - this.size.x)
			{
				vector4.x = 0f;
				this.position = new Vector2((float)Screen.width - this.size.x, this.position.y);
			}
			if (this.position.y + vector4.y > 0f)
			{
				vector4.y = 0f;
				this.position = new Vector2(this.position.x, 0f);
			}
			else if (this.position.y + vector4.y < (float)(-(float)Screen.height) + this.size.y)
			{
				vector4.y = 0f;
				this.position = new Vector2(this.position.x, (float)(-(float)Screen.height) + this.size.y);
			}
			this.position += vector4;
			this.dragOffset = vector3;
		}
		if (this.lastResolution.x != Screen.width || this.lastResolution.y != Screen.height)
		{
			Debug.Log("Screen resolution changed, resetting console position");
			this.lastResolution = new Vector2Int(Screen.width, Screen.height);
			this.ResetWindow();
		}
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0004FDE5 File Offset: 0x0004DFE5
	public void StartDrag(PointerEventData eventData)
	{
		this.isDragging = true;
		this.dragOffset = eventData.position;
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0004FDFA File Offset: 0x0004DFFA
	public void EndDrag(PointerEventData eventData)
	{
		this.isDragging = false;
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x0004FE03 File Offset: 0x0004E003
	public void StartResize(PointerEventData eventData, Vector2Int corner)
	{
		this.isResizing = true;
		this.resizeCursorStart = eventData.position;
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0004FE18 File Offset: 0x0004E018
	public void StopResize(PointerEventData eventData, Vector2Int corner)
	{
		this.isResizing = false;
	}

	// Token: 0x04000EB2 RID: 3762
	private Vector2 minSize = new Vector2(520f, 480f);

	// Token: 0x04000EB3 RID: 3763
	private Vector2 defaultSize;

	// Token: 0x04000EB4 RID: 3764
	private RectTransform selfFrame;

	// Token: 0x04000EB5 RID: 3765
	private bool isDragging;

	// Token: 0x04000EB6 RID: 3766
	private Vector2 dragOffset;

	// Token: 0x04000EB7 RID: 3767
	private bool isResizing;

	// Token: 0x04000EB8 RID: 3768
	private Vector2 resizeCursorStart;

	// Token: 0x04000EB9 RID: 3769
	private Vector2Int lastResolution;
}
