using UnityEngine;

public class Camerarotation : MonoBehaviour
{
	public GlobalManager Manager;
	public float senX = 10;
	public float senY = 10;
	public float X;
	public int Flip = 1;

	private GameObject rd;
	private float Xrotation;
	private float Yrotation;
	private void OnEnable()
	{
		GlobalManager.GameLoop += Camera;
	}
	private void OnDisable()
	{
		GlobalManager.GameLoop -= Camera;
	}
	private void Start()
	{
		Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalManager>();
		rd = GameObject.FindGameObjectWithTag("Player");
        Xrotation = transform.localEulerAngles.x;
        Yrotation = transform.localEulerAngles.y;
    }
	private void Update()
	{
		if (Manager.Playing)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
	private void Camera()
	{
		float MouseX = Input.GetAxisRaw("Mouse X") * senX * Flip;
		float MouseY = Input.GetAxisRaw("Mouse Y") * senY;

		Yrotation += MouseX;
		Xrotation -= MouseY;

		Xrotation = Mathf.Clamp(Xrotation, -85f, 90f);

		transform.localEulerAngles = new Vector3(Xrotation + X, Yrotation, 0);
		rd.transform.localEulerAngles = new Vector3(X, Yrotation, 0);
	}
}