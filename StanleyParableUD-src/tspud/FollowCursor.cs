using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000041 RID: 65
[RequireComponent(typeof(RawImage))]
public class FollowCursor : MonoBehaviour
{
	// Token: 0x06000158 RID: 344 RVA: 0x00009D29 File Offset: 0x00007F29
	private void Awake()
	{
		this.cursorImage = base.GetComponent<RawImage>();
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00009D37 File Offset: 0x00007F37
	private void Start()
	{
		this.eventSystem = Object.FindObjectOfType<EventSystem>();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00009D44 File Offset: 0x00007F44
	private void Update()
	{
		this.CheckStatus();
		switch (this.cursorState)
		{
		case FollowCursor.FakeCursorState.Disabled:
			this.cursorImage.enabled = false;
			base.transform.position = Input.mousePosition;
			return;
		case FollowCursor.FakeCursorState.Following:
			this.cursorImage.enabled = true;
			if (this.eventSystem != null && this.eventSystem.currentSelectedGameObject != null)
			{
				this.pointOfInterest = this.eventSystem.currentSelectedGameObject.transform.position;
				base.transform.position = Vector3.Lerp(base.transform.position, this.pointOfInterest, Time.deltaTime * 20f);
			}
			break;
		case FollowCursor.FakeCursorState.Hidden:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00009E04 File Offset: 0x00008004
	private void CheckStatus()
	{
		if (Input.GetAxis("Mouse X") > 0f || Input.GetAxis("Mouse Y") > 0f)
		{
			this.cursorState = FollowCursor.FakeCursorState.Disabled;
			Cursor.visible = true;
			return;
		}
		if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Vertical") > 0f)
		{
			this.cursorState = FollowCursor.FakeCursorState.Following;
			Cursor.visible = false;
		}
	}

	// Token: 0x040001B9 RID: 441
	[SerializeField]
	private EventSystem eventSystem;

	// Token: 0x040001BA RID: 442
	private RawImage cursorImage;

	// Token: 0x040001BB RID: 443
	private Vector3 pointOfInterest;

	// Token: 0x040001BC RID: 444
	private FollowCursor.FakeCursorState cursorState = FollowCursor.FakeCursorState.Following;

	// Token: 0x02000360 RID: 864
	private enum FakeCursorState
	{
		// Token: 0x0400123A RID: 4666
		Disabled,
		// Token: 0x0400123B RID: 4667
		Following,
		// Token: 0x0400123C RID: 4668
		Hidden
	}
}
