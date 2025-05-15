using System;
using UnityEngine;
using UnityEngine.UI;

namespace MeshBrush.Examples
{
	// Token: 0x0200025B RID: 603
	public class RuntimeExample : MonoBehaviour
	{
		// Token: 0x06000E4F RID: 3663 RVA: 0x00045706 File Offset: 0x00043906
		private void Start()
		{
			this.mainCamera = Camera.main.transform;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00045718 File Offset: 0x00043918
		private void Update()
		{
			this.meshbrushInstance.radius = this.radiusSlider.value;
			this.meshbrushInstance.scatteringRange = new Vector2(this.scatteringSlider.value, this.scatteringSlider.value);
			this.meshbrushInstance.densityRange = new Vector2(this.densitySlider.value, this.densitySlider.value);
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
			{
				this.circleBrush.position = raycastHit.point;
				this.circleBrush.forward = -raycastHit.normal;
				this.circleBrush.localScale = new Vector3(this.meshbrushInstance.radius, this.meshbrushInstance.radius, 1f);
				if (Input.GetKey(this.meshbrushInstance.paintKey))
				{
					this.meshbrushInstance.PaintMeshes(raycastHit);
				}
				if (Input.GetKey(this.meshbrushInstance.deleteKey))
				{
					this.meshbrushInstance.DeleteMeshes(raycastHit);
				}
				if (Input.GetKey(this.meshbrushInstance.randomizeKey))
				{
					this.meshbrushInstance.RandomizeMeshes(raycastHit);
				}
			}
		}

		// Token: 0x04000D11 RID: 3345
		[SerializeField]
		private MeshBrush meshbrushInstance;

		// Token: 0x04000D12 RID: 3346
		[SerializeField]
		private Transform circleBrush;

		// Token: 0x04000D13 RID: 3347
		[SerializeField]
		private Slider radiusSlider;

		// Token: 0x04000D14 RID: 3348
		[SerializeField]
		private Slider scatteringSlider;

		// Token: 0x04000D15 RID: 3349
		[SerializeField]
		private Slider densitySlider;

		// Token: 0x04000D16 RID: 3350
		private Transform mainCamera;
	}
}
