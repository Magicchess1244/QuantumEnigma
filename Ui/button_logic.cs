using UnityEngine;

public class Button_logic : MonoBehaviour
{
    private GameObject Menu;
    private GameObject Online;
    private GameObject Level;
    private void Start()
    {
        Menu = transform.GetChild(1).gameObject;
        Level = transform.GetChild(2).gameObject;
        Online = transform.GetChild(3).gameObject;
        Hide();
        ShowMenu();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true) 
        {
            Hide();
            ShowMenu();
        }
    }
    public void EndGame()
    {
        Application.Quit();
    }
    public void Hide() 
    {
        Menu.SetActive(false);
        Level.SetActive(false);
        Online.SetActive(false);
    }
    public void ShowMenu() 
    {
        Menu.SetActive(true);
    }
    public void ShowOffline()
    {
        Level.SetActive(true);
    }
    public void ShowOnline()
    {
        Online.SetActive(true);
    }
}
