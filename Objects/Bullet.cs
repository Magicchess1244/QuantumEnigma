using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float GravitySpeed;
    private Rigidbody rb;
    private void OnEnable()
    {
        GlobalManager.GameLoop += Move; 
    }
    private void OnDisable()
    {
        GlobalManager.GameLoop -= Move;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Move()
    {
        rb.linearVelocity = GravitySpeed * 10 * Time.deltaTime * transform.up;
    }
}
