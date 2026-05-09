using UnityEngine;

public class LookAtP : MonoBehaviour
{
    public Transform player;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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