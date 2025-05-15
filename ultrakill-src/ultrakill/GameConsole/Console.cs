using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameConsole.Commands;
using GameConsole.CommandTree;
using plog;
using plog.Handlers;
using plog.Models;
using plog.unity.Handlers;
using plog.unity.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameConsole
{
	// Token: 0x020005A8 RID: 1448
	[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
	public class Console : MonoSingleton<Console>, global::plog.Handlers.ILogHandler
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00105A87 File Offset: 0x00103C87
		public static bool IsOpen
		{
			get
			{
				return MonoSingleton<Console>.Instance != null && MonoSingleton<Console>.Instance.consoleContainer != null && MonoSingleton<Console>.Instance.consoleContainer.activeSelf;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x00105AB9 File Offset: 0x00103CB9
		// (set) Token: 0x06002079 RID: 8313 RVA: 0x00105AC1 File Offset: 0x00103CC1
		public bool ExtractStackTraces { get; private set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x00105ACA File Offset: 0x00103CCA
		private List<ConsoleLog> filteredLogs
		{
			get
			{
				return this.logs.Where((ConsoleLog l) => this.logLevelFilter.Contains(l.log.Level)).ToList<ConsoleLog>();
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00105AE8 File Offset: 0x00103CE8
		protected override void Awake()
		{
			if (MonoSingleton<Console>.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			base.Awake();
			this.binds = new Binds();
			this.binds.Initialize();
			Console.AutocompletePanel[] array = this.autocompletePanels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].background.transform.parent.gameObject.SetActive(true);
			}
			this.SelectSuggestion(0, false);
			if (this.binds.registeredBinds != null && this.binds.registeredBinds.ContainsKey("open") && this.binds.registeredBinds["open"].Action != null)
			{
				this.openBindText.text = this.binds.registeredBinds["open"].Action.GetBindingDisplayString((InputBinding.DisplayStringOptions)0, null);
			}
			this.RegisterCommands(new ICommand[]
			{
				new Help(),
				new Clear(),
				new Echo(),
				new Exit()
			});
			this.RegisterCommands(new ICommand[]
			{
				new Prefs(this),
				new Scenes(),
				new Scene(),
				new ConsoleCmd(this),
				new Style(this),
				new Buffs(this),
				new MapVar(this),
				new InputCommands(this),
				new Rumble(this)
			});
			if (global::UnityEngine.Debug.isDebugBuild)
			{
				this.RegisterCommand(new GameConsole.Commands.Debug(this));
				this.RegisterCommand(new Pcon(this));
			}
			for (int j = 0; j < 20; j++)
			{
				LogLine logLine = Object.Instantiate<LogLine>(this.logLine, this.logContainer.transform, false);
				logLine.Wipe();
				logLine.gameObject.SetActive(false);
				this.logLinePool.Add(logLine);
			}
			this.logLine.gameObject.SetActive(false);
			Application.logMessageReceived += this.HandleUnityLog;
			this.InitializePLog();
			if (!Consts.CONSOLE_ERROR_BADGE)
			{
				this.errorBadge.SetEnabled(false, true);
			}
			if (global::UnityEngine.Debug.isDebugBuild && MonoSingleton<PrefsManager>.Instance.GetBoolLocal("pcon.autostart", false))
			{
				this.StartPCon();
			}
			this.consoleInput.onValueChanged.AddListener(new UnityAction<string>(this.FindSuggestions));
			this.DefaultDevConsoleOff();
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00105D37 File Offset: 0x00103F37
		private void OnDisable()
		{
			Application.logMessageReceived -= this.HandleUnityLog;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00105D4A File Offset: 0x00103F4A
		private void Start()
		{
			this.ExtractStackTraces = global::UnityEngine.Debug.isDebugBuild || MonoSingleton<PrefsManager>.Instance.GetBoolLocal("forceStackTraceExtraction", false);
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00105D6C File Offset: 0x00103F6C
		private void InitializePLog()
		{
			global::plog.Logger.Root.AddHandler(this.unityProxyHandler);
			global::plog.Logger.Root.AddHandler(this);
			Console.RefreshPLogConfiguration();
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x00105D8E File Offset: 0x00103F8E
		private void HandleUnityLog(string message, string stacktrace, LogType type)
		{
			this.unityProxyHandler.LogMessageReceived(message, stacktrace, type);
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x00105DA0 File Offset: 0x00103FA0
		public Log HandleRecord(global::plog.Logger source, Log log)
		{
			if (log.StackTrace == null && (this.ExtractStackTraces || log.Level == Level.Error || log.Level == Level.Exception))
			{
				Log log2 = log.<Clone>$();
				log2.StackTrace = StackTraceUtility.ExtractStackTrace();
				log = log2;
			}
			this.logs.Add(new ConsoleLog(log, source));
			this.InsertLog(log);
			if (log.Level == Level.Error)
			{
				Action action = this.onError;
				if (action != null)
				{
					action();
				}
			}
			return log;
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00105E22 File Offset: 0x00104022
		public static void RefreshPLogConfiguration()
		{
			UnityConfigurationManager.SetConfiguration(PLogConfigHelper.GetCurrentConfiguration());
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00105E30 File Offset: 0x00104030
		public void StartPCon()
		{
			if (this.pconAdapter == null)
			{
				return;
			}
			if (!this.pconAdapter.PConLibraryExists())
			{
				this.pconAdapter = null;
				return;
			}
			this.pconAdapter.StartPConClient(new Action<string>(this.ProcessInput), delegate
			{
				MonoSingleton<CheatsController>.Instance.ActivateCheats();
			});
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00105E91 File Offset: 0x00104091
		public void UpdateDisplayString()
		{
			this.openBindText.text = this.binds.registeredBinds["open"].Action.GetBindingDisplayString((InputBinding.DisplayStringOptions)0, null);
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00105EC0 File Offset: 0x001040C0
		public bool CheatBlocker()
		{
			if (MonoSingleton<CheatsController>.Instance == null && CheatsManager.KeepCheatsEnabled)
			{
				return false;
			}
			if (MonoSingleton<CheatsController>.Instance == null || !MonoSingleton<CheatsController>.Instance.cheatsEnabled)
			{
				Console.Log.Error("Cheats aren't enabled!", null, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00105F14 File Offset: 0x00104114
		public void RegisterCommands(IEnumerable<ICommand> commands)
		{
			foreach (ICommand command in commands)
			{
				this.RegisterCommand(command);
			}
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00105F5C File Offset: 0x0010415C
		public void RegisterCommand(ICommand command)
		{
			if (this.registeredCommandTypes.Contains(command.GetType()))
			{
				Console.Log.Warning("Command " + command.GetType().Name + " already registered!", null, null, null);
				return;
			}
			this.recognizedCommands.Add(command.Command.ToLower(), command);
			this.registeredCommandTypes.Add(command.GetType());
			IConsoleLogger consoleLogger = command as IConsoleLogger;
			if (consoleLogger != null)
			{
				consoleLogger.Log.NotifyParent = false;
				consoleLogger.Log.AddHandler(this.unityProxyHandler);
				consoleLogger.Log.AddHandler(this);
			}
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x00106000 File Offset: 0x00104200
		public void Clear()
		{
			this.scrollState = 0;
			this.errorCount = 0;
			this.warningCount = 0;
			this.infoCount = 0;
			this.logs.Clear();
			this.RepopulateLogs();
			this.UpdateScroller();
			this.errorBadge.SetEnabled(false, false);
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x00106050 File Offset: 0x00104250
		private void IncrementCounters(Level type)
		{
			if (this.scrollState > 0)
			{
				this.scrollState++;
				this.UpdateScroller();
			}
			if (type == Level.Warning)
			{
				this.warningCount++;
				return;
			}
			if (type != Level.Error)
			{
				if (type != Level.CommandLine)
				{
					this.infoCount++;
				}
				return;
			}
			this.errorCount++;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x001060C0 File Offset: 0x001042C0
		public void UpdateFilters(bool showErrors, bool showWarnings, bool showLogs)
		{
			if (showErrors)
			{
				this.logLevelFilter.Add(Level.Error);
			}
			else
			{
				this.logLevelFilter.Remove(Level.Error);
			}
			if (showWarnings)
			{
				this.logLevelFilter.Add(Level.Warning);
			}
			else
			{
				this.logLevelFilter.Remove(Level.Warning);
			}
			if (showLogs)
			{
				this.logLevelFilter.Add(Level.Info);
				this.logLevelFilter.Add(Level.Fine);
				this.logLevelFilter.Add(Level.Debug);
			}
			else
			{
				this.logLevelFilter.Remove(Level.Info);
				this.logLevelFilter.Remove(Level.Fine);
				this.logLevelFilter.Remove(Level.Debug);
			}
			this.RepopulateLogs();
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00106186 File Offset: 0x00104386
		public void SetForceStackTraceExtraction(bool value)
		{
			this.ExtractStackTraces = value;
			MonoSingleton<PrefsManager>.Instance.SetBoolLocal("forceStackTraceExtraction", value);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x0010619F File Offset: 0x0010439F
		public string[] Parse(string text)
		{
			return text.Split(' ', StringSplitOptions.None);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x001061AA File Offset: 0x001043AA
		private void ProcessUserInput(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			this.ProcessInput(text);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x001061BC File Offset: 0x001043BC
		public void ProcessInput(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] array = this.Parse(text);
			if (array.Length == 0)
			{
				return;
			}
			string text2 = array[0];
			text2 = text2.ToLower();
			Console.Log.CommandLine("> " + text, null, null, null);
			if (text.ToLower() == "sv_cheats 1")
			{
				Console.Log.Warning("To enable cheats, you must enter the Konami code in-game.", null, null, null);
				return;
			}
			ICommand command;
			if (this.recognizedCommands.TryGetValue(text2, out command))
			{
				try
				{
					command.Execute(this, array.Skip(1).ToArray<string>());
					return;
				}
				catch (Exception ex)
				{
					Console.Log.Error("Command <b>'" + text2 + "'</b> failed.\n" + ex.Message, null, ex.StackTrace, null);
					return;
				}
			}
			Console.Log.Warning("Unknown command: '" + text2 + "'", null, null, null);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x001062A4 File Offset: 0x001044A4
		private void ScrollUp()
		{
			this.timeSinceScrollTick = 0f;
			this.scrollState++;
			if (this.scrollState > this.logs.Count - 1)
			{
				this.scrollState = this.logs.Count - 1;
			}
			if (this.logs.Count == 0)
			{
				this.scrollState = 0;
			}
			this.UpdateScroller();
			this.RepopulateLogs();
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00106317 File Offset: 0x00104517
		private void ScrollDown()
		{
			this.timeSinceScrollTick = 0f;
			this.scrollState--;
			if (this.scrollState < 0)
			{
				this.scrollState = 0;
			}
			this.UpdateScroller();
			this.RepopulateLogs();
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00004AE3 File Offset: 0x00002CE3
		private void DefaultDevConsoleOff()
		{
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00106354 File Offset: 0x00104554
		private void Update()
		{
			bool activeSelf = this.consoleContainer.activeSelf;
			if (this.binds.OpenPressed || (this.consoleOpen && Input.GetKeyDown(KeyCode.Escape)))
			{
				this.consoleOpen = !this.consoleOpen;
				if (this.consoleOpen)
				{
					GameStateManager.Instance.RegisterState(new GameState("console", this.hideOnPin)
					{
						cursorLock = LockMode.Unlock,
						playerInputLock = LockMode.Lock,
						cameraInputLock = LockMode.Lock,
						priority = 100
					});
					if (this.logsDirty)
					{
						this.RepopulateLogs();
					}
				}
				else
				{
					GameStateManager.Instance.PopState("console");
				}
				if (this.pinned)
				{
					GameObject[] array = this.hideOnPin;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetActive(this.consoleOpen);
					}
					if (!this.consoleOpen)
					{
						array = this.hideOnPinNoReopen;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].SetActive(false);
						}
					}
					Image[] array2 = this.backgrounds;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].enabled = this.consoleOpen;
					}
				}
				else
				{
					this.consoleContainer.SetActive(this.consoleOpen);
				}
				this.masterGroup.interactable = this.consoleOpen;
				bool flag = activeSelf;
				if (this.pinned)
				{
					flag = !this.consoleOpen;
				}
				if (flag)
				{
					if (MonoSingleton<OptionsManager>.Instance && this.binds.OpenPressed && !this.openedDuringPause && MonoSingleton<OptionsManager>.Instance == this.rememberedOptionsManager && SceneHelper.CurrentScene != "Main Menu")
					{
						MonoSingleton<OptionsManager>.Instance.UnPause();
					}
					base.StopAllCoroutines();
				}
				else
				{
					if (MonoSingleton<OptionsManager>.Instance && SceneHelper.CurrentScene != "Main Menu")
					{
						this.openedDuringPause = MonoSingleton<OptionsManager>.Instance.paused;
						this.rememberedOptionsManager = MonoSingleton<OptionsManager>.Instance;
						MonoSingleton<OptionsManager>.Instance.Pause();
					}
					this.consoleBlocker.alpha = 0f;
					base.StartCoroutine(this.FadeBlockerIn());
					this.consoleInput.ActivateInputField();
					this.errorBadge.Dismiss();
				}
			}
			if (!this.consoleOpen)
			{
				return;
			}
			if (this.binds.ScrollUpPressed || Input.mouseScrollDelta.y > 0f)
			{
				this.timeSincePgHeld = 0f;
				this.ScrollUp();
			}
			if (this.binds.ScrollDownPressed || Input.mouseScrollDelta.y < 0f)
			{
				this.timeSincePgHeld = 0f;
				this.ScrollDown();
			}
			if ((this.binds.ScrollUpHeld || this.binds.ScrollDownHeld) && this.timeSincePgHeld > 0.5f)
			{
				bool scrollUpHeld = this.binds.ScrollUpHeld;
				if (this.timeSinceScrollTick > 0.05f)
				{
					if (scrollUpHeld)
					{
						this.ScrollUp();
					}
					else
					{
						this.ScrollDown();
					}
				}
			}
			if (this.binds.ScrollToTopPressed)
			{
				this.scrollState = this.logs.Count - 1;
				this.UpdateScroller();
				this.RepopulateLogs();
			}
			if (this.binds.ScrollToBottomPressed)
			{
				this.scrollState = 0;
				this.UpdateScroller();
				this.RepopulateLogs();
			}
			if (this.suggestions.Count > 0)
			{
				if (this.binds.AutocompletePressed || this.binds.SubmitPressed)
				{
					this.consoleInput.text = this.suggestions[this.selectedSuggestionIndex];
					this.consoleInput.caretPosition = this.consoleInput.text.Length;
					this.consoleInput.ActivateInputField();
				}
				if (this.binds.CommandHistoryUpPressed)
				{
					this.SelectSuggestion(this.selectedSuggestionIndex + 1, true);
					this.consoleInput.caretPosition = this.consoleInput.text.Length;
				}
				if (this.binds.CommandHistoryDownPressed)
				{
					this.SelectSuggestion(this.selectedSuggestionIndex - 1, true);
					this.consoleInput.caretPosition = this.consoleInput.text.Length;
					return;
				}
			}
			else
			{
				if (this.binds.CommandHistoryUpPressed)
				{
					this.commandHistoryIndex++;
					if (this.commandHistoryIndex > this.commandHistory.Count - 1)
					{
						this.commandHistoryIndex = this.commandHistory.Count - 1;
					}
					this.consoleInput.text = ((this.commandHistoryIndex == -1) ? "" : this.commandHistory[this.commandHistoryIndex]);
					this.consoleInput.caretPosition = this.consoleInput.text.Length;
				}
				if (this.binds.CommandHistoryDownPressed)
				{
					this.commandHistoryIndex--;
					if (this.commandHistoryIndex < -1)
					{
						this.commandHistoryIndex = -1;
					}
					this.consoleInput.text = ((this.commandHistoryIndex == -1) ? "" : this.commandHistory[this.commandHistoryIndex]);
					this.consoleInput.caretPosition = this.consoleInput.text.Length;
				}
				if (this.binds.SubmitPressed)
				{
					this.consoleInput.ActivateInputField();
					if (string.IsNullOrEmpty(this.consoleInput.text))
					{
						return;
					}
					this.ProcessUserInput(this.consoleInput.text);
					this.commandHistory = this.commandHistory.Prepend(this.consoleInput.text).ToList<string>();
					this.commandHistoryIndex = -1;
					this.consoleInput.text = string.Empty;
				}
			}
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x001068E4 File Offset: 0x00104AE4
		private void UpdateScroller()
		{
			if (this.scrollState == 0)
			{
				this.scroller.SetActive(false);
				return;
			}
			this.scroller.SetActive(true);
			this.scrollText.text = string.Format("{0} lines below", this.scrollState);
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00106932 File Offset: 0x00104B32
		private IEnumerator FadeBlockerIn()
		{
			this.consoleBlocker.alpha = 0f;
			while (this.consoleBlocker.alpha < 1f)
			{
				this.consoleBlocker.alpha += 0.2f;
				yield return new WaitForSecondsRealtime(0.03f);
			}
			this.consoleBlocker.alpha = 1f;
			yield break;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00106944 File Offset: 0x00104B44
		private void SelectSuggestion(int newIndex, bool wrap = false)
		{
			if (this.suggestions.Count == 0)
			{
				Console.AutocompletePanel[] array = this.autocompletePanels;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].background.gameObject.SetActive(false);
				}
				return;
			}
			int num = Mathf.Max(0, this.suggestions.Count);
			int num3;
			if (wrap)
			{
				int num2 = newIndex % num;
				if (num2 < 0)
				{
					num2 += num;
				}
				num3 = num2;
			}
			else
			{
				num3 = Mathf.Clamp(newIndex, 0, num - 1);
			}
			this.selectedSuggestionIndex = num3;
			this.ShowSuggestions(num3);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x001069CC File Offset: 0x00104BCC
		private void ShowSuggestions(int selected)
		{
			int num = this.suggestionStartIndex + Mathf.Min(this.suggestions.Count, this.autocompletePanels.Length - 1);
			if (selected < this.suggestionStartIndex)
			{
				this.suggestionStartIndex = selected;
			}
			if (selected > num)
			{
				int num2 = selected - num;
				this.suggestionStartIndex += num2;
			}
			int num3 = this.suggestionStartIndex;
			this.suggestions.Skip(num3 - 1).Take(this.autocompletePanels.Length);
			for (int i = 0; i < this.autocompletePanels.Length; i++)
			{
				Console.AutocompletePanel autocompletePanel = this.autocompletePanels[i];
				int num4 = num3 + i;
				if (num4 >= this.suggestions.Count)
				{
					autocompletePanel.background.gameObject.SetActive(false);
				}
				else
				{
					autocompletePanel.text.text = "> " + this.suggestions[num4];
					if (num4 == this.selectedSuggestionIndex)
					{
						autocompletePanel.background.color = Color.gray;
					}
					else
					{
						autocompletePanel.background.color = Color.black;
					}
					autocompletePanel.background.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00106AF4 File Offset: 0x00104CF4
		private void FindSuggestions(string value)
		{
			this.suggestions.Clear();
			if (value == "")
			{
				this.SelectSuggestion(0, false);
				return;
			}
			string[] array = this.Parse(value);
			Queue<string> queue = new Queue<string>(array.Skip(1));
			ICommand command;
			if (this.recognizedCommands.TryGetValue(array[0], out command))
			{
				CommandRoot commandRoot = command as CommandRoot;
				if (commandRoot != null)
				{
					ValueTuple<string, Branch> valueTuple = commandRoot.FindLongestMatchingBranch(commandRoot.Root, queue, null, null);
					string soFar = valueTuple.Item1;
					IEnumerable<Branch> enumerable = valueTuple.Item2.children.Where((Node n) => n is Branch).Cast<Branch>();
					if (queue.Count > 0)
					{
						string next = queue.Peek();
						enumerable = enumerable.Where((Branch n) => n.name.StartsWith(next));
						this.suggestions.AddRange(enumerable.Select((Branch n) => soFar + " " + n.name));
					}
				}
			}
			else
			{
				foreach (KeyValuePair<string, ICommand> keyValuePair in this.recognizedCommands)
				{
					if (keyValuePair.Key.StartsWith(array[0]))
					{
						this.suggestions.Add(keyValuePair.Value.Command ?? "");
					}
				}
			}
			this.SelectSuggestion(0, false);
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00106C88 File Offset: 0x00104E88
		private void InsertLog(Log log)
		{
			this.IncrementCounters(log.Level);
			if (Console.IsOpen)
			{
				this.RepopulateLogs();
				return;
			}
			this.logsDirty = true;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00106CAC File Offset: 0x00104EAC
		private void RepopulateLogs()
		{
			List<ConsoleLog> list = ((this.logLevelFilter.Count == this.logLevelCount) ? this.logs : this.filteredLogs);
			for (int i = 0; i < this.logLinePool.Count; i++)
			{
				if (list.Count - i - 1 - this.scrollState < 0)
				{
					this.logLinePool[this.logLinePool.Count - i - 1].gameObject.SetActive(false);
				}
				else if (this.logLinePool.Count - i - 1 >= 0)
				{
					this.logLinePool[this.logLinePool.Count - i - 1].gameObject.SetActive(true);
					this.logLinePool[this.logLinePool.Count - i - 1].PopulateLine(list[list.Count - i - 1 - this.scrollState]);
				}
			}
		}

		// Token: 0x04002CBC RID: 11452
		public static readonly global::plog.Logger Log = new global::plog.Logger("Console");

		// Token: 0x04002CBE RID: 11454
		public bool pinned;

		// Token: 0x04002CBF RID: 11455
		public bool consoleOpen;

		// Token: 0x04002CC0 RID: 11456
		public List<ConsoleLog> logs = new List<ConsoleLog>();

		// Token: 0x04002CC1 RID: 11457
		public readonly HashSet<Level> logLevelFilter = new HashSet<Level>(Enum.GetValues(typeof(Level)).Cast<Level>());

		// Token: 0x04002CC2 RID: 11458
		private int logLevelCount = Enum.GetValues(typeof(Level)).Length;

		// Token: 0x04002CC3 RID: 11459
		public int errorCount;

		// Token: 0x04002CC4 RID: 11460
		public int warningCount;

		// Token: 0x04002CC5 RID: 11461
		public int infoCount;

		// Token: 0x04002CC6 RID: 11462
		private readonly List<LogLine> logLinePool = new List<LogLine>();

		// Token: 0x04002CC7 RID: 11463
		[SerializeField]
		private GameObject consoleContainer;

		// Token: 0x04002CC8 RID: 11464
		[SerializeField]
		private CanvasGroup consoleBlocker;

		// Token: 0x04002CC9 RID: 11465
		[SerializeField]
		private TMP_InputField consoleInput;

		// Token: 0x04002CCA RID: 11466
		[Space]
		[SerializeField]
		private LogLine logLine;

		// Token: 0x04002CCB RID: 11467
		[SerializeField]
		private GameObject logContainer;

		// Token: 0x04002CCC RID: 11468
		[Space]
		[SerializeField]
		private GameObject scroller;

		// Token: 0x04002CCD RID: 11469
		[SerializeField]
		private TMP_Text scrollText;

		// Token: 0x04002CCE RID: 11470
		[SerializeField]
		private TMP_Text openBindText;

		// Token: 0x04002CCF RID: 11471
		[SerializeField]
		private Console.AutocompletePanel[] autocompletePanels;

		// Token: 0x04002CD0 RID: 11472
		[Space]
		public ErrorBadge errorBadge;

		// Token: 0x04002CD1 RID: 11473
		[Space]
		[SerializeField]
		private GameObject[] hideOnPin;

		// Token: 0x04002CD2 RID: 11474
		[SerializeField]
		private GameObject[] hideOnPinNoReopen;

		// Token: 0x04002CD3 RID: 11475
		[SerializeField]
		private Image[] backgrounds;

		// Token: 0x04002CD4 RID: 11476
		[SerializeField]
		private CanvasGroup masterGroup;

		// Token: 0x04002CD5 RID: 11477
		[Space]
		public ConsoleWindow consoleWindow;

		// Token: 0x04002CD6 RID: 11478
		private const int MaxLogLines = 20;

		// Token: 0x04002CD7 RID: 11479
		private bool openedDuringPause;

		// Token: 0x04002CD8 RID: 11480
		private OptionsManager rememberedOptionsManager;

		// Token: 0x04002CD9 RID: 11481
		public readonly Dictionary<string, ICommand> recognizedCommands = new Dictionary<string, ICommand>();

		// Token: 0x04002CDA RID: 11482
		public readonly HashSet<Type> registeredCommandTypes = new HashSet<Type>();

		// Token: 0x04002CDB RID: 11483
		private bool logsDirty;

		// Token: 0x04002CDC RID: 11484
		private int scrollState;

		// Token: 0x04002CDD RID: 11485
		private UnscaledTimeSince timeSincePgHeld;

		// Token: 0x04002CDE RID: 11486
		private UnscaledTimeSince timeSinceScrollTick;

		// Token: 0x04002CDF RID: 11487
		private List<string> commandHistory = new List<string>();

		// Token: 0x04002CE0 RID: 11488
		private int commandHistoryIndex = -1;

		// Token: 0x04002CE1 RID: 11489
		public Action onError;

		// Token: 0x04002CE2 RID: 11490
		public Binds binds;

		// Token: 0x04002CE3 RID: 11491
		private List<string> suggestions = new List<string>();

		// Token: 0x04002CE4 RID: 11492
		private int selectedSuggestionIndex;

		// Token: 0x04002CE5 RID: 11493
		private int suggestionStartIndex;

		// Token: 0x04002CE6 RID: 11494
		private PconAdapter pconAdapter = new PconAdapter();

		// Token: 0x04002CE7 RID: 11495
		private UnityProxy unityProxyHandler = new UnityProxy();

		// Token: 0x020005A9 RID: 1449
		[Serializable]
		public class AutocompletePanel
		{
			// Token: 0x04002CE8 RID: 11496
			public TMP_Text text;

			// Token: 0x04002CE9 RID: 11497
			public Image background;
		}
	}
}
