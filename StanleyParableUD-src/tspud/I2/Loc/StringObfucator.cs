using System;
using System.Text;

namespace I2.Loc
{
	// Token: 0x020002D3 RID: 723
	public class StringObfucator
	{
		// Token: 0x0600128B RID: 4747 RVA: 0x00064D0C File Offset: 0x00062F0C
		public static string Encode(string NormalString)
		{
			string text;
			try
			{
				text = StringObfucator.ToBase64(StringObfucator.XoREncode(NormalString));
			}
			catch (Exception)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00064D40 File Offset: 0x00062F40
		public static string Decode(string ObfucatedString)
		{
			string text;
			try
			{
				text = StringObfucator.XoREncode(StringObfucator.FromBase64(ObfucatedString));
			}
			catch (Exception)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00064D74 File Offset: 0x00062F74
		private static string ToBase64(string regularString)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(regularString));
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00064D88 File Offset: 0x00062F88
		private static string FromBase64(string base64string)
		{
			byte[] array = Convert.FromBase64String(base64string);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00064DAC File Offset: 0x00062FAC
		private static string XoREncode(string NormalString)
		{
			string text;
			try
			{
				char[] stringObfuscatorPassword = StringObfucator.StringObfuscatorPassword;
				char[] array = NormalString.ToCharArray();
				int num = stringObfuscatorPassword.Length;
				int i = 0;
				int num2 = array.Length;
				while (i < num2)
				{
					array[i] = array[i] ^ stringObfuscatorPassword[i % num] ^ (char)((byte)((i % 2 == 0) ? (i * 23) : (-i * 51)));
					i++;
				}
				text = new string(array);
			}
			catch (Exception)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x04000EFA RID: 3834
		public static char[] StringObfuscatorPassword = "ÝúbUu\u00b8CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd1<QÛÝúbUu\u00b8CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd".ToCharArray();
	}
}
