using UnityEngine;

public class Bullet : MonoBehaviour
{
    //แก้แล้ว
    public int damage = 1;
    public float lifeTime = 2f;
    //public AudioSource audi;
    //public AudioClip AttackSound;

    void Start()
    {
        //audi = GetComponent<AudioSource>();
        Destroy(gameObject, lifeTime);
        //audi.PlayOneShot(AttackSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ชนกับ " + other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("โดนศัตรู");

            BossStatus boss = other.gameObject.GetComponent<BossStatus>();

            if (boss != null)
            {
                Debug.Log("ลดเลือด");

                boss.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}