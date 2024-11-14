using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishnet : MonoBehaviour
{
	float netUpSpeed = 3;

	public void PullNetToSurface()
	{
		if (transform.position.y < 8.5)
		{
			transform.Translate(0, netUpSpeed * Time.deltaTime, 0, Space.World);
		}
	}

	public bool TryGetPlayerFish()
	{
		Collider2D[] catchedFishs = new Collider2D[100];
		Physics2D.OverlapCollider(transform.GetChild(0).GetComponent<PolygonCollider2D>(), new ContactFilter2D(), catchedFishs);

		foreach (var fish in catchedFishs)
		{
			if (fish == null) return false;

			if (fish.gameObject.TryGetComponent(out FishPlayerInput playerInput))
			{
				return true;
			} 
			else if (fish.gameObject.TryGetComponent(out FishDead fishDead)) 
			{
				if(fishDead.player) return true;
			}
		}
		return false;
	}

	public void DestroyAllFish()
	{
		Collider2D[] catchedFishs = new Collider2D[100];
		Physics2D.OverlapCollider(transform.GetChild(0).GetComponent<PolygonCollider2D>(), new ContactFilter2D(), catchedFishs);

		foreach (var fish in catchedFishs)
		{
			if (fish != null)
			{
				Destroy(fish.gameObject);
			}
		}
	}
}
