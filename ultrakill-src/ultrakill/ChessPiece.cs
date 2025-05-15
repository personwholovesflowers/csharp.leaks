using System;
using Sandbox;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020004EF RID: 1263
public class ChessPiece : MonoBehaviour
{
	// Token: 0x06001CFC RID: 7420 RVA: 0x000F31CA File Offset: 0x000F13CA
	private void Awake()
	{
		this.chessMan = MonoSingleton<ChessManager>.Instance;
		this.sbp = base.GetComponent<SandboxProp>();
		if (!this.dragSound)
		{
			this.dragSound = base.GetComponent<AudioSource>();
		}
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x000F31FC File Offset: 0x000F13FC
	private void Start()
	{
		if (this.promotionPanel != null)
		{
			this.promotionPanel.SetActive(false);
		}
		float y = this.chessMan.GetComponent<Collider>().bounds.max.y;
		float y2 = base.transform.GetChild(0).GetComponent<Collider>().bounds.min.y;
		this.boardHeight = base.transform.position.y + (y - y2);
		base.transform.position = new Vector3(base.transform.position.x, this.boardHeight, base.transform.position.z);
		if (this.initialized)
		{
			return;
		}
		this.rb = base.GetComponent<Rigidbody>();
		this.startRot = base.transform.rotation;
		this.Data = new ChessPieceData(this.type, this.isWhite, this.queenSide)
		{
			timesMoved = this.timesMoved,
			autoControl = this.autoControl
		};
		if (this.isWhite)
		{
			this.SetAutoControl(this.chessMan.whiteIsBot);
		}
		if (!this.isWhite)
		{
			this.SetAutoControl(this.chessMan.blackIsBot);
		}
		this.chessMan.InitializePiece(this);
		this.PieceCanMove(false);
		this.initialized = true;
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000F335C File Offset: 0x000F155C
	public void SetAutoControl(bool useAutoControl)
	{
		this.autoControl = useAutoControl;
		this.Data.autoControl = this.autoControl;
		this.sbp.disallowManipulation = this.autoControl;
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x000F3388 File Offset: 0x000F1588
	private void Update()
	{
		if (this.autoControl)
		{
			return;
		}
		if (this.chessMan.gameLocked)
		{
			this.PieceCanMove(false);
		}
		else
		{
			this.PieceCanMove(this.isWhite == this.chessMan.isWhiteTurn);
		}
		if (this.sbp.frozen && !this.positionDirty)
		{
			int2 @int = this.chessMan.WorldPositionToIndex(base.transform.position);
			this.chessMan.GetLegalMoves(@int);
			this.chessMan.DisplayValidMoves();
			this.positionDirty = true;
		}
		if (this.positionDirty && !this.sbp.frozen)
		{
			this.rb.isKinematic = false;
		}
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x000F3438 File Offset: 0x000F1638
	public void PieceCanMove(bool canMove)
	{
		this.sbp.disallowManipulation = !canMove;
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x000F344C File Offset: 0x000F164C
	private void OnCollisionEnter(Collision collider)
	{
		if (this.autoControl)
		{
			return;
		}
		if (this.isWhite && this.chessMan.whiteIsBot)
		{
			return;
		}
		if (!this.isWhite && this.chessMan.blackIsBot)
		{
			return;
		}
		if (this.sbp.frozen)
		{
			return;
		}
		ChessManager chessManager;
		if (this.positionDirty && collider.gameObject.TryGetComponent<ChessManager>(out chessManager))
		{
			this.chessMan.FindMoveAtWorldPosition(this);
			Object.Instantiate<AudioSource>(this.snapSound, base.transform.position, Quaternion.identity);
		}
		ChessPiece chessPiece;
		if (this.positionDirty && collider.gameObject.TryGetComponent<ChessPiece>(out chessPiece))
		{
			this.chessMan.FindMoveAtWorldPosition(this);
			Object.Instantiate<AudioSource>(this.snapSound, base.transform.position, Quaternion.identity);
		}
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x000F351C File Offset: 0x000F171C
	public void UpdatePosition(int2 position)
	{
		Vector3 vector = this.chessMan.IndexToWorldPosition(position, this.boardHeight);
		base.transform.SetPositionAndRotation(vector, this.startRot);
		this.positionDirty = false;
		this.rb.isKinematic = true;
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x000F3561 File Offset: 0x000F1761
	public void ShowPromotionGUI(ChessManager.MoveData move)
	{
		this.promotionMove = move;
		this.promotionPanel.SetActive(true);
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x000F3578 File Offset: 0x000F1778
	public void PlayerPromotePiece(int type)
	{
		ChessPieceData.PieceType pieceType = ChessPieceData.PieceType.Pawn;
		switch (type)
		{
		case 0:
			pieceType = ChessPieceData.PieceType.Queen;
			break;
		case 1:
			pieceType = ChessPieceData.PieceType.Rook;
			break;
		case 2:
			pieceType = ChessPieceData.PieceType.Bishop;
			break;
		case 3:
			pieceType = ChessPieceData.PieceType.Knight;
			break;
		}
		this.Data.type = pieceType;
		this.promotionPanel.SetActive(false);
		this.PromoteVisualPiece();
		this.promotionMove.PromotionType = pieceType;
		this.chessMan.StylishMove(this.promotionMove);
		this.chessMan.UpdateGame(this.promotionMove);
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x000F35FC File Offset: 0x000F17FC
	public void PromoteVisualPiece()
	{
		base.transform.GetChild(0).gameObject.SetActive(false);
		int num = (this.Data.isWhite ? 0 : 4);
		switch (this.Data.type)
		{
		case ChessPieceData.PieceType.Rook:
			num++;
			break;
		case ChessPieceData.PieceType.Knight:
			num += 3;
			break;
		case ChessPieceData.PieceType.Bishop:
			num += 2;
			break;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.chessMan.originalExtras.transform.GetChild(num).gameObject, base.transform);
		gameObject.transform.SetPositionAndRotation(base.transform.position, base.transform.rotation);
		gameObject.SetActive(true);
		CapsuleCollider componentInChildren = gameObject.GetComponentInChildren<CapsuleCollider>();
		Vector3 vector = componentInChildren.transform.TransformPoint(componentInChildren.center);
		CapsuleCollider component = base.GetComponent<CapsuleCollider>();
		component.height = componentInChildren.height;
		component.radius = componentInChildren.radius;
		component.center = component.transform.InverseTransformPoint(vector);
		Object.Destroy(componentInChildren);
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x000F36FF File Offset: 0x000F18FF
	public void Captured()
	{
		Object.Instantiate<GameObject>(this.breakEffect, base.transform.position, Quaternion.identity);
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002915 RID: 10517
	public ChessPieceData Data;

	// Token: 0x04002916 RID: 10518
	public ChessPieceData.PieceType type;

	// Token: 0x04002917 RID: 10519
	public bool isWhite = true;

	// Token: 0x04002918 RID: 10520
	public bool queenSide;

	// Token: 0x04002919 RID: 10521
	private bool positionDirty;

	// Token: 0x0400291A RID: 10522
	private Quaternion startRot;

	// Token: 0x0400291B RID: 10523
	private Rigidbody rb;

	// Token: 0x0400291C RID: 10524
	private SandboxProp sbp;

	// Token: 0x0400291D RID: 10525
	public AudioSource snapSound;

	// Token: 0x0400291E RID: 10526
	[HideInInspector]
	public AudioSource dragSound;

	// Token: 0x0400291F RID: 10527
	public GameObject breakEffect;

	// Token: 0x04002920 RID: 10528
	public GameObject teleportEffect;

	// Token: 0x04002921 RID: 10529
	public GameObject promotionEffect;

	// Token: 0x04002922 RID: 10530
	public int timesMoved;

	// Token: 0x04002923 RID: 10531
	public bool autoControl;

	// Token: 0x04002924 RID: 10532
	public bool initialized;

	// Token: 0x04002925 RID: 10533
	public GameObject promotionPanel;

	// Token: 0x04002926 RID: 10534
	public float boardHeight = -900f;

	// Token: 0x04002927 RID: 10535
	private ChessManager chessMan;

	// Token: 0x04002928 RID: 10536
	private ChessManager.MoveData promotionMove;
}
