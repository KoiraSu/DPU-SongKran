using UnityEngine;

public class LookAtP : MonoBehaviour
{
    public Transform player;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // หา Player จาก Tag อัตโนมัติ
        GameObject target =
            GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            player = target.transform;
        }
        else
        {
            Debug.LogWarning("LookAtP: ไม่พบ GameObject ที่มี Tag = Player");
        }
    }

    void Update()
    {
        // ถ้ายังหา Player ไม่เจอ ก็ไม่ต้องทำอะไร
        if (player == null) return;

        // ถ้า player อยู่ด้านขวาของบอส
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        // ถ้า player อยู่ด้านซ้ายของบอส
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}