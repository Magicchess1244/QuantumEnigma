using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    public GlobalManager Manager;
    private int num = 0;

    private void Start()
    {
        GameObject manager = transform.parent.transform.gameObject;
        for (int x = 0; x < manager.transform.childCount; x++)
        {
            if (manager.transform.GetChild(x) == gameObject.transform)
            {
                num = x + 1;
            }
        }
        Text Text = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        Text.text = num.ToString();
    }
    public void LoadScene()
    {
        Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalManager>();
        Manager.Playing = true;
        SceneManager.LoadScene($"Level {num}");
    }
}
