using System.Collections;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    public GameObject prefab1;

    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public Transform spawn5;

    public int shoot = 1;
    public float delay = 1f;

    public IEnumerator Attack()
    {
        for (int i = 0; i < shoot; i++)
        {
            // ยิงจาก spawn1
            GameObject ball1 = Instantiate(prefab1, spawn1.position, spawn1.rotation);
            BallFire fire1 = ball1.GetComponent<BallFire>();
            if (fire1 != null)
            {
                fire1.Fire(spawn1.up);
            }
            yield return new WaitForSeconds(delay);

            // ยิงจาก spawn2
            GameObject ball2 = Instantiate(prefab1, spawn2.position, spawn2.rotation);
            BallFire fire2 = ball2.GetComponent<BallFire>();
            if (fire2 != null)
            {
                fire2.Fire(spawn2.up);
            }
            yield return new WaitForSeconds(delay);

            GameObject ball3 = Instantiate(prefab1, spawn3.position, spawn3.rotation);
            BallFire fire3 = ball3.GetComponent<BallFire>();
            if (fire3 != null) 
            {
                fire3.Fire(spawn3.up);
            }
            yield return new WaitForSeconds (delay);

            GameObject ball4 = Instantiate(prefab1, spawn4.position, spawn4.rotation);
            BallFire fire4 = ball4.GetComponent<BallFire>();
            if (fire4 != null)
            {
                fire4.Fire(spawn4.up);
            }
            yield return new WaitForSeconds(delay);

            GameObject ball5 = Instantiate(prefab1, spawn5.position, spawn5.rotation);
            BallFire fire5 = ball5.GetComponent<BallFire>();
            if (fire4 != null)
            {
                fire5.Fire(spawn5.up);
            }
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(1f);
    }
}