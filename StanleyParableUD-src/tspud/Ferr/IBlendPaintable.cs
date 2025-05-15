using System;

namespace Ferr
{
	// Token: 0x020002DC RID: 732
	public interface IBlendPaintable
	{
		// Token: 0x06001321 RID: 4897
		void OnPainterSelected(IBlendPaintType aPainter);

		// Token: 0x06001322 RID: 4898
		void OnPainterUnselected(IBlendPaintType aPainter);
	}
}
