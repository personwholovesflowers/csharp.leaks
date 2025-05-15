using System;
using System.Collections.Generic;
using UnityEngine;

namespace Randomness
{
	// Token: 0x02000579 RID: 1401
	public class RandomInstantiate : RandomBase<RandomGameObjectEntry>
	{
		// Token: 0x06001FD1 RID: 8145 RVA: 0x0010218C File Offset: 0x0010038C
		public override void PerformTheAction(RandomEntry entry)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(((RandomGameObjectEntry)entry).targetObject);
			if (this.useOwnPosition)
			{
				gameObject.transform.position = base.transform.position;
			}
			if (this.useOwnRotation)
			{
				gameObject.transform.rotation = base.transform.rotation;
			}
			if (this.reParent)
			{
				gameObject.transform.SetParent(base.transform);
				if (this.useOwnPosition)
				{
					gameObject.transform.localPosition = Vector3.zero;
				}
				if (this.useOwnRotation)
				{
					gameObject.transform.localRotation = Quaternion.identity;
				}
			}
			this.createdObjects.Add(gameObject);
			InstantiateObjectMode instantiateObjectMode = this.mode;
			if (instantiateObjectMode != InstantiateObjectMode.ForceEnable)
			{
				if (instantiateObjectMode == InstantiateObjectMode.ForceDisable)
				{
					gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				gameObject.SetActive(true);
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0010225C File Offset: 0x0010045C
		public override void RandomizeWithCount(int count)
		{
			if (this.removePreviousOnRandomize)
			{
				foreach (GameObject gameObject in this.createdObjects)
				{
					Object.Destroy(gameObject);
				}
				this.createdObjects.Clear();
			}
			base.RandomizeWithCount(count);
		}

		// Token: 0x04002C0E RID: 11278
		public bool removePreviousOnRandomize = true;

		// Token: 0x04002C0F RID: 11279
		[SerializeField]
		private InstantiateObjectMode mode;

		// Token: 0x04002C10 RID: 11280
		public bool reParent = true;

		// Token: 0x04002C11 RID: 11281
		public bool useOwnPosition = true;

		// Token: 0x04002C12 RID: 11282
		public bool useOwnRotation = true;

		// Token: 0x04002C13 RID: 11283
		private List<GameObject> createdObjects = new List<GameObject>();
	}
}
