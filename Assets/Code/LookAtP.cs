using UnityEngine;

public class LookAtP : MonoBehaviour
{
    public Transform player;

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
            Debug.LogWarning("LookAtP: ไม่พบ Player");
        }
    }

    void Update()
    {
        if (player == null) return;

        // หาทิศไปหา Player
        Vector3 dir = player.position - transform.position;

        // แปลงเป็นองศา
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // หมุนไปหา Player
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}