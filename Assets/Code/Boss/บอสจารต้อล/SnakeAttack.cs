using UnityEngine;

// งูที่ไม่แคร์โลก ชนอะไรก็วิ่งต่อ ยกเว้นตกน้ำก็จบชีวิตตามบท
public class SnakeAttack : MonoBehaviour
{
    //แก้แล้ว
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

    private void Update()
    {
        if (rb == null) return;

        // เคลื่อนที่แบบ deltaTime ตรง ๆ
        transform.position += new Vector3(
            direction * moveSpeed * Time.deltaTime,
            0f,
            0f
        );
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
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground2"))
        {
            Destroy(gameObject);
        }
    }
}