using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour
{
	public float UsedTime = 60;
	public int Level;
	public bool Editor;
	public bool Playing;
	public bool End = false;
	public static event Action GameLoop;

	[SerializeField] private List<List<GameObject>> DimentionHolder = new();
	private NewUI StopUI;
	public GameObject Canvas;
	public GameObject Canvas1;
	private Actions PlayerActions;
	private Text Timer;
	private bool CurrentDimention = false;
	private void OnEnable()
	{
		Actions.ModifyTime += UseTimer;
	}
	private void OnDisable()
	{
		Actions.ModifyTime -= UseTimer;
	}
	private void Start()
	{
		Level = SceneManager.GetActiveScene().buildIndex;
		Playing = (Level != 0);
		if (Playing)
		{
			GameObject Player = GameObject.FindGameObjectWithTag("Player");
			PlayerActions = Player.GetComponent<Actions>();
			Canvas = GameObject.FindGameObjectWithTag("Canvas");
			Canvas1 = GameObject.FindGameObjectWithTag("Canvas1");
			GameObject Time = Canvas.transform.GetChild(2).GetChild(0).gameObject;
			if(UsedTime != -1f)
			{
				Timer = Time.GetComponent<Text>();
			}else if (UsedTime == -1f)
			{
				Time.SetActive(false);
			}
			StopUI = Canvas1.GetComponent<NewUI>();
			Canvas.transform.GetChild(3).gameObject.SetActive(false);
			GetDimention();
		}
	}
	private void Update()
	{
		if (!End || Editor)
		{
			//UseTimer(1);
			if (Input.GetKeyUp(KeyCode.Escape) && Level != 0)
			{
				Stop();
			}
			if (Playing)
			{
				GameLoop.Invoke();
			}
		}
		else if (End && Playing)
		{
			Stop();
		}
	}
	private void GetDimention()
	{
		List<GameObject> Present = new();
		List<GameObject> Past = new();
		GameObject.FindGameObjectsWithTag("Dimention 1", Present);
		GameObject.FindGameObjectsWithTag("Dimention 2", Past);
		DimentionHolder.Add(Present);
		DimentionHolder.Add(Past);
		HideDimention();
	}
    public void HideDimention()
    {
        CurrentDimention = !CurrentDimention;
        foreach (List<GameObject> Dimention in DimentionHolder)
        {
            foreach (GameObject Object in Dimention)
            {
                if (Object.transform.childCount == 3 && Object.transform.GetChild(2).childCount == 1)
                {
                    Object.transform.GetChild(2).GetChild(0).SetParent(null);
                }
				Hide(Object);
            }
            CurrentDimention = !CurrentDimention;
        }
        PlayerActions.TimePoverUp();
    }
   	private void Stop()
	{
		StopUI.ShowPause();
	}
	private void UseTimer(float Mod)
	{
		if (UsedTime > 0f && Playing)
		{
			UsedTime -= Time.deltaTime * Mod;
			string Minut = ((int)(UsedTime / 60)).ToString();
			int Second = (int)(UsedTime % 60);
			string StrSecond = Second.ToString();
			if(Second < 10)
			{
				StrSecond = "0" + StrSecond;
			}
			Timer.text = $"{Minut}:{StrSecond}";
		}
		else if ((int)UsedTime == 0) 
		{
			End = true;
		}
	}
	private async Task Hide( GameObject Object)
	{
        if (CurrentDimention)
        {
            Object.SetActive(true);
            MeshRenderer material = Object.GetComponent<MeshRenderer>();
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            material.GetPropertyBlock(propertyBlock);
            
            for (float i = 1; i > -1; i -= 0.1f)
			{
                propertyBlock.SetFloat("_Dissolve_Amount", i);
                material.SetPropertyBlock(propertyBlock);
                await System.Threading.Tasks.Task.Delay(50);
            }
			
        }
        else
        {
            MeshRenderer material = Object.GetComponent<MeshRenderer>();
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            material.GetPropertyBlock(propertyBlock);
            
            for (float i = -1; i < 1; i += 0.1f)
            {
                propertyBlock.SetFloat("_Dissolve_Amount", i);
                material.SetPropertyBlock(propertyBlock);
                await System.Threading.Tasks.Task.Delay(50);
            }
            Object.SetActive(false);

        }
    }
}