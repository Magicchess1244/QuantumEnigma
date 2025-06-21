using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	private float Speed = 0;
	private float OgSpeed = 1.5f;
	public bool Door = false; 
	public bool End = true;

	public Vector3 CurentDirection;
	public Vector3 LastDirection;
	private int Direct = 0;
	private float TotalDist;
	private float Distance;
	private void OnEnable()
	{
        gameObject.isStatic = false;
        Actions.ModifyTime += ModifyTime;
		GlobalManager.GameLoop += MovePlatform;
	}
	private void OnDisable()
	{
		Actions.ModifyTime -= ModifyTime;
		GlobalManager.GameLoop -= MovePlatform;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !Door)
		{
			other.gameObject.transform.SetParent(transform);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !Door)
		{
			other.gameObject.transform.SetParent(null);
		}
	}
	private void Start()
	{

        Speed = OgSpeed;

        CurentDirection = transform.parent.GetChild(1).position;
		LastDirection = transform.parent.GetChild(0).position;
		TotalDist = Vector3.Distance(CurentDirection, LastDirection);
	}
	public void MovePlatform()
	{
		if ((Door && !End) || !Door)
		{
			ChangedPosition();
			Distance += Time.deltaTime * Speed;
			float RelDistance = Distance / TotalDist;
			RelDistance = Mathf.SmoothStep(0, 1, RelDistance);
			transform.position = Vector3.Lerp(LastDirection, CurentDirection, RelDistance);
		}
	}
	private void ChangedPosition()
	{
		if (TotalDist - Distance <= 0.2f)
		{
			CurentDirection = transform.parent.GetChild(Direct).position; 
			LastDirection = transform.parent.GetChild((Direct + 1) % 2).position;
			Direct = (Direct + 1) % 2;
			Distance = 0;
			End = true;
		}
	}
	private void ModifyTime(float Afect)
	{
		//print($"Afect: {Afect}");
		//print($"OgSpeed: {OgSpeed}");

        Speed = OgSpeed * Afect;
	}
}