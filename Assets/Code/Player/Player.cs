using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Player")]
    public int hp = 3;

    [Header("UI")]
    public TMP_Text HPText;

    public GameObject deathUI;

    public GameObject body;

    [Header("Death")]
    public GameObject coffinPrefab;
    public GameObject drownedPrefab;

    public float restartDelay = 3f;

    private bool isDead;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        HPText.text = new string('♥', Mathf.Max(hp, 0));
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;

        hp = Mathf.Max(hp, 0);

        if (hp <= 0)
        {
            StartCoroutine(RIP());
        }
    }
    public void Heal(int amount)
    {
        if (isDead) return;

        hp += amount;

        // จำกัดไม่ให้เลือดเกิน 10
        hp = Mathf.Min(hp, 3);
    }

    IEnumerator RIP()
    {
        isDead = true;

        Vector3 deathPosition = transform.position;

        // ซ่อน body
        if (body != null)
        {
            body.SetActive(false);
        }

        // ปิด collider
        GetComponent<Collider>().enabled = false;

        // หยุด movement
        Rigidbody playerRb = GetComponent<Rigidbody>();

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
        }

        // ปิด script เดิน
        GetComponent<PlayerMove>().enabled = false;

        // สร้างโลง
        GameObject coffin = Instantiate(
            coffinPrefab,
            deathPosition,
            Quaternion.identity
        );

        Rigidbody rb = coffin.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(
                Vector3.up * 14f +
                Vector3.right * Random.Range(-1f, 1f),
                ForceMode.Impulse
            );
        }

        // รอ
        yield return new WaitForSeconds(0.5f);

        // เปิด YOU DIED
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }

        // รอรีฉาก
        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator Drowned()
    {
        Vector3 deathPosition = transform.position;
        isDead = true;
        if (body != null)
        {
            body.SetActive(false);
        }
        GetComponent<Collider>().enabled = false; 

        Rigidbody playerRb = GetComponent<Rigidbody>();

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
        }

        // ปิด script เดิน
        GetComponent<PlayerMove>().enabled = false;

        // สร้างโลง
        GameObject coffin = Instantiate(drownedPrefab,deathPosition,Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy( coffin );

        yield return new WaitForSeconds(0.5f);

        // เปิด YOU DIED
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }

        // รอรีฉาก
        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}