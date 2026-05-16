using UnityEngine;

// งูที่ไม่แคร์โลก ชนอะไรก็วิ่งต่อ ยกเว้นตกน้ำก็จบชีวิตตามบท
public class SnakeAttack : MonoBehaviour
{
    public int damage = 1;
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private float direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        // สุ่มซ้ายหรือขวา
        direction = Random.value > 0.5f ? 1f : -1f;

        // พลิกสเกลให้หันตามทิศ
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    // ตั้งความเร็วทุกเฟรมฟิสิกส์ ไม่ให้การชนทำให้หยุด
    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(
                direction * moveSpeed,
                rb.linearVelocity.y,   // เก็บแรงโน้มถ่วงไว้
                0f
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // ไม่ทำอะไรเพิ่มเติม งูจะวิ่งต่อเพราะ FixedUpdate บังคับความเร็วตลอดเวลา
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }
}