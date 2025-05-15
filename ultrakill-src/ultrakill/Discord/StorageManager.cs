using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	// Token: 0x0200073B RID: 1851
	public class StorageManager
	{
		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x00111AB3 File Offset: 0x0010FCB3
		private StorageManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(StorageManager.FFIMethods));
				}
				return (StorageManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x00111AE4 File Offset: 0x0010FCE4
		internal StorageManager(IntPtr ptr, IntPtr eventsPtr, ref StorageManager.FFIEvents events)
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

		// Token: 0x0600284F RID: 10319 RVA: 0x00111B33 File Offset: 0x0010FD33
		private void InitEvents(IntPtr eventsPtr, ref StorageManager.FFIEvents events)
		{
			Marshal.StructureToPtr<StorageManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x00111B44 File Offset: 0x0010FD44
		public uint Read(string name, byte[] data)
		{
			uint num = 0U;
			Result result = this.Methods.Read(this.MethodsPtr, name, data, data.Length, ref num);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return num;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x00111B7C File Offset: 0x0010FD7C
		[MonoPInvokeCallback]
		private static void ReadAsyncCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.ReadAsyncHandler readAsyncHandler = (StorageManager.ReadAsyncHandler)gchandle.Target;
			gchandle.Free();
			byte[] array = new byte[dataLen];
			Marshal.Copy(dataPtr, array, 0, dataLen);
			readAsyncHandler(result, array);
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x00111BBC File Offset: 0x0010FDBC
		public void ReadAsync(string name, StorageManager.ReadAsyncHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ReadAsync(this.MethodsPtr, name, GCHandle.ToIntPtr(gchandle), new StorageManager.FFIMethods.ReadAsyncCallback(StorageManager.ReadAsyncCallbackImpl));
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x00111BFC File Offset: 0x0010FDFC
		[MonoPInvokeCallback]
		private static void ReadAsyncPartialCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.ReadAsyncPartialHandler readAsyncPartialHandler = (StorageManager.ReadAsyncPartialHandler)gchandle.Target;
			gchandle.Free();
			byte[] array = new byte[dataLen];
			Marshal.Copy(dataPtr, array, 0, dataLen);
			readAsyncPartialHandler(result, array);
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x00111C3C File Offset: 0x0010FE3C
		public void ReadAsyncPartial(string name, ulong offset, ulong length, StorageManager.ReadAsyncPartialHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.ReadAsyncPartial(this.MethodsPtr, name, offset, length, GCHandle.ToIntPtr(gchandle), new StorageManager.FFIMethods.ReadAsyncPartialCallback(StorageManager.ReadAsyncPartialCallbackImpl));
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x00111C7C File Offset: 0x0010FE7C
		public void Write(string name, byte[] data)
		{
			Result result = this.Methods.Write(this.MethodsPtr, name, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x00111CB0 File Offset: 0x0010FEB0
		[MonoPInvokeCallback]
		private static void WriteAsyncCallbackImpl(IntPtr ptr, Result result)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			StorageManager.WriteAsyncHandler writeAsyncHandler = (StorageManager.WriteAsyncHandler)gchandle.Target;
			gchandle.Free();
			writeAsyncHandler(result);
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x00111CE0 File Offset: 0x0010FEE0
		public void WriteAsync(string name, byte[] data, StorageManager.WriteAsyncHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.WriteAsync(this.MethodsPtr, name, data, data.Length, GCHandle.ToIntPtr(gchandle), new StorageManager.FFIMethods.WriteAsyncCallback(StorageManager.WriteAsyncCallbackImpl));
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x00111D24 File Offset: 0x0010FF24
		public void Delete(string name)
		{
			Result result = this.Methods.Delete(this.MethodsPtr, name);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x00111D54 File Offset: 0x0010FF54
		public bool Exists(string name)
		{
			bool flag = false;
			Result result = this.Methods.Exists(this.MethodsPtr, name, ref flag);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return flag;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00111D88 File Offset: 0x0010FF88
		public int Count()
		{
			int num = 0;
			this.Methods.Count(this.MethodsPtr, ref num);
			return num;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00111DB0 File Offset: 0x0010FFB0
		public FileStat Stat(string name)
		{
			FileStat fileStat = default(FileStat);
			Result result = this.Methods.Stat(this.MethodsPtr, name, ref fileStat);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return fileStat;
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x00111DEC File Offset: 0x0010FFEC
		public FileStat StatAt(int index)
		{
			FileStat fileStat = default(FileStat);
			Result result = this.Methods.StatAt(this.MethodsPtr, index, ref fileStat);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return fileStat;
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x00111E28 File Offset: 0x00110028
		public string GetPath()
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			Result result = this.Methods.GetPath(this.MethodsPtr, stringBuilder);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x00111E68 File Offset: 0x00110068
		public IEnumerable<FileStat> Files()
		{
			int num = this.Count();
			List<FileStat> list = new List<FileStat>();
			for (int i = 0; i < num; i++)
			{
				list.Add(this.StatAt(i));
			}
			return list;
		}

		// Token: 0x04003054 RID: 12372
		private IntPtr MethodsPtr;

		// Token: 0x04003055 RID: 12373
		private object MethodsStructure;

		// Token: 0x0200073C RID: 1852
		internal struct FFIEvents
		{
		}

		// Token: 0x0200073D RID: 1853
		internal struct FFIMethods
		{
			// Token: 0x04003056 RID: 12374
			internal StorageManager.FFIMethods.ReadMethod Read;

			// Token: 0x04003057 RID: 12375
			internal StorageManager.FFIMethods.ReadAsyncMethod ReadAsync;

			// Token: 0x04003058 RID: 12376
			internal StorageManager.FFIMethods.ReadAsyncPartialMethod ReadAsyncPartial;

			// Token: 0x04003059 RID: 12377
			internal StorageManager.FFIMethods.WriteMethod Write;

			// Token: 0x0400305A RID: 12378
			internal StorageManager.FFIMethods.WriteAsyncMethod WriteAsync;

			// Token: 0x0400305B RID: 12379
			internal StorageManager.FFIMethods.DeleteMethod Delete;

			// Token: 0x0400305C RID: 12380
			internal StorageManager.FFIMethods.ExistsMethod Exists;

			// Token: 0x0400305D RID: 12381
			internal StorageManager.FFIMethods.CountMethod Count;

			// Token: 0x0400305E RID: 12382
			internal StorageManager.FFIMethods.StatMethod Stat;

			// Token: 0x0400305F RID: 12383
			internal StorageManager.FFIMethods.StatAtMethod StatAt;

			// Token: 0x04003060 RID: 12384
			internal StorageManager.FFIMethods.GetPathMethod GetPath;

			// Token: 0x0200073E RID: 1854
			// (Invoke) Token: 0x06002860 RID: 10336
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ReadMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen, ref uint read);

			// Token: 0x0200073F RID: 1855
			// (Invoke) Token: 0x06002864 RID: 10340
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			// Token: 0x02000740 RID: 1856
			// (Invoke) Token: 0x06002868 RID: 10344
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, IntPtr callbackData, StorageManager.FFIMethods.ReadAsyncCallback callback);

			// Token: 0x02000741 RID: 1857
			// (Invoke) Token: 0x0600286C RID: 10348
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			// Token: 0x02000742 RID: 1858
			// (Invoke) Token: 0x06002870 RID: 10352
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void ReadAsyncPartialMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ulong offset, ulong length, IntPtr callbackData, StorageManager.FFIMethods.ReadAsyncPartialCallback callback);

			// Token: 0x02000743 RID: 1859
			// (Invoke) Token: 0x06002874 RID: 10356
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result WriteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen);

			// Token: 0x02000744 RID: 1860
			// (Invoke) Token: 0x06002878 RID: 10360
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncCallback(IntPtr ptr, Result result);

			// Token: 0x02000745 RID: 1861
			// (Invoke) Token: 0x0600287C RID: 10364
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void WriteAsyncMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, byte[] data, int dataLen, IntPtr callbackData, StorageManager.FFIMethods.WriteAsyncCallback callback);

			// Token: 0x02000746 RID: 1862
			// (Invoke) Token: 0x06002880 RID: 10368
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result DeleteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name);

			// Token: 0x02000747 RID: 1863
			// (Invoke) Token: 0x06002884 RID: 10372
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result ExistsMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref bool exists);

			// Token: 0x02000748 RID: 1864
			// (Invoke) Token: 0x06002888 RID: 10376
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void CountMethod(IntPtr methodsPtr, ref int count);

			// Token: 0x02000749 RID: 1865
			// (Invoke) Token: 0x0600288C RID: 10380
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref FileStat stat);

			// Token: 0x0200074A RID: 1866
			// (Invoke) Token: 0x06002890 RID: 10384
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result StatAtMethod(IntPtr methodsPtr, int index, ref FileStat stat);

			// Token: 0x0200074B RID: 1867
			// (Invoke) Token: 0x06002894 RID: 10388
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetPathMethod(IntPtr methodsPtr, StringBuilder path);
		}

		// Token: 0x0200074C RID: 1868
		// (Invoke) Token: 0x06002898 RID: 10392
		public delegate void ReadAsyncHandler(Result result, byte[] data);

		// Token: 0x0200074D RID: 1869
		// (Invoke) Token: 0x0600289C RID: 10396
		public delegate void ReadAsyncPartialHandler(Result result, byte[] data);

		// Token: 0x0200074E RID: 1870
		// (Invoke) Token: 0x060028A0 RID: 10400
		public delegate void WriteAsyncHandler(Result result);
	}
}
