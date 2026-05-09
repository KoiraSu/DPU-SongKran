using UnityEngine;

public class Leg : MonoBehaviour
{
    public PlayerMove playerMove;     // เอา movement มาจากนี่
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerMove.movement.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerMove.movement.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}