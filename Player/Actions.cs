using System;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
	[Header("-------- Gravity Bomb ---------")]
	public GameObject BombObject;
	public bool Bomb = false;
	public float Flip = -1;
	private float Gravity = 9.81f;
	private float GravitySpeed = 1;
	private readonly float MaxGravity = 1.4f;
	private readonly float MinGravity = 0.5f;

    [Header("-------- Teleport ---------")]
    public int Teleport = 0;
	public bool TeleportPlaced = false;

    [Header("-------- Random ---------")]
    private GlobalManager GlobalManager;
	private ObjectPropeties Propeties;
	private Player_Movement Player_Movement;
	private GameObject Camera;
	private Rigidbody Rb;
	private Collider CubeCollider;
	private GameObject Colliding;



    [Header("-------- Change Time ---------")]
	public float SpeedUp;
	public float Slowdown;
	public static event Action<float> ModifyTime;
	private float TimeFillAmount = 0.1f;
	private readonly float TimeTotalFillAmount = 10;
	private Image TimeBar;

    [Header("-------- Health ---------")]
    public Image Health;
	public float HealthFillAmount = 10;
	private readonly float HealthTotalFillAmount = 10;

	[Header("-------- Grab ---------")]
	public LayerMask GrabLayers;
    public bool Connected = false;
	private GameObject ConnectedObj;
	public Rigidbody ConnectedObjRd;
	public float GrapSpeed = 3;

	private void OnEnable()
	{
		GlobalManager.GameLoop += Powers;
	}
	private void OnDisable()
	{
		GlobalManager.GameLoop -= Powers;
	}
	private void Start()
	{
		GlobalManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalManager>();
		Camera = GameObject.FindGameObjectWithTag("MainCamera");
		GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
		Health = Canvas.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>();
		TimeBar = Canvas.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
		Rb = GetComponent<Rigidbody>();
		Player_Movement = GetComponent<Player_Movement>();
	}
	private void Powers()
	{
		Pressed();
		Conected();
		TimePoverUp();
		FillHealthBar();
	}
	private void Pressed()
	{
		bool Left = Input.GetButton("Fire1");
		bool Right = Input.GetButton("Fire2");
		bool UpLeft = Input.GetButtonDown("Fire1");
		bool Shift = Input.GetKey(KeyCode.LeftShift);
		bool Control = Input.GetKey(KeyCode.LeftControl);

        if (Left || Right) 
		{
			if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hitInfo, 3.3f))
			{
				if (hitInfo.collider.gameObject.TryGetComponent<ObjectPropeties>(out ObjectPropeties Propeties))
				{
					if (Propeties.Charge && Shift && Right && Left)
					{
						TimeFillAmount -= Time.deltaTime * 2;
						if (hitInfo.transform.gameObject.TryGetComponent<ObjectProrg>(out ObjectProrg Prog))
						{
							Prog.IsClicked();
						}
						else if (hitInfo.transform.gameObject.TryGetComponent<FisicalButton>(out FisicalButton Button)) 
						{
							Button.Charge();
						}
					}
					else
					{
						Propeties.Charging = false;
						Propeties.ChargeAmount = 0;

						if (Propeties.CanFreeze && Right && UpLeft && !Control)
						{
							FreezeObj(Propeties, hitInfo.collider.gameObject);
						}
						else if (Right && !Connected)
						{
							Connected = true;
							ConnectedObj = hitInfo.collider.gameObject; 
							ConnectedObjRd = hitInfo.rigidbody;
							CubeCollider = hitInfo.collider;
                            CubeCollider.excludeLayers = GrabLayers;
                        }
                    }
				}
			}
		}
		else if (Connected)
		{
            CubeCollider.excludeLayers = new LayerMask();
            Connected = false;
		}
	}
	private void Conected()
	{
		if ( Connected && ConnectedObjRd != null && !ConnectedObjRd.isKinematic)
		{
            float Dist = Vector3.Distance(Camera.transform.position, ConnectedObj.transform.position);
			Vector3 PlayerPos = (Camera.transform.position + Camera.transform.forward * (2.5f + (ConnectedObj.transform.localScale.x / 2)));
			ConnectedObjRd.linearVelocity = (PlayerPos - ConnectedObj.transform.position) * Mathf.Sqrt(Dist) * GrapSpeed;
			float CubeDist = Vector3.Distance(transform.position, PlayerPos);
            if (CubeDist > 3.1f)
            {
                CubeCollider.excludeLayers = GrabLayers;
            }
			else
			{
                CubeCollider.excludeLayers = new LayerMask();
            }
        }
	}
	private void FreezeObj(ObjectPropeties Propeties, GameObject Object)
	{
		Rigidbody Rb = Object.GetComponent<Rigidbody>(); 
		MeshRenderer mesh = Object.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
		int Id;
		if (Propeties.Freeze)
		{
			Propeties.Freeze = false;
			Rb.isKinematic = false;
			Id = 0;
		}
		else
		{
			Propeties.Freeze = true;
			Rb.isKinematic = true;
			Id = 1;
		}
		mesh.material = Propeties.Materials[Id];
	}
	public void TimePoverUp()
	{
		bool LeftControl = Input.GetKey(KeyCode.LeftControl);
		bool UpLeftControl = Input.GetKeyUp(KeyCode.LeftControl);
		bool LeftShift = Input.GetKey(KeyCode.LeftShift);
		bool UpLeftShift = Input.GetKeyUp(KeyCode.LeftShift);
		TimeFillAmount = Mathf.Clamp(TimeFillAmount, 0f, 10f);
		if (LeftControl && TimeFillAmount != 0)
		{
			GravitySpeed = Slowdown;
			FillTimeBar(-Time.deltaTime);
			ChangeGlavity(GravitySpeed);
			ModifyTime?.Invoke(GravitySpeed);
		}
		else if (LeftShift && TimeFillAmount != 10)
		{
			GravitySpeed = SpeedUp;
			FillTimeBar(Time.deltaTime);
			ChangeGlavity(GravitySpeed);
			ModifyTime?.Invoke(GravitySpeed);
		}
		else if (UpLeftControl || UpLeftShift || LeftControl && TimeFillAmount == 0 || LeftShift && TimeFillAmount == 10)
		{
			GravitySpeed = 1;
            ChangeGlavity(GravitySpeed);
            ModifyTime?.Invoke(1);
        }
        else
		{
            ModifyTime?.Invoke(1);
        }
    }
	private void FillTimeBar(float Amount)
	{
		TimeFillAmount += Amount;
		TimeBar.fillAmount = TimeFillAmount / TimeTotalFillAmount;
	}
	private void FillHealthBar()
	{
		Ray Ray = new Ray(transform.position, Vector3.down);
		float Fall = Rb.linearVelocity.y;
		float FallSpeed = (Fall * (GravitySpeed / 3)) + 9f;
		if (HealthFillAmount <= 0)
		{
			GlobalManager.End = true;
		}
		else if (Physics.SphereCast(Ray, 0.5f, 1.1f) && FallSpeed <= -0.9f)
		{
			HealthFillAmount += FallSpeed;
			Health.fillAmount = HealthFillAmount / HealthTotalFillAmount;
			print($"FallSpeed: {FallSpeed}");
			print($"Health: {HealthFillAmount}");
			print($"Fall: {Fall}");
		}
	}
	public void ChangeGlavity(float Amount)
	{
		Gravity = 9.8f * Mathf.Clamp(Amount, MinGravity, MaxGravity) * Flip;
		Physics.gravity = new Vector3(0, Gravity, 0);
	}
	private void OnCollisionEnter(Collision collision)
	{
		Colliding =  collision.gameObject;
	}
	private void OnCollisionExit(Collision collision)
	{
		Colliding = null;
	}
}
