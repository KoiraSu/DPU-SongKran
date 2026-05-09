using System.Collections;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("Objects")]
    public GameObject warningObject;
    public GameObject balloonObject;
    public GameObject splashObject;
    public float splashHeightOffset = 5f;
    private Rigidbody rb;
    public float fallSpeed = 30f;
    public int damage = 1;
    public int Hp = 1;
    void Start()
    {
        rb = balloonObject.GetComponent<Rigidbody>();

        // เริ่มต้น
        warningObject.SetActive(true);
        balloonObject.GetComponent<MeshRenderer>().enabled = false;
        splashObject.SetActive(false);

        // ยังไม่ตก
        rb.isKinematic = true;

        StartCoroutine(StartFall());
    }

    IEnumerator StartFall()
    {
        // ค้างเตือน 2 วิ
        yield return new WaitForSeconds(2f);

        // ปิดเตือน
        warningObject.SetActive(false);

        // เปิดลูกโป่ง
        balloonObject.SetActive(true);

        // เริ่มตก
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.down * fallSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.instance.TakeDamage(damage);

            Destroy(transform.root.gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {

        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("แตะพื้น");

            // จุดชน
            Vector3 hitPoint = collision.contacts[0].point;

            // เพิ่มความสูง
            hitPoint.y += splashHeightOffset;

            // ปิดลูกโป่ง
            balloonObject.SetActive(false);

            // แยก splash ออกจาก parent
            splashObject.transform.SetParent(null);

            // ย้ายตำแหน่ง
            splashObject.transform.position = hitPoint;

            // เปิดน้ำแตก
            splashObject.SetActive(true);

            // ลบน้ำแตกใน 1 วิ
            Destroy(splashObject, 0.3f);

            // ลบ parent ทันที
            Destroy(transform.root.gameObject);
        }

    }
}