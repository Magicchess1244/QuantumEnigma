using UnityEngine;
using UnityEngine.SceneManagement;

public class Gobacktomenu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
