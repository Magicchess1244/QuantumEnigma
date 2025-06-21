using System.Collections.Generic;
using UnityEngine;

public class FisicalButton : MonoBehaviour
{
	public List<Material> materials;
	public float Timer = 0f;
	private float MaxTimer = 1f;
    public bool Buffer = false;
	public bool Open = false;
	public bool Hold = false;
	public bool TimeButton = false;
	public float ChargeAmoutMax = 4;
	private GlobalManager GlobalManager;
	private ObjectPropeties Propeties;
	private MeshRenderer mesh;
	private Animator Animator;
	private void OnEnable()
	{
		gameObject.isStatic = false;
	}
	private void Start()
	{
		Propeties = GetComponent<ObjectPropeties>();
		mesh = GetComponent<MeshRenderer>();
		Animator = GetComponent<Animator>();

		if (TimeButton)
		{
			mesh.material = materials[0];
			GlobalManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalManager>();
		}
		else if (Propeties.Charge)
		{
			Hold = true;
			mesh.material = materials[1];
		}
	}
	private void Update()
	{
		Timer += Time.deltaTime;
		if (Timer > MaxTimer)
		{
			Buffer = true;
			print("Buffer: " + Buffer);
        }
		else
		{
            Buffer = false;
        }
	}
	private void OnCollisionEnter(Collision other)
	{
		if (!Propeties.Charge && Buffer)
		{
			Timer = 0f;
			Activate(!Open);
		}
	}
	private void OnCollisionExit(Collision other)
	{
		if (!Propeties.Charge && Buffer) 
		{
			if (Hold)
			{
				Timer = 0f;
				Activate(false);
			}
		}
	}
	private void Activate( bool Bool)
	{
		Open = Bool;
		Animator.SetBool("Open", Open);
		if (TimeButton)
		{
			GlobalManager.HideDimention();
		}
	}
	public void Charge()
	{
		Propeties.Charging = true;
		Propeties.ChargeAmount += Time.deltaTime;
		if (Propeties.ChargeAmount >= ChargeAmoutMax)
		{
			Activate(true);
		}
	}
}
