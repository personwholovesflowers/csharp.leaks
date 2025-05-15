using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003DB RID: 987
public class ScrollUIImage : MonoBehaviour
{
	// Token: 0x06001654 RID: 5716 RVA: 0x000B3D30 File Offset: 0x000B1F30
	private void Start()
	{
		this.img = base.GetComponent<RawImage>();
	}

	// Token: 0x06001655 RID: 5717 RVA: 0x000B3D40 File Offset: 0x000B1F40
	private void Update()
	{
		Vector2 vector = this.img.uvRect.position + new Vector2(this.xSpeed, this.ySpeed) * Time.deltaTime;
		while (vector.x > 1f)
		{
			vector.x -= 1f;
		}
		while (vector.x < -1f)
		{
			vector.x += 1f;
		}
		while (vector.y > 1f)
		{
			vector.y -= 1f;
		}
		while (vector.y < -1f)
		{
			vector.y += 1f;
		}
		this.img.uvRect = new Rect(this.img.uvRect.position + new Vector2(this.xSpeed, this.ySpeed) * Time.deltaTime, this.img.uvRect.size);
	}

	// Token: 0x04001EC0 RID: 7872
	private RawImage img;

	// Token: 0x04001EC1 RID: 7873
	public float xSpeed;

	// Token: 0x04001EC2 RID: 7874
	public float ySpeed;
}
