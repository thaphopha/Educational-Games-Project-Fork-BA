using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] float shrimpsPerMinute;
    [SerializeField] float fishPerMinute;
    [SerializeField] float stopSpawningAfterSeconds;

    [SerializeField] GameObject shrimpPrefab;
    [SerializeField] GameObject[] fishPrefabs;

    float newTimer = 0;
    bool spawnShrimps = true;
    bool spawnFishs = true;

    [SerializeField] int fishCountForNarrator;
    Transform fishHolder;

    private void Awake()
    {
        fishHolder = GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(SpawnShrimps());
        StartCoroutine(SpawnFishs());  
    }

    void Update()
    {
        newTimer += Time.deltaTime;
        LowerSpawningRate();
        CheckFishCount();
    }

    void LowerSpawningRate()
    {
        if (newTimer > stopSpawningAfterSeconds)
        {
            spawnShrimps = false;
            spawnFishs = false;
        }
    }

    IEnumerator SpawnShrimps()
    {
        while (spawnShrimps)
        {
            yield return new WaitForSeconds(60 / shrimpsPerMinute);
            int sideFactor;
            if (Random.Range(0, 2) == 0)
            {
                sideFactor = 1;
            }
            else
            {
                sideFactor = -1;
            }

            // Spawn Shrimp
            GameObject shrimp;
            shrimp = Instantiate(shrimpPrefab, new Vector3(55 * sideFactor, Random.Range(-30f, 9f), 0), Quaternion.identity, transform);
            shrimp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    IEnumerator SpawnFishs()
    {

        while (spawnFishs)
        {
            yield return new WaitForSeconds(60 / fishPerMinute);
            int sideFactor;
            if (Random.Range(0, 2) == 0)
            {
                sideFactor = 1;
            }
            else
            {
                sideFactor = -1;
            }

            // Spawn Fish
            GameObject fish;
            fish = Instantiate(fishPrefabs[Random.Range(0, fishPrefabs.Length)], new Vector3(55 * sideFactor, Random.Range(-30f, 9f), 0), Quaternion.identity, transform);
            fish.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    void CheckFishCount()
    {
        if (fishHolder.childCount - 2 < fishCountForNarrator)
        {
            if (!NarratorAudioManager.instance.populationDyingBoolean)
            {
                NarratorAudioManager.instance.populationDyingBoolean = NarratorAudioManager.instance.PlayNarratorClip(NarratorAudioManager.instance.populationDyingClip);
            }
        }
    }
}
