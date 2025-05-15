using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A6 RID: 166
public class CreditsScroll : MonoBehaviour
{
	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00018D5E File Offset: 0x00016F5E
	private float MaxScrollPosition
	{
		get
		{
			return this.scrollContents.sizeDelta.y - base.GetComponent<RectTransform>().sizeDelta.y;
		}
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x00018D81 File Offset: 0x00016F81
	private void OnEnable()
	{
		this.position = 0f;
		this.holdPositionTimer = this.holdTime;
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00018D9A File Offset: 0x00016F9A
	private void OnDisable()
	{
		this.position = -10000f;
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x00018DA8 File Offset: 0x00016FA8
	private void Update()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		float num = this.scrollSpeed;
		if (Singleton<GameMaster>.Instance.stanleyActions.MenuConfirm.IsPressed || Singleton<GameMaster>.Instance.stanleyActions.MoveBackward.IsPressed)
		{
			num = this.fastScrollSpeed;
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.Up.IsPressed || Singleton<GameMaster>.Instance.stanleyActions.MoveForward.IsPressed)
		{
			num *= -1f;
		}
		if (this.holdPositionTimer > 0f)
		{
			this.holdPositionTimer -= Singleton<GameMaster>.Instance.GameDeltaTime;
		}
		else
		{
			this.position += num * Singleton<GameMaster>.Instance.GameDeltaTime;
		}
		this.position = Mathf.Clamp(this.position, 0f, this.MaxScrollPosition);
		float y = this.scrollContents.localPosition.y;
		this.scrollContents.localPosition = new Vector3(0f, this.position, 0f);
		if (y != 0f && y == this.scrollContents.localPosition.y)
		{
			UnityEvent unityEvent = this.onContentScrolledToBottom;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x040003EE RID: 1006
	public RectTransform scrollContents;

	// Token: 0x040003EF RID: 1007
	public float scrollSpeed = 40f;

	// Token: 0x040003F0 RID: 1008
	public float fastScrollSpeed = 120f;

	// Token: 0x040003F1 RID: 1009
	public float holdTime = 1f;

	// Token: 0x040003F2 RID: 1010
	private float position;

	// Token: 0x040003F3 RID: 1011
	private float holdPositionTimer;

	// Token: 0x040003F4 RID: 1012
	public UnityEvent onContentScrolledToBottom;
}
