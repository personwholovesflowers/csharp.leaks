using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
public class UciChessEngine
{
	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06001D13 RID: 7443 RVA: 0x000F3DBD File Offset: 0x000F1FBD
	private static string EngineDirectory
	{
		get
		{
			return Path.Combine(Application.streamingAssetsPath, "ChessEngine");
		}
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000F3DD0 File Offset: 0x000F1FD0
	public UciChessEngine()
	{
		string text = this.FindExecutableInDirectory(UciChessEngine.EngineDirectory);
		this.engineProcess = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = text,
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			}
		};
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000F3E28 File Offset: 0x000F2028
	private string FindExecutableInDirectory(string directoryPath)
	{
		string text;
		try
		{
			string[] files = Directory.GetFiles(directoryPath, "*.exe");
			if (files.Length != 0)
			{
				text = files[0];
			}
			else
			{
				text = null;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Error finding executable: " + ex.Message);
			text = null;
		}
		return text;
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x000F3E7C File Offset: 0x000F207C
	public async Task InitializeUciModeAsync(bool whiteIsBot, int elo)
	{
		this.engineProcess.Start();
		await Task.Delay(1);
		await this.SendCommandAsync("uci");
		string text;
		do
		{
			text = await this.ReadResponseAsync();
		}
		while (!text.StartsWith("uciok"));
		await this.SendCommandAsync("isready");
		do
		{
			text = await this.ReadResponseAsync();
		}
		while (text != "readyok");
		if (elo < 1500)
		{
			await this.SetEloRatingAsync(500);
		}
		else
		{
			await this.SetEloRatingAsync(elo);
		}
		await this.SendCommandAsync("ucinewgame");
		await this.SendCommandAsync("position startpos");
		if (whiteIsBot)
		{
			MonoSingleton<ChessManager>.Instance.BotStartGame();
		}
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000F3ED0 File Offset: 0x000F20D0
	public async Task SendPlayerMoveAndGetEngineResponseAsync(string moves, Action<string> callback, int moveTimeInMilliseconds = 2000)
	{
		await this.SendCommandAsync("position startpos moves " + moves);
		await this.SendCommandAsync("go movetime " + moveTimeInMilliseconds.ToString());
		string text;
		do
		{
			text = await this.ReadResponseAsync();
		}
		while (!text.StartsWith("bestmove"));
		callback(text);
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000F3F2C File Offset: 0x000F212C
	public async Task SetEloRatingAsync(int eloRating)
	{
		if (eloRating < 0 || eloRating > 3200)
		{
			Debug.LogError("Elo rating must be between 0 and 3200.");
		}
		await this.SendCommandAsync("setoption name UCI_LimitStrength value true");
		await this.SendCommandAsync(string.Format("setoption name UCI_Elo value {0}", eloRating));
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x000F3F78 File Offset: 0x000F2178
	public async Task SendCommandAsync(string command)
	{
		await this.engineProcess.StandardInput.WriteLineAsync(command);
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000F3FC4 File Offset: 0x000F21C4
	public async Task<string> ReadResponseAsync()
	{
		return await this.engineProcess.StandardOutput.ReadLineAsync();
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000F4008 File Offset: 0x000F2208
	public async Task StopEngine()
	{
		await this.SendCommandAsync("quit");
		if (!this.engineProcess.HasExited)
		{
			this.engineProcess.Close();
		}
	}

	// Token: 0x04002935 RID: 10549
	private Process engineProcess;
}
