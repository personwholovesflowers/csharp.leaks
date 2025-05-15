using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200074F RID: 1871
	public class StoreManager
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x00111E9C File Offset: 0x0011009C
		private StoreManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(StoreManager.FFIMethods));
				}
				return (StoreManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060028A4 RID: 10404 RVA: 0x00111ECC File Offset: 0x001100CC
		// (remove) Token: 0x060028A5 RID: 10405 RVA: 0x00111F04 File Offset: 0x00110104
		public event StoreManager.EntitlementCreateHandler OnEntitlementCreate;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060028A6 RID: 10406 RVA: 0x00111F3C File Offset: 0x0011013C
		// (remove) Token: 0x060028A7 RID: 10407 RVA: 0x00111F74 File Offset: 0x00110174
		public event StoreManager.EntitlementDeleteHandler OnEntitlementDelete;

		// Token: 0x060028A8 RID: 10408 RVA: 0x00111FAC File Offset: 0x001101AC
		internal StoreManager(IntPtr ptr, IntPtr eventsPtr, ref StoreManager.FFIEvents events)
		{
			if (eventsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
			this.InitEvents(eventsPtr, ref events);
			this.MethodsPtr = ptr;
			if (this.MethodsPtr == IntPtr.Zero)
			{
				throw new ResultException(Result.InternalError);
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x00111FFB File Offset: 0x001101FB
		private void InitEvents(IntPtr eventsPtr, ref StoreManager.FFIEvents events)
		{
			events.OnEntitlementCreate = new StoreManager.FFIEvents.EntitlementCreateHandler(StoreManager.OnEntitlementCreateImpl);
			events.OnEntitlementDelete = new StoreManager.FFIEvents.EntitlementDeleteHandler(StoreManager.OnEntitlementDeleteImpl);
			Marshal.StructureToPtr<StoreManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x00112030 File Offset: 0x00110230
		[MonoPInvokeCallback]
		private static void FetchSkusCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.FetchSkusHandler fetchSkusHandler = (StoreManager.FetchSkusHandler)gchandle.Target;
			gchandle.Free();
			fetchSkusHandler(result);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x00112060 File Offset: 0x00110260
		public void FetchSkus(StoreManager.FetchSkusHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.FetchSkus(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new StoreManager.FFIMethods.FetchSkusCallback(StoreManager.FetchSkusCallbackImpl));
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x0011209C File Offset: 0x0011029C
		public int CountSkus()
		{
			int num = 0;
			this.Methods.CountSkus(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x001120C4 File Offset: 0x001102C4
		public Sku GetSku(long skuId)
		{
			Sku sku = default(Sku);
			Result result = this.Methods.GetSku(this.MethodsPtr, skuId, ref sku);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return sku;
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x00112100 File Offset: 0x00110300
		public Sku GetSkuAt(int index)
		{
			Sku sku = default(Sku);
			Result result = this.Methods.GetSkuAt(this.MethodsPtr, index, ref sku);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return sku;
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x0011213C File Offset: 0x0011033C
		[MonoPInvokeCallback]
		private static void FetchEntitlementsCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.FetchEntitlementsHandler fetchEntitlementsHandler = (StoreManager.FetchEntitlementsHandler)gchandle.Target;
			gchandle.Free();
			fetchEntitlementsHandler(result);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x0011216C File Offset: 0x0011036C
		public void FetchEntitlements(StoreManager.FetchEntitlementsHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.FetchEntitlements(this.MethodsPtr, GCHandle.ToIntPtr(gchandle), new StoreManager.FFIMethods.FetchEntitlementsCallback(StoreManager.FetchEntitlementsCallbackImpl));
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x001121A8 File Offset: 0x001103A8
		public int CountEntitlements()
		{
			int num = 0;
			this.Methods.CountEntitlements(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x001121D0 File Offset: 0x001103D0
		public Entitlement GetEntitlement(long entitlementId)
		{
			Entitlement entitlement = default(Entitlement);
			Result result = this.Methods.GetEntitlement(this.MethodsPtr, entitlementId, ref entitlement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return entitlement;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0011220C File Offset: 0x0011040C
		public Entitlement GetEntitlementAt(int index)
		{
			Entitlement entitlement = default(Entitlement);
			Result result = this.Methods.GetEntitlementAt(this.MethodsPtr, index, ref entitlement);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return entitlement;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x00112248 File Offset: 0x00110448
		public bool HasSkuEntitlement(long skuId)
		{
			bool flag = false;
			Result result = this.Methods.HasSkuEntitlement(this.MethodsPtr, skuId, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x0011227C File Offset: 0x0011047C
		[MonoPInvokeCallback]
		private static void StartPurchaseCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StoreManager.StartPurchaseHandler startPurchaseHandler = (StoreManager.StartPurchaseHandler)gchandle.Target;
			gchandle.Free();
			startPurchaseHandler(result);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x001122AC File Offset: 0x001104AC
		public void StartPurchase(long skuId, StoreManager.StartPurchaseHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.StartPurchase(this.MethodsPtr, skuId, GCHandle.ToIntPtr(gchandle), new StoreManager.FFIMethods.StartPurchaseCallback(StoreManager.StartPurchaseCallbackImpl));
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x001122EC File Offset: 0x001104EC
		[MonoPInvokeCallback]
		private static void OnEntitlementCreateImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.StoreManagerInstance.OnEntitlementCreate != null)
			{
				discord.StoreManagerInstance.OnEntitlementCreate(ref entitlement);
			}
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x0011232C File Offset: 0x0011052C
		[MonoPInvokeCallback]
		private static void OnEntitlementDeleteImpl(IntPtr ptr, ref Entitlement entitlement)
		{
			Discord discord = (Discord)GCHandle.FromIntPtr(ptr).Target;
			if (discord.StoreManagerInstance.OnEntitlementDelete != null)
			{
				discord.StoreManagerInstance.OnEntitlementDelete(ref entitlement);
			}
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x0011236C File Offset: 0x0011056C
		public IEnumerable<Entitlement> GetEntitlements()
		{
			int num = this.CountEntitlements();
			List<Entitlement> list = new List<Entitlement>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetEntitlementAt(i));
			}
			return list;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x001123A0 File Offset: 0x001105A0
		public IEnumerable<Sku> GetSkus()
		{
			int num = this.CountSkus();
			List<Sku> list = new List<Sku>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.GetSkuAt(i));
			}
			return list;
		}

		// Token: 0x04003061 RID: 12385
		private IntPtr MethodsPtr;

		// Token: 0x04003062 RID: 12386
		private object MethodsStructure;

		// Token: 0x02000750 RID: 1872
		internal struct FFIEvents
		{
			// Token: 0x04003065 RID: 12389
			internal StoreManager.FFIEvents.EntitlementCreateHandler OnEntitlementCreate;

			// Token: 0x04003066 RID: 12390
			internal StoreManager.FFIEvents.EntitlementDeleteHandler OnEntitlementDelete;

			// Token: 0x02000751 RID: 1873
			// (Invoke) Token: 0x060028BC RID: 10428
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementCreateHandler(IntPtr ptr, ref Entitlement entitlement);

			// Token: 0x02000752 RID: 1874
			// (Invoke) Token: 0x060028C0 RID: 10432
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void EntitlementDeleteHandler(IntPtr ptr, ref Entitlement entitlement);
		}

		// Token: 0x02000753 RID: 1875
		internal struct FFIMethods
		{
			// Token: 0x04003067 RID: 12391
			internal StoreManager.FFIMethods.FetchSkusMethod FetchSkus;

			// Token: 0x04003068 RID: 12392
			internal StoreManager.FFIMethods.CountSkusMethod CountSkus;

			// Token: 0x04003069 RID: 12393
			internal StoreManager.FFIMethods.GetSkuMethod GetSku;

			// Token: 0x0400306A RID: 12394
			internal StoreManager.FFIMethods.GetSkuAtMethod GetSkuAt;

			// Token: 0x0400306B RID: 12395
			internal StoreManager.FFIMethods.FetchEntitlementsMethod FetchEntitlements;

			// Token: 0x0400306C RID: 12396
			internal StoreManager.FFIMethods.CountEntitlementsMethod CountEntitlements;

			// Token: 0x0400306D RID: 12397
			internal StoreManager.FFIMethods.GetEntitlementMethod GetEntitlement;

			// Token: 0x0400306E RID: 12398
			internal StoreManager.FFIMethods.GetEntitlementAtMethod GetEntitlementAt;

			// Token: 0x0400306F RID: 12399
			internal StoreManager.FFIMethods.HasSkuEntitlementMethod HasSkuEntitlement;

			// Token: 0x04003070 RID: 12400
			internal StoreManager.FFIMethods.StartPurchaseMethod StartPurchase;

			// Token: 0x02000754 RID: 1876
			// (Invoke) Token: 0x060028C4 RID: 10436
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusCallback(IntPtr ptr, Result result);

			// Token: 0x02000755 RID: 1877
			// (Invoke) Token: 0x060028C8 RID: 10440
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchSkusMethod(IntPtr methodsPtr, IntPtr callbackData, StoreManager.FFIMethods.FetchSkusCallback callback);

			// Token: 0x02000756 RID: 1878
			// (Invoke) Token: 0x060028CC RID: 10444
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountSkusMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000757 RID: 1879
			// (Invoke) Token: 0x060028D0 RID: 10448
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuMethod(IntPtr methodsPtr, long skuId, ref Sku sku);

			// Token: 0x02000758 RID: 1880
			// (Invoke) Token: 0x060028D4 RID: 10452
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetSkuAtMethod(IntPtr methodsPtr, int index, ref Sku sku);

			// Token: 0x02000759 RID: 1881
			// (Invoke) Token: 0x060028D8 RID: 10456
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsCallback(IntPtr ptr, Result result);

			// Token: 0x0200075A RID: 1882
			// (Invoke) Token: 0x060028DC RID: 10460
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchEntitlementsMethod(IntPtr methodsPtr, IntPtr callbackData, StoreManager.FFIMethods.FetchEntitlementsCallback callback);

			// Token: 0x0200075B RID: 1883
			// (Invoke) Token: 0x060028E0 RID: 10464
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountEntitlementsMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x0200075C RID: 1884
			// (Invoke) Token: 0x060028E4 RID: 10468
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementMethod(IntPtr methodsPtr, long entitlementId, ref Entitlement entitlement);

			// Token: 0x0200075D RID: 1885
			// (Invoke) Token: 0x060028E8 RID: 10472
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetEntitlementAtMethod(IntPtr methodsPtr, int index, ref Entitlement entitlement);

			// Token: 0x0200075E RID: 1886
			// (Invoke) Token: 0x060028EC RID: 10476
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result HasSkuEntitlementMethod(IntPtr methodsPtr, long skuId, ref bool hasEntitlement);

			// Token: 0x0200075F RID: 1887
			// (Invoke) Token: 0x060028F0 RID: 10480
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseCallback(IntPtr ptr, Result result);

			// Token: 0x02000760 RID: 1888
			// (Invoke) Token: 0x060028F4 RID: 10484
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void StartPurchaseMethod(IntPtr methodsPtr, long skuId, IntPtr callbackData, StoreManager.FFIMethods.StartPurchaseCallback callback);
		}

		// Token: 0x02000761 RID: 1889
		// (Invoke) Token: 0x060028F8 RID: 10488
		public delegate void FetchSkusHandler(Result result);

		// Token: 0x02000762 RID: 1890
		// (Invoke) Token: 0x060028FC RID: 10492
		public delegate void FetchEntitlementsHandler(Result result);

		// Token: 0x02000763 RID: 1891
		// (Invoke) Token: 0x06002900 RID: 10496
		public delegate void StartPurchaseHandler(Result result);

		// Token: 0x02000764 RID: 1892
		// (Invoke) Token: 0x06002904 RID: 10500
		public delegate void EntitlementCreateHandler(ref Entitlement entitlement);

		// Token: 0x02000765 RID: 1893
		// (Invoke) Token: 0x06002908 RID: 10504
		public delegate void EntitlementDeleteHandler(ref Entitlement entitlement);
	}
}
