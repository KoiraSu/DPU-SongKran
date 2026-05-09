using System.Collections;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    public int hp;
    public int Damage;
    private Vector3 lastposi;
    public GameObject[] DropItem;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color hitColor = Color.red;
    public float blinkDuration = 0.1f;

    public void Drop()
    {
        int randomIndex = Random.Range(0, DropItem.Length);
        Instantiate(
            DropItem[randomIndex],
            lastposi,
            Quaternion.identity
        );
    }

    public void TakeDamage(int amount)
    { 
        StartCoroutine(BlinkRoutine());
        hp -= amount;

       

        if (hp <= 0)
        {
            Drop();
            Destroy(gameObject);
        }
    }
    IEnumerator BlinkRoutine()
    {
        if (spriteRenderer == null)
            yield break;

        spriteRenderer.material.color = hitColor;

        yield return new WaitForSeconds(blinkDuration);

        spriteRenderer.material.color = originalColor;
    }

    void Start()
    {
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
}
