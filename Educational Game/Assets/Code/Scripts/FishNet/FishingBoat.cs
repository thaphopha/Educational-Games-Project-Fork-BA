using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBoat : MonoBehaviour
{
	[SerializeField] float boatTravelSpeed;

	[SerializeField] float trashDelay;
	[SerializeField] GameObject[] trashList;

	public float maxTravelDistance;
	public int leftSideFactor;

	float distanceTravelled = 0;

	Transform trashHolder;
	bool throwTrashStarted = false;

	void Start()
	{
		trashHolder = GameObject.Find("TrashHolder").GetComponent<Transform>();
	}

	void Update()
    {
        MoveBoat();
    }

    void MoveBoat()
    {
		if (distanceTravelled < maxTravelDistance + 35f)
		{
			transform.Translate(leftSideFactor * boatTravelSpeed * Time.deltaTime, 0, 0, Space.World);
			distanceTravelled += boatTravelSpeed * Time.deltaTime;

			if (!throwTrashStarted && distanceTravelled > 24)
			{
				throwTrashStarted = true;
				StartCoroutine(ThrowTrash());
			}
		}
		else
		{
			throwTrashStarted = false;
		}
	}

	IEnumerator ThrowTrash()
	{
		while (throwTrashStarted)
		{
			Instantiate(trashList[Random.Range(0, trashList.Length)], new Vector3(transform.position.x - 8 * leftSideFactor, 25, 0), Quaternion.identity, trashHolder);
			Instantiate(trashList[Random.Range(0, trashList.Length)], new Vector3(transform.position.x, 20, 0), Quaternion.identity, trashHolder);
			yield return new WaitForSeconds(trashDelay);
		}
	}
}
