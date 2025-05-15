using System;
using System.Collections;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002F7 RID: 759
	public class WebMessager : MonoBehaviour
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x00068D0A File Offset: 0x00066F0A
		public static WebMessager Instance
		{
			get
			{
				if (WebMessager.mInstance == null)
				{
					WebMessager.mInstance = new GameObject("_WebMessager").AddComponent<WebMessager>();
				}
				return WebMessager.mInstance;
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060013BC RID: 5052 RVA: 0x00068D34 File Offset: 0x00066F34
		// (remove) Token: 0x060013BD RID: 5053 RVA: 0x00068D6C File Offset: 0x00066F6C
		public event Action OnAllMessagesComplete;

		// Token: 0x060013BE RID: 5054 RVA: 0x00068DA1 File Offset: 0x00066FA1
		private void Start()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00068DB0 File Offset: 0x00066FB0
		public void Post(string aTo, byte[] aData, Action<WWW> aCallback, Action<WWW> aOnError)
		{
			WWWForm wwwform = new WWWForm();
			wwwform.AddBinaryData("body", aData);
			base.StartCoroutine(this.Send(aTo, wwwform, aCallback, aOnError));
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00068DE4 File Offset: 0x00066FE4
		public void Post(string aTo, string aData, Action<WWW> aCallback, Action<WWW> aOnError)
		{
			byte[] array = new byte[aData.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (byte)aData[i];
			}
			this.Post(aTo, array, aCallback, aOnError);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00068E21 File Offset: 0x00067021
		public void PostForm(string aTo, WWWForm aForm, Action<WWW> aCallback, Action<WWW> aOnError)
		{
			base.StartCoroutine(this.Send(aTo, aForm, aCallback, aOnError));
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00068E35 File Offset: 0x00067035
		public void GetText(string aTo, Action<string> aCallback, Action<WWW> aOnError)
		{
			base.StartCoroutine(this.Send(aTo, aCallback, aOnError));
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00068E47 File Offset: 0x00067047
		public void GetRaw(string aTo, Action<WWW> aCallback, Action<WWW> aOnError)
		{
			base.StartCoroutine(this.Send(aTo, aCallback, aOnError));
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00068E59 File Offset: 0x00067059
		public void GetImage(string aTo, Action<Texture> aCallback, Action<WWW> aOnError)
		{
			base.StartCoroutine(this.Send(aTo, aCallback, aOnError));
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00068E6B File Offset: 0x0006706B
		private IEnumerator Send(string aTo, Action<WWW> aCallback, Action<WWW> aOnError)
		{
			this.BeginMessage();
			WWW www = new WWW(aTo);
			yield return www;
			if (string.IsNullOrEmpty(www.error) && aCallback != null)
			{
				aCallback(www);
			}
			else if (!string.IsNullOrEmpty(www.error) && aOnError != null)
			{
				aOnError(www);
			}
			this.FinishMessage();
			yield break;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00068E8F File Offset: 0x0006708F
		private IEnumerator Send(string aTo, Action<string> aCallback, Action<WWW> aOnError)
		{
			this.BeginMessage();
			WWW www = new WWW(aTo);
			yield return www;
			if (string.IsNullOrEmpty(www.error) && aCallback != null)
			{
				aCallback(www.text);
			}
			else if (!string.IsNullOrEmpty(www.error) && aOnError != null)
			{
				aOnError(www);
			}
			this.FinishMessage();
			yield break;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00068EB3 File Offset: 0x000670B3
		private IEnumerator Send(string aTo, Action<Texture> aCallback, Action<WWW> aOnError)
		{
			this.BeginMessage();
			WWW www = new WWW(aTo);
			yield return www;
			if (string.IsNullOrEmpty(www.error) && aCallback != null)
			{
				aCallback(www.texture);
			}
			else if (!string.IsNullOrEmpty(www.error) && aOnError != null)
			{
				aOnError(www);
			}
			this.FinishMessage();
			yield break;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00068ED7 File Offset: 0x000670D7
		private IEnumerator Send(string aTo, WWWForm aForm, Action<WWW> aCallback, Action<WWW> aOnError)
		{
			this.BeginMessage();
			WWW www = new WWW(aTo, aForm);
			yield return www;
			if (string.IsNullOrEmpty(www.error) && aCallback != null)
			{
				aCallback(www);
			}
			else if (!string.IsNullOrEmpty(www.error) && aOnError != null)
			{
				aOnError(www);
			}
			this.FinishMessage();
			yield break;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00068F03 File Offset: 0x00067103
		private IEnumerator Send(string aTo, byte[] aData, Action<WWW> aCallback, Action<WWW> aOnError)
		{
			this.BeginMessage();
			WWW www = new WWW(aTo, aData);
			yield return www;
			if (string.IsNullOrEmpty(www.error) && aCallback != null)
			{
				aCallback(www);
			}
			else if (!string.IsNullOrEmpty(www.error) && aOnError != null)
			{
				aOnError(www);
			}
			this.FinishMessage();
			yield break;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00068F2F File Offset: 0x0006712F
		public int GetActive()
		{
			return this.activeMessages;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00068F37 File Offset: 0x00067137
		private void BeginMessage()
		{
			this.activeMessages++;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00068F47 File Offset: 0x00067147
		private void FinishMessage()
		{
			this.activeMessages--;
			if (this.activeMessages == 0 && this.OnAllMessagesComplete != null)
			{
				this.OnAllMessagesComplete();
			}
		}

		// Token: 0x04000F55 RID: 3925
		private static WebMessager mInstance;

		// Token: 0x04000F56 RID: 3926
		private int activeMessages;
	}
}
