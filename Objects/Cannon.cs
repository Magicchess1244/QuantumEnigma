using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject Bullet;
    public float ShootRate;
    private float Timer;
    private Vector3 Spawn;

    private void OnEnable()
    {
        GlobalManager.GameLoop += Shoot;
    }
    private void OnDisable()
    {
        GlobalManager.GameLoop -= Shoot; 
    }
    private void Start()
    {
        Spawn = transform.GetChild(1).position;
    }
    private void Shoot()
    {
        Timer += Time.deltaTime;
        if (Timer >= ShootRate)
        {
            Timer = 0;
            Instantiate(Bullet, Spawn, transform.rotation);
        }
    }
}
