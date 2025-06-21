using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private int Level = 0;
    private void Start()
    {
        Level = SceneManager.GetActiveScene().buildIndex;
        print("Level: " + Level);
    }
    private void OnTriggerEnter(Collider other)
    {
        print("Level finished");
        if (other.CompareTag("Player"))
        {
            Level++;
            if (Level < SceneManager.sceneCountInBuildSettings) 
            {
                if (PlayerPrefs.GetInt("MaxLevel") <= Level) 
                {
                    PlayerPrefs.SetInt("MaxLevel", Level - 1);
                }
                SceneManager.LoadScene(Level);
            }
            else
            {
                print("You win !");
            }
        }
    }
}
