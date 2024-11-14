using UnityEngine;

public class Diver : MonoBehaviour
{
    public float initialGravityScale = 1f;
    public float slowGravityScale = 0.5f;
    public float slowDownYValue = 16f;
    public float stopFallingYValue = 0f;
    public float floatSpeed = 1f;
    public float floatAmplitude = 1f;

    private Rigidbody2D rb;
    private bool isFloating = false;
    public float initialYPosition = 40f;
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (!isFloating)
        {
            if (transform.position.y <= stopFallingYValue)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                isFloating = true;
                initialYPosition = transform.position.y;
            }
            else if (transform.position.y <= slowDownYValue)
            {
                rb.gravityScale = slowGravityScale;
            }
        }
        else
        {
            float newY = initialYPosition + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public void draw()
    {
        gameObject.SetActive(true);
        Vector3 startPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 5f, Camera.main.nearClipPlane));
        startPosition.z = 0;
        transform.position = startPosition;
        initialYPosition = transform.position.y;
    }
}
