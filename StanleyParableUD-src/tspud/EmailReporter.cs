using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000053 RID: 83
public class EmailReporter : MonoBehaviour
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000212 RID: 530 RVA: 0x0000F621 File Offset: 0x0000D821
	private string logPath
	{
		get
		{
			return Environment.ExpandEnvironmentVariables(string.Concat(new string[]
			{
				"%USERPROFILE%/AppData/LocalLow/",
				Application.companyName,
				"/",
				Application.productName,
				"/Player.log"
			}));
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000213 RID: 531 RVA: 0x0000F65B File Offset: 0x0000D85B
	private string logPathCopy
	{
		get
		{
			return Environment.ExpandEnvironmentVariables(string.Concat(new string[]
			{
				"%USERPROFILE%/AppData/LocalLow/",
				Application.companyName,
				"/",
				Application.productName,
				"/Player_logCopy.txt"
			}));
		}
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000F695 File Offset: 0x0000D895
	public bool SendReport(string userName, string userPassword, string userSmtp, string userReceivingAddress, string title, string description, List<Texture2D> screenshots, out string message)
	{
		this.from = userName;
		this.password = userPassword;
		this.smtp = userSmtp;
		this.receivingAddress = userReceivingAddress;
		this.subject = title;
		this.body = description;
		this.attachedScreenshots = screenshots;
		return this.SendEmail(out message);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000F6D4 File Offset: 0x0000D8D4
	private void SetEmailing(bool dir)
	{
		this.isemailing = dir;
		Time.timeScale = (dir ? 0f : 1f);
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
	private bool SendEmail(out string message)
	{
		MailMessage mailMessage = new MailMessage();
		string text = "";
		int num = 0;
		foreach (Texture2D texture2D in this.attachedScreenshots)
		{
			byte[] array = texture2D.EncodeToJPG();
			string text2 = string.Format(Application.dataPath + "/../reportscreenshot_{0}.jpg", num);
			File.WriteAllBytes(text2, array);
			LinkedResource linkedResource = new LinkedResource(text2);
			linkedResource.ContentId = Guid.NewGuid().ToString();
			Attachment attachment = new Attachment(text2);
			attachment.ContentDisposition.Inline = true;
			mailMessage.Attachments.Add(attachment);
			text += string.Format("<img src=\"cid:{0}\" />", linkedResource.ContentId);
			num++;
		}
		if (File.Exists(this.logPath))
		{
			File.Copy(this.logPath, this.logPathCopy, true);
			Attachment attachment2 = new Attachment(this.logPathCopy);
			attachment2.ContentDisposition.Inline = true;
			mailMessage.Attachments.Add(attachment2);
		}
		mailMessage.From = new MailAddress(this.from);
		mailMessage.To.Add(this.receivingAddress);
		mailMessage.Subject = this.subject;
		mailMessage.Body = string.Format(string.Concat(new string[]
		{
			this.body,
			"<br/><br/><hr>",
			text,
			"<br/>Scene: ",
			SceneManager.GetActiveScene().name,
			"<br/>",
			this.GetHardwareInfo(),
			"<br/><hr>"
		}), Array.Empty<object>());
		mailMessage.IsBodyHtml = true;
		SmtpClient smtpClient = new SmtpClient(this.smtp);
		smtpClient.Port = 587;
		smtpClient.EnableSsl = true;
		smtpClient.Credentials = new NetworkCredential(this.from, this.password);
		ServicePointManager.ServerCertificateValidationCallback = (object obj, X509Certificate cert, X509Chain chain, SslPolicyErrors sslerrors) => true;
		message = "Report sent!";
		try
		{
			smtpClient.Send(mailMessage);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			message = ex.Message;
			this.SetEmailing(false);
			return false;
		}
		this.SetEmailing(false);
		return true;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000F960 File Offset: 0x0000DB60
	private string GetHardwareInfo()
	{
		return string.Concat(new string[]
		{
			"Graphics Device Name: ",
			SystemInfo.graphicsDeviceName,
			"<br/>Graphics Device Type: ",
			SystemInfo.graphicsDeviceType.ToString(),
			"<br/>Graphics Device Version: ",
			SystemInfo.graphicsDeviceVersion,
			"<br/>Graphics Memory Size: ",
			this.MBtoGB(SystemInfo.graphicsMemorySize),
			"<br/>Graphics Shader Level: ",
			this.ShaderLevel(SystemInfo.graphicsShaderLevel),
			"<br/>Maximum Texture Size: ",
			this.MBtoGB(SystemInfo.maxTextureSize),
			"<br/>Operating System: ",
			SystemInfo.operatingSystem,
			"<br/>Processor Type: ",
			SystemInfo.processorType,
			"<br/>Processor Count: ",
			SystemInfo.processorCount.ToString(),
			"<br/>Processor Frequency: ",
			SystemInfo.processorFrequency.ToString(),
			"<br/>System Memory Size: ",
			this.MBtoGB(SystemInfo.systemMemorySize),
			"<br/>Screen Size: ",
			Screen.width.ToString(),
			"x",
			Screen.height.ToString()
		});
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
	private string ShaderLevel(int level)
	{
		if (level <= 30)
		{
			if (level == 20)
			{
				return "Shader Model 2.x";
			}
			if (level == 30)
			{
				return "Shader Model 3.0";
			}
		}
		else
		{
			if (level == 40)
			{
				return "Shader Model 4.0 ( DX10.0 )";
			}
			if (level == 41)
			{
				return "Shader Model 4.1 ( DX10.1 )";
			}
			if (level == 50)
			{
				return "Shader Model 5.0 ( DX11.0 )";
			}
		}
		return "";
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000FAF4 File Offset: 0x0000DCF4
	private string MBtoGB(int mb)
	{
		return Mathf.Ceil((float)mb / 1024f).ToString() + "GB";
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000FB20 File Offset: 0x0000DD20
	private IEnumerator TakeScreenshot()
	{
		yield return new WaitForEndOfFrame();
		ScreenCapture.CaptureScreenshot("debugscreenshot.png");
		this.SetEmailing(!this.isemailing);
		yield break;
	}

	// Token: 0x04000230 RID: 560
	private string from = "";

	// Token: 0x04000231 RID: 561
	private string password = "";

	// Token: 0x04000232 RID: 562
	private string smtp;

	// Token: 0x04000233 RID: 563
	private string receivingAddress = "";

	// Token: 0x04000234 RID: 564
	private bool isemailing;

	// Token: 0x04000235 RID: 565
	private string subject;

	// Token: 0x04000236 RID: 566
	private string body = "";

	// Token: 0x04000237 RID: 567
	private List<Texture2D> attachedScreenshots = new List<Texture2D>();
}
