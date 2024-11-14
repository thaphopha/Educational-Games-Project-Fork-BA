using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashEffect : MonoBehaviour
{
	[SerializeField] float slowFactor;

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.TryGetComponent<FishMovement>(out FishMovement fishMovement)){
			Rigidbody2D fishRigidbody = collision.GetComponent<Rigidbody2D>();
			fishRigidbody.velocity = fishRigidbody.velocity * slowFactor;
		}
	}
}
