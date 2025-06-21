using UnityEngine;

public class CameraPos : MonoBehaviour
{
	private GameObject Position;
	public bool Separate;
	public bool Animation;
	void Start()
	{
		Position = transform.parent.GetChild(1).gameObject;
    }
    void Update()
	{
		if (Animation)
		{
			transform.parent.GetComponent<Animator>().enabled = false;
			Animation = false;
        }
		if (Separate)
		{
			transform.SetParent(null);
			Separate = false;
            transform.eulerAngles = Vector3.zero;
        }
        transform.position = Position.transform.position;
	}
}
