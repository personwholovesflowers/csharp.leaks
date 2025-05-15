﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class FlockController : MonoBehaviour
{
	// Token: 0x060000A8 RID: 168 RVA: 0x00006CA4 File Offset: 0x00004EA4
	public void Start()
	{
		this._thisT = base.transform;
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		this._posBuffer = this._thisT.position + this._startPosOffset;
		if (!this._slowSpawn)
		{
			this.AddChild(this._childAmount);
		}
		if (this._randomPositionTimer > 0f)
		{
			base.InvokeRepeating("SetFlockRandomPosition", this._randomPositionTimer, this._randomPositionTimer);
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00006D44 File Offset: 0x00004F44
	public void AddChild(int amount)
	{
		if (this._groupChildToNewTransform)
		{
			this.InstantiateGroup();
		}
		for (int i = 0; i < amount; i++)
		{
			FlockChild flockChild = Object.Instantiate<FlockChild>(this._childPrefab);
			flockChild._spawner = this;
			this._roamers.Add(flockChild);
			this.AddChildToParent(flockChild.transform);
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00006D96 File Offset: 0x00004F96
	public void AddChildToParent(Transform obj)
	{
		if (this._groupChildToFlock)
		{
			obj.parent = base.transform;
			return;
		}
		if (this._groupChildToNewTransform)
		{
			obj.parent = this._groupTransform;
			return;
		}
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00006DC4 File Offset: 0x00004FC4
	public void RemoveChild(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Component component = this._roamers[this._roamers.Count - 1];
			this._roamers.RemoveAt(this._roamers.Count - 1);
			Object.Destroy(component.gameObject);
		}
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00006E18 File Offset: 0x00005018
	public void Update()
	{
		if (this._activeChildren > 0f)
		{
			if (this._updateDivisor > 1)
			{
				this._updateCounter++;
				this._updateCounter %= this._updateDivisor;
				this._newDelta = Time.deltaTime * (float)this._updateDivisor;
			}
			else
			{
				this._newDelta = Time.deltaTime;
			}
		}
		this.UpdateChildAmount();
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00006E84 File Offset: 0x00005084
	public void InstantiateGroup()
	{
		if (this._groupTransform != null)
		{
			return;
		}
		GameObject gameObject = new GameObject();
		this._groupTransform = gameObject.transform;
		this._groupTransform.position = this._thisT.position;
		if (this._groupName != "")
		{
			gameObject.name = this._groupName;
			return;
		}
		gameObject.name = this._thisT.name + " Fish Container";
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00006F02 File Offset: 0x00005102
	public void UpdateChildAmount()
	{
		if (this._childAmount >= 0 && this._childAmount < this._roamers.Count)
		{
			this.RemoveChild(1);
			return;
		}
		if (this._childAmount > this._roamers.Count)
		{
			this.AddChild(1);
		}
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00006F44 File Offset: 0x00005144
	public void OnDrawGizmos()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (!Application.isPlaying && this._posBuffer != this._thisT.position + this._startPosOffset)
		{
			this._posBuffer = this._thisT.position + this._startPosOffset;
		}
		if (this._positionSphereDepth == -1f)
		{
			this._positionSphereDepth = this._positionSphere;
		}
		if (this._spawnSphereDepth == -1f)
		{
			this._spawnSphereDepth = this._spawnSphere;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this._posBuffer, new Vector3(this._spawnSphere * 2f, this._spawnSphereHeight * 2f, this._spawnSphereDepth * 2f));
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(this._thisT.position, new Vector3(this._positionSphere * 2f + this._spawnSphere * 2f, this._positionSphereHeight * 2f + this._spawnSphereHeight * 2f, this._positionSphereDepth * 2f + this._spawnSphereDepth * 2f));
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000708C File Offset: 0x0000528C
	public void SetFlockRandomPosition()
	{
		Vector3 zero = Vector3.zero;
		zero.x = Random.Range(-this._positionSphere, this._positionSphere) + this._thisT.position.x;
		zero.z = Random.Range(-this._positionSphereDepth, this._positionSphereDepth) + this._thisT.position.z;
		zero.y = Random.Range(-this._positionSphereHeight, this._positionSphereHeight) + this._thisT.position.y;
		this._posBuffer = zero;
		if (this._forceChildWaypoints)
		{
			for (int i = 0; i < this._roamers.Count; i++)
			{
				this._roamers[i].Wander(Random.value * this._forcedRandomDelay);
			}
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00007160 File Offset: 0x00005360
	public void destroyBirds()
	{
		for (int i = 0; i < this._roamers.Count; i++)
		{
			Object.Destroy(this._roamers[i].gameObject);
		}
		this._childAmount = 0;
		this._roamers.Clear();
	}

	// Token: 0x040000E2 RID: 226
	public FlockChild _childPrefab;

	// Token: 0x040000E3 RID: 227
	public int _childAmount = 250;

	// Token: 0x040000E4 RID: 228
	public bool _slowSpawn;

	// Token: 0x040000E5 RID: 229
	public float _spawnSphere = 3f;

	// Token: 0x040000E6 RID: 230
	public float _spawnSphereHeight = 3f;

	// Token: 0x040000E7 RID: 231
	public float _spawnSphereDepth = -1f;

	// Token: 0x040000E8 RID: 232
	public float _minSpeed = 6f;

	// Token: 0x040000E9 RID: 233
	public float _maxSpeed = 10f;

	// Token: 0x040000EA RID: 234
	public float _minScale = 0.7f;

	// Token: 0x040000EB RID: 235
	public float _maxScale = 1f;

	// Token: 0x040000EC RID: 236
	public float _soarFrequency;

	// Token: 0x040000ED RID: 237
	public string _soarAnimation = "Soar";

	// Token: 0x040000EE RID: 238
	public string _flapAnimation = "Flap";

	// Token: 0x040000EF RID: 239
	public string _idleAnimation = "Idle";

	// Token: 0x040000F0 RID: 240
	public float _diveValue = 7f;

	// Token: 0x040000F1 RID: 241
	public float _diveFrequency = 0.5f;

	// Token: 0x040000F2 RID: 242
	public float _minDamping = 1f;

	// Token: 0x040000F3 RID: 243
	public float _maxDamping = 2f;

	// Token: 0x040000F4 RID: 244
	public float _waypointDistance = 1f;

	// Token: 0x040000F5 RID: 245
	public float _minAnimationSpeed = 2f;

	// Token: 0x040000F6 RID: 246
	public float _maxAnimationSpeed = 4f;

	// Token: 0x040000F7 RID: 247
	public float _randomPositionTimer = 10f;

	// Token: 0x040000F8 RID: 248
	public float _positionSphere = 25f;

	// Token: 0x040000F9 RID: 249
	public float _positionSphereHeight = 25f;

	// Token: 0x040000FA RID: 250
	public float _positionSphereDepth = -1f;

	// Token: 0x040000FB RID: 251
	public bool _childTriggerPos;

	// Token: 0x040000FC RID: 252
	public bool _forceChildWaypoints;

	// Token: 0x040000FD RID: 253
	public float _forcedRandomDelay = 1.5f;

	// Token: 0x040000FE RID: 254
	public bool _flatFly;

	// Token: 0x040000FF RID: 255
	public bool _flatSoar;

	// Token: 0x04000100 RID: 256
	public bool _birdAvoid;

	// Token: 0x04000101 RID: 257
	public int _birdAvoidHorizontalForce = 1000;

	// Token: 0x04000102 RID: 258
	public bool _birdAvoidDown;

	// Token: 0x04000103 RID: 259
	public bool _birdAvoidUp;

	// Token: 0x04000104 RID: 260
	public int _birdAvoidVerticalForce = 300;

	// Token: 0x04000105 RID: 261
	public float _birdAvoidDistanceMax = 4.5f;

	// Token: 0x04000106 RID: 262
	public float _birdAvoidDistanceMin = 5f;

	// Token: 0x04000107 RID: 263
	public float _soarMaxTime;

	// Token: 0x04000108 RID: 264
	public LayerMask _avoidanceMask = -1;

	// Token: 0x04000109 RID: 265
	public List<FlockChild> _roamers;

	// Token: 0x0400010A RID: 266
	public Vector3 _posBuffer;

	// Token: 0x0400010B RID: 267
	public int _updateDivisor = 1;

	// Token: 0x0400010C RID: 268
	public float _newDelta;

	// Token: 0x0400010D RID: 269
	public int _updateCounter;

	// Token: 0x0400010E RID: 270
	public float _activeChildren;

	// Token: 0x0400010F RID: 271
	public bool _groupChildToNewTransform;

	// Token: 0x04000110 RID: 272
	public Transform _groupTransform;

	// Token: 0x04000111 RID: 273
	public string _groupName = "";

	// Token: 0x04000112 RID: 274
	public bool _groupChildToFlock;

	// Token: 0x04000113 RID: 275
	public Vector3 _startPosOffset;

	// Token: 0x04000114 RID: 276
	public Transform _thisT;
}
