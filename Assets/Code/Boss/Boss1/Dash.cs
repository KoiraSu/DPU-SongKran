using System.Collections;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public Rigidbody rb;
    public Transform player;

    [Header("Dash Area")]
    public float leftX = -50f;
    public float rightX = 50f;
    public float dashY = 30f;

    [Header("Dash Settings")]
    public float moveSpeed = 25f;
    public float dashSpeed = 80f;
    public float prepareTime = 0.5f;
    public float attackCooldown = 1f;

    [Header("Damage")]
    public int damage = 1;

    [Header("Knockback")]
    public float horizontalKnockbackForce = 30f;
    public float verticalKnockbackForce = 3f;


    private bool isDashing = false;
    // เก็บทิศทางที่กำลังพุ่ง
    private Vector3 currentDashDirection = Vector3.zero;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (player == null)
        {
            GameObject target = GameObject.FindGameObjectWithTag("Player");

            if (target != null)
            {
                player = target.transform;
            }
        }
    }

    public IEnumerator Attack()
    {
        if (rb == null)
        {
            Debug.LogError("DashAttack: หา Rigidbody ไม่เจอ");
            yield break;
        }

        if (player == null)
        {
            Debug.LogError("DashAttack: หา Player ไม่เจอ");
            yield break;
        }
        isDashing = false;
        rb.linearVelocity = Vector3.zero;

        // เลือกฝั่งเริ่มตามตำแหน่ง Player
        float startX;

        if (player.position.x < 0f)
        {
            startX = rightX;// Player อยู่ฝั่งซ้าย -> บอสไปเริ่มฝั่งขวา
        }
        else if (player.position.x > 0f)
        {
            startX = leftX;// Player อยู่ฝั่งขวา -> บอสไปเริ่มฝั่งซ้าย
        }
        else
        {
            startX = Random.value > 0.5f ? rightX : leftX;// Player อยู่ตรงกลาง -> สุ่ม
        }

        float endX = (startX == rightX) ? leftX : rightX;// ปลายทางคืออีกฝั่ง

        Vector3 startPos = new Vector3(startX, dashY, 0f);
        Vector3 endPos = new Vector3(endX, dashY, 0f);

        // ย้ายไปจุดเริ่ม
        while (Vector3.Distance(transform.position, startPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position,startPos,moveSpeed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x,transform.position.y,0f);

            yield return null;
        }

        // จัดตำแหน่ง
        transform.position = startPos;
        rb.position = startPos;
        rb.linearVelocity = Vector3.zero;

        // รอชาร์จก่อนพุ่ง
        yield return new WaitForSeconds(prepareTime);

        // พุ่งตรงไปอีกฝั่ง
        currentDashDirection = (endPos - startPos).normalized;
        isDashing = true;

        rb.linearVelocity = currentDashDirection * dashSpeed;

        while (Vector3.Distance(transform.position, endPos) > 0.5f)
        {
            rb.position = new Vector3(rb.position.x,dashY,0f);

            yield return new WaitForFixedUpdate();
        }

        // หยุดทันที
        isDashing = false;
        rb.linearVelocity = Vector3.zero;
        rb.position = endPos;

        // คูลดาวน์
        yield return new WaitForSeconds(attackCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ทำดาเมจเฉพาะตอนกำลังพุ่ง
        if (!isDashing) return;

        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();

            if (playerScript != null)
            {
                // ลดเลือด
                playerScript.TakeDamage(damage);
            }

            // ทำให้ผู้เล่นกระเด็น
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // ล้างความเร็วเดิมก่อน
                playerRb.linearVelocity = Vector3.zero;

                // ===== แก้ตรงนี้ =====

                // เอาเฉพาะแรงแนวนอน
                Vector3 dir = currentDashDirection;
                dir.y = 0f;
                dir.Normalize();
                Vector3 knockback = dir * horizontalKnockbackForce + Vector3.up * verticalKnockbackForce;
                // ใส่แรงกระเด็น
                playerRb.AddForce(knockback, ForceMode.Impulse);
            }
        }
    }
}