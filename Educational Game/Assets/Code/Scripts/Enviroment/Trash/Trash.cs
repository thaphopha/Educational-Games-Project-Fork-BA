using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
	[SerializeField] float sinkSpeed;
	[SerializeField] float fallSpeed;

    float depthPosition;

	void Start()
	{
        depthPosition = Random.Range(-30, 13);
	}

	void Update()
    {
        Move();
    }

    void Move()
    {
        float currentSpeed = 0;

        if (transform.position.y > 14.5)
        {
            currentSpeed = fallSpeed;
        }
        else if (transform.position.y > depthPosition)
        {
            currentSpeed = sinkSpeed;
        }

        transform.Translate(0,-currentSpeed * Time.deltaTime,0, Space.World);
    }
}
