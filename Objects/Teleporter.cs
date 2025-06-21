using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    public bool IsPlaced = false;
    public float SeflDestrucTimer = 15;
    private float Timer = 0;
    private GameObject Camera;
    private Transform Ui;
    private void OnEnable()
    {
        GlobalManager.GameLoop += CheckPoint;
    }
    private void OnDisable()
    {
        GlobalManager.GameLoop -= CheckPoint;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !IsPlaced)
        {
            other.gameObject.GetComponent<Actions>().Teleport++;
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("Canvas");
        Ui = Camera.transform.GetChild(4);
    }
    private void CheckPoint()
    {
        if (IsPlaced)
        {
            Timer += Time.deltaTime;
            Ui.GetChild(0).GetComponent<Image>().fillAmount = (SeflDestrucTimer - Timer) / SeflDestrucTimer;
            Ui.GetChild(1).GetComponent<Text>().text = (SeflDestrucTimer - (int)Timer).ToString();
            if (Timer > SeflDestrucTimer) 
            {
                Ui.gameObject.SetActive(false);
                GameObject Player = GameObject.FindGameObjectWithTag("Player");
                Player.GetComponent<Actions>().TeleportPlaced = false;
                Player.transform.position = transform.position;
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
