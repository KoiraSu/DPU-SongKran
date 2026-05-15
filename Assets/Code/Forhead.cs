using UnityEngine;

public class Forhead : MonoBehaviour
{
    public Transform player;
    public float trackingTime = 2f; // หมุนตามผู้เล่นเป็นเวลา 2 วินาที
    private float timer = 0f;
    private bool isTracking = true;

    void Start()
    {
        // หา Player อัตโนมัติ
        GameObject target =
            GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            player = target.transform;
        }
        else
        {
            Debug.LogWarning("Forhead: ไม่พบ Player");
            isTracking = false;
        }
    }

    void Update()
    {
        // ถ้าหยุดหมุนแล้ว หรือไม่พบ Player
        if (!isTracking || player == null)
            return;

        // นับเวลา
        timer += Time.deltaTime;

        // หาทิศไปหา Player
        Vector3 dir =
            player.position - transform.position;

        // คำนวณมุม
        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // หมุนไปหา Player
        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        // พลิกซ้าย/ขวา
        if (dir.x < 0f)
        {
            transform.localScale =
                new Vector3(1f, -1f, 1f);
        }
        else
        {
            transform.localScale =
                new Vector3(1f, 1f, 1f);
        }

        // ครบเวลาที่กำหนดแล้วหยุดหมุน
        if (timer >= trackingTime)
        {
            isTracking = false;
        }
    }
}