using System.Collections;
using UnityEngine;

public class Skill2 : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject eggPrefab;

    [Header("Spawn Point")]
    public Transform spawnEgg;

    [Header("Head")]
    public Transform head;                  // หัวที่ต้องการหมุน
    public MonoBehaviour lookScript;        // สคริปต์ที่ทำให้หัวมองตามผู้เล่น

    [Header("Shoot Settings")]
    public int eggCount = 5;
    public float delayBetweenEggs = 0.3f;

    [Header("Landing Range (X)")]
    public float minX = -63f;
    public float maxX = 15f;

    [Header("Projectile Settings")]
    [Tooltip("สุ่มความเร็วต่ำสุด")]
    public float minLaunchSpeed = 20f;

    [Tooltip("สุ่มความเร็วสูงสุด")]
    public float maxLaunchSpeed = 45f;

    public float gravity = 9.81f;

    public IEnumerator Attack()
    {
        Debug.Log("Skill2");

        // ปิดสคริปต์ที่ทำให้หัวมองตามผู้เล่น
        if (lookScript != null)
        {
            lookScript.enabled = false;
        }

        for (int i = 0; i < eggCount; i++)
        {
            bool success = false;

            // พยายามสุ่มจนกว่าจะได้ค่าที่ยิงถึง
            for (int attempt = 0; attempt < 20; attempt++)
            {
                // สุ่มตำแหน่งที่จะให้ไข่ตก
                float targetX = Random.Range(minX, maxX);

                // สุ่มความเร็วต้น
                float launchSpeed =
                    Random.Range(minLaunchSpeed, maxLaunchSpeed);

                // จุดเริ่มและจุดตก
                Vector3 start = spawnEgg.position;
                Vector3 targetPos = new Vector3(
                    targetX,
                    0f,
                    start.z
                );

                // ระยะ
                float dx = targetPos.x - start.x;
                float dy = targetPos.y - start.y;
                float R = Mathf.Abs(dx);

                // ใกล้เกินไปจนสูตรรวน
                if (R < 0.01f)
                {
                    continue;
                }

                // ตรวจว่าความเร็วนี้ยิงถึงหรือไม่
                float v2 = launchSpeed * launchSpeed;

                float discriminant =
                    v2 * v2
                    - gravity *
                    (gravity * R * R + 2f * dy * v2);

                if (discriminant < 0f)
                {
                    continue;
                }

                // คำนวณมุมยิงแบบวิถีสูง
                float sqrtD = Mathf.Sqrt(discriminant);

                float angleRad = Mathf.Atan(
                    (v2 + sqrtD) / (gravity * R)
                );

                float angleDeg =
                    angleRad * Mathf.Rad2Deg;

                // แยกความเร็ว
                float vx =
                    launchSpeed * Mathf.Cos(angleRad);

                float vy =
                    launchSpeed * Mathf.Sin(angleRad);

                // ถ้ายิงไปทางซ้าย
                if (dx < 0f)
                {
                    vx = -vx;
                    angleDeg = 180f - angleDeg;
                }

                // หมุนหัวให้เล็งไปตามมุมยิง
                if (head != null)
                {
                    head.rotation =
                        Quaternion.Euler(0f, 0f, angleDeg);

                    // พลิก sprite ถ้าจำเป็น
                    if (dx < 0f)
                    {
                        head.localScale =
                            new Vector3(1f, -1f, 1f);
                    }
                    else
                    {
                        head.localScale =
                            new Vector3(1f, 1f, 1f);
                    }
                }

                // รอ 1 เฟรมให้เห็นหัวหมุนก่อนยิง
                yield return null;

                // สร้างไข่
                GameObject egg = Instantiate(
                    eggPrefab,
                    spawnEgg.position,
                    Quaternion.identity
                );

                // ใส่ความเร็วให้ไข่
                Rigidbody rb = egg.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = new Vector3(
                        vx,
                        vy,
                        0f
                    );
                }

                Debug.Log(
                    $"Egg {i} | " +
                    $"TargetX={targetX:F2} | " +
                    $"Speed={launchSpeed:F2} | " +
                    $"Angle={angleDeg:F2}"
                );

                success = true;
                break;
            }

            if (!success)
            {
                Debug.LogWarning("ยิงไข่ฟองนี้ไม่สำเร็จ");
            }

            yield return new WaitForSeconds(delayBetweenEggs);
        }

        // เปิดสคริปต์มองตามผู้เล่นกลับมา
        if (lookScript != null)
        {
            lookScript.enabled = true;
        }

        // คูลดาวน์หลังใช้สกิล
        yield return new WaitForSeconds(1f);
    }
}