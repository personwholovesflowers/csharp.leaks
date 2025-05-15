using System;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class PropDynamic : HammerEntity
{
	// Token: 0x06000886 RID: 2182 RVA: 0x0002857C File Offset: 0x0002677C
	private void Awake()
	{
		this._renderer = base.GetComponent<MeshRenderer>();
		this._collider = base.GetComponent<MeshCollider>();
		this._anim = base.GetComponent<Animator>();
		this.gotAnim = this._anim != null;
		if (!this._renderer)
		{
			this._skinRenderer = base.GetComponentInChildren<SkinnedMeshRenderer>();
			this._collider = base.GetComponentInChildren<MeshCollider>();
		}
		this.renderColor != Color.white;
		this.skins = base.GetComponents<Skin>();
		if (!this.isEnabled)
		{
			if (this._renderer)
			{
				this._renderer.enabled = false;
			}
			if (this._skinRenderer)
			{
				this._skinRenderer.enabled = false;
			}
			if (this._collider != null)
			{
				this._collider.enabled = false;
			}
		}
		if (this.startAnim != "")
		{
			this.Input_SetAnimation(this.startAnim);
		}
		if (this._anim != null)
		{
			this.originalAnimSpeed = this._anim.speed;
		}
		GameMaster.OnPause += this.Pause;
		GameMaster.OnResume += this.Resume;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000286B4 File Offset: 0x000268B4
	private void OnDestroy()
	{
		GameMaster.OnPause -= this.Pause;
		GameMaster.OnResume -= this.Resume;
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x000286D8 File Offset: 0x000268D8
	public override void Input_Enable()
	{
		base.Input_Enable();
		if (this._renderer)
		{
			this._renderer.enabled = true;
		}
		if (this._skinRenderer)
		{
			this._skinRenderer.enabled = true;
		}
		if (this._collider != null)
		{
			this._collider.enabled = true;
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00028738 File Offset: 0x00026938
	public override void Input_Disable()
	{
		base.Input_Disable();
		if (this._renderer)
		{
			this._renderer.enabled = false;
		}
		if (this._skinRenderer)
		{
			this._skinRenderer.enabled = false;
		}
		if (this._collider != null)
		{
			this._collider.enabled = false;
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00028797 File Offset: 0x00026997
	public void Input_SetAnimation(string anim)
	{
		if (this._anim)
		{
			this._anim.Play(anim);
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x000287B4 File Offset: 0x000269B4
	public void Input_Skin(float index)
	{
		int num = Mathf.FloorToInt(index);
		int i = 0;
		while (i < this.skins.Length)
		{
			if (this.skins[i].index == num)
			{
				if (this._renderer)
				{
					this._renderer.materials = this.skins[i].materials;
					return;
				}
				if (this._skinRenderer)
				{
					this._skinRenderer.materials = this.skins[i].materials;
				}
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00028838 File Offset: 0x00026A38
	public void Input_Color(string color)
	{
		string[] array = color.Split(new char[] { ' ' });
		if (array.Length != 3)
		{
			return;
		}
		Color color2 = new Color(float.Parse(array[0]) / 255f, float.Parse(array[1]) / 255f, float.Parse(array[2]) / 255f);
		this.renderColor = color2;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x00028896 File Offset: 0x00026A96
	private void Pause()
	{
		if (this.gotAnim)
		{
			this._anim.speed = 0f;
		}
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x000288B0 File Offset: 0x00026AB0
	private void Resume()
	{
		if (this.gotAnim)
		{
			this._anim.speed = this.originalAnimSpeed;
		}
	}

	// Token: 0x04000854 RID: 2132
	private MeshRenderer _renderer;

	// Token: 0x04000855 RID: 2133
	private SkinnedMeshRenderer _skinRenderer;

	// Token: 0x04000856 RID: 2134
	private MeshCollider _collider;

	// Token: 0x04000857 RID: 2135
	private Animator _anim;

	// Token: 0x04000858 RID: 2136
	private float originalAnimSpeed = 1f;

	// Token: 0x04000859 RID: 2137
	public Color renderColor = Color.white;

	// Token: 0x0400085A RID: 2138
	public Skin[] skins;

	// Token: 0x0400085B RID: 2139
	private int currentSkin;

	// Token: 0x0400085C RID: 2140
	public string startAnim = "";

	// Token: 0x0400085D RID: 2141
	private bool gotAnim;
}
