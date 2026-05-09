using UnityEngine;

public class WaterBallBullet : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 15f;
    public float rotateSpeed = 250f;
    public float gravityForce = 0f;

    [Header("Life")]
    public float lifeTime = 5f;
    public int hp = 1;

    private Rigidbody rb;
    private Transform player;
    private Vector3 currentDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject target =
            GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            player = target.transform;
            Debug.Log("หา Player เจอ = กำลังตาม");

            // เริ่มต้นหันไปหาผู้เล่นทันที
            currentDirection =
                (player.position - transform.position).normalized;
        }
        else
        {
            Debug.Log("ไม่เจอเป้าหมาย");

            // กันกรณีไม่เจอ Player
            currentDirection = transform.right;
        }

        // หมุนให้หน้าหันไปทางผู้เล่นตั้งแต่แรก
        float angle =
            Mathf.Atan2(currentDirection.y, currentDirection.x)
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0, 0, angle);

        // ไม่ต้อง AddForce พุ่งออกไปก่อนแล้ว
        // มนุษย์ชอบให้ลูกบอลโผล่มาแล้วเลี้ยวขวาแบบไร้เหตุผล
        // แต่เราเลิกทำแบบนั้นได้

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // หาทิศไปหาผู้เล่น
        Vector3 targetDirection =
            (player.position - transform.position).normalized;

        // ค่อย ๆ หมุนตาม
        currentDirection = Vector3.RotateTowards(
            currentDirection,
            targetDirection,
            rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime,
            0f
        );

        // หมุน sprite
        float angle =
            Mathf.Atan2(currentDirection.y, currentDirection.x)
            * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0, 0, angle);

        // พุ่งไปหาผู้เล่น
        rb.linearVelocity =
            currentDirection * moveSpeed;

        // กันตก (ถ้า Use Gravity เปิดอยู่)
        rb.AddForce(
            Vector3.up * gravityForce,
            ForceMode.Acceleration
        );

        Debug.DrawRay(
            transform.position,
            currentDirection * 3f,
            Color.cyan
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("ลูกโป่งโดนยิง");

            TakeDamage(1);

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("ลูกโป่งชนผู้เล่น");

            Player playerScript =
                other.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.TakeDamage(1);
                Debug.Log("ผู้เล่นโดนดาเมจ 1");
            }

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        Debug.Log("HP เหลือ = " + hp);

        if (hp <= 0)
        {
            Debug.Log("ลูกโป่งแตก");
            Destroy(gameObject);
        }
    }
}