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
    public string[] tagsToDestroy;

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

        // ปิดสคริปต์ทั้งหมดบนบอส ยกเว้น BossStatus เอง
        // ไม่ต้อง StopAllCoroutines() เพราะมันจะฆ่า DieRoutine ตัวนี้ด้วย
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
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
            bossRb.angularVelocity = Vector3.zero;
        }

        // สร้างโลง
        GameObject coffin = null;

        if (coffinPrefab != null)
        {
            coffin = Instantiate(
                coffinPrefab,
                deathPosition,
                Quaternion.identity
            );
        }

        if (coffin != null)
        {
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
                coffinRb.collisionDetectionMode =
                    CollisionDetectionMode.Continuous;

                // เด้งขึ้น + สุ่มซ้ายขวา
                coffinRb.AddForce(
                    Vector3.up * 8f +
                    Vector3.right * Random.Range(-2f, 2f),
                    ForceMode.Impulse
                );
            }

            // ให้โลงทะลุพื้น
            GameObject ground =
                GameObject.FindGameObjectWithTag("Ground");

            if (ground != null && coffinCollider != null)
            {
                Collider groundCollider =
                    ground.GetComponent<Collider>();

                if (groundCollider != null)
                {
                    Physics.IgnoreCollision(
                        coffinCollider,
                        groundCollider,
                        true
                    );
                }
            }
        }

        // รอ 1 วินาที
        yield return null;

        // ลบวัตถุตาม Tag ที่กำหนด
        if (tagsToDestroy != null)
        {
            foreach (string tag in tagsToDestroy)
            {
                if (string.IsNullOrEmpty(tag))
                    continue;

                GameObject[] objects =
                    GameObject.FindGameObjectsWithTag(tag);

                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
            }
        }

        // ลบบอส
        Destroy(gameObject);
    }
}