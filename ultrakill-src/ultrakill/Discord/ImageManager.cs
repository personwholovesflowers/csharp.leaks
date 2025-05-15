using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Discord
{
	// Token: 0x020006BA RID: 1722
	public class ImageManager
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06002603 RID: 9731 RVA: 0x0010FD4A File Offset: 0x0010DF4A
		private ImageManager.FFIMethods Methods
		{
			get
			{
				if (this.MethodsStructure == null)
				{
					this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof(ImageManager.FFIMethods));
				}
				return (ImageManager.FFIMethods)this.MethodsStructure;
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x0010FD7C File Offset: 0x0010DF7C
		internal ImageManager(IntPtr ptr, IntPtr eventsPtr, ref ImageManager.FFIEvents events)
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

		// Token: 0x06002605 RID: 9733 RVA: 0x0010FDCB File Offset: 0x0010DFCB
		private void InitEvents(IntPtr eventsPtr, ref ImageManager.FFIEvents events)
		{
			Marshal.StructureToPtr<ImageManager.FFIEvents>(events, eventsPtr, false);
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x0010FDDC File Offset: 0x0010DFDC
		[MonoPInvokeCallback]
		private static void FetchCallbackImpl(IntPtr ptr, Result result, ImageHandle handleResult)
		{
			GCHandle gchandle = GCHandle.FromIntPtr(ptr);
			ImageManager.FetchHandler fetchHandler = (ImageManager.FetchHandler)gchandle.Target;
			gchandle.Free();
			fetchHandler(result, handleResult);
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x0010FE0C File Offset: 0x0010E00C
		public void Fetch(ImageHandle handle, bool refresh, ImageManager.FetchHandler callback)
		{
			GCHandle gchandle = GCHandle.Alloc(callback);
			this.Methods.Fetch(this.MethodsPtr, handle, refresh, GCHandle.ToIntPtr(gchandle), new ImageManager.FFIMethods.FetchCallback(ImageManager.FetchCallbackImpl));
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0010FE4C File Offset: 0x0010E04C
		public ImageDimensions GetDimensions(ImageHandle handle)
		{
			ImageDimensions imageDimensions = default(ImageDimensions);
			Result result = this.Methods.GetDimensions(this.MethodsPtr, handle, ref imageDimensions);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
			return imageDimensions;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0010FE88 File Offset: 0x0010E088
		public void GetData(ImageHandle handle, byte[] data)
		{
			Result result = this.Methods.GetData(this.MethodsPtr, handle, data, data.Length);
			if (result != Result.Ok)
			{
				throw new ResultException(result);
			}
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0010FEBB File Offset: 0x0010E0BB
		public void Fetch(ImageHandle handle, ImageManager.FetchHandler callback)
		{
			this.Fetch(handle, false, callback);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0010FEC8 File Offset: 0x0010E0C8
		public byte[] GetData(ImageHandle handle)
		{
			ImageDimensions dimensions = this.GetDimensions(handle);
			byte[] array = new byte[dimensions.Width * dimensions.Height * 4U];
			this.GetData(handle, array);
			return array;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0010FEFC File Offset: 0x0010E0FC
		public Texture2D GetTexture(ImageHandle handle)
		{
			ImageDimensions dimensions = this.GetDimensions(handle);
			Texture2D texture2D = new Texture2D((int)dimensions.Width, (int)dimensions.Height, TextureFormat.RGBA32, false, true);
			texture2D.LoadRawTextureData(this.GetData(handle));
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x04002FFA RID: 12282
		private IntPtr MethodsPtr;

		// Token: 0x04002FFB RID: 12283
		private object MethodsStructure;

		// Token: 0x020006BB RID: 1723
		internal struct FFIEvents
		{
		}

		// Token: 0x020006BC RID: 1724
		internal struct FFIMethods
		{
			// Token: 0x04002FFC RID: 12284
			internal ImageManager.FFIMethods.FetchMethod Fetch;

			// Token: 0x04002FFD RID: 12285
			internal ImageManager.FFIMethods.GetDimensionsMethod GetDimensions;

			// Token: 0x04002FFE RID: 12286
			internal ImageManager.FFIMethods.GetDataMethod GetData;

			// Token: 0x020006BD RID: 1725
			// (Invoke) Token: 0x0600260E RID: 9742
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchCallback(IntPtr ptr, Result result, ImageHandle handleResult);

			// Token: 0x020006BE RID: 1726
			// (Invoke) Token: 0x06002612 RID: 9746
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void FetchMethod(IntPtr methodsPtr, ImageHandle handle, bool refresh, IntPtr callbackData, ImageManager.FFIMethods.FetchCallback callback);

			// Token: 0x020006BF RID: 1727
			// (Invoke) Token: 0x06002616 RID: 9750
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDimensionsMethod(IntPtr methodsPtr, ImageHandle handle, ref ImageDimensions dimensions);

			// Token: 0x020006C0 RID: 1728
			// (Invoke) Token: 0x0600261A RID: 9754
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result GetDataMethod(IntPtr methodsPtr, ImageHandle handle, byte[] data, int dataLen);
		}

		// Token: 0x020006C1 RID: 1729
		// (Invoke) Token: 0x0600261E RID: 9758
		public delegate void FetchHandler(Result result, ImageHandle handleResult);
	}
}
