// Spin.cs
using UnityEngine;

// พุ่งออกไปแล้วกลับมาจุดเริ่ม จากนั้นแจ้งเจ้าของว่า "โยนเสร็จแล้ว" ก่อนหายไปจากโลก
public class Spin : MonoBehaviour
{
    [Header("Positions")]
    public Vector2 startPosition = new Vector2(72f, 30f);
    public float targetX = -65f;
    public float minTargetY = 20f;
    public float maxTargetY = 70f;

    [Header("Movement")]
    public float moveSpeed = 50f;

    [Header("Rotation")]
    public float rotationSpeed = 180f;

    [HideInInspector] public NakYoneJan owner;

    private Vector3 currentTarget;
    private bool goingOut = true;
    public int damage = 1;
    void Start()
    {
        transform.position = new Vector3(
            startPosition.x,
            startPosition.y,
            transform.position.z
        );

        SetRandomTarget();
    }

    void Update()
    {
        // หมุนไปเรื่อย ๆ เพราะความสงบเป็นสิ่งหายาก
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // เคลื่อนที่
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            moveSpeed * Time.deltaTime
        );

        // ถึงเป้าหมาย
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (goingOut)
            {
                // กลับบ้าน
                currentTarget = new Vector3(
                    startPosition.x,
                    startPosition.y,
                    transform.position.z
                );
                goingOut = false;
            }
            else
            {
                // กลับถึงบ้านแล้ว แจ้งเจ้าของก่อนดับสูญ
                if (owner != null)
                {
                    owner.OnJanReturned();
                }

                Destroy(gameObject);
            }
        }
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
            
        }
    }
    void SetRandomTarget()
    {
        float randomY = Random.Range(minTargetY, maxTargetY);

        currentTarget = new Vector3(
            targetX,
            randomY,
            transform.position.z
        );
    }
}