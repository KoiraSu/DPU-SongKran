using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletN;
    public GameObject bulletB;
    public Transform firePoint;
    public float bulletForce = 30f;
    public float fireRate = 0.1f;

    [Header("Big Shoot")]
    public float chargeTime = 2f;
    public float cooldown = 3f;

    private float nextFireTime;

    private bool isCharging = false;
    private bool isCooldown = false;

    void Update()
    {
        // ถ้ากำลังชาร์จหรือคูลดาวน์ ห้ามยิงอะไรเลย
        if (isCharging || isCooldown)
            return;

        // ยิงปกติ
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // ยิงใหญ่
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartCoroutine(BigShoot());
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletN, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletForce;
        }

        Destroy(bullet, 3f);
    }

    IEnumerator BigShoot()
    {
        isCharging = true;

        Debug.Log("Charging...");

        // รอชาร์จ 2 วิ
        yield return new WaitForSeconds(chargeTime);

        Debug.Log("BIG SHOOT!");

        GameObject bullet = Instantiate(bulletB, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletForce;
        }

        Destroy(bullet, 3f);

        isCharging = false;
        isCooldown = true;

        Debug.Log("Cooldown...");

        // คูลดาวน์
        yield return new WaitForSeconds(cooldown);

        isCooldown = false;

        Debug.Log("Ready");
    }
}