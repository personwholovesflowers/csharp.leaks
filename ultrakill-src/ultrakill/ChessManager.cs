using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ULTRAKILL.Cheats;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020004E5 RID: 1253
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class ChessManager : MonoSingleton<ChessManager>
{
	// Token: 0x06001CB9 RID: 7353 RVA: 0x000F1008 File Offset: 0x000EF208
	private new void Awake()
	{
		this.gameLocked = true;
		this.colBounds = base.GetComponent<Collider>().bounds;
		this.colorSetter = new MaterialPropertyBlock();
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				Renderer component = this.helperTileGroup.GetChild(i).GetChild(j).GetComponent<Renderer>();
				component.SetPropertyBlock(this.colorSetter);
				this.helperTiles[i + j * 8] = component;
			}
		}
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x000F1081 File Offset: 0x000EF281
	private void Start()
	{
		this.ResetBoard();
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x000F108C File Offset: 0x000EF28C
	public void SetupNewGame()
	{
		base.StopAllCoroutines();
		this.ResetBoard();
		this.gameLocked = false;
		if (!this.whiteIsBot || !this.blackIsBot)
		{
			SummonSandboxArm cheatInstance = MonoSingleton<CheatsManager>.Instance.GetCheatInstance<SummonSandboxArm>();
			if (cheatInstance != null)
			{
				cheatInstance.TryCreateArmType(SpawnableType.MoveHand);
			}
			if (!this.tutorialMessageSent)
			{
				MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Chess pieces can be moved with the <color=orange>mover arm</color>.", "", "", 0, false, false, true);
				this.tutorialMessageSent = true;
			}
		}
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000F10FE File Offset: 0x000EF2FE
	public void ToggleWhiteBot(bool isBot)
	{
		this.whiteIsBot = isBot;
		this.whiteOpponent.SetActive(this.whiteIsBot);
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000F1118 File Offset: 0x000EF318
	public void ToggleBlackBot(bool isBot)
	{
		this.blackIsBot = isBot;
		this.blackOpponent.SetActive(this.blackIsBot);
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x000F1134 File Offset: 0x000EF334
	public void ResetBoard()
	{
		this.numMoves = 0;
		this.blackWinner.SetActive(false);
		this.whiteWinner.SetActive(false);
		this.HideHelperTiles();
		this.UCIMoves.Clear();
		if (this.clonedPieces != null)
		{
			Object.Destroy(this.clonedPieces);
		}
		foreach (ChessPiece chessPiece in this.allPieces.Values)
		{
			if (chessPiece != null)
			{
				Object.Destroy(chessPiece.gameObject);
			}
		}
		this.clonedPieces = null;
		this.clonedPieces = Object.Instantiate<GameObject>(this.originalPieces, base.transform.parent);
		this.clonedPieces.SetActive(true);
		this.originalPieces.SetActive(false);
		this.allPieces.Clear();
		for (int i = 0; i < this.chessBoard.Length; i++)
		{
			this.chessBoard[i] = null;
		}
		this.isWhiteTurn = true;
		this.whiteOpponent.SetActive(this.whiteIsBot);
		this.blackOpponent.SetActive(this.blackIsBot);
		if (this.whiteIsBot || this.blackIsBot)
		{
			this.StartEngine();
		}
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x000F1284 File Offset: 0x000EF484
	public void UpdateGame(ChessManager.MoveData move)
	{
		this.gameLocked = false;
		string text = ChessStringHandler.UCIMove(move);
		if (this.UCIMoves.Count > 0 && this.UCIMoves[this.UCIMoves.Count - 1].Equals(text))
		{
			Debug.LogError("tried to perform the same move twice");
			return;
		}
		this.UCIMoves.Add(text);
		if (this.UCIMoves.Count == 3 && this.UCIMoves[0] == "e2e4" && this.UCIMoves[1] == "e7e5" && this.UCIMoves[2] == "e1e2")
		{
			MonoSingleton<StyleHUD>.Instance.AddPoints(420, "<color=green>BONGCLOUD</color>", null, null, -1, "", "");
		}
		string text2 = string.Join(" ", this.UCIMoves);
		if (this.isWhiteTurn)
		{
			this.numMoves++;
		}
		this.isWhiteTurn = !this.isWhiteTurn;
		if (this.IsGameOver())
		{
			if (this.numMoves == 2)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(1, "<color=red>FOOLS MATE</color>", null, null, -1, "", "");
			}
			return;
		}
		if ((this.isWhiteTurn && this.whiteIsBot) || (!this.isWhiteTurn && this.blackIsBot))
		{
			base.StartCoroutine(this.SendToBotCoroutine(text2));
		}
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x000F13F0 File Offset: 0x000EF5F0
	private bool IsGameOver()
	{
		if (!this.IsSufficientMaterial())
		{
			this.WinTrigger(null);
			return true;
		}
		this.allLegalMoves.Clear();
		for (int i = 0; i < this.chessBoard.Length; i++)
		{
			ChessPieceData chessPieceData = this.chessBoard[i];
			if (chessPieceData != null && chessPieceData.isWhite == this.isWhiteTurn)
			{
				this.GetLegalMoves(new int2(i % 8, i / 8));
				this.allLegalMoves.AddRange(this.legalMoves);
			}
		}
		if (this.allLegalMoves.Count == 0)
		{
			if (this.IsSquareAttacked(this.GetPiecePos(this.isWhiteTurn ? this.whiteKing : this.blackKing), this.isWhiteTurn))
			{
				this.WinTrigger(new bool?(!this.isWhiteTurn));
			}
			else
			{
				this.WinTrigger(null);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x000F14D0 File Offset: 0x000EF6D0
	public bool IsSufficientMaterial()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		bool flag2 = false;
		int2? @int = null;
		int2? int2 = null;
		for (int i = 0; i < this.chessBoard.Length; i++)
		{
			ChessPieceData chessPieceData = this.chessBoard[i];
			if (chessPieceData != null && chessPieceData.type != ChessPieceData.PieceType.King)
			{
				int2 int3 = new int2(i % 8, i / 8);
				if (chessPieceData.isWhite)
				{
					num++;
					if (chessPieceData.type == ChessPieceData.PieceType.Bishop)
					{
						flag = true;
						@int = new int2?(int3);
					}
				}
				else
				{
					num2++;
					if (chessPieceData.type == ChessPieceData.PieceType.Bishop)
					{
						flag2 = true;
						int2 = new int2?(int3);
					}
				}
				if (num > 1 || num2 > 1 || chessPieceData.type == ChessPieceData.PieceType.Pawn || chessPieceData.type == ChessPieceData.PieceType.Rook || chessPieceData.type == ChessPieceData.PieceType.Queen)
				{
					return true;
				}
			}
		}
		return flag && flag2 && (@int.Value.x + @int.Value.y) % 2 != (int2.Value.x + int2.Value.y) % 2;
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x000F15E0 File Offset: 0x000EF7E0
	public void WinTrigger(bool? whiteWin)
	{
		this.gameLocked = true;
		this.StopEngine();
		if (whiteWin == null)
		{
			this.draw.GetComponent<AudioSource>().Play();
			return;
		}
		GameObject gameObject = (whiteWin.Value ? this.whiteWinner : this.blackWinner);
		gameObject.SetActive(true);
		AudioSource[] components = gameObject.GetComponents<AudioSource>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].Play();
		}
		gameObject.GetComponent<ParticleSystem>().Play();
		bool? flag = whiteWin;
		bool flag2 = true;
		if (!((flag.GetValueOrDefault() == flag2) & (flag != null)) || this.whiteIsBot)
		{
			flag = whiteWin;
			flag2 = false;
			if (!((flag.GetValueOrDefault() == flag2) & (flag != null)) || this.blackIsBot)
			{
				goto IL_0103;
			}
		}
		StyleHUD instance = MonoSingleton<StyleHUD>.Instance;
		int num = 5000;
		string text = "<color=orange>";
		flag = whiteWin;
		flag2 = true;
		instance.AddPoints(num, text + (((flag.GetValueOrDefault() == flag2) & (flag != null)) ? "WHITE" : "BLACK") + " WINS</color>", null, null, -1, "", "");
		IL_0103:
		flag = whiteWin;
		flag2 = true;
		if (!((flag.GetValueOrDefault() == flag2) & (flag != null)) || this.whiteIsBot || !this.blackIsBot)
		{
			flag = whiteWin;
			flag2 = false;
			if (!((flag.GetValueOrDefault() == flag2) & (flag != null)) || this.blackIsBot || !this.whiteIsBot)
			{
				return;
			}
		}
		MonoSingleton<StyleHUD>.Instance.AddPoints(5000, "<color=red>ULTRAVICTORY</color>", null, null, -1, "", "");
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x000F1765 File Offset: 0x000EF965
	public void SetElo(float newElo)
	{
		this.elo = Mathf.FloorToInt(newElo);
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x000F1773 File Offset: 0x000EF973
	public void WhiteIsBot(bool isBot)
	{
		this.whiteIsBot = isBot;
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x000F177C File Offset: 0x000EF97C
	public void BlackIsBot(bool isBot)
	{
		this.blackIsBot = isBot;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x000F1788 File Offset: 0x000EF988
	private async void StartEngine()
	{
		this.chessEngine = new UciChessEngine();
		await this.chessEngine.InitializeUciModeAsync(this.whiteIsBot, this.elo);
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x000F17C0 File Offset: 0x000EF9C0
	public async void StopEngine()
	{
		if (this.chessEngine != null)
		{
			await this.chessEngine.StopEngine();
			this.chessEngine = null;
		}
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x000F17F7 File Offset: 0x000EF9F7
	public void BotStartGame()
	{
		base.StartCoroutine(this.SendToBotCoroutine(""));
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x000F180B File Offset: 0x000EFA0B
	private IEnumerator SendToBotCoroutine(string newMoveData)
	{
		ChessManager.<>c__DisplayClass54_0 CS$<>8__locals1 = new ChessManager.<>c__DisplayClass54_0();
		CS$<>8__locals1.isResponseReceived = false;
		CS$<>8__locals1.response = "";
		if (this.elo < 1500)
		{
			int num = this.elo - 1000;
			this.chessEngine.SendPlayerMoveAndGetEngineResponseAsync(newMoveData, new Action<string>(CS$<>8__locals1.<SendToBotCoroutine>g__onReceivedResponse|0), 250 + num);
		}
		else
		{
			this.chessEngine.SendPlayerMoveAndGetEngineResponseAsync(newMoveData, new Action<string>(CS$<>8__locals1.<SendToBotCoroutine>g__onReceivedResponse|0), 2000);
		}
		yield return new WaitUntil(() => CS$<>8__locals1.isResponseReceived);
		if (CS$<>8__locals1.response.StartsWith("bestmove"))
		{
			string text = this.ParseBotMove(CS$<>8__locals1.response);
			this.MakeBotMove(text);
		}
		yield break;
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x000F1824 File Offset: 0x000EFA24
	private string ParseBotMove(string engineResponse)
	{
		string[] array = engineResponse.Split(' ', StringSplitOptions.None);
		if (array.Length >= 2)
		{
			return array[1];
		}
		return string.Empty;
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x000F184A File Offset: 0x000EFA4A
	private IEnumerator LerpBotMove(ChessPiece piece, int2 endIndex, ChessManager.MoveData moveData)
	{
		Transform trans = piece.transform;
		Vector3 startPos = trans.position;
		Vector3 endPos = this.IndexToWorldPosition(endIndex, piece.boardHeight);
		float duration = global::UnityEngine.Random.Range(0.5f, 1f);
		float elapsed = 0f;
		if (global::UnityEngine.Random.Range(0, 1000) == 666)
		{
			duration = 15f;
		}
		piece.dragSound.pitch = global::UnityEngine.Random.Range(0.75f, 1.25f);
		piece.dragSound.Play();
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float num = Mathf.Clamp01(elapsed / duration);
			trans.position = Vector3.Lerp(startPos, endPos, num);
			yield return null;
		}
		piece.dragSound.Stop();
		Object.Instantiate<AudioSource>(piece.snapSound, piece.transform.position, Quaternion.identity);
		yield return null;
		this.MakeMove(moveData, true);
		yield return null;
		yield break;
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x000F1870 File Offset: 0x000EFA70
	private void MakeBotMove(string botMove)
	{
		ValueTuple<int2, int2, ChessPieceData.PieceType> valueTuple = ChessStringHandler.ProcessFullMove(botMove);
		int2 item = valueTuple.Item1;
		int2 endPos = valueTuple.Item2;
		ChessPieceData.PieceType promotionType = valueTuple.Item3;
		ChessPieceData pieceAt = this.GetPieceAt(item);
		if (pieceAt == null)
		{
			Debug.LogError("found no piece for move " + botMove);
		}
		this.GetLegalMoves(item);
		ChessManager.MoveData moveData = this.legalMoves.FirstOrDefault((ChessManager.MoveData move) => move.EndPosition.Equals(endPos) && move.PromotionType == promotionType);
		if (moveData.EndPosition.Equals(endPos))
		{
			ChessPiece chessPiece = this.allPieces[pieceAt];
			base.StartCoroutine(this.LerpBotMove(chessPiece, endPos, moveData));
		}
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x000F1920 File Offset: 0x000EFB20
	public int2 WorldPositionToIndex(Vector3 pos)
	{
		Vector3 min = this.colBounds.min;
		Vector3 max = this.colBounds.max;
		Vector3 vector = new Vector3((pos.x - min.x) / (max.x - min.x), 0f, (pos.z - min.z) / (max.z - min.z));
		int num = Mathf.FloorToInt(vector.x * 8f);
		int num2 = Mathf.FloorToInt(vector.z * 8f);
		return new int2(num, num2);
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x000F19B0 File Offset: 0x000EFBB0
	public Vector3 IndexToWorldPosition(int2 index, float height)
	{
		Vector3 min = this.colBounds.min;
		Vector3 max = this.colBounds.max;
		float num = (float)Mathf.Clamp(index.x, 0, 7) + 0.5f;
		float num2 = (float)Mathf.Clamp(index.y, 0, 7) + 0.5f;
		return new Vector3(min.x + num * (max.x - min.x) / 8f, height, min.z + num2 * (max.z - min.z) / 8f);
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x000F1A40 File Offset: 0x000EFC40
	public void DisplayValidMoves()
	{
		foreach (ChessManager.MoveData moveData in this.legalMoves)
		{
			int x = moveData.EndPosition.x;
			int y = moveData.EndPosition.y;
			if (x >= 0 && x < 8 && y >= 0 && y < 8)
			{
				Renderer renderer = this.helperTiles[x + y * 8];
				this.colorSetter.SetColor("_TintColor", (moveData.CapturePiece != null) ? Color.green : Color.cyan);
				renderer.SetPropertyBlock(this.colorSetter);
			}
			else
			{
				Debug.LogError("Trying to display a move out of range");
			}
		}
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x000F1AFC File Offset: 0x000EFCFC
	public void HideHelperTiles()
	{
		this.colorSetter.SetColor("_TintColor", Color.clear);
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				this.helperTiles[i + j * 8].SetPropertyBlock(this.colorSetter);
			}
		}
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x000F1B50 File Offset: 0x000EFD50
	public void FindMoveAtWorldPosition(ChessPiece chessPiece)
	{
		Vector3 position = chessPiece.transform.position;
		int2 tileID = this.WorldPositionToIndex(position);
		ChessPieceData data = chessPiece.Data;
		if (this.legalMoves.Count == 0)
		{
			chessPiece.UpdatePosition(this.GetPiecePos(data));
		}
		else
		{
			ChessManager.MoveData moveData = this.legalMoves.FirstOrDefault((ChessManager.MoveData move) => move.EndPosition.Equals(tileID));
			if (!moveData.EndPosition.Equals(tileID) || moveData.StartPosition.Equals(moveData.EndPosition))
			{
				chessPiece.UpdatePosition(this.GetPiecePos(data));
			}
			else
			{
				this.MakeMove(moveData, true);
			}
		}
		this.HideHelperTiles();
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x000F1BFC File Offset: 0x000EFDFC
	public void InitializePiece(ChessPiece piece)
	{
		ChessPieceData data = piece.Data;
		this.allPieces.Add(data, piece);
		Vector3 position = piece.transform.position;
		int2 @int = this.WorldPositionToIndex(position);
		if (data.type == ChessPieceData.PieceType.King)
		{
			if (piece.isWhite)
			{
				this.whiteKing = data;
			}
			else
			{
				this.blackKing = data;
			}
		}
		this.SetPieceAt(@int, data);
		piece.UpdatePosition(@int);
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x000F1C61 File Offset: 0x000EFE61
	public ChessPieceData GetPieceAt(int2 index)
	{
		return this.chessBoard[index.x + index.y * 8];
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x000F1C79 File Offset: 0x000EFE79
	public void SetPieceAt(int2 index, ChessPieceData piece)
	{
		this.chessBoard[index.x + index.y * 8] = piece;
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x000F1C94 File Offset: 0x000EFE94
	private int2 GetPiecePos(ChessPieceData piece)
	{
		int num = Array.IndexOf<ChessPieceData>(this.chessBoard, piece);
		return new int2(num % 8, num / 8);
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x000F1CBC File Offset: 0x000EFEBC
	public void MakeMove(ChessManager.MoveData moveData, bool updateVisuals = false)
	{
		ChessPieceData pieceToMove = moveData.PieceToMove;
		int2 endPosition = moveData.EndPosition;
		if (moveData.SpecialMove == ChessManager.SpecialMove.EnPassantCapture)
		{
			this.SetPieceAt(endPosition + new int2(0, pieceToMove.isWhite ? (-1) : 1), null);
		}
		if (moveData.SpecialMove == ChessManager.SpecialMove.PawnTwoStep)
		{
			this.enPassantPos = endPosition + new int2(0, pieceToMove.isWhite ? (-1) : 1);
		}
		else
		{
			this.enPassantPos = new int2(-1, -1);
		}
		pieceToMove.timesMoved++;
		this.SetPieceAt(moveData.StartPosition, null);
		this.SetPieceAt(endPosition, pieceToMove);
		if (moveData.SpecialMove == ChessManager.SpecialMove.ShortCastle || moveData.SpecialMove == ChessManager.SpecialMove.LongCastle)
		{
			int num = ((moveData.SpecialMove == ChessManager.SpecialMove.ShortCastle) ? 7 : 0);
			int num2 = ((moveData.SpecialMove == ChessManager.SpecialMove.ShortCastle) ? 5 : 3);
			int2 @int = new int2(num, pieceToMove.isWhite ? 0 : 7);
			int2 int2 = new int2(num2, pieceToMove.isWhite ? 0 : 7);
			ChessPieceData pieceAt = this.GetPieceAt(@int);
			pieceAt.timesMoved++;
			this.SetPieceAt(@int, null);
			this.SetPieceAt(int2, pieceAt);
			ChessPiece chessPiece;
			if (updateVisuals && this.allPieces.TryGetValue(pieceAt, out chessPiece))
			{
				chessPiece.UpdatePosition(int2);
			}
		}
		if (moveData.SpecialMove == ChessManager.SpecialMove.PawnPromotion)
		{
			pieceToMove.type = moveData.PromotionType;
			if (updateVisuals)
			{
				ChessPiece chessPiece2 = this.allPieces[pieceToMove];
				if (chessPiece2.autoControl)
				{
					chessPiece2.PromoteVisualPiece();
				}
				else
				{
					this.gameLocked = true;
					foreach (KeyValuePair<ChessPieceData, ChessPiece> keyValuePair in this.allPieces)
					{
						keyValuePair.Value.PieceCanMove(false);
					}
					chessPiece2.ShowPromotionGUI(moveData);
				}
			}
		}
		ChessPiece chessPiece3;
		if (updateVisuals && this.allPieces.TryGetValue(pieceToMove, out chessPiece3))
		{
			chessPiece3.UpdatePosition(endPosition);
			if (moveData.SpecialMove == ChessManager.SpecialMove.LongCastle || moveData.SpecialMove == ChessManager.SpecialMove.ShortCastle)
			{
				Object.Instantiate<GameObject>(chessPiece3.teleportEffect, chessPiece3.transform.position, Quaternion.identity);
			}
			if (moveData.SpecialMove == ChessManager.SpecialMove.PawnPromotion)
			{
				Object.Instantiate<GameObject>(chessPiece3.promotionEffect, chessPiece3.transform.position, Quaternion.identity);
			}
			if (!pieceToMove.autoControl && moveData.SpecialMove != ChessManager.SpecialMove.PawnPromotion)
			{
				this.StylishMove(moveData);
			}
		}
		ChessPiece chessPiece4;
		if (updateVisuals && moveData.CapturePiece != null && this.allPieces.TryGetValue(moveData.CapturePiece, out chessPiece4))
		{
			chessPiece4.Captured();
		}
		if (updateVisuals && (moveData.SpecialMove != ChessManager.SpecialMove.PawnPromotion || moveData.PieceToMove.autoControl))
		{
			this.UpdateGame(moveData);
		}
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x000F1F60 File Offset: 0x000F0160
	public void StylishMove(ChessManager.MoveData move)
	{
		if (move.SpecialMove == ChessManager.SpecialMove.LongCastle || move.SpecialMove == ChessManager.SpecialMove.ShortCastle)
		{
			MonoSingleton<StyleHUD>.Instance.AddPoints(50, "<color=#00ffffff>CASTLED</color>", null, null, -1, "", "");
		}
		if (move.SpecialMove == ChessManager.SpecialMove.PawnPromotion)
		{
			MonoSingleton<StyleHUD>.Instance.AddPoints(500, "<color=green>" + move.PromotionType.ToString().ToUpper() + " PROMOTION</color>", null, null, -1, "", "");
		}
		int num = 0;
		string text = "<color=white>";
		if (move.CapturePiece != null)
		{
			switch (move.CapturePiece.type)
			{
			case ChessPieceData.PieceType.Rook:
				num = 200;
				text = "<color=orange>";
				break;
			case ChessPieceData.PieceType.Knight:
				num = 100;
				text = "<color=green>";
				break;
			case ChessPieceData.PieceType.Bishop:
				num = 100;
				text = "<color=green>";
				break;
			case ChessPieceData.PieceType.Queen:
				num = 400;
				text = "<color=red>";
				break;
			}
			if (move.SpecialMove == ChessManager.SpecialMove.EnPassantCapture)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(100, "<color=#00ffffff>EN PASSANT</color>", null, null, -1, "", "");
				return;
			}
			MonoSingleton<StyleHUD>.Instance.AddPoints(100 + num, text + move.CapturePiece.type.ToString().ToUpper() + " CAPTURE</color>", null, null, -1, "", "");
		}
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x000F20BC File Offset: 0x000F02BC
	public void UnmakeMove(ChessManager.MoveData moveData, bool updateVisuals = false)
	{
		this.enPassantPos = moveData.LastEnPassantPos;
		ChessPieceData pieceToMove = moveData.PieceToMove;
		this.SetPieceAt(moveData.StartPosition, moveData.PieceToMove);
		int2 @int = moveData.EndPosition;
		if (moveData.SpecialMove == ChessManager.SpecialMove.EnPassantCapture)
		{
			this.SetPieceAt(@int, null);
			@int += new int2(0, pieceToMove.isWhite ? (-1) : 1);
		}
		this.SetPieceAt(@int, moveData.CapturePiece);
		pieceToMove.timesMoved--;
		if (moveData.SpecialMove == ChessManager.SpecialMove.ShortCastle || moveData.SpecialMove == ChessManager.SpecialMove.LongCastle)
		{
			int num = ((moveData.SpecialMove == ChessManager.SpecialMove.ShortCastle) ? 7 : 0);
			int num2 = ((moveData.SpecialMove == ChessManager.SpecialMove.ShortCastle) ? 5 : 3);
			int2 int2 = new int2(num, pieceToMove.isWhite ? 0 : 7);
			int2 int3 = new int2(num2, pieceToMove.isWhite ? 0 : 7);
			ChessPieceData pieceAt = this.GetPieceAt(int3);
			pieceAt.timesMoved--;
			this.SetPieceAt(int3, null);
			this.SetPieceAt(int2, pieceAt);
		}
		if (moveData.SpecialMove == ChessManager.SpecialMove.PawnPromotion)
		{
			pieceToMove.type = ChessPieceData.PieceType.Pawn;
		}
		ChessPiece chessPiece;
		if (updateVisuals && this.allPieces.TryGetValue(pieceToMove, out chessPiece))
		{
			chessPiece.UpdatePosition(moveData.StartPosition);
		}
		ChessPiece chessPiece2;
		if (updateVisuals && moveData.CapturePiece != null && this.allPieces.TryGetValue(moveData.CapturePiece, out chessPiece2))
		{
			chessPiece2.UpdatePosition(@int);
		}
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x000F2216 File Offset: 0x000F0416
	private bool IsValidPosition(int2 index)
	{
		return index.x >= 0 && index.x < 8 && index.y >= 0 && index.y < 8;
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x000F2240 File Offset: 0x000F0440
	public void GetLegalMoves(int2 index)
	{
		ChessPieceData pieceAt = this.GetPieceAt(index);
		if (pieceAt == null)
		{
			string text = "Found no piece at ";
			int2 @int = index;
			Debug.LogError(text + @int.ToString());
		}
		this.pseudoLegalMoves.Clear();
		this.legalMoves.Clear();
		switch (pieceAt.type)
		{
		case ChessPieceData.PieceType.Pawn:
			this.GetPawnMoves(pieceAt, index, this.pseudoLegalMoves);
			break;
		case ChessPieceData.PieceType.Rook:
		case ChessPieceData.PieceType.Bishop:
		case ChessPieceData.PieceType.Queen:
			this.GetSlidingMoves(pieceAt, index, this.pseudoLegalMoves);
			break;
		case ChessPieceData.PieceType.Knight:
		case ChessPieceData.PieceType.King:
			this.GetKnightKingMoves(pieceAt, index, this.pseudoLegalMoves);
			break;
		}
		int2 int2 = this.GetPiecePos(pieceAt.isWhite ? this.whiteKing : this.blackKing);
		foreach (ChessManager.MoveData moveData in this.pseudoLegalMoves)
		{
			this.MakeMove(moveData, false);
			if (moveData.PieceToMove.type == ChessPieceData.PieceType.King)
			{
				int2 = moveData.EndPosition;
			}
			if (!this.IsSquareAttacked(int2, pieceAt.isWhite))
			{
				this.legalMoves.Add(moveData);
			}
			this.UnmakeMove(moveData, false);
		}
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x000F2384 File Offset: 0x000F0584
	private void GetPawnMoves(ChessPieceData pawn, int2 startPos, List<ChessManager.MoveData> validMoves)
	{
		int num = (pawn.isWhite ? 1 : (-1));
		int2 @int = startPos + ChessManager.pawnMoves[0] * num;
		if (this.GetPieceAt(@int) == null)
		{
			if (@int.y == (pawn.isWhite ? 7 : 0))
			{
				validMoves.Add(new ChessManager.MoveData(pawn, startPos, null, @int, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Queen));
				validMoves.Add(new ChessManager.MoveData(pawn, startPos, null, @int, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Rook));
				validMoves.Add(new ChessManager.MoveData(pawn, startPos, null, @int, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Bishop));
				validMoves.Add(new ChessManager.MoveData(pawn, startPos, null, @int, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Knight));
			}
			else
			{
				validMoves.Add(new ChessManager.MoveData(pawn, startPos, null, @int, this.enPassantPos, ChessManager.SpecialMove.None, ChessPieceData.PieceType.Pawn));
			}
			if (pawn.timesMoved == 0)
			{
				int2 int2 = startPos + ChessManager.pawnMoves[1] * num;
				if (this.GetPieceAt(int2) == null)
				{
					validMoves.Add(new ChessManager.MoveData(pawn, startPos, null, int2, this.enPassantPos, ChessManager.SpecialMove.PawnTwoStep, ChessPieceData.PieceType.Pawn));
				}
			}
		}
		foreach (int2 int3 in ChessManager.pawnCaptures)
		{
			int2 int4 = startPos + int3 * num;
			if (this.IsValidPosition(int4))
			{
				ChessPieceData pieceAt = this.GetPieceAt(int4);
				if (pieceAt != null && pieceAt.isWhite != pawn.isWhite)
				{
					if (@int.y == (pawn.isWhite ? 7 : 0))
					{
						validMoves.Add(new ChessManager.MoveData(pawn, startPos, pieceAt, int4, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Queen));
						validMoves.Add(new ChessManager.MoveData(pawn, startPos, pieceAt, int4, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Rook));
						validMoves.Add(new ChessManager.MoveData(pawn, startPos, pieceAt, int4, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Bishop));
						validMoves.Add(new ChessManager.MoveData(pawn, startPos, pieceAt, int4, this.enPassantPos, ChessManager.SpecialMove.PawnPromotion, ChessPieceData.PieceType.Knight));
					}
					else
					{
						validMoves.Add(new ChessManager.MoveData(pawn, startPos, pieceAt, int4, this.enPassantPos, ChessManager.SpecialMove.None, ChessPieceData.PieceType.Pawn));
					}
				}
				if (this.enPassantPos.Equals(int4))
				{
					int2 int5 = new int2(this.enPassantPos.x, this.enPassantPos.y - num);
					ChessPieceData pieceAt2 = this.GetPieceAt(int5);
					if (pieceAt2 != null && pieceAt2.isWhite != pawn.isWhite)
					{
						validMoves.Add(new ChessManager.MoveData(pawn, startPos, pieceAt2, this.enPassantPos, this.enPassantPos, ChessManager.SpecialMove.EnPassantCapture, ChessPieceData.PieceType.Pawn));
					}
				}
			}
		}
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x000F25F0 File Offset: 0x000F07F0
	private void GetSlidingMoves(ChessPieceData slidingPiece, int2 startPos, List<ChessManager.MoveData> validMoves)
	{
		int2[] array;
		switch (slidingPiece.type)
		{
		case ChessPieceData.PieceType.Rook:
			array = ChessManager.rookDirections;
			goto IL_004A;
		case ChessPieceData.PieceType.Bishop:
			array = ChessManager.bishopDirections;
			goto IL_004A;
		case ChessPieceData.PieceType.Queen:
			array = ChessManager.queenDirections;
			goto IL_004A;
		}
		Debug.LogError("Invalid piece type for sliding moves");
		array = new int2[1];
		IL_004A:
		int2[] array2 = array;
		int i = 0;
		while (i < array2.Length)
		{
			int2 @int = array2[i];
			int2 int2 = startPos;
			ChessPieceData pieceAt;
			for (;;)
			{
				int2 += @int;
				if (!this.IsValidPosition(int2))
				{
					break;
				}
				pieceAt = this.GetPieceAt(int2);
				if (pieceAt != null)
				{
					goto Block_3;
				}
				validMoves.Add(new ChessManager.MoveData(slidingPiece, startPos, null, int2, this.enPassantPos, ChessManager.SpecialMove.None, ChessPieceData.PieceType.Pawn));
			}
			IL_00C3:
			i++;
			continue;
			Block_3:
			if (pieceAt.isWhite != slidingPiece.isWhite)
			{
				validMoves.Add(new ChessManager.MoveData(slidingPiece, startPos, pieceAt, int2, this.enPassantPos, ChessManager.SpecialMove.None, ChessPieceData.PieceType.Pawn));
				goto IL_00C3;
			}
			goto IL_00C3;
		}
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x000F26CC File Offset: 0x000F08CC
	private void GetKnightKingMoves(ChessPieceData piece, int2 startPos, List<ChessManager.MoveData> validMoves)
	{
		foreach (int2 @int in (piece.type == ChessPieceData.PieceType.Knight) ? ChessManager.knightOffsets : ChessManager.kingDirections)
		{
			int2 int2 = startPos + @int;
			if (this.IsValidPosition(int2))
			{
				ChessPieceData pieceAt = this.GetPieceAt(int2);
				if (pieceAt == null || pieceAt.isWhite != piece.isWhite)
				{
					validMoves.Add(new ChessManager.MoveData(piece, startPos, pieceAt, int2, this.enPassantPos, ChessManager.SpecialMove.None, ChessPieceData.PieceType.Pawn));
				}
			}
		}
		if (piece.type == ChessPieceData.PieceType.King)
		{
			this.TryCastle(piece, startPos, true, validMoves);
			this.TryCastle(piece, startPos, false, validMoves);
		}
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x000F2768 File Offset: 0x000F0968
	private void TryCastle(ChessPieceData king, int2 startPos, bool isKingSide, List<ChessManager.MoveData> validMoves)
	{
		if (king.timesMoved > 0 || this.IsSquareAttacked(startPos, king.isWhite))
		{
			return;
		}
		int num = (isKingSide ? 7 : 0);
		int2 @int = new int2(num, startPos.y);
		ChessPieceData pieceAt = this.GetPieceAt(@int);
		if (pieceAt == null || pieceAt.isWhite != king.isWhite || pieceAt.type != ChessPieceData.PieceType.Rook || pieceAt.timesMoved > 0)
		{
			return;
		}
		int num2 = (isKingSide ? 1 : (-1));
		for (int num3 = startPos.x + num2; num3 != num; num3 += num2)
		{
			int2 int2 = new int2(num3, startPos.y);
			if (this.GetPieceAt(int2) != null)
			{
				return;
			}
		}
		int2 int3 = new int2(startPos.x + num2, startPos.y);
		if (this.IsSquareAttacked(int3, king.isWhite))
		{
			return;
		}
		ChessManager.SpecialMove specialMove = (isKingSide ? ChessManager.SpecialMove.ShortCastle : ChessManager.SpecialMove.LongCastle);
		validMoves.Add(new ChessManager.MoveData(king, startPos, null, new int2(isKingSide ? 6 : 2, startPos.y), this.enPassantPos, specialMove, ChessPieceData.PieceType.Pawn));
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x000F2864 File Offset: 0x000F0A64
	public bool IsSquareAttacked(int2 position, bool isWhite)
	{
		foreach (int2 @int in ChessManager.kingDirections)
		{
			if (this.IsPieceAtPositionOfType(position + @int, isWhite, ChessPieceData.PieceType.King))
			{
				return true;
			}
		}
		if (this.IsSlidingPieceAttacking(position, isWhite, true))
		{
			return true;
		}
		if (this.IsSlidingPieceAttacking(position, isWhite, false))
		{
			return true;
		}
		foreach (int2 int2 in ChessManager.knightOffsets)
		{
			if (this.IsPieceAtPositionOfType(position + int2, isWhite, ChessPieceData.PieceType.Knight))
			{
				return true;
			}
		}
		int num = (isWhite ? 1 : (-1));
		foreach (int2 int3 in ChessManager.pawnCaptures)
		{
			if (this.IsPieceAtPositionOfType(position + int3 * new int2(1, num), isWhite, ChessPieceData.PieceType.Pawn))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x000F2934 File Offset: 0x000F0B34
	private bool IsSlidingPieceAttacking(int2 position, bool isWhite, bool isRookMovement)
	{
		foreach (int2 @int in isRookMovement ? ChessManager.rookDirections : ChessManager.bishopDirections)
		{
			int2 int2 = position + @int;
			while (this.IsValidPosition(int2))
			{
				ChessPieceData pieceAt = this.GetPieceAt(int2);
				if (pieceAt != null)
				{
					if (pieceAt.isWhite == isWhite)
					{
						break;
					}
					if (pieceAt.type == ChessPieceData.PieceType.Queen)
					{
						return true;
					}
					if ((isRookMovement ? ChessPieceData.PieceType.Rook : ChessPieceData.PieceType.Bishop) == pieceAt.type)
					{
						return true;
					}
					break;
				}
				else
				{
					int2 += @int;
				}
			}
		}
		return false;
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x000F29B8 File Offset: 0x000F0BB8
	private bool IsPieceAtPositionOfType(int2 position, bool isWhite, ChessPieceData.PieceType type)
	{
		if (this.IsValidPosition(position))
		{
			ChessPieceData pieceAt = this.GetPieceAt(position);
			if (pieceAt != null && pieceAt.isWhite != isWhite && pieceAt.type == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040028C6 RID: 10438
	public GameObject originalPieces;

	// Token: 0x040028C7 RID: 10439
	public GameObject originalExtras;

	// Token: 0x040028C8 RID: 10440
	public GameObject blackWinner;

	// Token: 0x040028C9 RID: 10441
	public GameObject whiteWinner;

	// Token: 0x040028CA RID: 10442
	public GameObject blackOpponent;

	// Token: 0x040028CB RID: 10443
	public GameObject whiteOpponent;

	// Token: 0x040028CC RID: 10444
	public GameObject draw;

	// Token: 0x040028CD RID: 10445
	public Transform helperTileGroup;

	// Token: 0x040028CE RID: 10446
	private Renderer[] helperTiles = new Renderer[64];

	// Token: 0x040028CF RID: 10447
	private MaterialPropertyBlock colorSetter;

	// Token: 0x040028D0 RID: 10448
	private Bounds colBounds;

	// Token: 0x040028D1 RID: 10449
	private GameObject clonedPieces;

	// Token: 0x040028D2 RID: 10450
	private ChessPieceData[] chessBoard = new ChessPieceData[64];

	// Token: 0x040028D3 RID: 10451
	private Dictionary<ChessPieceData, ChessPiece> allPieces = new Dictionary<ChessPieceData, ChessPiece>();

	// Token: 0x040028D4 RID: 10452
	private ChessPieceData whiteKing;

	// Token: 0x040028D5 RID: 10453
	private ChessPieceData blackKing;

	// Token: 0x040028D6 RID: 10454
	private int2 enPassantPos = new int2(-1, -1);

	// Token: 0x040028D7 RID: 10455
	private List<ChessManager.MoveData> legalMoves = new List<ChessManager.MoveData>(27);

	// Token: 0x040028D8 RID: 10456
	private List<ChessManager.MoveData> pseudoLegalMoves = new List<ChessManager.MoveData>(27);

	// Token: 0x040028D9 RID: 10457
	private List<ChessManager.MoveData> allLegalMoves = new List<ChessManager.MoveData>(27);

	// Token: 0x040028DA RID: 10458
	private UciChessEngine chessEngine;

	// Token: 0x040028DB RID: 10459
	private List<string> UCIMoves = new List<string>();

	// Token: 0x040028DC RID: 10460
	[HideInInspector]
	public bool isWhiteTurn = true;

	// Token: 0x040028DD RID: 10461
	[HideInInspector]
	public bool whiteIsBot;

	// Token: 0x040028DE RID: 10462
	[HideInInspector]
	public bool blackIsBot = true;

	// Token: 0x040028DF RID: 10463
	[HideInInspector]
	public bool gameLocked = true;

	// Token: 0x040028E0 RID: 10464
	private bool tutorialMessageSent;

	// Token: 0x040028E1 RID: 10465
	private int numMoves;

	// Token: 0x040028E2 RID: 10466
	public int elo = 1000;

	// Token: 0x040028E3 RID: 10467
	private static readonly int2[] pawnMoves = new int2[]
	{
		new int2(0, 1),
		new int2(0, 2)
	};

	// Token: 0x040028E4 RID: 10468
	private static readonly int2[] pawnCaptures = new int2[]
	{
		new int2(1, 1),
		new int2(-1, 1)
	};

	// Token: 0x040028E5 RID: 10469
	private static readonly int2[] rookDirections = new int2[]
	{
		new int2(1, 0),
		new int2(-1, 0),
		new int2(0, 1),
		new int2(0, -1)
	};

	// Token: 0x040028E6 RID: 10470
	private static readonly int2[] bishopDirections = new int2[]
	{
		new int2(1, 1),
		new int2(-1, 1),
		new int2(1, -1),
		new int2(-1, -1)
	};

	// Token: 0x040028E7 RID: 10471
	private static readonly int2[] queenDirections = ChessManager.rookDirections.Concat(ChessManager.bishopDirections).ToArray<int2>();

	// Token: 0x040028E8 RID: 10472
	private static readonly int2[] knightOffsets = new int2[]
	{
		new int2(1, 2),
		new int2(2, 1),
		new int2(2, -1),
		new int2(1, -2),
		new int2(-1, -2),
		new int2(-2, -1),
		new int2(-2, 1),
		new int2(-1, 2)
	};

	// Token: 0x040028E9 RID: 10473
	private static readonly int2[] kingDirections = new int2[]
	{
		new int2(1, 0),
		new int2(-1, 0),
		new int2(0, 1),
		new int2(0, -1),
		new int2(1, 1),
		new int2(-1, 1),
		new int2(1, -1),
		new int2(-1, -1)
	};

	// Token: 0x020004E6 RID: 1254
	public enum SpecialMove
	{
		// Token: 0x040028EB RID: 10475
		None,
		// Token: 0x040028EC RID: 10476
		ShortCastle,
		// Token: 0x040028ED RID: 10477
		LongCastle,
		// Token: 0x040028EE RID: 10478
		PawnTwoStep,
		// Token: 0x040028EF RID: 10479
		PawnPromotion,
		// Token: 0x040028F0 RID: 10480
		EnPassantCapture
	}

	// Token: 0x020004E7 RID: 1255
	public struct MoveData
	{
		// Token: 0x06001CE4 RID: 7396 RVA: 0x000F2C7C File Offset: 0x000F0E7C
		public MoveData(ChessPieceData pieceToMove, int2 startPosition, ChessPieceData capturePiece, int2 endPosition, int2 lastEPPos, ChessManager.SpecialMove specialMove = ChessManager.SpecialMove.None, ChessPieceData.PieceType promotionType = ChessPieceData.PieceType.Pawn)
		{
			this.PieceToMove = pieceToMove;
			this.StartPosition = startPosition;
			this.EndPosition = endPosition;
			this.CapturePiece = capturePiece;
			this.SpecialMove = specialMove;
			this.LastEnPassantPos = lastEPPos;
			this.PromotionType = promotionType;
		}

		// Token: 0x040028F1 RID: 10481
		public int2 StartPosition;

		// Token: 0x040028F2 RID: 10482
		public ChessPieceData PieceToMove;

		// Token: 0x040028F3 RID: 10483
		public int2 EndPosition;

		// Token: 0x040028F4 RID: 10484
		public ChessPieceData CapturePiece;

		// Token: 0x040028F5 RID: 10485
		public ChessManager.SpecialMove SpecialMove;

		// Token: 0x040028F6 RID: 10486
		public int2 LastEnPassantPos;

		// Token: 0x040028F7 RID: 10487
		public ChessPieceData.PieceType PromotionType;
	}
}
