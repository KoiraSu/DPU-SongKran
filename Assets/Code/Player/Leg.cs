using Unity.VisualScripting;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public PlayerMove playerMove;     // เอา movement มาจากนี่
    private SpriteRenderer spriteRenderer;
    public Animator ani;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerMove.movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerMove.movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        // ถ้ากำลัง Dash ให้เล่นอนิเมชันวิ่งต่อ
        if (playerMove.isDashing)
        {
            ani.SetFloat("speed", 1f);
            return;
        }

        // ถ้ามีการกดซ้าย/ขวา ให้ส่งค่าความแรงของการเคลื่อนที่
        if (playerMove.movement != Vector2.zero)
        {
            ani.SetFloat("speed", playerMove.movement.magnitude);
        }
        else
        {
            ani.SetFloat("speed", 0f);
        }
    }
}