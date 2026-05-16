using System.Collections;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefab1;   // ลูกโป่งน้ำปกติ (80%)
    public GameObject prefab2;   // ลูกโป่งพิเศษ (20%)

    [Header("Spawn Points")]
    public Transform spawnBall;
    public Transform spawnBall2;

    [Header("References")]
    public BossFly bossFly;

    [Header("Attack")]
    public int shoot = 5;
    public float delay = 0.5f;

    public IEnumerator Attack()
    {
        // หยุดการเคลื่อนที่ของบอสระหว่างยิง
        if (bossFly != null)
        {
            bossFly.canMove = false;

            if (bossFly.rb != null)
            {
                bossFly.rb.linearVelocity = Vector3.zero;
            }
        }

        for (int i = 0; i < shoot; i++)
        {
            // สุ่ม prefab
            GameObject selectedPrefab = GetRandomPrefab();

            // ยิงจากปากซ้าย
            Instantiate(
                selectedPrefab,
                spawnBall.position,
                spawnBall.rotation
            );

            // ยิงจากปากขวา
            Instantiate(
                selectedPrefab,
                spawnBall2.position,
                spawnBall2.rotation
            );

            yield return new WaitForSeconds(delay);
        }

        // พักหลังยิง
        yield return new WaitForSeconds(1f);
    }

    GameObject GetRandomPrefab()
    {
        // Random.value ได้ค่า 0.0 - 1.0
        // < 0.8 = 80%
        // >= 0.8 = 20%
        if (Random.value < 0.8f)
        {
            return prefab1;
        }
        else
        {
            return prefab2;
        }
    }
}