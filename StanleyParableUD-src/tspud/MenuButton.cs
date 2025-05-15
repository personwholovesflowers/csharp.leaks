using System;
using TMPro;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class MenuButton : MonoBehaviour
{
	// Token: 0x0600069C RID: 1692 RVA: 0x00023888 File Offset: 0x00021A88
	protected virtual void Update()
	{
		if (this.hoveringLastFrame && !this.hoveringThisFrame)
		{
			this.OnExit();
		}
		if (this.hoveringThisFrame)
		{
			this.hoveringThisFrame = false;
			this.hoveringLastFrame = true;
			return;
		}
		this.hoveringLastFrame = false;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void OnClick(Vector3 point = default(Vector3))
	{
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void OnHold(Vector3 point = default(Vector3))
	{
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000238C0 File Offset: 0x00021AC0
	public virtual void OnEnter()
	{
		for (int i = 0; i < this.text.Length; i++)
		{
			if (this.text[i] != null)
			{
				this.text[i].color = new Color(0.6f, 0.6f, 0.6f, 1f);
			}
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00023918 File Offset: 0x00021B18
	public virtual void OnExit()
	{
		for (int i = 0; i < this.text.Length; i++)
		{
			if (this.text[i] != null)
			{
				this.text[i].color = new Color(1f, 1f, 1f, 1f);
			}
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00023970 File Offset: 0x00021B70
	public virtual void OnHover()
	{
		if (!this.hoveringLastFrame)
		{
			this.OnEnter();
		}
		this.hoveringThisFrame = true;
		if (this.control != null)
		{
			if (Singleton<GameMaster>.Instance.stanleyActions.Right.IsPressed)
			{
				if (this.pressedTimer <= 0f)
				{
					this.refireCounter++;
					this.pressedTimer = this.refireTime / Mathf.Clamp((float)this.refireCounter, 1f, 10f);
					this.control.OnInput(DPadDir.Right);
				}
				this.pressedTimer -= Time.deltaTime;
				return;
			}
			if (Singleton<GameMaster>.Instance.stanleyActions.Left.IsPressed)
			{
				if (this.pressedTimer <= 0f)
				{
					this.refireCounter++;
					this.pressedTimer = this.refireTime / Mathf.Clamp((float)this.refireCounter, 1f, 5f);
					this.control.OnInput(DPadDir.Left);
				}
				this.pressedTimer -= Time.deltaTime;
				return;
			}
			this.pressedTimer = 0f;
			this.refireCounter = 0;
		}
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void OnInput(DPadDir direction)
	{
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00023A9B File Offset: 0x00021C9B
	protected virtual void OnDisable()
	{
		this.hoveringThisFrame = false;
		this.hoveringLastFrame = false;
		this.OnExit();
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void SaveChange()
	{
	}

	// Token: 0x040006F4 RID: 1780
	public bool original = true;

	// Token: 0x040006F5 RID: 1781
	private bool hoveringLastFrame;

	// Token: 0x040006F6 RID: 1782
	private bool hoveringThisFrame;

	// Token: 0x040006F7 RID: 1783
	public TextMeshProUGUI[] text;

	// Token: 0x040006F8 RID: 1784
	public MenuButton control;

	// Token: 0x040006F9 RID: 1785
	private float pressedTimer;

	// Token: 0x040006FA RID: 1786
	private float refireTime = 0.25f;

	// Token: 0x040006FB RID: 1787
	private int refireCounter;
}
