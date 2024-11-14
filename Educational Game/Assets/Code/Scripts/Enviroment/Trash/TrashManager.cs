using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
	[SerializeField] float trashPerMinute;
	[SerializeField] float maxTrash;
	[SerializeField] GameObject[] trashList;

    bool spawnTrash = true;
	float trashSpawned = 0;

	void Start()
	{
        StartCoroutine(SpawnTrash());
	}

	void Update()
    {
        HandleTrashSpawnRate();
    }

    IEnumerator SpawnTrash()
    {
        while (spawnTrash)
        {
            yield return new WaitForSeconds(60 / trashPerMinute);
            Instantiate(trashList[Random.Range(0, trashList.Length)], new Vector3(Random.Range(-37, 37), 25, 0), Quaternion.identity, transform);
            trashSpawned++;
        }
    }

    void HandleTrashSpawnRate()
    {
        if (trashSpawned >= maxTrash - 1)
        {
            spawnTrash = false;
        }
        else
        {
            if (trashPerMinute < 30)
            {
                trashPerMinute += Time.deltaTime * (Time.time / 300);
            }
		}
    }
}
