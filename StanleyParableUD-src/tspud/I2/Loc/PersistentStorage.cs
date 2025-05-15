using System;

namespace I2.Loc
{
	// Token: 0x0200028E RID: 654
	public static class PersistentStorage
	{
		// Token: 0x06001067 RID: 4199 RVA: 0x000560E7 File Offset: 0x000542E7
		public static void SetSetting_String(string key, string value)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.SetSetting_String(key, value);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00056106 File Offset: 0x00054306
		public static string GetSetting_String(string key, string defaultValue)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.GetSetting_String(key, defaultValue);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00056125 File Offset: 0x00054325
		public static void DeleteSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.DeleteSetting(key);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00056143 File Offset: 0x00054343
		public static bool HasSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasSetting(key);
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00056161 File Offset: 0x00054361
		public static void ForceSaveSettings()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.ForceSaveSettings();
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0005617E File Offset: 0x0005437E
		public static bool CanAccessFiles()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.CanAccessFiles();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0005619B File Offset: 0x0005439B
		public static bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.SaveFile(fileType, fileName, data, logExceptions);
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x000561BC File Offset: 0x000543BC
		public static string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.LoadFile(fileType, fileName, logExceptions);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x000561DC File Offset: 0x000543DC
		public static bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.DeleteFile(fileType, fileName, logExceptions);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000561FC File Offset: 0x000543FC
		public static bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasFile(fileType, fileName, logExceptions);
		}

		// Token: 0x04000DCB RID: 3531
		private static I2CustomPersistentStorage mStorage;

		// Token: 0x0200047E RID: 1150
		public enum eFileType
		{
			// Token: 0x04001713 RID: 5907
			Raw,
			// Token: 0x04001714 RID: 5908
			Persistent,
			// Token: 0x04001715 RID: 5909
			Temporal,
			// Token: 0x04001716 RID: 5910
			Streaming
		}
	}
}
