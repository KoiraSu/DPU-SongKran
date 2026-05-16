using System.Collections;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    public Transform player;
    public BossFly bossFly;

    [Header("Tongue")]
    public float tongueLength = 1f;
   
    [Header("Body Swap")]
    public GameObject body1;       // ตัวปกติของบอส (ในฉาก)
    public GameObject body2Prefab; // ตัวหัว/หน้าผากที่จะถูกสร้างขึ้นตอนใช้สกิล

    private GameObject body2Instance;
    [Header("Prefabs")]
    public GameObject warningArrow;
    public GameObject tonguePrefab;

    [Header("References")]
    public Transform firePoint;

    [Header("Timing")]
    public float warningTime = 0.7f;      // เวลาที่ลูกศรหมุนตามผู้เล่น
    public float lockDelay = 1.8f;        // ล็อกเป้าแล้ว รอก่อนยิงจริง
    public float tongueDuration = 0.3f;   // เวลาที่ลิ้นค้างอยู่
    public float attackCooldown = 1f;     // พักหลังใช้สกิล

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
        // หยุดการเคลื่อนที่ของบอส
        if (bossFly != null)
        {
            bossFly.canMove = false;

            if (bossFly.rb != null)
            {
                bossFly.rb.linearVelocity = Vector3.zero;
            }
        }
        // ใส่ไว้ตอนเริ่ม Attack() หลังจากหยุดการเคลื่อนที่ของบอส

        // ===== สลับร่าง =====
        if (body1 != null)
        {
            body1.SetActive(false);
        }

        if (body2Prefab != null)
        {
            body2Instance = Instantiate(
                body2Prefab,
                firePoint.position,
                firePoint.rotation
            );
        }

        // ===== WARNING : หมุนตามผู้เล่นแบบ real-time =====
        GameObject warning = Instantiate(warningArrow,firePoint.position,Quaternion.identity);

        float timer = 0f;

        while (timer < warningTime)
        {
            timer += Time.deltaTime;

            Vector3 direction = (player.position - firePoint.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            warning.transform.position = firePoint.position;
            warning.transform.rotation = Quaternion.Euler(0, 0, angle);

            warning.SetActive(!warning.activeSelf);

            yield return new WaitForSeconds(0.1f);
        }

        // ===== ล็อกตำแหน่งสุดท้าย =====
        Vector3 targetPosition = player.position;
        Vector3 directionToTarget = targetPosition - firePoint.position;

        float distance = directionToTarget.magnitude;

        float finalAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // ลูกศรค้างไว้ให้ผู้เล่นเห็นว่าถูกล็อกแล้ว
        warning.SetActive(true);
        warning.transform.position = firePoint.position;
        warning.transform.rotation = Quaternion.Euler(0, 0, finalAngle);

        // รอก่อนยิงจริง
        yield return new WaitForSeconds(lockDelay);

        Destroy(warning);

        // ===== สร้างลิ้น =====
        GameObject tongue = Instantiate(tonguePrefab,firePoint.position,Quaternion.Euler(0, 0, finalAngle));

        // ปรับความยาวให้ถึงตำแหน่งที่ล็อกไว้พอดี
        Vector3 scale = tongue.transform.localScale;
        scale.x = distance / tongueLength;
        tongue.transform.localScale = scale;

        // ตั้งดาเมจ
        TongueHit hit = tongue.GetComponent<TongueHit>();
        if (hit != null)
        {
            hit.damage = 1;
        }

        Destroy(tongue, tongueDuration);

        // รอให้ลิ้นค้างอยู่
        yield return new WaitForSeconds(tongueDuration);

        // พักหลังใช้สกิล
        yield return new WaitForSeconds(attackCooldown);
        // ใส่หลังจาก Destroy(tongue, tongueDuration);
        // และหลังจาก yield return new WaitForSeconds(tongueDuration);

        // ===== กลับเป็นร่างเดิม =====
        if (body2Instance != null)
        {
            Destroy(body2Instance);
        }

        if (body1 != null)
        {
            body1.SetActive(true);
        }

        // กลับมาเคลื่อนที่ต่อ
        if (bossFly != null)
        {
            bossFly.canMove = true;
        }
    }
}