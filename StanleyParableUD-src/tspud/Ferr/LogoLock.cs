using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002F0 RID: 752
	public class LogoLock : MonoBehaviour
	{
		// Token: 0x06001394 RID: 5012 RVA: 0x000681DC File Offset: 0x000663DC
		private void Awake()
		{
			if (this.mCamera == null)
			{
				this.mCamera = Camera.main;
			}
			base.transform.parent = this.mCamera.transform;
			base.transform.localPosition = LogoLock.GetLockPosition(this.mCamera, this.mLockHorizontal, this.mLockVertical, this.mPadding);
			float pixelScale = LogoLock.GetPixelScale(this.mCamera, base.GetComponent<SpriteRenderer>().sprite);
			base.transform.localScale = new Vector3(pixelScale, pixelScale, pixelScale) * this.mScale;
			base.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00068288 File Offset: 0x00066488
		private static float GetPixelScale(Camera aCam, Sprite aSprite)
		{
			ref Vector2 viewSizeAtDistance = LogoLock.GetViewSizeAtDistance(1f, aCam);
			float num = aSprite.textureRect.width / (float)Screen.width;
			return viewSizeAtDistance.x * num / (aSprite.bounds.extents.x * 2f);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000682D8 File Offset: 0x000664D8
		private static Vector3 GetLockPosition(Camera aCam, LogoLock.LockPosition aHLock, LogoLock.LockPosition aVLock, float aPadding)
		{
			Vector3 zero = Vector3.zero;
			Vector2 viewSizeAtDistance = LogoLock.GetViewSizeAtDistance(1f, aCam);
			zero.z = 1f;
			aPadding *= 1f / (float)Screen.width * viewSizeAtDistance.x;
			if (aHLock == LogoLock.LockPosition.Left)
			{
				zero.x = -viewSizeAtDistance.x / 2f + aPadding;
			}
			else if (aHLock == LogoLock.LockPosition.Center)
			{
				zero.x = 0f;
			}
			else if (aHLock == LogoLock.LockPosition.Right)
			{
				zero.x = viewSizeAtDistance.x / 2f - aPadding;
			}
			if (aVLock == LogoLock.LockPosition.Left)
			{
				zero.y = viewSizeAtDistance.y / 2f - aPadding;
			}
			else if (aVLock == LogoLock.LockPosition.Center)
			{
				zero.y = 0f;
			}
			else if (aVLock == LogoLock.LockPosition.Right)
			{
				zero.y = -viewSizeAtDistance.y / 2f + aPadding;
			}
			return zero;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000683A8 File Offset: 0x000665A8
		private static Vector2 GetViewSizeAtDistance(float aDist, Camera aCamera)
		{
			if (aCamera == null)
			{
				aCamera = Camera.main;
			}
			if (aCamera.orthographic)
			{
				return new Vector2((float)Screen.width / (float)Screen.height * aCamera.orthographicSize * 2f, aCamera.orthographicSize * 2f);
			}
			float num = 2f * aDist * Mathf.Tan(aCamera.fieldOfView * 0.5f * 0.017453292f);
			return new Vector2(num * aCamera.aspect, num);
		}

		// Token: 0x04000F46 RID: 3910
		[SerializeField]
		private Camera mCamera;

		// Token: 0x04000F47 RID: 3911
		[SerializeField]
		private LogoLock.LockPosition mLockHorizontal;

		// Token: 0x04000F48 RID: 3912
		[SerializeField]
		private LogoLock.LockPosition mLockVertical;

		// Token: 0x04000F49 RID: 3913
		[SerializeField]
		private float mPadding;

		// Token: 0x04000F4A RID: 3914
		[SerializeField]
		private float mScale = 1f;

		// Token: 0x020004A4 RID: 1188
		private enum LockPosition
		{
			// Token: 0x0400178B RID: 6027
			Left,
			// Token: 0x0400178C RID: 6028
			Center,
			// Token: 0x0400178D RID: 6029
			Right
		}
	}
}
