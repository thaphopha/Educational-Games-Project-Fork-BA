using UnityEngine;

public class AvatarCameraScript : MonoBehaviour
{
    public static AvatarCameraScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Transform target;
    // Can be removed ? 
    public bool AvatarCamera;
    public float smoothSpeed = 0.125f;

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, 10), smoothSpeed * Time.deltaTime);
        }
    }

    public void ChangeZoom(float size)
    {
        GetComponent<Camera>().orthographicSize = size * 10;
    }
}
