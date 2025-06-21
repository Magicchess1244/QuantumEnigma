using UnityEngine;

public class LavaOrAcid : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Actions actions = other.GetComponent<Actions>();
			actions.HealthFillAmount = 0;
			actions.Health.fillAmount = 0;
		}
	}
}
