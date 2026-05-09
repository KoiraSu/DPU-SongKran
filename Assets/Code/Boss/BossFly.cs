using UnityEngine;

public class BossFly : MonoBehaviour
{
    public Rigidbody rb;

    public float maxSpeed = 8f;
    public float steerForce = 5f;
    public float changeTargetTime = 3f;
    private Vector3 targetPoint;
    private float timer;
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PickNewTarget();
    }

    void PickNewTarget()
    {
        targetPoint = new Vector3(
            Random.Range(-83f, 25f), // X
            Random.Range(60f, 65f),  // Y
            0f                       // Z ล็อคไว้
        );
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        timer += Time.fixedDeltaTime;

        if (timer >= changeTargetTime)
        {
            PickNewTarget();
            timer = 0f;
        }

        // ทิศทางที่จะไป
        Vector3 desiredVelocity = (targetPoint - rb.position).normalized * maxSpeed;
        // แรงเลี้ยว
        Vector3 steering = desiredVelocity - rb.linearVelocity;
        steering = Vector3.ClampMagnitude(steering, steerForce);
        // ใส่แรง
        rb.AddForce(steering);
        // จำกัดความเร็ว
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        // ล็อคแกน Z
        rb.position = new Vector3(rb.position.x,rb.position.y,0f);
    }
}