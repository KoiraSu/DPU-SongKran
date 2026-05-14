using System.Collections;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    private Rigidbody rb;
    private Transform player;

    [Header("Dash Area")]
    public float leftX = -50f;
    public float rightX = 50f;
    public float dashY = 30f;

    [Header("Dash Settings")]
    public float moveSpeed = 25f;
    public float dashSpeed = 80f;
    public float prepareTime = 0.5f;
    public float attackCooldown = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject target = GameObject.FindGameObjectWithTag("Player");
    }
    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        // หา Player อัตโนมัติ ถ้ายังไม่ได้ลากใส่ Inspector
        if (player == null)
        {
            GameObject target =
                GameObject.FindGameObjectWithTag("Player");

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

        // หยุดก่อน
        rb.linearVelocity = Vector3.zero;

        // =========================
        // เลือกฝั่งเริ่มตามตำแหน่ง Player
        // =========================
        float startX;

        if (player.position.x < 0f)
        {
            // Player อยู่ฝั่งซ้าย -> บอสไปเริ่มฝั่งขวา
            startX = rightX;
        }
        else if (player.position.x > 0f)
        {
            // Player อยู่ฝั่งขวา -> บอสไปเริ่มฝั่งซ้าย
            startX = leftX;
        }
        else
        {
            // Player อยู่ตรงกลาง -> สุ่ม
            startX = Random.value > 0.5f ? rightX : leftX;
        }

        // ปลายทางคืออีกฝั่ง
        float endX = (startX == rightX) ? leftX : rightX;

        Vector3 startPos = new Vector3(startX, dashY, 0f);
        Vector3 endPos = new Vector3(endX, dashY, 0f);

        // =========================
        // ย้ายไปจุดเริ่ม
        // =========================
        while (Vector3.Distance(transform.position, startPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPos,
                moveSpeed * Time.deltaTime
            );

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                0f
            );

            yield return null;
        }

        // จัดตำแหน่งให้เป๊ะ
        transform.position = startPos;
        rb.position = startPos;
        rb.linearVelocity = Vector3.zero;

        // รอชาร์จก่อนพุ่ง
        yield return new WaitForSeconds(prepareTime);

        // =========================
        // พุ่งตรงไปอีกฝั่ง
        // =========================
        Vector3 dashDirection =
            (endPos - startPos).normalized;

        rb.linearVelocity = dashDirection * dashSpeed;

        while (Vector3.Distance(transform.position, endPos) > 0.5f)
        {
            rb.position = new Vector3(
                rb.position.x,
                dashY,
                0f
            );

            yield return new WaitForFixedUpdate();
        }

        // หยุดทันที
        rb.linearVelocity = Vector3.zero;
        rb.position = endPos;

        // คูลดาวน์
        yield return new WaitForSeconds(attackCooldown);
    }
}