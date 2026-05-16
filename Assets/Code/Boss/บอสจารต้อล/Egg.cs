using System.Collections;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public Rigidbody rb;
    public Collider col;

    [Header("Spawn")]
    public GameObject snakePrefab;
    public int spawnCount = 15;
    public float timeBorn = 5f;

    [Header("Snake Movement")]
    public float snakeSpeed = 5f;

    [Header("Lifetime")]
    public float lifeTime = 3f;

    [Header("Water Effect")]
    public GameObject drownedPrefab;
    public GameObject body;

    [Header("HP")]
    public int hp = 100;

    private bool isCounting = false;
    private bool isDestroyed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // ถ้าไม่ตกพื้นภายในเวลาที่กำหนด ก็หายไป
        Destroy(gameObject, lifeTime);
    }

    // ใช้ Trigger แทน Collision เพื่อไม่ให้กระสุนผลักไข่จนลอยเหมือนวิญญาณติดบั๊ก
    private void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return;

        // แตะพื้น เริ่มฟัก
        if (other.CompareTag("Ground") && !isCounting)
        {
            StartCoroutine(BornSnake());
        }
        // โดนกระสุน
        else if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        // ตกน้ำ
        else if (other.CompareTag("Water"))
        {
            StartCoroutine(Drowned());
        }
    }

    IEnumerator BornSnake()
    {
        isCounting = true;

        // หยุดการเคลื่อนไหวให้เหมือนวางนิ่งบนพื้น
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // ยกเลิกการทำลายอัตโนมัติ เพราะตอนนี้กำลังฟัก
        CancelInvoke();

        // รอเวลาฟัก
        yield return new WaitForSeconds(timeBorn);

        if (isDestroyed) yield break;

        // สร้างงู
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject snake = Instantiate(
                snakePrefab,
                transform.position,
                Quaternion.identity
            );
        }

        isDestroyed = true;
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        hp -= damage;

        if (hp <= 0)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    IEnumerator Drowned()
    {
        if (isDestroyed) yield break;

        isDestroyed = true;

        // ซ่อนโมเดล
        if (body != null)
        {
            body.SetActive(false);
        }

        // ปิด Collider
        if (col != null)
        {
            col.enabled = false;
        }

        // หยุดฟิสิกส์
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // เอฟเฟกต์ตกน้ำ
        if (drownedPrefab != null)
        {
            GameObject effect = Instantiate(
                drownedPrefab,
                transform.position,
                Quaternion.identity
            );

            yield return new WaitForSeconds(0.2f);
            Destroy(effect);
        }

        Destroy(gameObject);
    }
}