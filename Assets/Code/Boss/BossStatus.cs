using System.Collections;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    [Header("Boss")]
    public int hp = 10;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float blinkDuration = 0.1f;

    [Header("Death")]
    public GameObject body;
    public GameObject coffinPrefab;

    private Color originalColor;
    private bool isDead;

    void Start()
    {
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;

        StartCoroutine(BlinkRoutine());

        if (hp <= 0)
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator BlinkRoutine()
    {
        if (spriteRenderer == null)
            yield break;

        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(blinkDuration);

        spriteRenderer.color = originalColor;
    }

    IEnumerator DieRoutine()
    {
        isDead = true;
        // หยุด Coroutine ทั้งหมดที่กำลังทำงาน
        StopAllCoroutines();

        // ปิดสคริปต์โจมตีและการเคลื่อนที่ทั้งหมด
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // ไม่ปิด BossStatus เอง
            {
                script.enabled = false;
            }
        }

        Vector3 deathPosition = transform.position;

        // ซ่อน body
        if (body != null)
        {
            body.SetActive(false);
        }

        // ปิด collider ของบอส
        Collider bossCollider = GetComponent<Collider>();

        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        // หยุดการเคลื่อนที่
        Rigidbody bossRb = GetComponent<Rigidbody>();

        if (bossRb != null)
        {
            bossRb.linearVelocity = Vector3.zero;
        }

        // สร้างโลง
        GameObject coffin = Instantiate(coffinPrefab,deathPosition,Quaternion.identity);

        // Collider ของโลง
        Collider coffinCollider = coffin.GetComponent<Collider>();

        if (coffinCollider != null)
        {
            coffinCollider.isTrigger = true;
        }

        // ฟิสิกส์ของโลง
        Rigidbody coffinRb = coffin.GetComponent<Rigidbody>();

        if (coffinRb != null)
        {
            coffinRb.useGravity = true;
            coffinRb.linearDamping = 0f;
            coffinRb.angularDamping = 0f;
            coffinRb.mass = 1f;

            coffinRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // เด้งขึ้น + สุ่มซ้ายขวา
            coffinRb.AddForce(Vector3.up * 8f + Vector3.right * Random.Range(-2f, 2f),ForceMode.Impulse);
        }

        // ทะลุพื้น
        GameObject ground =GameObject.FindGameObjectWithTag("Ground");

        if (coffinCollider != null && ground != null)
        {
            Collider groundCollider = ground.GetComponent<Collider>();

            if (groundCollider != null)
            {
                Physics.IgnoreCollision(coffinCollider,groundCollider,true);
            }
        }

        // รอ 1 frame
        yield return null;
        GameObject[] attacks = GameObject.FindGameObjectsWithTag("Boss1");
        foreach (GameObject attack in attacks)
        {
            Destroy(attack);
        }
        // ลบบอส
        Destroy(gameObject);
    }
}