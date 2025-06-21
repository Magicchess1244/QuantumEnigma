using UnityEngine;

public class BoostPlatform : MonoBehaviour
{
	[Header("--------Direction---------")]
	[SerializeField] private Transform Direction;
	[SerializeField] private float Speed;
    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody Rb))
		{
            Rb.linearVelocity = ((Direction.position - transform.position) * Speed);
		}
	}
}
