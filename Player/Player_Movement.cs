using UnityEngine;
public class Player_Movement : MonoBehaviour
{
    public float MaxCoyotiTime;
    public float CoyotiTime;
	private Rigidbody rb;
	private Actions Action;
	public Vector3 Direction;
	public bool Onground = true;
	private float GravitySpeed = 5;
	private float Walk = 5.0f;
	private float Sneek = 2.5f;
	private float jump = 5;
	private float run = 7.0f;
	private readonly float AirDrag = 0.75f;
	private void OnEnable()
	{
		GlobalManager.GameLoop += MovePlayer;
	}
	private void OnDisable()
	{
		GlobalManager.GameLoop -= MovePlayer;
	}
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		Action = GetComponent<Actions>();
	}
	private void MovePlayer()
	{
		Run();
		Move();
		Jump();
		rb.linearVelocity = Direction;
	}
	private void Move()
	{
		if (Onground)
		{
			GravitySpeed *= AirDrag;
		}
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");
		Vector3 DirectionPos = transform.right * x + transform.forward * z;
		Direction = GravitySpeed * DirectionPos.normalized;
	}
	private void Jump()
	{
		Ray Ray = new Ray(transform.position, -transform.up.normalized); 
		Onground = Physics.SphereCast(Ray, 0.4f, out RaycastHit Hit, 0.8f);
        OnCube(Hit);
		float FallingSpeed = rb.linearVelocity.y;
		bool JumpInput = Input.GetKey(KeyCode.Space);
		if (Onground)
		{
			CoyotiTime = MaxCoyotiTime;
		}
		else
		{
            CoyotiTime -= Time.deltaTime;
		}

		if (CoyotiTime > 0 && JumpInput) 
		{
            Direction.y = transform.up.normalized.y * jump;
			CoyotiTime = 0;
		}
		else
		{
			Direction.y = FallingSpeed;
		}
	}
	private void Run()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			GravitySpeed = Sneek;
		}
		else if (Input.GetKey(KeyCode.LeftControl))
		{
			GravitySpeed = run;
		}
		else
		{
			GravitySpeed = Walk;
		}
	}
	private void OnCube(RaycastHit HitInfo)
	{
		if (Action.ConnectedObjRd == HitInfo.rigidbody && Action.Connected)
		{
			Onground = false;
		}
	}
}
