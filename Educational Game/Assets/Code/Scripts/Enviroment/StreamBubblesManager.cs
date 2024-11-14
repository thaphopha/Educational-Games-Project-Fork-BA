using UnityEngine;

public class StreamBubbles : MonoBehaviour
{
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject bubble;
    [SerializeField] Sprite[] bubbleSprites;

    private void Start()
    {
        SpawnBubble();
    }

    void SpawnBubble()
    {
        GameObject b = Instantiate(bubble, new Vector3(-45, Random.Range(-30, 12), 0), Quaternion.identity, transform);

        if (Random.Range(0, 2) == 1)
        {
            b.transform.position = new Vector3(45, b.transform.position.y, 0);
        }
        b.GetComponent<SpriteRenderer>().sprite = bubbleSprites[Random.Range(0, bubbleSprites.Length)];

        Invoke("SpawnBubble", spawnTimer);
    }
}
