using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x02000215 RID: 533
public class GamepadSelectionOutline : MonoBehaviour
{
	// Token: 0x06000B3B RID: 2875 RVA: 0x00050524 File Offset: 0x0004E724
	private void Update()
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		Canvas canvas = (currentSelectedGameObject ? currentSelectedGameObject.GetComponentInParent<Canvas>() : null);
		Selectable selectable;
		if (currentSelectedGameObject == null || !currentSelectedGameObject.activeInHierarchy || !currentSelectedGameObject.TryGetComponent<Selectable>(out selectable) || MonoSingleton<InputManager>.Instance == null || !(MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad) || canvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			this.image.enabled = false;
			return;
		}
		ControllerDisallowedSelection controllerDisallowedSelection;
		if (selectable.TryGetComponent<ControllerDisallowedSelection>(out controllerDisallowedSelection))
		{
			controllerDisallowedSelection.ApplyFallbackSelection();
			return;
		}
		this.image.enabled = true;
		RectTransform component = currentSelectedGameObject.GetComponent<RectTransform>();
		RectTransform rectTransform;
		Bounds selectedBounds = this.GetSelectedBounds(component, out rectTransform);
		this.image.rectTransform.anchoredPosition = selectedBounds.center;
		this.image.rectTransform.sizeDelta = selectedBounds.size + this.outlineSize;
		ScrollRect componentInParent = component.GetComponentInParent<ScrollRect>();
		Scrollbar scrollbar;
		if (componentInParent != null && !currentSelectedGameObject.TryGetComponent<Scrollbar>(out scrollbar))
		{
			this.EnsureVisibility(componentInParent, component, componentInParent != this.lastScrollRect);
		}
		this.lastScrollRect = componentInParent;
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x00050654 File Offset: 0x0004E854
	private Bounds GetSelectedBounds(RectTransform selected, out RectTransform rect)
	{
		Selectable selectable;
		if (selected.TryGetComponent<Selectable>(out selectable) && selectable.targetGraphic)
		{
			rect = selectable.targetGraphic.rectTransform;
			return this.GetRelativeBounds(this.image.transform.parent, rect);
		}
		rect = selected;
		return this.GetRelativeBounds(this.image.transform.parent, selected);
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x000506B8 File Offset: 0x0004E8B8
	private Bounds GetRelativeBounds(Transform root, RectTransform child)
	{
		child.GetWorldCorners(GamepadSelectionOutline.s_Corners);
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector3 = root.InverseTransformPoint(GamepadSelectionOutline.s_Corners[i]);
			vector = Vector3.Min(vector3, vector);
			vector2 = Vector3.Max(vector3, vector2);
		}
		Bounds bounds = new Bounds(vector, Vector3.zero);
		bounds.Encapsulate(vector2);
		return bounds;
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x00050740 File Offset: 0x0004E940
	private void EnsureVisibility(ScrollRect scrollRect, RectTransform child, bool instantScroll = false)
	{
		Bounds relativeBounds = this.GetRelativeBounds(scrollRect.content, child);
		GamepadSelectionBoundsExtension gamepadSelectionBoundsExtension;
		if (child.TryGetComponent<GamepadSelectionBoundsExtension>(out gamepadSelectionBoundsExtension) && gamepadSelectionBoundsExtension.Transforms != null)
		{
			foreach (RectTransform rectTransform in gamepadSelectionBoundsExtension.Transforms)
			{
				relativeBounds.Encapsulate(this.GetRelativeBounds(scrollRect.content, rectTransform));
			}
		}
		relativeBounds.min -= scrollRect.content.rect.min;
		relativeBounds.max -= scrollRect.content.rect.min;
		float num = scrollRect.content.rect.height - scrollRect.content.rect.height * scrollRect.verticalNormalizedPosition;
		float num2 = scrollRect.content.rect.height - relativeBounds.min.y;
		RectTransform rectTransform2;
		float num3;
		if (scrollRect.TryGetComponent<RectTransform>(out rectTransform2) && num2 < rectTransform2.rect.height * 0.75f)
		{
			num3 = 1f;
		}
		else if (relativeBounds.min.y < num)
		{
			num3 = relativeBounds.min.y / scrollRect.content.rect.height;
		}
		else
		{
			num3 = relativeBounds.max.y / scrollRect.content.rect.height;
		}
		if (instantScroll)
		{
			scrollRect.verticalNormalizedPosition = num3;
			return;
		}
		float num4 = this.scrollSpeedPixelsPerSecond / scrollRect.content.rect.height * Time.unscaledDeltaTime;
		if (scrollRect.verticalNormalizedPosition < num3)
		{
			scrollRect.verticalNormalizedPosition = Mathf.Min(scrollRect.verticalNormalizedPosition + num4, num3);
			return;
		}
		if (scrollRect.verticalNormalizedPosition > num3)
		{
			scrollRect.verticalNormalizedPosition = Mathf.Max(scrollRect.verticalNormalizedPosition - num4, num3);
		}
	}

	// Token: 0x04000EDB RID: 3803
	private static readonly Vector3[] s_Corners = new Vector3[4];

	// Token: 0x04000EDC RID: 3804
	[SerializeField]
	private Image image;

	// Token: 0x04000EDD RID: 3805
	[SerializeField]
	private float scrollSpeedPixelsPerSecond = 800f;

	// Token: 0x04000EDE RID: 3806
	[SerializeField]
	private Vector2 outlineSize = new Vector2(4f, 4f);

	// Token: 0x04000EDF RID: 3807
	private ScrollRect lastScrollRect;
}
