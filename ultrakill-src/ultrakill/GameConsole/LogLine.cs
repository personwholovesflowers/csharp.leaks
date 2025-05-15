using System;
using plog;
using plog.Helpers;
using plog.Models;
using plog.unity.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameConsole
{
	// Token: 0x020005B6 RID: 1462
	public class LogLine : MonoBehaviour
	{
		// Token: 0x060020CA RID: 8394 RVA: 0x00107538 File Offset: 0x00105738
		private void Awake()
		{
			this.rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00107546 File Offset: 0x00105746
		public void Wipe()
		{
			this.log = null;
			this.timestamp.text = "";
			this.message.text = "";
			this.mainPanel.color = this.normalLogColor;
			this.RefreshSize();
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00107588 File Offset: 0x00105788
		public void PopulateLine(ConsoleLog capture)
		{
			this.log = capture;
			this.timestamp.text = string.Format("{0:HH:mm:ss.f}", capture.log.Timestamp);
			this.RefreshSize();
			if (capture.expanded && !string.IsNullOrEmpty(capture.log.StackTrace))
			{
				string text = capture.log.StackTrace;
				text = text.Replace("\r\n", "\n").Replace("\n", "");
				this.message.text = string.Format("<b><size={0}>{1}</size></b>\n{2}", this.message.fontSizeMax, capture.log.Message, text);
				this.message.enableAutoSizing = true;
			}
			else
			{
				this.message.text = capture.log.Message;
				this.message.fontSize = this.message.fontSizeMax;
				this.message.enableAutoSizing = false;
			}
			Image image = this.mainPanel;
			Level level = capture.log.Level;
			Color color;
			if (level <= Level.Warning)
			{
				if (level == Level.Info)
				{
					color = this.normalLogColor;
					goto IL_016A;
				}
				if (level == Level.Warning)
				{
					color = this.warningLogColor;
					goto IL_016A;
				}
			}
			else
			{
				if (level == Level.Error)
				{
					color = this.errorLogColor;
					goto IL_016A;
				}
				if (level == Level.Exception)
				{
					color = this.errorLogColor;
					goto IL_016A;
				}
				if (level == Level.CommandLine)
				{
					color = this.cliLogColor;
					goto IL_016A;
				}
			}
			color = this.normalLogColor;
			IL_016A:
			image.color = color;
			global::plog.Logger source = capture.source;
			if (((source != null) ? source.Tag : null) != null)
			{
				this.context.text = capture.source.Tag.ToString();
				ValueTuple<UniversalColor, UniversalColor> colorPair = ColorHelper.GetColorPair(capture.source.Tag.Color);
				UniversalColor item = colorPair.Item1;
				UniversalColor item2 = colorPair.Item2;
				this.context.color = item2.ToUnityColor();
				Color color2 = item.ToUnityColor();
				color2.a = this.contextPanel.color.a;
				this.contextPanel.color = color2;
				if (!this.contextPanel.gameObject.activeSelf)
				{
					this.contextPanel.gameObject.SetActive(true);
					RectTransform rectTransform = this.message.rectTransform;
					if (this.defaultTextOffsetMin != null)
					{
						rectTransform.offsetMin = this.defaultTextOffsetMin.Value;
					}
					if (this.defaultTextOffsetMax != null)
					{
						rectTransform.offsetMax = this.defaultTextOffsetMax.Value;
					}
					if (this.defaultTextSizeDelta != null)
					{
						rectTransform.sizeDelta = this.defaultTextSizeDelta.Value;
					}
				}
			}
			else if (this.contextPanel.gameObject.activeSelf)
			{
				this.contextPanel.gameObject.SetActive(false);
				float x = this.contextPanel.rectTransform.sizeDelta.x;
				RectTransform rectTransform2 = this.message.rectTransform;
				if (this.defaultTextOffsetMin == null)
				{
					this.defaultTextOffsetMin = new Vector2?(rectTransform2.offsetMin);
				}
				if (this.defaultTextOffsetMax == null)
				{
					this.defaultTextOffsetMax = new Vector2?(rectTransform2.offsetMax);
				}
				if (this.defaultTextSizeDelta == null)
				{
					this.defaultTextSizeDelta = new Vector2?(rectTransform2.sizeDelta);
				}
				rectTransform2.offsetMin = new Vector2(rectTransform2.offsetMin.x - x * 2f, this.defaultTextOffsetMin.Value.y);
				rectTransform2.offsetMax = new Vector2(rectTransform2.offsetMax.x + x, this.defaultTextOffsetMax.Value.y);
				rectTransform2.sizeDelta = new Vector2(rectTransform2.sizeDelta.x - x * 2f, this.defaultTextSizeDelta.Value.y);
			}
			if (capture.timeSinceLogged < 0.5f && base.gameObject.activeInHierarchy)
			{
				this.attentionFlashGroup.alpha = this.TimeSinceToFlashAlpha(capture.timeSinceLogged);
			}
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x001079A5 File Offset: 0x00105BA5
		public void ToggleExpand()
		{
			this.log.expanded = !this.log.expanded;
			this.RefreshSize();
			this.PopulateLine(this.log);
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x001079D4 File Offset: 0x00105BD4
		private void RefreshSize()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			RectTransform rectTransform = this.rectTransform;
			ConsoleLog consoleLog = this.log;
			rectTransform.sizeDelta = ((consoleLog == null || !consoleLog.expanded) ? new Vector2(this.rectTransform.sizeDelta.x, this.normalHeight) : new Vector2(this.rectTransform.sizeDelta.x, this.expandedHeight));
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00107A50 File Offset: 0x00105C50
		private void Update()
		{
			if (this.log != null)
			{
				if (this.log.timeSinceLogged > 0.5f)
				{
					this.attentionFlashGroup.alpha = 0f;
					return;
				}
				this.attentionFlashGroup.alpha = this.TimeSinceToFlashAlpha(this.log.timeSinceLogged);
			}
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00107AAE File Offset: 0x00105CAE
		private float TimeSinceToFlashAlpha(float timeSinceLogged)
		{
			if (timeSinceLogged < 0.2f)
			{
				return timeSinceLogged / 0.2f;
			}
			return 1f - (timeSinceLogged - 0.2f) / 0.3f;
		}

		// Token: 0x04002D0F RID: 11535
		[SerializeField]
		private TMP_Text timestamp;

		// Token: 0x04002D10 RID: 11536
		[SerializeField]
		private TMP_Text message;

		// Token: 0x04002D11 RID: 11537
		[SerializeField]
		private TMP_Text context;

		// Token: 0x04002D12 RID: 11538
		[SerializeField]
		private Image contextPanel;

		// Token: 0x04002D13 RID: 11539
		[SerializeField]
		private Image mainPanel;

		// Token: 0x04002D14 RID: 11540
		[Space]
		[SerializeField]
		private CanvasGroup attentionFlashGroup;

		// Token: 0x04002D15 RID: 11541
		[Space]
		[SerializeField]
		private Color normalLogColor;

		// Token: 0x04002D16 RID: 11542
		[SerializeField]
		private Color warningLogColor;

		// Token: 0x04002D17 RID: 11543
		[SerializeField]
		private Color errorLogColor;

		// Token: 0x04002D18 RID: 11544
		[SerializeField]
		private Color cliLogColor;

		// Token: 0x04002D19 RID: 11545
		[Space]
		[SerializeField]
		private float normalHeight = 35f;

		// Token: 0x04002D1A RID: 11546
		[SerializeField]
		private float expandedHeight = 120f;

		// Token: 0x04002D1B RID: 11547
		private RectTransform rectTransform;

		// Token: 0x04002D1C RID: 11548
		private Vector2? defaultTextOffsetMin;

		// Token: 0x04002D1D RID: 11549
		private Vector2? defaultTextOffsetMax;

		// Token: 0x04002D1E RID: 11550
		private Vector2? defaultTextSizeDelta;

		// Token: 0x04002D1F RID: 11551
		private ConsoleLog log;
	}
}
