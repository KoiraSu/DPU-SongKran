using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public Animator anim;
    private Transform playerTransform;
    private Vector3 lastposi;
    private Rigidbody rb;
    private Renderer rend;
    private Color originalColor;
    public GameObject[] DropItem;//ของดรอป
    public Color hitColor = Color.red;
    public float moveSpeed = 3f;
    public float attackRange = 10f; // ระยะยิง
    public float blinkDuration = 0.1f;
    
    public GameObject bulletPrefab;
    public bool finishAttack = true;


    public void TakeDamage(int amount)
    {
        hp -= amount;

        StartCoroutine(BlinkRoutine());

        if (hp <= 0)
        {
            
            Drop();
            Destroy(gameObject);
        }
    }
    public void Drop()
    {
        int randomIndex = Random.Range(0, DropItem.Length);

        Instantiate(
            DropItem[randomIndex],
            lastposi,
            Quaternion.identity
        );
    }

    IEnumerator BlinkRoutine()
    {
        rend.material.color = hitColor;

        yield return new WaitForSeconds(blinkDuration);

        rend.material.color = originalColor;
    }
    IEnumerator Attack()
    {
        finishAttack = false;

        yield return new WaitForSeconds(0.4f);

        GameObject bullet = Instantiate(
            bulletPrefab,
            transform.position,
            transform.rotation
        );

        Destroy(bullet, 3);

        Rigidbody rbBullet =
            bullet.GetComponent<Rigidbody>();

        rbBullet.linearVelocity =
            transform.forward * 60f;

        yield return new WaitForSeconds(1f);

        finishAttack = true;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }


    void FixedUpdate()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(
            transform.position,
            playerTransform.position
        );

        if (distance > attackRange)
        {
            Vector3 direction =
                (playerTransform.position - transform.position)
                .normalized;

            direction.y = 0;

            rb.linearVelocity =
                direction * moveSpeed;

            transform.LookAt(
                new Vector3(
                    playerTransform.position.x,
                    transform.position.y,
                    playerTransform.position.z
                )
            );

            anim.SetBool("move", true);
            anim.SetBool("attack", false);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;

            transform.LookAt(
                new Vector3(
                    playerTransform.position.x,
                    transform.position.y,
                    playerTransform.position.z
                )
            );

            anim.SetBool("move", false);
            anim.SetBool("attack", true);

            if (finishAttack)
            {
                StartCoroutine(Attack());
            }
        }

        lastposi = transform.position;
    }
}