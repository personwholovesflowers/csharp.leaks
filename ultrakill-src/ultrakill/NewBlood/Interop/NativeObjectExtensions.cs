using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace NewBlood.Interop
{
	// Token: 0x020005F3 RID: 1523
	public static class NativeObjectExtensions
	{
		// Token: 0x060021DD RID: 8669 RVA: 0x0010B248 File Offset: 0x00109448
		public unsafe static NativeObject* GetNativeObject(this Object @this)
		{
			return (NativeObject*)(void*)UnsafeUtility.As<Object, StrongBox<IntPtr>>(ref @this)->Value;
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x0010B25C File Offset: 0x0010945C
		public unsafe static NativeComponent* GetNativeObject(this Component @this)
		{
			return (NativeComponent*)@this.GetNativeObject();
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0010B25C File Offset: 0x0010945C
		public unsafe static NativeRenderer* GetNativeObject(this Renderer @this)
		{
			return (NativeRenderer*)@this.GetNativeObject();
		}
	}
}
