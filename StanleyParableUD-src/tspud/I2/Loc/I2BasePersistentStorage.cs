using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200028F RID: 655
	public abstract class I2BasePersistentStorage
	{
		// Token: 0x06001071 RID: 4209 RVA: 0x0005621C File Offset: 0x0005441C
		public virtual void SetSetting_String(string key, string value)
		{
			try
			{
				int length = value.Length;
				int num = 8000;
				if (length <= num)
				{
					PlayerPrefs.SetString(key, value);
				}
				else
				{
					int num2 = Mathf.CeilToInt((float)length / (float)num);
					for (int i = 0; i < num2; i++)
					{
						int num3 = num * i;
						PlayerPrefs.SetString(string.Format("[I2split]{0}{1}", i, key), value.Substring(num3, Mathf.Min(num, length - num3)));
					}
					PlayerPrefs.SetString(key, "[$I2#@div$]" + num2);
				}
			}
			catch (Exception)
			{
				Debug.LogError("Error saving PlayerPrefs " + key);
			}
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x000562C4 File Offset: 0x000544C4
		public virtual string GetSetting_String(string key, string defaultValue)
		{
			string text2;
			try
			{
				string text = PlayerPrefs.GetString(key, defaultValue);
				if (!string.IsNullOrEmpty(text) && text.StartsWith("[I2split]"))
				{
					int num = int.Parse(text.Substring("[I2split]".Length));
					text = "";
					for (int i = 0; i < num; i++)
					{
						text += PlayerPrefs.GetString(string.Format("[I2split]{0}{1}", i, key), "");
					}
				}
				text2 = text;
			}
			catch (Exception)
			{
				Debug.LogError("Error loading PlayerPrefs " + key);
				text2 = defaultValue;
			}
			return text2;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00056364 File Offset: 0x00054564
		public virtual void DeleteSetting(string key)
		{
			try
			{
				string @string = PlayerPrefs.GetString(key, null);
				if (!string.IsNullOrEmpty(@string) && @string.StartsWith("[I2split]"))
				{
					int num = int.Parse(@string.Substring("[I2split]".Length));
					for (int i = 0; i < num; i++)
					{
						PlayerPrefs.DeleteKey(string.Format("[I2split]{0}{1}", i, key));
					}
				}
				PlayerPrefs.DeleteKey(key);
			}
			catch (Exception)
			{
				Debug.LogError("Error deleting PlayerPrefs " + key);
			}
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00027CD5 File Offset: 0x00025ED5
		public virtual void ForceSaveSettings()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00027CCD File Offset: 0x00025ECD
		public virtual bool HasSetting(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0001C409 File Offset: 0x0001A609
		public virtual bool CanAccessFiles()
		{
			return true;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x000563F4 File Offset: 0x000545F4
		private string UpdateFilename(PersistentStorage.eFileType fileType, string fileName)
		{
			switch (fileType)
			{
			case PersistentStorage.eFileType.Persistent:
				fileName = Application.persistentDataPath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Temporal:
				fileName = Application.temporaryCachePath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Streaming:
				fileName = Application.streamingAssetsPath + "/" + fileName;
				break;
			}
			return fileName;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00056454 File Offset: 0x00054654
		public virtual bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool flag;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.WriteAllText(fileName, data, Encoding.UTF8);
				flag = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[] { "Error saving file '", fileName, "'\n", ex }));
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x000564C8 File Offset: 0x000546C8
		public virtual string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return null;
			}
			string text;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				text = File.ReadAllText(fileName, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[] { "Error loading file '", fileName, "'\n", ex }));
				}
				text = null;
			}
			return text;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0005653C File Offset: 0x0005473C
		public virtual bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool flag;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.Delete(fileName);
				flag = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[] { "Error deleting file '", fileName, "'\n", ex }));
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x000565AC File Offset: 0x000547AC
		public virtual bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool flag;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				flag = File.Exists(fileName);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					Debug.LogError(string.Concat(new object[] { "Error requesting file '", fileName, "'\n", ex }));
				}
				flag = false;
			}
			return flag;
		}
	}
}
