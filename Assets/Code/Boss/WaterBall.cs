using System.Collections;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnBall;
    public Transform spawnBall2;
    public BossFly bossFly;
    [Header("Attack")]
    public int shoot = 5;
    public float delay = 0.5f;

    public IEnumerator Attack()
    {
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
            Instantiate(prefab,spawnBall.position,spawnBall.rotation);
            Instantiate(prefab,spawnBall2.position,spawnBall2.rotation);

            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(1f);
    }
}