using UnityEngine;

public class BallFire : MonoBehaviour
{
    public Rigidbody rb;
    public int damage = 1;
    public float speed = 20f;
    public float lifeTime = 2f;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    void Start()
    {
        Destroy(gameObject, lifeTime);

    }
    public void Fire(Vector3 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}