using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002DB RID: 731
	public interface IBlendPaintType
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06001304 RID: 4868
		// (set) Token: 0x06001305 RID: 4869
		Color Color { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06001306 RID: 4870
		// (set) Token: 0x06001307 RID: 4871
		float Size { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06001308 RID: 4872
		// (set) Token: 0x06001309 RID: 4873
		float Strength { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600130A RID: 4874
		// (set) Token: 0x0600130B RID: 4875
		float Falloff { get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600130C RID: 4876
		// (set) Token: 0x0600130D RID: 4877
		bool Backfaces { get; set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600130E RID: 4878
		Texture2D Cursor { get; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600130F RID: 4879
		Vector2 CursorHotspot { get; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06001310 RID: 4880
		bool ShowColorSettings { get; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06001311 RID: 4881
		bool ShowBrushSettings { get; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06001312 RID: 4882
		string Name { get; }

		// Token: 0x06001313 RID: 4883
		void PaintObjectsBegin(List<GameObject> aObjects, RaycastHit aHit, RaycastHit? aPreviousHit);

		// Token: 0x06001314 RID: 4884
		void PaintObjects(List<GameObject> aObjects, RaycastHit aHit, RaycastHit? aPreviousHit);

		// Token: 0x06001315 RID: 4885
		void PaintObjectsEnd(List<GameObject> aObjects, RaycastHit aHit, RaycastHit? aPreviousHit);

		// Token: 0x06001316 RID: 4886
		void PaintBegin(GameObject aObject, RaycastHit aHit, RaycastHit? aPreviousHit);

		// Token: 0x06001317 RID: 4887
		void Paint(GameObject aObject, RaycastHit aHit, RaycastHit? aPreviousHit);

		// Token: 0x06001318 RID: 4888
		void PaintEnd(GameObject aObject, RaycastHit aHit, RaycastHit? aPreviousHit);

		// Token: 0x06001319 RID: 4889
		float GetPointInfluence(Vector3 aObjScale, Vector3 aHitPt, Vector3 aHitDirection, Vector3 aVert, Vector3 aVertNormal);

		// Token: 0x0600131A RID: 4890
		void RenderScenePreview(Camera aSceneCamera, RaycastHit aHit, List<GameObject> aObjects);

		// Token: 0x0600131B RID: 4891
		void RenderScenePreview(Camera aSceneCamera, RaycastHit aHit, GameObject aObject);

		// Token: 0x0600131C RID: 4892
		int CheckPriority(GameObject aOfObject);

		// Token: 0x0600131D RID: 4893
		void OnSelect(List<GameObject> aObjects);

		// Token: 0x0600131E RID: 4894
		void OnUnselect(List<GameObject> aObjects);

		// Token: 0x0600131F RID: 4895
		void DrawToolGUI();

		// Token: 0x06001320 RID: 4896
		bool GUIInput();
	}
}
