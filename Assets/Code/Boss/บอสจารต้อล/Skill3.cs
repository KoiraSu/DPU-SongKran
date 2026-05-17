using System.Collections;
using UnityEngine;

public class Skill3 : MonoBehaviour
{
    public GameObject body1;
    public GameObject body2Prefab;

    private GameObject body2Instance;

    public IEnumerator Attack()
    {
        // ซ่อนร่างเดิม
        if (body1 != null)
            body1.SetActive(false);

        // สร้างร่างใหม่ที่ตำแหน่งและการหมุนเดียวกับวัตถุที่ติดสคริปต์นี้
        // ไม่ต้องมี spawnPoint ให้ชีวิตง่ายขึ้นสักเรื่อง
        if (body2Prefab != null)
        {
            body2Instance = Instantiate(
                body2Prefab,
                transform.position,
                transform.rotation,
                transform // ให้เป็นลูกของวัตถุนี้ จะได้ขยับตามถ้าจำเป็น
            );
        }

        // ถ้าสร้างไม่สำเร็จ ก็คืนร่างเดิมทันที
        if (body2Instance == null)
        {
            if (body1 != null)
                body1.SetActive(true);

            yield break;
        }

        // หา Animator ของ body2
        Animator anim = body2Instance.GetComponent<Animator>();

        // รอจนกว่า Animator bool "done" จะเป็น true
        if (anim != null)
        {
            yield return new WaitUntil(() => anim.GetBool("done"));
        }
        else
        {
            // ถ้าไม่มี Animator ก็รอ 1 เฟรมแล้วไปต่อ
            yield return null;
        }

        // ลบร่าง body2
        if (body2Instance != null)
            Destroy(body2Instance);

        // เปิดร่างเดิมกลับมา
        if (body1 != null)
            body1.SetActive(true);

        // คูลดาวน์
        yield return new WaitForSeconds(1f);
    }
}
