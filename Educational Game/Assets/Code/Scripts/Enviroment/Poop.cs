using UnityEngine;

public class Poop : MonoBehaviour
{
    private void Start()
    {
        Invoke("EnableCollider", 3f);
    }

    void EnableCollider()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Invoke("RemoveSelf", 10f);
        }
    }

    void RemoveSelf()
    {
        Destroy(gameObject);
    }
}
