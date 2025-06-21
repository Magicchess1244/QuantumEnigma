using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ObjectProrg : MonoBehaviour
{
	public bool Scale;
	public bool Gravity;
	public bool Teleport;

	private float SelfDestructTimer = 7;
	private float mass = 5;
	private	float ScaleAmount;
	private	float ScaleDir;
	private bool Filp = false;
	private bool Update = false;
	private Rigidbody rb;
	private Camerarotation CamRotation;
	private Actions Actions;

	private float TpTimer = 15;
	private GameObject Camera;
	private Image Image;
	private Text text;

	private ObjectPropeties Propeties;

	private void OnEnable()
	{
		gameObject.isStatic = false;
		GlobalManager.GameLoop += FakeUpdate;
	}
	private void OnDisable()
	{
		GlobalManager.GameLoop -= FakeUpdate;
	}
	private void Start()
	{
		Propeties = GetComponent<ObjectPropeties>();
		rb = GetComponent<Rigidbody>();
		if (Gravity)
		{
			GameObject Player = GameObject.FindGameObjectWithTag("Player");
			GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
			CamRotation = Camera.GetComponent<Camerarotation>();
			Actions = Player.GetComponent<Actions>();
		}
		else if (Teleport)
		{
			Camera = GameObject.FindGameObjectWithTag("Canvas");
			Transform Ui = Camera.transform.GetChild(3);
			Image = Ui.GetChild(0).GetComponent<Image>();
			text = Ui.GetChild(1).GetComponent<Text>();
		}
	}
	public void IsClicked()
	{
        Propeties.ChargeAmount += Time.deltaTime;
		Propeties.Charging = true;
		if (Propeties.ChargeAmount >= 4 || !Propeties.Charge)
		{
			if (Scale)
			{
				ChangeSize();
			}
			else if (Gravity)
			{
				Update = true;
			}
			else if (Teleport)
			{
				Update = true;
				Image.transform.parent.gameObject.SetActive(true);
			}
		}
	}
	private void FakeUpdate()
	{
		GravityBomb();
		CheckPoint();
	}
	private void ChangeSize()
	{
		if (ScaleAmount == 4)
		{
			ScaleDir = -1;
		}
		else if (ScaleAmount == 1)
		{
			ScaleDir = 1;
		}

		ScaleAmount += 0.05f * ScaleDir;
		ScaleAmount = Mathf.Clamp(ScaleAmount, 1.0f, 4.0f);
		transform.localScale = new Vector3 (ScaleAmount, ScaleAmount, ScaleAmount);
		mass = ScaleAmount * 4;
		rb.mass = mass;
	}
	private async Task GravityBomb()
	{
		if (Update && Gravity)
		{
			if (!Filp)
			{
				CamRotation.X = 180;
				CamRotation.Flip *= -1;
				Actions.Flip *= -1;
				Filp = true;
			}
			else
			{
				SelfDestructTimer -= Time.deltaTime;
				if (SelfDestructTimer <= 0)
				{
                    //await Hide();
                    CamRotation.Flip *= -1;
					Actions.Flip *= -1;
					CamRotation.X = 0;
					Actions.ChangeGlavity(1);
                    Destroy(gameObject);
                }
            }
		}
	}
	private async Task CheckPoint()
	{
		if (Update && Teleport)
		{
			TpTimer -= Time.deltaTime;
			Image.fillAmount = TpTimer / 15;
			text.text = ((int)TpTimer).ToString();
			if (TpTimer <= 0)
			{
                await Hide();
                Image.transform.parent.gameObject.SetActive(false);
				GameObject Player = GameObject.FindGameObjectWithTag("Player");
				Player.transform.position = transform.position;
				Destroy(gameObject);
            }
        }
	}
	private async Task Hide()
	{
		MeshRenderer material = transform.GetChild(0).GetComponent<MeshRenderer>();
		MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
		material.GetPropertyBlock(propertyBlock);

		for (float i = -1; i < 1; i += 0.1f)
		{
			propertyBlock.SetFloat("_Dissolve_Amount", i);
			material.SetPropertyBlock(propertyBlock);
			await System.Threading.Tasks.Task.Delay(50);
		}
	}
}
