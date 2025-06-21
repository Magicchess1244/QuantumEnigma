using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField] private List<GameObject> ButtonList = new();
	private bool Open = true;
	public bool Range = true;
	public bool Reverse = false;
	public bool LastOpen = false;
	private MovingPlatforms platforms;

	private void Start()
	{
		platforms = GetComponent<MovingPlatforms>();
	}
	private void OnEnable()
	{
		Ticker.OnTick += Action;
		transform.parent.gameObject.isStatic = false;
	}
	private void OnDisable()
	{
		Ticker.OnTick -= Action;
	}
	private void Action()
	{
		if (!Range)
		{
			Open = true;

			foreach (GameObject Button in ButtonList)
			{
				if (!Button.transform.GetChild(0).GetComponent<FisicalButton>().Open)
				{
					Open = false;
				}
			}
		}
		else if (!Open && Range)
		{
			GameObject Player = GameObject.FindGameObjectWithTag("Player");
			if (Vector3.Distance(transform.position, Player.transform.position) <= 4.5)
			{
				Open = true;
			}
		}
		if (Reverse)
		{
			Open = !Open;
		}
		if (LastOpen != Open)
		{
			LastOpen = Open;
			platforms.End = false;
		}
	}
}
