using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public interface IPlaceholdableComponent
{
	// Token: 0x06000138 RID: 312
	void WillReplace(GameObject oldObject, GameObject newObject, bool isSelfBeingReplaced);
}
