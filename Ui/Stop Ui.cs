using UnityEngine;
using UnityEngine.SceneManagement;

public class StopUi : MonoBehaviour
{
    private GlobalManager GlobalManager;
    private void Start()
    {
        GlobalManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalManager>();
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Resume()
    {
        GlobalManager.Playing = true;
        gameObject.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
