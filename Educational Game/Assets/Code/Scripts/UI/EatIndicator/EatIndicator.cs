using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatIndicator : MonoBehaviour
{
	[SerializeField]
	private GameObject fishMarkerPrefab;

	private GameObject nearestFish = null;
	private GameObject oldNearestFish = null;
	private float nearestDistance;
	private float currentDistance;
	private Vector3 currentFishPosition;

	void Update()
    {
		FindNearestEatableFish();
	}

	void FindNearestEatableFish()
	{
		int playerFoodCount = GetComponentInParent<FishEating>().foodCount;
		nearestDistance = 100;
		Collider2D[] nearestFishs = Physics2D.OverlapCircleAll(new Vector2(transform.parent.position.x, transform.parent.position.y), 8 + playerFoodCount/2);
		foreach (var fish in nearestFishs)
		{
			if (fish != null)
			{
				if (fish.gameObject.TryGetComponent<Eatable>(out Eatable eatable))
				{
					if (playerFoodCount >= fish.gameObject.GetComponent<Eatable>().requiredEatFactor) {
						currentFishPosition = fish.transform.position;
						currentDistance = Vector2.Distance(transform.parent.position, currentFishPosition);
						if (currentDistance != 0 && currentDistance < nearestDistance)
						{
							nearestDistance = currentDistance;
							nearestFish = fish.gameObject;
						}
					}
				}
			}
		}

		if (oldNearestFish != null && nearestFish != oldNearestFish)
		{
			if (oldNearestFish.transform.childCount > 0)
			{
				Destroy(oldNearestFish.transform.GetChild(0).gameObject);
			}
		}

		oldNearestFish = nearestFish;

		AdjustEatMarker();
	}

	void AdjustEatMarker()
	{
		if (nearestFish != null)
		{

			Color color;

			if (GetComponentInParent<FishEating>().canEat)
			{
				color = Color.green;
			}
			else
			{
				color = Color.yellow;
			}

			if (nearestFish.TryGetComponent<Trash>(out Trash trash))
			{
				color = Color.red;
			}

			color.a = 0.5f;

			transform.position = (nearestFish.transform.position - transform.parent.position) * 0.5f + transform.parent.position;

			if (nearestFish.transform.childCount == 0)
			{
				GameObject fishMarker = Instantiate(fishMarkerPrefab, nearestFish.transform);
				fishMarker.GetComponent<SpriteRenderer>().enabled = true;
				fishMarker.GetComponent<SpriteRenderer>().color = color;
			}
			if (oldNearestFish != null)
			{
				oldNearestFish.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
			}
			nearestFish = null;
		}
	}
}
