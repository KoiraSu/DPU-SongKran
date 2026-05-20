using System.Collections;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    public Transform player;
    public BossFly bossFly;

    [Header("Tongue")]
    public float tongueLength = 1f;

    [Header("Body Swap")]
    public GameObject body1;
    public GameObject body2Prefab;

    private GameObject body2Instance;

    [Header("Prefabs")]
    public GameObject warningArrow;
    public GameObject tonguePrefab;

    [Header("References")]
    private Transform firePoint;

    [Header("Timing")]
    public float warningTime = 0.7f;
    public float lockDelay = 1.8f;
    public float tongueDuration = 0.3f;
    public float attackCooldown = 1f;

    void Start()
    {
        GameObject target =
            GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            player = target.transform;
        }
    }

    public IEnumerator Attack()
    {
        if (bossFly != null)
        {
            bossFly.canMove = false;

            if (bossFly.rb != null)
            {
                bossFly.rb.linearVelocity = Vector3.zero;
            }
        }

        // ===== สลับร่าง =====

        if (body1 != null)
        {
            body1.SetActive(false);
        }

        if (body2Prefab != null)
        {
            body2Instance = Instantiate(
                body2Prefab,
                transform.position,
                transform.rotation
            );

            // หา firePoint จาก prefab ที่สร้าง
            firePoint = body2Instance.transform.Find("Head/newfire");

            if (firePoint == null)
            {
                Debug.LogError("ไม่พบ newfire ใน prefab");
                yield break;
            }
        }

        // ===== WARNING =====

        GameObject warning = Instantiate(
            warningArrow,
            firePoint.position,
            Quaternion.identity
        );

        float timer = 0f;
        float blinkTimer = 0f;

        while (timer < warningTime)
        {
            timer += Time.deltaTime;
            blinkTimer += Time.deltaTime;

            Vector3 direction =
                (player.position - firePoint.position).normalized;

            float angle =
                Mathf.Atan2(direction.y, direction.x) *
                Mathf.Rad2Deg;

            warning.transform.position = firePoint.position;

            warning.transform.rotation =
                Quaternion.Euler(0, 0, angle);

            // กระพริบทุก 0.1 วิ
            if (blinkTimer >= 0.1f)
            {
                warning.SetActive(!warning.activeSelf);
                blinkTimer = 0f;
            }

            yield return null;
        }

        // ===== ล็อกตำแหน่ง =====

        Vector3 targetPosition = player.position;

        Vector3 directionToTarget =
            targetPosition - firePoint.position;

        float distance = directionToTarget.magnitude;

        float finalAngle =
            Mathf.Atan2(
                directionToTarget.y,
                directionToTarget.x
            ) * Mathf.Rad2Deg;

        warning.SetActive(true);

        warning.transform.position = firePoint.position;

        warning.transform.rotation =
            Quaternion.Euler(0, 0, finalAngle);

        // ===== รอล็อก =====

        float lockTimer = 0f;

        while (lockTimer < lockDelay)
        {
            lockTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(warning);

        // ===== สร้างลิ้น =====

        GameObject tongue = Instantiate(
            tonguePrefab,
            firePoint.position,
            Quaternion.Euler(0, 0, finalAngle)
        );

        Vector3 scale = tongue.transform.localScale;
        scale.x = distance / tongueLength;
        tongue.transform.localScale = scale;

        TongueHit hit = tongue.GetComponent<TongueHit>();

        if (hit != null)
        {
            hit.damage = 1;
        }

        // ===== เวลาลิ้น =====

        float tongueTimer = 0f;

        while (tongueTimer < tongueDuration)
        {
            tongueTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(tongue);

        // ===== คูลดาวน์ =====

        float cooldownTimer = 0f;

        while (cooldownTimer < attackCooldown)
        {
            cooldownTimer += Time.deltaTime;
            yield return null;
        }

        // ===== กลับร่าง =====

        if (body2Instance != null)
        {
            Destroy(body2Instance);
        }

        if (body1 != null)
        {
            body1.SetActive(true);
        }

        if (bossFly != null)
        {
            bossFly.canMove = true;
        }
    }
}