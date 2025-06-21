using UnityEngine;

public class SoundClip : MonoBehaviour
{
    public AudioClip Clip;
    private AudioSource audioSource;
    private bool Played = false;
    public bool IsBg = false;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (IsBg)
        {
            audioSource.clip = Clip;
            audioSource.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !Played)
        {
            audioSource.clip = Clip;
            audioSource.Play();
            Played = true;
        }
    }
}
