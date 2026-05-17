// NakYoneJan.cs
using System.Collections;
using UnityEngine;

public class NakYoneJan : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public GameObject janPrefab;

    [Header("Repeat Count")]
    public int again = 3;   // จำนวนครั้งที่จะโยนซ้ำ

    [HideInInspector] public bool haveJan = true;

    private bool spawnedThisCycle = false;

    private void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        // ส่งค่าเริ่มต้นเข้า Animator
        anim.SetInteger("again", again);
        anim.SetBool("haveJan", true);
        anim.SetBool("done", false);
    }

    private void Update()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // ===== เข้า State "AlreadyT" แล้วสร้าง Jan =====
        if (state.IsName("AlreadyT") && haveJan && !spawnedThisCycle)
        {
            spawnedThisCycle = true;
            StartCoroutine(SpawnJan());
        }

        // ===== เมื่อเข้า State "Again" ให้เตรียมพร้อมสร้างรอบถัดไป =====
        if (state.IsName("Again"))
        {
            spawnedThisCycle = false;
        }

        // ===== ถ้าจำนวนครั้งหมดแล้ว ให้ติ๊ก done =====
        if (again <= 0)
        {
            anim.SetBool("done", true);
        }
    }

    private IEnumerator SpawnJan()
    {
        // สร้างวัตถุ Jan
        GameObject jan = Instantiate(
            janPrefab,
            transform.position,
            Quaternion.identity
        );

        // บอกว่าในสนามมี Jan อยู่แล้ว
        haveJan = false;
        anim.SetBool("haveJan", false);

        // ส่ง reference ให้ Spin เพื่อให้แจ้งกลับมาได้
        Spin spin = jan.GetComponent<Spin>();
        if (spin != null)
        {
            spin.owner = this;
        }

        yield return null;
    }

    // เรียกจาก Spin เมื่อ Jan กลับมาถึงจุดเริ่มก่อนถูกทำลาย
    public void OnJanReturned()
    {
        haveJan = true;
        anim.SetBool("haveJan", true);

        // ลดจำนวนครั้งที่จะโยนซ้ำ
        again--;
        if (again < 0) again = 0;

        anim.SetInteger("again", again);

        // ถ้าหมดแล้ว ให้ส่งสัญญาณว่าจบสกิล
        if (again <= 0)
        {
            anim.SetBool("done", true);
        }
    }
}