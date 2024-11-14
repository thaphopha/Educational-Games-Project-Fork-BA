using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishnetBack : Fishnet
{
	[SerializeField] float scalePerSecond;

	PolygonCollider2D polygonCollider;
	SpriteRenderer spriteRenderer;

	void Start()
	{
		polygonCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
		polygonCollider.enabled = false;
		spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	void Update()
    {
		if (transform.localScale.x < 2)
		{
			GrowNet();
		}
		else
		{
			if (polygonCollider != null && polygonCollider.enabled == false)
			{
				polygonCollider.enabled = true;
			}
			PullNetToSurface();
		}
    }

	void GrowNet()
	{
		//size
		Vector3 scaleIncrease = new Vector3(scalePerSecond * Time.deltaTime, scalePerSecond * Time.deltaTime, scalePerSecond * Time.deltaTime);
		transform.localScale += scaleIncrease * (1 + transform.localScale.x);

		//transparency
		Color currentColor = spriteRenderer.color;
		currentColor.a = transform.localScale.x / 2;
		spriteRenderer.color = currentColor;
	}
}
