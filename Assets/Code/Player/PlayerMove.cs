using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Player Player;
    private Rigidbody rb;
    public Animator anim;
    public Vector2 movement;
    

    [Header("Movement Bounds (World Space)")]
    [Tooltip("มุมซ้ายล่างของพื้นที่ที่เดินได้")]
    public Vector2 boundsMin = new Vector2(-50f, 0f);

    [Tooltip("มุมขวาบนของพื้นที่ที่เดินได้")]
    public Vector2 boundsMax = new Vector2(50f, 100f);


    [Header("Move")]
    public float walk = 10f;
    private float speed;

    [Header("Jump")]
    public float jumpForce = 12f;
    public int maxJump = 1;
    private int jumpCount = 0;

    [Header("Jump Physics")]
    public float fallMultiplier = 4.5f;
    public float lowJumpMultiplier = 3f;

    [Header("Dash")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private Collider[] playerColliders;

    public bool isDashing = false;
    private bool canDash = true;
    private Vector3 dashDirection = Vector3.right;

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>();

        // จำเฉพาะซ้าย/ขวา
        if (Mathf.Abs(movement.x) > 0.01f)
        {
            dashDirection = new Vector3(Mathf.Sign(movement.x),0f,0f);
        }
    }

    public void OnJump(InputValue input)
    {
        if (input.isPressed && jumpCount < maxJump && !isDashing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
            anim.SetBool("Jump", true);
        }
    }

    public void OnDash(InputValue input)
    {
        if (input.isPressed && canDash && !isDashing)
        {
            anim.SetBool("Dash", true);
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        canDash = false;

        // เปิดสถานะอมตะระหว่าง Dash
        SetInvincible(true);

        // ถ้าไม่มีทิศเดินเลย ให้พุ่งไปทางขวา
        Vector3 dir = dashDirection;

        if (dir.sqrMagnitude < 0.01f)
        {
            dir = Vector3.right;
        }

        // ล้างความเร็วเดิม แต่เก็บความเร็วแนวดิ่งไว้
        rb.linearVelocity = new Vector3(
            0f,
            rb.linearVelocity.y,
            0f
        );

        float timer = 0f;

        // ===== แก้เฉพาะส่วน while ใน DashRoutine() =====
        while (timer < dashDuration)
        {
            // พุ่งเฉพาะแกน X
            rb.linearVelocity = new Vector3(
                dir.x * dashSpeed,
                rb.linearVelocity.y,
                0f
            );

            // จำกัดเฉพาะแกน X
            Vector3 pos = rb.position;
            pos.x = Mathf.Clamp(pos.x, boundsMin.x, boundsMax.x);
            rb.position = pos;

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // หยุดพุ่งเฉพาะแนวนอน
        rb.linearVelocity = new Vector3(
            0f,
            rb.linearVelocity.y,
            0f
        );

        // เปิด Collider กลับมา
        SetInvincible(false);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // ===== แก้ฟังก์ชัน Move() =====
    void Move()
    {
        if (isDashing) return;

        Vector3 moveDir = new Vector3(movement.x, 0f, movement.y);
        Vector3 vel = transform.TransformDirection(moveDir) * speed;

        // ตั้งความเร็ว
        rb.linearVelocity = new Vector3(
            vel.x,
            rb.linearVelocity.y,
            0f
        );

        // จำกัดเฉพาะแกน X (เดินซ้าย-ขวา)
        Vector3 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, boundsMin.x, boundsMax.x);
        rb.position = pos;
    }

    void ApplyBetterJumpPhysics()
    {
        if (isDashing) return;

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    void SetInvincible(bool value)
    {
        foreach (Collider col in playerColliders)
        {
            if (col != null)
            {
                col.enabled = !value;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            StartCoroutine(Player.Drowned());
        }
    }

    void Start()
    {
        speed = walk;
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0f;
        Player = GetComponent<Player>();
        // เก็บ Collider ทั้งหมดของผู้เล่น
        playerColliders = GetComponentsInChildren<Collider>();
    }
    void Update()
    {
        if (!isDashing)
        {
            speed = walk;
        }

    }

    void FixedUpdate()
    {
        Move();
        ApplyBetterJumpPhysics();
    }
}