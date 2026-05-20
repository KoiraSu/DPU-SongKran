
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public BossStatus BossCheck;

    [Header("Effects")]
    public GameObject JanDance;
    public GameObject JumpScare;

    [Header("JumpScare Movement")]
    public float moveSpeed = 50f;
    public float targetZ = -74f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpScareSound;
    public AudioClip thxSound;

    private bool hasSpawned = false;
    private GameObject currentJumpScare;

    void Update()
    {
        // บอสตายครั้งเดียวพอ โลกไม่ต้องรับ jumpscare 60 ตัวต่อวินาที
        if (BossCheck != null && BossCheck.isDead && !hasSpawned)
        {
            Instantiate(JanDance);
            hasSpawned = true;

            StartCoroutine(LoadJS());
        }

        // ทำให้ JumpScare พุ่งเข้ามาที่ Z = -74
        if (currentJumpScare != null)
        {
            Vector3 targetPosition = new Vector3(
                currentJumpScare.transform.position.x,
                currentJumpScare.transform.position.y,
                targetZ
            );

            currentJumpScare.transform.position = Vector3.MoveTowards(
                currentJumpScare.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
    }

    IEnumerator LoadJS()
    {
        // รอ 5 วินาทีหลังบอสตาย
        yield return new WaitForSeconds(5f);

        // สร้าง JumpScare
        currentJumpScare = Instantiate(JumpScare);

        // หา AudioSource ถ้าไม่ได้ลากมาให้
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // เล่นเสียงที่กำหนด
        if (audioSource != null && jumpScareSound != null)
        {
            audioSource.PlayOneShot(jumpScareSound);
            audioSource.PlayOneShot(thxSound);
        }
    }
}

