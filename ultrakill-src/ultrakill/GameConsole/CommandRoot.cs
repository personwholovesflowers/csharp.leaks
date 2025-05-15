using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GameConsole.CommandTree;

namespace GameConsole
{
	// Token: 0x020005A2 RID: 1442
	public abstract class CommandRoot : ICommand
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06002050 RID: 8272
		public abstract string Name { get; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06002051 RID: 8273
		public abstract string Description { get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06002052 RID: 8274 RVA: 0x00104EFB File Offset: 0x001030FB
		public string Command
		{
			get
			{
				return this.root.name;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06002053 RID: 8275 RVA: 0x00104F08 File Offset: 0x00103108
		public Branch Root
		{
			get
			{
				return this.root;
			}
		}

		// Token: 0x06002054 RID: 8276
		protected abstract Branch BuildTree(Console con);

		// Token: 0x06002055 RID: 8277 RVA: 0x00104F10 File Offset: 0x00103110
		public CommandRoot(Console con)
		{
			this.root = this.BuildTree(con);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00104F28 File Offset: 0x00103128
		public void Execute(Console con, string[] args)
		{
			Queue<string> queue = new Queue<string>(args.Where((string arg) => arg != ""));
			ValueTuple<string, Branch> valueTuple = this.FindLongestMatchingBranch(this.root, queue, con, null);
			string item = valueTuple.Item1;
			Branch item2 = valueTuple.Item2;
			if (item == null || item2 == null)
			{
				return;
			}
			if (!this.TryFindCorrectLeaf(item, item2, queue, con))
			{
				this.PrintUsage(con, item, item2);
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x00104F98 File Offset: 0x00103198
		private bool TryFindCorrectLeaf(string soFar, Branch branch, Queue<string> args, Console con)
		{
			bool flag = false;
			foreach (Node node in branch.children)
			{
				if (!(node is Branch))
				{
					if (node.requireCheats && con.CheatBlocker())
					{
						return true;
					}
					Leaf leaf = node as Leaf;
					ParameterInfo[] parameters = leaf.onExecute.Method.GetParameters();
					Queue<string> queue = new Queue<string>(args);
					if (args.Count == parameters.Length)
					{
						Dictionary<ParameterInfo, object> dictionary = new Dictionary<ParameterInfo, object>();
						try
						{
							foreach (ParameterInfo parameterInfo in parameters)
							{
								Type parameterType = parameterInfo.ParameterType;
								string text = queue.Dequeue();
								if (parameterType == typeof(bool))
								{
									dictionary[parameterInfo] = bool.Parse(text);
								}
								else if (parameterType == typeof(int))
								{
									dictionary[parameterInfo] = int.Parse(text);
								}
								else if (parameterType == typeof(float))
								{
									dictionary[parameterInfo] = float.Parse(text);
								}
								else if (parameterType == typeof(string))
								{
									dictionary[parameterInfo] = text;
								}
								else
								{
									if (parameterType == typeof(string[]))
									{
										List<string> list = new List<string> { text };
										list.AddRange(args);
										dictionary[parameterInfo] = list.ToArray();
										break;
									}
									if (!parameterType.IsSubclassOf(typeof(Enum)))
									{
										throw new ArgumentException(string.Format("{0} has an unsupported parameter type: {1}", soFar, parameterType));
									}
									dictionary[parameterInfo] = Enum.Parse(parameterType, text);
								}
							}
						}
						catch (FormatException)
						{
							dictionary.Clear();
							goto IL_01ED;
						}
						Delegate onExecute = leaf.onExecute;
						if (onExecute != null)
						{
							onExecute.DynamicInvoke(dictionary.Values.ToArray<object>());
						}
						flag = true;
						break;
					}
				}
				IL_01ED:;
			}
			return flag;
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x001051BC File Offset: 0x001033BC
		private void PrintUsage(Console con, string soFar, Branch branch)
		{
			Console.Log.Info("Usage: " + soFar + " <subcommand>", null, null, null);
			Console.Log.Info("Subcommands:", null, null, null);
			foreach (Node node in branch.children)
			{
				Branch branch2 = node as Branch;
				if (branch2 != null)
				{
					Console.Log.Info("- " + soFar + " " + branch2.name, null, null, null);
				}
				Leaf leaf = node as Leaf;
				if (leaf != null)
				{
					Console.Log.Info(leaf.onExecute.Method.GetParameters().Aggregate("- " + soFar, (string acc, ParameterInfo param) => string.Format("{0} <{1}: <color=grey>{2}</color>>", acc, param.Name, param.ParameterType)), null, null, null);
				}
			}
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00105298 File Offset: 0x00103498
		[return: TupleElementNames(new string[] { "soFar", "branch" })]
		public ValueTuple<string, Branch> FindLongestMatchingBranch(Branch root, Queue<string> args, Console con = null, Func<Branch, string, bool> matches = null)
		{
			string text = root.name;
			Branch branch = root;
			bool flag = false;
			while (!flag)
			{
				flag = true;
				if (args.Count == 0)
				{
					break;
				}
				Node[] children = branch.children;
				int i = 0;
				while (i < children.Length)
				{
					Node node = children[i];
					Branch branch2 = node as Branch;
					if (branch2 != null && ((matches != null) ? matches(branch2, args.Peek()) : (matches == null && branch2.name == args.Peek())))
					{
						if (con != null && node.requireCheats && con.CheatBlocker())
						{
							return new ValueTuple<string, Branch>(null, null);
						}
						text = text + " " + args.Dequeue();
						branch = branch2;
						flag = false;
						break;
					}
					else
					{
						i++;
					}
				}
			}
			return new ValueTuple<string, Branch>(text, branch);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x0010536C File Offset: 0x0010356C
		public static Branch Branch(string name, params Node[] children)
		{
			return new Branch(name, children);
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00105375 File Offset: 0x00103575
		public static Branch Branch(string name, bool requireCheats, params Node[] children)
		{
			return new Branch(name, requireCheats, children);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0010537F File Offset: 0x0010357F
		public static Branch Leaf(string name, Action onExecute, bool requireCheats = false)
		{
			return new Branch(name, requireCheats, new Node[]
			{
				new Leaf(onExecute, requireCheats)
			});
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00105398 File Offset: 0x00103598
		public static Leaf Leaf(Action onExecute, bool requireCheats = false)
		{
			return new Leaf(onExecute, requireCheats);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x0010537F File Offset: 0x0010357F
		public static Branch Leaf<T>(string name, Action<T> onExecute, bool requireCheats = false)
		{
			return new Branch(name, requireCheats, new Node[]
			{
				new Leaf(onExecute, requireCheats)
			});
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00105398 File Offset: 0x00103598
		public static Leaf Leaf<T>(Action<T> onExecute, bool requireCheats = false)
		{
			return new Leaf(onExecute, requireCheats);
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x001053A1 File Offset: 0x001035A1
		public static Branch Leaf<T, U>(string name, Action<T, U> onExecute, bool requireCheats = false)
		{
			return new Branch(name, new Node[]
			{
				new Leaf(onExecute, requireCheats)
			});
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00105398 File Offset: 0x00103598
		public static Leaf Leaf<T, U>(Action<T, U> onExecute, bool requireCheats = false)
		{
			return new Leaf(onExecute, requireCheats);
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x001053A1 File Offset: 0x001035A1
		public static Branch Leaf<T, U, V>(string name, Action<T, U, V> onExecute, bool requireCheats = false)
		{
			return new Branch(name, new Node[]
			{
				new Leaf(onExecute, requireCheats)
			});
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00105398 File Offset: 0x00103598
		public static Leaf Leaf<T, U, V>(Action<T, U, V> onExecute, bool requireCheats = false)
		{
			return new Leaf(onExecute, requireCheats);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x001053BC File Offset: 0x001035BC
		public Branch BuildPrefsEditor(List<CommandRoot.PrefReference> pref)
		{
			return CommandRoot.Leaf("prefs", delegate
			{
				Console.Log.Info("Available prefs:", null, null, null);
				foreach (CommandRoot.PrefReference prefReference in pref)
				{
					string text = (prefReference.Local ? "<color=red>LOCAL</color>" : string.Empty);
					if (prefReference.Type == typeof(int))
					{
						string text2;
						if (!MonoSingleton<PrefsManager>.Instance.HasKey(prefReference.Key))
						{
							text2 = (string.IsNullOrEmpty(prefReference.Default) ? "<color=red>NOT SET</color>" : prefReference.Default);
						}
						else
						{
							text2 = (prefReference.Local ? MonoSingleton<PrefsManager>.Instance.GetIntLocal(prefReference.Key, 0) : MonoSingleton<PrefsManager>.Instance.GetInt(prefReference.Key, 0)).ToString();
						}
						Console.Log.Info(string.Concat(new string[] { "- <color=#db872c>", prefReference.Key, "</color>: <color=#4ac246>", text2, "</color>   [<color=#879fff>int</color>] ", text }), null, null, null);
					}
					else if (prefReference.Type == typeof(float))
					{
						string text3;
						if (!MonoSingleton<PrefsManager>.Instance.HasKey(prefReference.Key))
						{
							text3 = (string.IsNullOrEmpty(prefReference.Default) ? "<color=red>NOT SET</color>" : prefReference.Default);
						}
						else
						{
							text3 = (prefReference.Local ? MonoSingleton<PrefsManager>.Instance.GetFloatLocal(prefReference.Key, 0f) : MonoSingleton<PrefsManager>.Instance.GetFloat(prefReference.Key, 0f)).ToString(CultureInfo.InvariantCulture);
						}
						Console.Log.Info(string.Concat(new string[] { "- <color=#db872c>", prefReference.Key, "</color>: <color=#4ac246>", text3, "</color>   [<color=#879fff>float</color>] ", text }), null, null, null);
					}
					else if (prefReference.Type == typeof(bool))
					{
						string text4;
						if (!MonoSingleton<PrefsManager>.Instance.HasKey(prefReference.Key))
						{
							text4 = (string.IsNullOrEmpty(prefReference.Default) ? "<color=red>NOT SET</color>" : prefReference.Default);
						}
						else
						{
							text4 = ((prefReference.Local ? MonoSingleton<PrefsManager>.Instance.GetBoolLocal(prefReference.Key, false) : MonoSingleton<PrefsManager>.Instance.GetBool(prefReference.Key, false)) ? "True" : "False");
						}
						Console.Log.Info(string.Concat(new string[] { "- <color=#db872c>", prefReference.Key, "</color>: <color=#4ac246>", text4, "</color>   [<color=#879fff>float</color>] ", text }), null, null, null);
					}
					else if (prefReference.Type == typeof(string))
					{
						string text5 = (prefReference.Local ? MonoSingleton<PrefsManager>.Instance.GetStringLocal(prefReference.Key, null) : MonoSingleton<PrefsManager>.Instance.GetString(prefReference.Key, null));
						Console.Log.Info(string.Concat(new string[]
						{
							"- <color=#db872c>",
							prefReference.Key,
							"</color>: <color=#4ac246>\"",
							string.IsNullOrEmpty(text5) ? prefReference.Default : text5,
							"\"</color>   [<color=#879fff>float</color>] ",
							text
						}), null, null, null);
					}
					else
					{
						Console.Log.Info(string.Concat(new string[]
						{
							"Pref ",
							prefReference.Key,
							" is type ",
							prefReference.Type.Name,
							" (Unrecognized)"
						}), null, null, null);
					}
				}
				Console.Log.Info("You can use `<color=#7df59d>prefs set <type> <value></color>` to change a pref", null, null, null);
				Console.Log.Info("or `<color=#7df59d>prefs set_local <type> <value></color>` to change a <color=#db872c>local</color> pref. (it matters)", null, null, null);
			}, false);
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x001053F0 File Offset: 0x001035F0
		public Branch BoolMenu(string commandKey, Func<bool> valueGetter, Action<bool> valueSetter, bool inverted = false, bool requireCheats = false)
		{
			return CommandRoot.Branch(commandKey, requireCheats, new Node[]
			{
				CommandRoot.Leaf("toggle", delegate
				{
					bool flag = !valueGetter();
					valueSetter(flag);
					Console.Log.Info(string.Concat(new string[]
					{
						"<color=#db872c>",
						commandKey,
						"</color> is now <color=#4ac246>",
						this.GetStateName(flag, inverted),
						"</color>"
					}), null, null, null);
				}, false),
				CommandRoot.Leaf("on", delegate
				{
					valueSetter(!inverted);
					Console.Log.Info(string.Concat(new string[]
					{
						"<color=#db872c>",
						commandKey,
						"</color> is now <color=#4ac246>",
						this.GetStateName(!inverted, inverted),
						"</color>"
					}), null, null, null);
				}, false),
				CommandRoot.Leaf("off", delegate
				{
					valueSetter(inverted);
					Console.Log.Info(string.Concat(new string[]
					{
						"<color=#db872c>",
						commandKey,
						"</color> is now <color=#4ac246>",
						this.GetStateName(inverted, inverted),
						"</color>"
					}), null, null, null);
				}, false),
				CommandRoot.Leaf("read", delegate
				{
					Console.Log.Info("The current value is <color=#4ac246>" + this.GetStateName(valueGetter(), inverted) + "</color>", null, null, null);
				}, false)
			});
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x001054A2 File Offset: 0x001036A2
		private string GetStateName(bool value, bool inverted)
		{
			if (!inverted)
			{
				if (!value)
				{
					return "off";
				}
				return "on";
			}
			else
			{
				if (!value)
				{
					return "on";
				}
				return "off";
			}
		}

		// Token: 0x04002CAB RID: 11435
		private Branch root;

		// Token: 0x04002CAC RID: 11436
		private const string KeyColor = "#db872c";

		// Token: 0x04002CAD RID: 11437
		private const string TypeColor = "#879fff";

		// Token: 0x04002CAE RID: 11438
		private const string ValueColor = "#4ac246";

		// Token: 0x020005A3 RID: 1443
		// (Invoke) Token: 0x06002068 RID: 8296
		private delegate bool ParseMyThing(string s, out object result);

		// Token: 0x020005A4 RID: 1444
		public class PrefReference
		{
			// Token: 0x04002CAF RID: 11439
			public string Key;

			// Token: 0x04002CB0 RID: 11440
			public Type Type;

			// Token: 0x04002CB1 RID: 11441
			public bool Local;

			// Token: 0x04002CB2 RID: 11442
			public string Default;
		}
	}
}
